float4x4 View;
float4x4 Projection;
float4x4 World;
float3 CameraPosition;
float4 ClipPlane;
float3 SunlightDirection = float3(0.7, 0.7, 0.7);
SamplerState TextureSampler;

float4 TexOffsetAndScale = float4(0,0,1,1);

bool DoShadowMapping = true;
float ShadowMult = 0.3f;
float ShadowBias = 0.001f;
float4x4 ShadowViewProjection;
texture2D ShadowMap;

float Ambient = 0.2;

//------- Texture Samplers --------

Texture2D TextureA;
Texture2D TextureB;
Texture2D TextureC;
Texture2D TextureD;
Texture2D TextureE;
Texture2D TextureF;
Texture2D TextureG;
Texture2D TextureH;

Texture2D HeightsMap;
Texture2D NormalsMap;
Texture2D WeightsMap1;
Texture2D WeightsMap2;

struct MTVertexToPixel
{
	float4 Position         : SV_Position;
    float3 WorldPosition    : TEXCOORD0;
    float2 TextureCoords    : TEXCOORD1;
	float  Depth            : TEXCOORD2;
    float4 PositionCopy     : TEXCOORD3;
    float4 ShadowScreenPosition : TEXCOORD4;
};

float DispMap(float2 tex)
{
	return HeightsMap.SampleLevel(TextureSampler, tex, 0).r;
}

MTVertexToPixel MultiTexturedVS(float4 inPos : SV_Position, float2 inTexCoords: TEXCOORD0)
{
    MTVertexToPixel output;

    float4 worldPosition = mul(inPos, World);
    float4x4 viewProjection = mul(View, Projection);

    output.TextureCoords = float2(
	  TexOffsetAndScale.x + inTexCoords.x * TexOffsetAndScale.z,
	  TexOffsetAndScale.y + inTexCoords.y * TexOffsetAndScale.w );
    
	worldPosition.y += DispMap(output.TextureCoords); // HeightsMap.SampleLevel(mySampler, output.TextureCoords, 0).r;

    output.Position = output.PositionCopy = mul(worldPosition, viewProjection);
    output.WorldPosition = worldPosition;
    
	output.Depth = output.Position.z / output.Position.w;

	output.ShadowScreenPosition = mul(worldPosition, ShadowViewProjection);

    return output;    
}


float2 sampleShadowMap(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return float2(1, 1);

	return ShadowMap.Sample(TextureSampler, UV).rg;
}


float4 MultiTexturedPS(MTVertexToPixel input) : SV_Target
{
    float4 Outpu;        
    
	float3 normal = HeightsMap.SampleLevel(TextureSampler, input.TextureCoords, 0).xyz - float3(0.5, 0.5, 0.5);
    float lightingFactor = saturate(Ambient+dot(normal, -SunlightDirection));

    float blendDistance = 0.99f;
	float blendWidth = 0.005f;
	float blendFactor = clamp((input.Depth-blendDistance)/blendWidth, 0, 1);

	float4 textureWeights1 = WeightsMap1.Sample(TextureSampler, input.TextureCoords);
	float4 textureWeights2 = WeightsMap2.Sample(TextureSampler, input.TextureCoords);

	float4 farColor;
	float2 farTextureCoords = input.TextureCoords*3;
	farColor  = TextureA.Sample(TextureSampler, farTextureCoords)*textureWeights1.x;
	farColor += TextureB.Sample(TextureSampler, farTextureCoords)*textureWeights1.y;
	farColor += TextureC.Sample(TextureSampler, farTextureCoords)*textureWeights1.z;
	farColor += TextureD.Sample(TextureSampler, farTextureCoords)*textureWeights1.w;
	farTextureCoords *= 40;
	farColor += TextureE.Sample(TextureSampler, farTextureCoords)*textureWeights2.x;
	farColor += TextureF.Sample(TextureSampler, farTextureCoords)*textureWeights2.y;
	farColor += TextureG.Sample(TextureSampler, farTextureCoords)*textureWeights2.z;
	farColor += TextureH.Sample(TextureSampler, farTextureCoords)*textureWeights2.w;
    
	float4 nearColor;
	float2 nearTextureCoords = input.TextureCoords*9;
	nearColor = TextureA.Sample(TextureSampler, nearTextureCoords)*textureWeights1.x;
	nearColor += TextureB.Sample(TextureSampler, nearTextureCoords)*textureWeights1.y;
	nearColor += TextureC.Sample(TextureSampler, nearTextureCoords)*textureWeights1.z;
	nearColor += TextureD.Sample(TextureSampler, nearTextureCoords)*textureWeights1.w;
	nearTextureCoords = farTextureCoords;
	nearColor += TextureE.Sample(TextureSampler, nearTextureCoords)*textureWeights2.x;
	nearColor += TextureF.Sample(TextureSampler, nearTextureCoords)*textureWeights2.y;
	nearColor += TextureG.Sample(TextureSampler, nearTextureCoords)*textureWeights2.z;
	nearColor += TextureH.Sample(TextureSampler, nearTextureCoords)*textureWeights2.w;

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
			float2 moments = sampleShadowMap(shadowTexCoord);

			// Check if we're in shadow
			float lit_factor = (realDepth <= moments.x);
    
			// Variance shadow mapping
			float E_x2 = moments.y;
			float Ex_2 = moments.x * moments.x;
			float variance = min(max(E_x2 - Ex_2, 0.0) + 1.0f / 10000.0f, 1.0);
			float m_d = (moments.x - realDepth);
			float p = variance / (variance + m_d * m_d);

			lightingFactor *= clamp(max(lit_factor, p), ShadowMult, 1.0f);
		}
	}

	return lerp(nearColor, farColor, blendFactor) * lightingFactor;
}

float4 MultiTexturedPSClipPlane(MTVertexToPixel input) : SV_Target
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	return MultiTexturedPS(input);
}

float4 MultiTexturedPSDepth(MTVertexToPixel input) : SV_Target
{
	// Determine the depth of this vertex / by the far plane distance,
	// limited to [0, 1]
    float depth = clamp(input.PositionCopy.z / input.ShadowScreenPosition.w, 0, 1);
    
	// Return only the depth value
    return float4(depth, depth * depth, 0, 1);
}

technique TechStandard
{
	pass Pass0
    {   
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, MultiTexturedVS()));
		SetPixelShader(CompileShader(ps_4_0, MultiTexturedPS()));
	}
}

technique TechClipPlane
{
	pass Pass0
    {   
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, MultiTexturedVS()));
		SetPixelShader(CompileShader(ps_4_0, MultiTexturedPSClipPlane()));
	}
}

technique TechDepthMap
{
	pass Pass0
    {   
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, MultiTexturedVS()));
		SetPixelShader(CompileShader(ps_4_0, MultiTexturedPSDepth()));
	}
}

