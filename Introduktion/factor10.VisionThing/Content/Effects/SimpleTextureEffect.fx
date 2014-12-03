float4x4 World;
float3x3 WorldInverseTranspose;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;
float3 SunlightDirection = float3(-10, 20, 5);
SamplerState TextureSampler;
Texture2D Texture;

bool DoShadowMapping = false;
float4x4 ShadowViewProjection;
float ShadowMult = 0.3f;
float ShadowBias = 0.001f;
Texture2D ShadowMap;

float4 DiffuseColor = float4(1, 1, 1, 1);
float3 AmbientColor = float3(0.1, 0.1, 0.1);
float3 LightColor = float3(0.9, 0.9, 0.9);
float SpecularPower = 128;
float3 SpecularColor = float3(1, 1, 1);

struct VertexShaderInput
{
	float4 Position : SV_Position;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_Position;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float3 WorldPosition : TEXCOORD2;
	float4 ShadowScreenPosition : TEXCOORD3;
	float4 PositionCopy  : TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
    
    float4 worldPosition = mul(input.Position, World);
    float4x4 viewProjection = mul(View, Projection);
    
    output.WorldPosition = worldPosition;
    output.Position = output.PositionCopy = mul(worldPosition, viewProjection);

	output.UV = input.UV;
	output.Normal = mul(input.Normal, (float3x3)WorldInverseTranspose);
	output.ShadowScreenPosition = mul(worldPosition, ShadowViewProjection);

    return output;
}

float2 sampleShadowMap(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return float2(1, 1);
	return ShadowMap.Sample(TextureSampler, UV).rg;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
	// Start with diffuse color
	float3 txColor = Texture.Sample(TextureSampler, input.UV);
	float3 color = DiffuseColor * txColor;

	// Start with ambient lighting
	float3 lighting = AmbientColor;

	float3 normal = normalize(input.Normal);

	// Add lambertian lighting
	lighting += saturate(dot(SunlightDirection, normal)) * LightColor;

	float3 refl = reflect(-SunlightDirection, normal);
	float3 toEyeW = normalize(CameraPosition - input.WorldPosition);

	// Add specular highlights
	lighting += pow(saturate(dot(refl, toEyeW)), SpecularPower) * SpecularColor;

	if (DoShadowMapping)
	{
		float realDepth = input.ShadowScreenPosition.z / input.ShadowScreenPosition.w - ShadowBias;

		if (realDepth < 1)
		{
			// Sample from depth texture
			float2 screenPos = input.ShadowScreenPosition.xy / input.ShadowScreenPosition.w;
			float2 shadowTexCoord = 0.5f * (float2(screenPos.x, -screenPos.y) + 1);

			float2 moments = sampleShadowMap(shadowTexCoord);

			// Check if we're in shadow
			float lit_factor = (realDepth <= moments.x);
    
			// Variance shadow mapping
			float E_x2 = moments.y;
			float Ex_2 = moments.x * moments.x;
			float variance = min(max(E_x2 - Ex_2, 0.0) + 1.0f / 10000.0f, 1.0);
			float m_d = (moments.x - realDepth);
			float p = variance / (variance + m_d * m_d);

			lighting *= clamp(max(lit_factor, p), ShadowMult, 1.0f);
		}
	}

	// Calculate final color
	return float4(saturate(lighting) * color * DiffuseColor.a, DiffuseColor.a);
}


float4 PixelShaderFunctionClipPlane(VertexShaderOutput input) : SV_Target
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	return PixelShaderFunction(input);
}


float4 PixelShaderFunctionDepthMap(VertexShaderOutput input) : SV_Target
{
	// Determine the depth of this vertex / by the far plane distance,
	// limited to [0, 1]
    float depth = clamp(input.PositionCopy.z / input.PositionCopy.w, 0, 1);
    
	// Return only the depth value
    return float4(depth, depth * depth, 0, 1);
}


technique TechStandard
{
    pass Pass1
    {
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VertexShaderFunction()));
		SetPixelShader(CompileShader(ps_4_0, PixelShaderFunction()));
    }
}

technique TechClipPlane
{
    pass Pass1
    {
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VertexShaderFunction()));
		SetPixelShader(CompileShader(ps_4_0, PixelShaderFunctionClipPlane()));
    }
}

technique TechDepthMap
{
    pass Pass1
    {
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VertexShaderFunction()));
		SetPixelShader(CompileShader(ps_4_0, PixelShaderFunctionDepthMap()));
    }
}