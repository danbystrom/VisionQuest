float4x4 World;
float4x4 View;
float4x4 Projection;
float3 SunlightDirection = float3(-10, 0, 0);

float4 AmbientColor = float4(0.2, 0.2, 0.2, 1);
float4 LightColor = float4(0.8, 0.8, 0.8, 1);

struct VertexShaderInput
{
	float4 Position : SV_Position;
	float3 Normal : NORMAL0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_Position;
	float3 Normal : Normal;
	float4 Color : Color;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4x4 viewProjection = mul(View, Projection);

	output.Position = mul(worldPosition, viewProjection);
	output.Normal = mul(input.Normal, World);
	output.Color = input.Color;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
	// Start with ambient lighting
	float4 lighting = AmbientColor;

	float3 normal = normalize(input.Normal);
	float3 lightDir = normalize(SunlightDirection);

	// Add lambertian lighting
	lighting += saturate(dot(-lightDir, normal)) * LightColor;

	// Calculate final color
	return saturate(lighting) * input.Color;
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
