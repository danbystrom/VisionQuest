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
uniform extern float3	CameraPosition;
uniform extern float3   LightDirection;

uniform extern float4   gMtrlAmbient;
uniform extern float4   gMtrlDiffuse;
uniform extern float4   gMtrlSpec;
uniform extern float    gMtrlSpecPower;
uniform extern float4   gLightAmbient;
uniform extern float4   gLightDiffuse;
uniform extern float4   gLightSpec;

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

uniform float4 LakeTextureTransformation;

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

struct LakeWaterVertexOutput
{
    float4 Position  : POSITION;
    float4 ReflectionMapPos : TEXCOORD0;
    float2 BumpMapSamplingPos : TEXCOORD1;
    float4 Position3D : TEXCOORD2;
};

struct OceanWaterVertexOutput
{
    float4 Position         : POSITION0;
	float4 ReflectionMapPos : TEXCOORD0;
    float2 BumpMapSamplingPos : TEXCOORD1;
    float4 Position3D       : TEXCOORD2;
    float2 tex0             : TEXCOORD3;
    float2 tex1             : TEXCOORD4;
    float3 toEyeT           : TEXCOORD5;
    float3 lightDirT        : TEXCOORD6;
	float  LakeBlendFactor  : TEXCOORD7;
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


OceanWaterVertexOutput OceanWaterVS(
    float4 posLocal        : POSITION0, 
    float2 normalizedTexC : TEXCOORD0,
    float2 scaledTexC     : TEXCOORD1)
{
	float4x4 wvp = mul( World, mul( View, Projection ) );

    // Zero out our output.
	OceanWaterVertexOutput outVS = (OceanWaterVertexOutput)0;

	// Scroll vertex texture coordinates to animate waves.
	float2 vTex0 = normalizedTexC + gWaveDMapOffset0;
	float2 vTex1 = normalizedTexC + gWaveDMapOffset1;
	
	// Scroll texture coordinates.
	outVS.tex0 = scaledTexC + gWaveNMapOffset0;
	outVS.tex1 = scaledTexC + gWaveNMapOffset1;

	float4 depthVector = mul( posLocal, wvp );      
	float blendDistance = 0.97f;
	float blendWidth = 0.03f;
	outVS.LakeBlendFactor = clamp((depthVector.z/depthVector.w-blendDistance)/blendWidth, 0, 1);

	// Set y-coordinate of water grid vertices based on displacement mapping.
	posLocal.y = lerp( DoDispMapping(vTex0, vTex1), 0.75, outVS.LakeBlendFactor );
	
	float4x4 rwvp = mul( World, mul(ReflectedView, Projection));
    outVS.ReflectionMapPos =  mul(posLocal, rwvp);

	// Estimate TBN-basis using finite differencing in local space.  
	float r = DoDispMapping(vTex0 + float2(DMAP_DX, 0.0f), 
	                        vTex1 + float2(0.0f, DMAP_DX));
	float b = DoDispMapping(vTex0 + float2(DMAP_DX, 0.0f), 
	                        vTex1 + float2(0.0f, DMAP_DX));  
	                        
	float3x3 TBN;                       
	TBN[0] = normalize(float3(1.0f, (r-posLocal.y)/gGridStepSizeL.x, 0.0f)); 
	TBN[1] = normalize(float3(0.0f, (b-posLocal.y)/gGridStepSizeL.y, -1.0f));
	TBN[2] = normalize(cross(TBN[0], TBN[1]));
	
	// Matrix transforms from object space to tangent space.
	float3x3 toTangentSpace = transpose(TBN);
	
	// Transform eye position to local space.
	float3 eyePosL = mul(float4(CameraPosition, 1.0f), WorldInv).xyz;
	
	// Transform to-eye vector to tangent space.
	float3 toEyeL = eyePosL - posLocal;
	outVS.toEyeT = mul(toEyeL, toTangentSpace);

	// Transform light direction to tangent space.
	float3 lightDirL = mul(float4(LightDirection, 0.0f), WorldInv).xyz;
	outVS.lightDirT  = mul(lightDirL, toTangentSpace);
	
	// Transform to homogeneous clip space.
	outVS.Position = mul(posLocal, wvp);
	
    return outVS;
}

// Calculate the 2D screenposition of a position vector
float2 postProjToScreen(float4 position)
{
	float2 screenPos = position.xy / position.w;
	return 0.5f * (float2(screenPos.x, -screenPos.y) + 1);
}

float4 OceanWaterPS( OceanWaterVertexOutput input ) : COLOR
{
	// Light vector is opposite the direction of the light.
	float3 lightDirT = normalize(input.lightDirT);
	
	// Sample normal map.
	float3 normalT0 = tex2D(WaveMapS0, input.tex0);
	float3 normalT1 = tex2D(WaveMapS1, input.tex1);
	
	// Expand from [0, 1] compressed interval to true [-1, 1] interval.
    normalT0 = 2.0f*normalT0 - 1.0f;
    normalT1 = 2.0f*normalT1 - 1.0f;
    
	// Average the two vectors.
	float3 normalT = normalize(normalT0 + normalT1);
	
	// Compute the reflection vector.
	float3 r = reflect(lightDirT, normalT);
	
	float3 toEye = normalize(input.toEyeT);
	// Determine how much (if any) specular light makes it into the eye.
	float t  = pow(max(dot(r, toEye), 0.0f), gMtrlSpecPower);
	
	// Determine the diffuse light intensity that strikes the vertex.
	float s = max(dot(-lightDirT, normalT), 0.0f);
	
	// If the diffuse light intensity is low, kill the specular lighting term.
	// It doesn't look right to add specular light when the surface receives 
	// little diffuse light.
	if(s <= 0.0f)
	     t = 0.0f;

	//BEGIN reflection	
	float2 reflectionUV = postProjToScreen(input.ReflectionMapPos);
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


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

float WaveLength;
float WaveHeight;

float Time;
float WindForce;
float3 WindDirection;
//float Overcast;

Texture WaterBumpMap;
sampler WaterBumpMapSampler = sampler_state { texture = <WaterBumpMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture Checker;
sampler CheckerSampler = sampler_state { texture = <Checker> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
 
 struct WPixelToFrame
 {
     float4 Color : COLOR0;
 };
 
 LakeWaterVertexOutput LakeWaterVS(
   float4 posLocal       : POSITION,
   float2 normalizedTexC : TEXCOORD0,
   float2 scaledTexC     : TEXCOORD1)
{    
    LakeWaterVertexOutput Output = (LakeWaterVertexOutput)0;

    float4x4 preViewProjection = mul (View, Projection);
    float4x4 preWorldViewProjection = mul (World, preViewProjection);
    float4x4 preReflectionViewProjection = mul (ReflectedView, Projection);
    float4x4 preWorldReflectionViewProjection = mul (World, preReflectionViewProjection);

    Output.Position = mul(posLocal, preWorldViewProjection);
    Output.ReflectionMapPos = mul(posLocal, preWorldReflectionViewProjection);
    Output.Position3D = mul(posLocal, World);        
    
	float2 bmsp = scaledTexC/8 + gWaveNMapOffset0;
    Output.BumpMapSamplingPos = float2(
	  (bmsp.x + LakeTextureTransformation.x) * LakeTextureTransformation.z,
	  (bmsp.y + LakeTextureTransformation.y) * LakeTextureTransformation.w);

    return Output;
}

WPixelToFrame LakeWaterPS(LakeWaterVertexOutput PSIn)
{
    WPixelToFrame Output = (WPixelToFrame)0;        
    
    float4 bumpColor = tex2D(WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
    float2 perturbation = WaveHeight*(bumpColor.rg - 0.5f)*2.0f;
    
    float2 ProjectedTexCoords;
    ProjectedTexCoords.x = PSIn.ReflectionMapPos.x/PSIn.ReflectionMapPos.w/2.0f + 0.5f;
    ProjectedTexCoords.y = -PSIn.ReflectionMapPos.y/PSIn.ReflectionMapPos.w/2.0f + 0.5f;        
    float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
    float4 reflectiveColor = tex2D(reflectionSampler, perturbatedTexCoords);
    
    float4 refractiveColor = float4(0.01,0.01,0.2,1); // tex2D(RefractionSampler, perturbatedRefrTexCoords);
    
    float3 eyeVector = normalize(CameraPosition - PSIn.Position3D);
    float3 normalVector = (bumpColor.rbg-0.5f)*2.0f;
    
    float fresnelTerm = dot(eyeVector, normalVector);    
    float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm);
    float4 dullColor = float4(0.3f, 0.3f, 0.5f, 1.0f);
    Output.Color = lerp(combinedColor, dullColor, 0.2f);    
    
    float3 reflectionVector = -reflect(LightDirection, normalVector);
    float specular = dot(normalize(reflectionVector), eyeVector);
    specular = pow(abs(specular), 256);        
    Output.Color.rgb += specular;

//Output.Color = tex2D(CheckerSampler, PSIn.BumpMapSamplingPos);
    
    return Output;
}

OceanWaterVertexOutput CombinedWaterVS(
   float4 posLocal       : POSITION,
   float2 normalizedTexC : TEXCOORD0,
   float2 scaledTexC     : TEXCOORD1)
{    
    OceanWaterVertexOutput oceanVS = OceanWaterVS( posLocal, normalizedTexC, scaledTexC );

	float2 bmsp = scaledTexC/8 + gWaveNMapOffset0;
    oceanVS.BumpMapSamplingPos = float2(
	  (bmsp.x + LakeTextureTransformation.x) * LakeTextureTransformation.z,
	  (bmsp.y + LakeTextureTransformation.y) * LakeTextureTransformation.w);
	oceanVS.Position3D = mul( posLocal, World );
	return oceanVS;
}

WPixelToFrame CombinedWaterPS(OceanWaterVertexOutput input)
{
    WPixelToFrame output = (WPixelToFrame)0;

	LakeWaterVertexOutput lake;
	lake.Position = input.Position;
	lake.ReflectionMapPos = input.ReflectionMapPos;
	lake.BumpMapSamplingPos = input.BumpMapSamplingPos;
	lake.Position3D = input.Position3D;

    //output.Color = LakeWaterPS(lake).Color * 0.5 + OceanWaterPS(input) * 0.5;
	output.Color = lerp( OceanWaterPS(input), LakeWaterPS(lake).Color,  1 /*input.LakeBlendFactor*/ );
	//output.Color = float4( input.LakeBlendFactor, input.LakeBlendFactor, input.LakeBlendFactor, 1 );
	return output;
}

technique OceanWaterTech
{
    pass P0
    {
        // Specify the vertex and pixel shader associated with this pass.
        vertexShader = compile vs_3_0 OceanWaterVS();
        pixelShader  = compile ps_3_0 OceanWaterPS();
    }    
}

 technique LakeWaterTech
 {
     pass Pass0
     {
         VertexShader = compile vs_3_0 LakeWaterVS();
         PixelShader = compile ps_3_0 LakeWaterPS();
     }
 }


 technique CombinedWaterTech
 {
     pass Pass0
     {
         VertexShader = compile vs_3_0 CombinedWaterVS();
         PixelShader = compile ps_3_0 CombinedWaterPS();
     }
 }

