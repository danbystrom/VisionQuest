float4x4 World;
float3x3 WorldInverseTranspose;
float4x4 View;
float4x4 Projection;
float3 CameraPosition = float3(0, 0, 7);;
float3 SunlightDirection = float3(-10, 0, 0);

float4 AmbientColor = float4(0.3, 0.3, 0.3, 1);
float4 LightColor = float4(0.8, 0.8, 0.8, 1);
float SpecularPower = 32;
float4 SpecularColor = float4(1, 1, 1, 1);

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
	float3 ViewDirection : TextCoord;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4x4 viewProjection = mul(View, Projection);

	output.Position = mul(worldPosition, viewProjection);
	output.Normal = mul(input.Normal, World);
	output.Color = input.Color;
	output.ViewDirection = worldPosition - CameraPosition;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
	// Start with ambient lighting
	float4 lighting = AmbientColor;

	float3 normal = normalize(input.Normal);
	float3 lightDir = -normalize(SunlightDirection);

	// Add lambertian lighting
	lighting += saturate(dot(lightDir, normal)) * LightColor;

	// Add specular highlights
	float3 refl = reflect(lightDir, normal);
	float3 view = normalize(input.ViewDirection);
	lighting += pow(saturate(dot(refl, view)), SpecularPower) * SpecularColor;

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
