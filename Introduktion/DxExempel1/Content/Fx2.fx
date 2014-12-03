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
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_Position;
	float4 Color : Color;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4x4 viewProjection = mul(View, Projection);

	output.Position = mul(worldPosition, viewProjection);
	output.Color = input.Color;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
	return input.Color;
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
