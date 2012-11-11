//=============================================================================
// waterdmap.fx by Frank Luna (C) 2004 All Rights Reserved.
//
// Scrolls normal maps over for per pixel lighting of high 
// frequency waves; uses displacement mapping for modifying geometry.
//=============================================================================


uniform extern float4x4 World;
uniform extern float4x4 WorldInv;
uniform extern float4x4 View;
uniform extern float4x4 Projection;

uniform extern float4   gMtrlAmbient;
uniform extern float4   gMtrlDiffuse;
uniform extern float4   gMtrlSpec;
uniform extern float    gMtrlSpecPower;
uniform extern float4   gLightAmbient;
uniform extern float4   gLightDiffuse;
uniform extern float4   gLightSpec;
uniform extern float3   gLightDirW;
uniform extern float3   gCameraPosition;

// Texture coordinate offset vectors for scrolling
// normal maps and displacement maps.
uniform extern float2   gWaveNMapOffset0;
uniform extern float2   gWaveNMapOffset1;
uniform extern float2   gWaveDMapOffset0;
uniform extern float2   gWaveDMapOffset1;

// Two normal maps and displacement maps.
uniform extern texture  gWaveMap0;
uniform extern texture  gWaveMap1;
uniform extern texture  gWaveDispMap0;
uniform extern texture  gWaveDispMap1;

// User-defined scaling factors to scale the heights
// sampled from the displacement map into a more convenient
// range.
uniform extern float2   gScaleHeights;

// Space between grids in x,z directions in local space
// used for finite differencing.
uniform extern float2   gGridStepSizeL;

// Shouldn't be hardcoded, but ok for demo.
static const float DMAP_SIZE = 128.0f;
static const float DMAP_DX   = 1.0f / DMAP_SIZE;

uniform extern texture  ReflectedMap;
uniform extern float4x4 ReflectedView;

sampler WaveMapS0 = sampler_state
{
	Texture = <gWaveMap0>;
	MinFilter = ANISOTROPIC;
	MaxAnisotropy = 12;
	MagFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU  = WRAP;
    AddressV  = WRAP;
};

sampler WaveMapS1 = sampler_state
{
	Texture = <gWaveMap1>;
	MinFilter = ANISOTROPIC;
	MaxAnisotropy = 12;
	MagFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU  = WRAP;
    AddressV  = WRAP;
};

sampler DMapS0 = sampler_state
{
	Texture = <gWaveDispMap0>;
	MinFilter = POINT;
	MagFilter = POINT;
	MipFilter = POINT;
	AddressU  = WRAP;
    AddressV  = WRAP;
};

sampler DMapS1 = sampler_state
{
	Texture = <gWaveDispMap1>;
	MinFilter = POINT;
	MagFilter = POINT;
	MipFilter = POINT;
	AddressU  = WRAP;
    AddressV  = WRAP;
};

sampler2D reflectionSampler = sampler_state {
	texture = <ReflectedMap>; 
	MinFilter = Anisotropic; 
	MagFilter = Anisotropic;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct OutputVS
{
    float4 posH      : POSITION0;
    float3 toEyeT    : TEXCOORD0;
    float3 lightDirT : TEXCOORD1;
    float2 tex0      : TEXCOORD2;
    float2 tex1      : TEXCOORD3;
	float4 ReflectionPosition : TEXCOORD4;
};

float DoDispMapping(float2 texC0, float2 texC1)
{
	// Transform to texel space
    float2 texelpos = DMAP_SIZE * texC0;
        
    // Determine the lerp amounts.           
    float2 lerps = frac(texelpos);
    
	float dmap0[4];
	dmap0[0] = tex2Dlod(DMapS0, float4(texC0, 0.0f, 0.0f)).r;
	dmap0[1] = tex2Dlod(DMapS0, float4(texC0, 0.0f, 0.0f)+float4(DMAP_DX, 0.0f, 0.0f, 0.0f)).r;
	dmap0[2] = tex2Dlod(DMapS0, float4(texC0, 0.0f, 0.0f)+float4(0.0f, DMAP_DX, 0.0f, 0.0f)).r;
	dmap0[3] = tex2Dlod(DMapS0, float4(texC0, 0.0f, 0.0f)+float4(DMAP_DX, DMAP_DX, 0.0f, 0.0f)).r;
	
	// Filter displacement map:
	float h0 = lerp( lerp( dmap0[0], dmap0[1], lerps.x ),
                     lerp( dmap0[2], dmap0[3], lerps.x ),
                     lerps.y );
	
	texelpos = DMAP_SIZE * texC1;
	lerps    = frac(texelpos);
	
	float dmap1[4];
	dmap1[0] = tex2Dlod(DMapS1, float4(texC1, 0.0f, 0.0f)).r;
	dmap1[1] = tex2Dlod(DMapS1, float4(texC1, 0.0f, 0.0f)+float4(DMAP_DX, 0.0f, 0.0f, 0.0f)).r;
	dmap1[2] = tex2Dlod(DMapS1, float4(texC1, 0.0f, 0.0f)+float4(0.0f, DMAP_DX, 0.0f, 0.0f)).r;
	dmap1[3] = tex2Dlod(DMapS1, float4(texC1, 0.0f, 0.0f)+float4(DMAP_DX, DMAP_DX, 0.0f, 0.0f)).r;
	
	// Filter displacement map:
	float h1 = lerp( lerp( dmap1[0], dmap1[1], lerps.x ),
                     lerp( dmap1[2], dmap1[3], lerps.x ),
                     lerps.y );
                   
	// Sum and scale the sampled heights.  
    return gScaleHeights.x*h0 + gScaleHeights.y*h1;
}


OutputVS WaterVS(float3 posL           : POSITION0, 
                 float2 scaledTexC     : TEXCOORD0,
                 float2 normalizedTexC : TEXCOORD1)
{
    // Zero out our output.
	OutputVS outVS = (OutputVS)0;

	// Scroll vertex texture coordinates to animate waves.
	float2 vTex0 = normalizedTexC + gWaveDMapOffset0;
	float2 vTex1 = normalizedTexC + gWaveDMapOffset1;
	
	float4x4 rwvp = mul( World, mul(ReflectedView, Projection));
    outVS.ReflectionPosition =  mul(float4(posL, 1.0f), rwvp);

	// Set y-coordinate of water grid vertices based on displacement mapping.
	posL.y = DoDispMapping(vTex0, vTex1);
	
	// Estimate TBN-basis using finite differencing in local space.  
	float r = DoDispMapping(vTex0 + float2(DMAP_DX, 0.0f), 
	                        vTex1 + float2(0.0f, DMAP_DX));
	float b = DoDispMapping(vTex0 + float2(DMAP_DX, 0.0f), 
	                        vTex1 + float2(0.0f, DMAP_DX));  
	                        
	float3x3 TBN;                       
	TBN[0] = normalize(float3(1.0f, (r-posL.y)/gGridStepSizeL.x, 0.0f)); 
	TBN[1] = normalize(float3(0.0f, (b-posL.y)/gGridStepSizeL.y, -1.0f));
	TBN[2] = normalize(cross(TBN[0], TBN[1]));
	
	// Matrix transforms from object space to tangent space.
	float3x3 toTangentSpace = transpose(TBN);
	
	// Transform eye position to local space.
	float3 eyePosL = mul(float4(gCameraPosition, 1.0f), WorldInv).xyz;
	
	// Transform to-eye vector to tangent space.
	float3 toEyeL = eyePosL - posL;
	outVS.toEyeT = mul(toEyeL, toTangentSpace);
	
	// Transform light direction to tangent space.
	float3 lightDirL = mul(float4(gLightDirW, 0.0f), WorldInv).xyz;
	outVS.lightDirT  = mul(lightDirL, toTangentSpace);
	
	// Scroll texture coordinates.
	outVS.tex0 = scaledTexC + gWaveNMapOffset0;
	outVS.tex1 = scaledTexC + gWaveNMapOffset1;

	// Transform to homogeneous clip space.
	float4x4 wvp = mul( World, mul( View, Projection ) );
	outVS.posH = mul(float4(posL, 1.0f), wvp);
	
    return outVS;
}

// Calculate the 2D screenposition of a position vector
float2 postProjToScreen(float4 position)
{
	float2 screenPos = position.xy / position.w;
	return 0.5f * (float2(screenPos.x, -screenPos.y) + 1);
}

float4 WaterPS(	OutputVS input ) : COLOR
{
	// Interpolated normals can become unnormal--so normalize.
	// Note that toEyeW and normalW do not need to be normalized
	// because they are just used for a reflection and environment
	// map look-up and only direction matters.
	input.toEyeT    = normalize(input.toEyeT);
	input.lightDirT = normalize(input.lightDirT);
	
	// Light vector is opposite the direction of the light.
	float3 lightVecT = -input.lightDirT;
	
	// Sample normal map.
	float3 normalT0 = tex2D(WaveMapS0, input.tex0);
	float3 normalT1 = tex2D(WaveMapS1, input.tex1);
	
	// Expand from [0, 1] compressed interval to true [-1, 1] interval.
    normalT0 = 2.0f*normalT0 - 1.0f;
    normalT1 = 2.0f*normalT1 - 1.0f;
    
	// Average the two vectors.
	float3 normalT = normalize(normalT0 + normalT1);
	
	// Compute the reflection vector.
	float3 r = reflect(-lightVecT, normalT);
	
	// Determine how much (if any) specular light makes it into the eye.
	float t  = pow(max(dot(r, input.toEyeT), 0.0f), gMtrlSpecPower);
	
	// Determine the diffuse light intensity that strikes the vertex.
	float s = max(dot(lightVecT, normalT), 0.0f);
	
	// If the diffuse light intensity is low, kill the specular lighting term.
	// It doesn't look right to add specular light when the surface receives 
	// little diffuse light.
	if(s <= 0.0f)
	     t = 0.0f;

	//BEGIN reflection	
	float2 reflectionUV = postProjToScreen(input.ReflectionPosition);
	float2 uv = float2( normalT.x * 0.02f, -0.012f );
	float3 reflection = tex2D(reflectionSampler, reflectionUV + uv);
	//END reflection

	// Compute the ambient, diffuse and specular terms separatly. 
	float3 spec = t*(gMtrlSpec*gLightSpec).rgb;
	float3 diffuse = s*(gMtrlDiffuse*gLightDiffuse.rgb);
	float3 ambient = gMtrlAmbient*gLightAmbient;
	
	float3 final = ( 4*(ambient + diffuse + spec) + reflection ) / 5;

	// Output the color and the alpha.
    return float4(final, gMtrlDiffuse.a);

}

technique WaterTech
{
    pass P0
    {
        // Specify the vertex and pixel shader associated with this pass.
        vertexShader = compile vs_3_0 WaterVS();
        pixelShader  = compile ps_3_0 WaterPS();
    }    
}
