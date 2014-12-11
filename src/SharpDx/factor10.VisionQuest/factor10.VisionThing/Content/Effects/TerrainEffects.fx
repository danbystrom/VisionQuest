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

Texture2D Texture0;
Texture2D Texture1;
Texture2D Texture2;
Texture2D Texture3;
Texture2D Texture4;
Texture2D Texture5;
Texture2D Texture6;
Texture2D Texture7;
Texture2D Texture8;

Texture2D HeightsMap;
Texture2D NormalsMap;
Texture2D WeightsMap;

struct MTVertexToPixel
{
	float4 Position         : SV_Position;
    float3 WorldPosition    : TEXCOORD0;
    float2 TextureCoords    : TEXCOORD1;
	float  Depth            : TEXCOORD2;
    float4 PositionCopy     : TEXCOORD3;
    float4 ShadowScreenPosition : TEXCOORD4;
};


MTVertexToPixel MultiTexturedVS(float4 inPos : SV_Position, float2 inTexCoords: TEXCOORD0)
{
    MTVertexToPixel output;

	output.TextureCoords = float2(
		TexOffsetAndScale.x + inTexCoords.x * TexOffsetAndScale.z,
		TexOffsetAndScale.y + inTexCoords.y * TexOffsetAndScale.w);

	inPos.y += HeightsMap.SampleLevel(TextureSampler, output.TextureCoords, 0).r;
	
	float4 worldPosition = mul(inPos, World);
    float4x4 viewProjection = mul(View, Projection);
    
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

	float3 normal = NormalsMap.SampleLevel(TextureSampler, input.TextureCoords, 0).xyz - float3(0.5, 0.5, 0.5);
	float lightingFactor = saturate(Ambient + dot(normal, SunlightDirection));

	float blendDistance = 0.99f;
	float blendWidth = 0.005f;
	float blendFactor = saturate((input.Depth - blendDistance) / blendWidth);

	float4 textureWeights = WeightsMap.Sample(TextureSampler, input.TextureCoords);
	float weight1 = saturate(textureWeights.x - 0.5) * 2;
	float weight2 = saturate(textureWeights.y - 0.5) * 2;
	float weight3 = saturate(textureWeights.z - 0.5) * 2;
	float weight4 = saturate(textureWeights.w - 0.5) * 2;
	float weight5 = saturate(0.5 - textureWeights.x) * 2;
	float weight6 = saturate(0.5 - textureWeights.y) * 2;
	float weight7 = saturate(0.5 - textureWeights.z) * 2;
	float weight8 = saturate(0.5 - textureWeights.w) * 2;
	float weight0 = saturate(1 - weight1 - weight2 - weight3 - weight4 - weight5 - weight6 - weight7 - weight8);

	float4 farColor;
	float2 farTextureCoords = input.TextureCoords * 3;
	farColor = Texture0.Sample(TextureSampler, farTextureCoords)*weight0;
	farColor += Texture1.Sample(TextureSampler, farTextureCoords)*weight1;
	farColor += Texture2.Sample(TextureSampler, farTextureCoords)*weight2;
	farColor += Texture3.Sample(TextureSampler, farTextureCoords)*weight3;
	farColor += Texture4.Sample(TextureSampler, farTextureCoords)*weight4;
	farTextureCoords *= 40;
	farColor += Texture5.Sample(TextureSampler, farTextureCoords)*weight5;
	farColor += Texture6.Sample(TextureSampler, farTextureCoords)*weight6;
	farColor += Texture7.Sample(TextureSampler, farTextureCoords)*weight7;
	farColor += Texture8.Sample(TextureSampler, farTextureCoords)*weight8;

	float4 nearColor;
	float2 nearTextureCoords = input.TextureCoords * 9;
	nearColor = Texture0.Sample(TextureSampler, nearTextureCoords)*weight0;
	nearColor += Texture1.Sample(TextureSampler, nearTextureCoords)*weight1;
	nearColor += Texture2.Sample(TextureSampler, nearTextureCoords)*weight2;
	nearColor += Texture3.Sample(TextureSampler, nearTextureCoords)*weight3;
	nearColor += Texture4.Sample(TextureSampler, nearTextureCoords)*weight4;
	nearTextureCoords = farTextureCoords;
	nearColor += Texture5.Sample(TextureSampler, nearTextureCoords)*weight5;
	nearColor += Texture6.Sample(TextureSampler, nearTextureCoords)*weight6;
	nearColor += Texture7.Sample(TextureSampler, nearTextureCoords)*weight7;
	nearColor += Texture8.Sample(TextureSampler, nearTextureCoords)*weight8;

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

	float4 color = lerp(nearColor, farColor, blendFactor) * lightingFactor;
	color.w = 1;
	return color;
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

