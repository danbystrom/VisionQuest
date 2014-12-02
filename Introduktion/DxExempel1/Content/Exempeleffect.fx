float4x4 World;
float3x3 WorldInverseTranspose;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float3 SunlightDirection = float3(-10, 20, 5);
SamplerState TextureSampler;
Texture2D Texture;
Texture2D BumpMap;

float4 DiffuseColor = float4(1, 1, 1, 1);
float3 AmbientColor = float3(0.2, 0.2, 0.2);
float3 LightColor = float3(0.9, 0.9, 0.9);
float SpecularPower = 128;
float3 SpecularColor = float3(1, 1, 1);

struct VertexShaderInput
{
	float4 Position : SV_Position;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
	float3 Tangent : TANGENT0;
};

struct VertexShaderOutput
{
	float4 Position : SV_Position;
	float3 Normal : NORMAL;
	float3 Tangent : TANGENT;
	float2 UV : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4x4 viewProjection = mul(View, Projection);

	output.WorldPosition = (float3)worldPosition;
	output.Position = mul(worldPosition, viewProjection);

	output.UV = input.UV;
	output.Normal = mul(input.Normal, WorldInverseTranspose);
	output.Tangent = mul(input.Tangent, (float3x3)World);

	return output;
}

//---------------------------------------------------------------------------------------
// Transforms a normal map sample to world space.
//---------------------------------------------------------------------------------------
float3 NormalSampleToWorldSpace(float3 normalMapSample, float3 unitNormalW, float3 tangentW)
{
	// Uncompress each component from [0,1] to [-1,1].
	float3 normalT = 2.0f*normalMapSample - 1.0f;

	// Build orthonormal basis.
	float3 N = unitNormalW;
	float3 T = normalize(tangentW - dot(tangentW, N)*N);
	float3 B = cross(N, T);

	float3x3 TBN = float3x3(T, B, N);

	// Transform from tangent space to world space.
	float3 bumpedNormalW = mul(normalT, TBN);

	return bumpedNormalW;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
	// Start with diffuse color
	float3 txColor = Texture.Sample(TextureSampler, input.UV);
	float3 color = DiffuseColor * txColor;

	// Start with ambient lighting
	float3 lighting = AmbientColor;

	float3 normal = NormalSampleToWorldSpace(
		BumpMap.Sample(TextureSampler, input.UV),
		normalize(input.Normal),
		input.Tangent);

	// Add lambertian lighting
	lighting += saturate(dot(-SunlightDirection, normal)) * LightColor;

	float3 refl = reflect(-SunlightDirection, normal);
	float3 toEyeW = normalize(CameraPosition - input.WorldPosition);

		// Add specular highlights
	float q = dot(refl, toEyeW);
	lighting += pow(saturate(q), SpecularPower) * SpecularColor;

	// Calculate final color
	return float4(saturate(lighting) * color * DiffuseColor.a, DiffuseColor.a);
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
