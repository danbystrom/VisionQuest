uniform extern float4x4 World;
uniform extern float4x4 WorldInv;
uniform extern float4x4 View;
uniform extern float4x4 Projection;
uniform extern float3	CameraPosition;
uniform extern float3   LightDirection;
SamplerState TextureSampler;
SamplerState TextureSampler2;

// Texture coordinate offset vectors for scrolling
// normal maps and displacement maps.
uniform extern float2   WaveNMapOffset0;
uniform extern float2   WaveNMapOffset1;
uniform extern float2   WaveDMapOffset0;
uniform extern float2   WaveDMapOffset1;

// Two normal maps and displacement maps.
uniform extern Texture2D  WaveDispMap0;
uniform extern Texture2D  WaveDispMap1;

// User-defined scaling factors to scale the heights
// sampled from the displacement map into a more convenient
// range.
uniform extern float2   ScaleHeights;

// Space between grids in x,z directions in local space
// used for finite differencing.
uniform extern float2   GridStepSizeL;

// Shouldn't be hardcoded, but ok for demo.
static const float DMAP_SIZE = 128.0f;
static const float DMAP_DX   = 1.0f / DMAP_SIZE;

uniform extern Texture2D  ReflectedMap;
uniform extern float4x4 ReflectedView;

uniform float4 LakeTextureTransformation;

uniform extern float   FogStart = 50;
uniform extern float   FogRange = 2000;
uniform extern float4  FogColor = float4(0,0,0,1);

bool DoShadowMapping = true;
float4x4 ShadowViewProjection;
float ShadowMult = 0.95f;
float ShadowBias = 0.01;
Texture2D ShadowMap;


SamplerState MySampler3
{
	Fil = MIN_MAG_MIP_POINT;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct OceanWaterVertexOutput
{
    float4 Position	        : SV_Position;
	float4 ReflectionMapPos : TEXCOORD0;
    float2 BumpSamplingPos0 : TEXCOORD1;
    float2 BumpSamplingPos1 : TEXCOORD2;
    float4 Position3D       : TEXCOORD3;
    float2 tex0             : TEXCOORD4;
    float2 tex1             : TEXCOORD5;
    float3 toEyeT           : TEXCOORD6;
    float3 lightDirT        : TEXCOORD7;
	float4 ShadowScreenPosition : TEXCOORD8;
};


float DoDispMapping(float2 texC0, float2 texC1)
{
	// Transform to texel space
	float2 texelpos = DMAP_SIZE * texC0;

		// Determine the lerp amounts.           
		float2 lerps = frac(texelpos);

	float dmap0[4];
	dmap0[0] = 0;  // WaveDispMap0.SampleLevel(TextureSampler2, texC0, 0).r;
	dmap0[1] = 0 ; //WaveDispMap0.SampleLevel(TextureSampler2, texC0 + float2(DMAP_DX, 0.0f), 0).r;
	dmap0[2] = 0 ; //WaveDispMap0.SampleLevel(TextureSampler, texC0 + float2(0.0f, DMAP_DX), 0).r;
	dmap0[3] = 0 ; //WaveDispMap0.SampleLevel(TextureSampler, texC0 + float2(DMAP_DX, DMAP_DX), 0).r;

	// Filter displacement map:
	float h0 = lerp( lerp( dmap0[0], dmap0[1], lerps.x ),
	lerp( dmap0[2], dmap0[3], lerps.x ),
	lerps.y );

	texelpos = DMAP_SIZE * texC1;
	lerps    = frac(texelpos);

	float dmap1[4];
	dmap1[0] = 0 ; //WaveDispMap1.SampleLevel(TextureSampler, texC1, 0).r;
	dmap1[1] = 0 ; //WaveDispMap1.SampleLevel(TextureSampler, texC1 + float2(DMAP_DX, 0.0f), 0).r;
	dmap1[2] = 0 ; //WaveDispMap1.SampleLevel(TextureSampler, texC1 + float2(0.0f, DMAP_DX), 0).r;
	dmap1[3] = 0 ; //WaveDispMap1.SampleLevel(TextureSampler, texC1 + float2(DMAP_DX, DMAP_DX), 0).r;

	// Filter diplacement map:
	float h1 = lerp( lerp( dmap1[0], dmap1[1], lerps.x ),
	lerp( dmap1[2], dmap1[3], lerps.x ),
	lerps.y );

	// Sum and scale the sampled heights.  
    return ScaleHeights.x*h0 + ScaleHeights.y*h1;
}

float2 sampleShadowMap(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return float2(1, 1);
	return ShadowMap.Sample(TextureSampler, UV).rg;
}


OceanWaterVertexOutput OceanWaterVS(
    float4 posLocal       : POSITION0, 
    float2 normalizedTexC : TEXCOORD0,
    float2 scaledTexC     : TEXCOORD1)
{
	float4x4 wvp = mul( World, mul( View, Projection ) );

    // Zero out our output.
	OceanWaterVertexOutput output = (OceanWaterVertexOutput)0;

	// Scroll vertex texture coordinates to animate waves.
	float2 vTex0 = normalizedTexC + WaveDMapOffset0;
	float2 vTex1 = normalizedTexC + WaveDMapOffset1;
	
	// Scroll texture coordinates.
	output.tex0 = scaledTexC + WaveNMapOffset0;

	float4 depthVector = mul( posLocal, wvp );      
	float blendDistance = 0.97f;
	float blendWidth = 0.03f;
	float lakeBlendFactor = clamp((depthVector.z/depthVector.w-blendDistance)/blendWidth, 0, 1);

	// Set y-coordinate of water grid vertices based on displacement mapping.
	posLocal.y = lerp(DoDispMapping(vTex0, vTex1), 0.75, lakeBlendFactor);

	float4x4 rwvp = mul( World, mul(ReflectedView, Projection));
    output.ReflectionMapPos =  mul(posLocal, rwvp);

	// Estimate TBN-basis using finite differencing in local space.  
	float r = DoDispMapping(vTex0 + float2(DMAP_DX, 0.0f), vTex1 + float2(0.0f, DMAP_DX));
	float b = DoDispMapping(vTex0 + float2(DMAP_DX, 0.0f), vTex1 + float2(0.0f, DMAP_DX));
	
	float3x3 TBN;                       
	TBN[0] = normalize(float3(1.0f, (r-posLocal.y)/GridStepSizeL.x, 0.0f)); 
	TBN[1] = normalize(float3(0.0f, (b-posLocal.y)/GridStepSizeL.y, -1.0f));
	TBN[2] = normalize(cross(TBN[0], TBN[1]));
	
	// Matrix transforms from object space to tangent space.
	float3x3 toTangentSpace = transpose(TBN);
	
	// Transform eye position to local space.
	float3 eyePosL = mul(float4(CameraPosition, 1.0f), WorldInv).xyz;
	
	// Transform to-eye vector to tangent space.
	float3 toEyeL = eyePosL - posLocal;
	output.toEyeT = mul(toEyeL, toTangentSpace);

	// Transform light direction to tangent space.
	float3 lightDirL = mul(float4(LightDirection, 0.0f), WorldInv).xyz;
	output.lightDirT  = mul(lightDirL, toTangentSpace);
	
	// Transform to homogeneous clip space.
	output.Position = mul(posLocal, wvp);
	
	output.ShadowScreenPosition = mul( mul(posLocal,World), ShadowViewProjection );

    return output;
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

float WaveHeight;
Texture2D BumpMap0;
Texture2D BumpMap1;

 
float4 WaterPS(OceanWaterVertexOutput input) : SV_Target
{
	float4 bumpColor = BumpMap0.Sample(TextureSampler, input.BumpSamplingPos0);
    float2 perturbation = WaveHeight*(bumpColor.rg - 0.5f)*0.8f; //dan: is too much *2.0f;

	float bumpMapDistribution = lerp( 0.5, 1, saturate(sqrt(abs(CameraPosition.y-3))/4) );
	bumpColor = bumpColor * bumpMapDistribution + BumpMap1.Sample(TextureSampler, input.BumpSamplingPos1)   * (1-bumpMapDistribution);
	    
    float2 ProjectedTexCoords;
    ProjectedTexCoords.x = input.ReflectionMapPos.x/input.ReflectionMapPos.w/2.0f + 0.5f;
    ProjectedTexCoords.y = -input.ReflectionMapPos.y/input.ReflectionMapPos.w/2.0f + 0.5f;        
    float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
    float4 reflectiveColor = ReflectedMap.Sample(TextureSampler, perturbatedTexCoords);
    
    float4 dullColor = float4(0.1f, 0.1f, 0.3f, 1.0f);
    float4 refractiveColor = float4(0.1,0.1,0.3,1); // tex2D(RefractionSampler, perturbatedRefrTexCoords);
    
    float3 eyeVector = normalize(CameraPosition - input.Position3D);
    float3 normalVector = (bumpColor.rbg-0.5f)*2.0f;
    
	float2 cameraPosXZ = float2(CameraPosition.x,CameraPosition.z);
	float2 worldPosXZ = float2(input.Position3D.x,input.Position3D.z);
	float fog = saturate((distance(cameraPosXZ,worldPosXZ) - FogStart) / FogRange);

    float fresnelTerm = dot(eyeVector, normalVector);    
    float4 combinedColor = lerp(reflectiveColor, refractiveColor, sqrt(fresnelTerm) * (1-fog) );
    float4 color = lerp(combinedColor, dullColor, 0.4);

    float3 reflectionVector = -reflect(LightDirection, normalVector);
    float specular = dot(normalize(reflectionVector), eyeVector);
    specular = pow(abs(specular), 256);  

	if (DoShadowMapping)
	{
		float realDepth = input.ShadowScreenPosition.z / input.ShadowScreenPosition.w - ShadowBias;
		if (realDepth < 1)
		{
			float2 screenPos = input.ShadowScreenPosition.xy / input.ShadowScreenPosition.w;
			float2 shadowTexCoord = 0.5f * (float2(screenPos.x, -screenPos.y) + 1);

			// Variance shadow mapping code below from the variance shadow
			// mapping demo code @ http://www.punkuser.net/vsm/

			// Sample from depth texture
			float2 moments = sampleShadowMap(shadowTexCoord+perturbation);

			// Check if we're in shadow
			float lit_factor = (realDepth <= moments.x);
    
			// Variance shadow mapping
			float E_x2 = moments.y;
			float Ex_2 = moments.x * moments.x;
			float variance = min(max(E_x2 - Ex_2, 0.0) + 1.0f / 10000.0f, 1.0);
			float m_d = (moments.x - realDepth);
			float p = variance / (variance + m_d * m_d);

			float shadowFactor = clamp(max(lit_factor, p), ShadowMult, 1.0f);
			specular *= pow( shadowFactor, 32 );  //no specular when in shadow
			color.rgb *= shadowFactor;
		}
	}

    color.rgb += specular;

//    float2 cameraPosXZ = float2(CameraPosition.x,CameraPosition.z);
//    float2 worldPosXZ = float2(input.Position3D.x,input.Position3D.z);
//	float fog = saturate((distance(cameraPosXZ,worldPosXZ) - FogStart) / FogRange);

//	color.r = fog;
//	color.g = fog;
//	color.b = fog;

    return color;
}

OceanWaterVertexOutput CombinedWaterVS(
   float4 posLocal       : POSITION,
   float2 normalizedTexC : TEXCOORD0,
   float2 scaledTexC     : TEXCOORD1)
{    
	OceanWaterVertexOutput oceanVS = OceanWaterVS(posLocal, normalizedTexC, scaledTexC);

	float2 bmsp = scaledTexC/8 + WaveNMapOffset0;
    oceanVS.BumpSamplingPos0 = float2(
	  (bmsp.x + LakeTextureTransformation.x) * LakeTextureTransformation.z,
	  (bmsp.y + LakeTextureTransformation.y) * LakeTextureTransformation.w);
	bmsp = scaledTexC/8 + WaveNMapOffset1;
    oceanVS.BumpSamplingPos1 = float2(
	  (bmsp.x + LakeTextureTransformation.x) * LakeTextureTransformation.z,
	  (bmsp.y + LakeTextureTransformation.y) * LakeTextureTransformation.w);

	oceanVS.Position3D = mul( posLocal, World );

	return oceanVS;
}

 technique DisplacedWaterTech
 {
     pass Pass0
     {
         VertexShader = compile vs_3_0 CombinedWaterVS();
         PixelShader = compile ps_3_0 WaterPS();
     }
 }

 //technique LakeWaterTech
 //{
 //    pass Pass0
 //    {
	//	 Shader = compile vs_3_0 CombinedWaterVS(); //vs_3_0 LakeWaterVS();
 //        PixelShader = compile ps_3_0 WaterPS();
 //    }
 //}
