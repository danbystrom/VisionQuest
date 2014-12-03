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
float3 AmbientColor = float3(0.1, 0.1, 0.1);
float3 LightColor = float3(0.9, 0.9, 0.9);
float SpecularPower = 16;
float3 SpecularColor = float3(0.3, 0.3, 0.3);

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
	float3 NormalW : NORMAL;
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
	output.NormalW = mul(input.Normal, (float3x3)WorldInverseTranspose);

	return output;
}

void ComputeDirectionalLight(
	float3 normal,
	float3 toEye,
	out float4 ambient,
	out float4 diffuse,
	out float4 spec)
{
	// Initialize outputs.
	ambient = float4(0.0f, 0.0f, 0.0f, 0.0f);
	diffuse = float4(0.0f, 0.0f, 0.0f, 0.0f);
	spec = float4(0.0f, 0.0f, 0.0f, 0.0f);

	// The light vector aims opposite the direction the light rays travel.
	float3 lightVec = SunlightDirection;

	// Add ambient term.
	ambient = float4(AmbientColor,1);

	// Add diffuse and specular term, provided the surface is in 
	// the line of site of the light.

	float diffuseFactor = dot(lightVec, normal);

	// Flatten to avoid dynamic branching.
	[flatten]
	if (diffuseFactor > 0.0f)
	{
		float3 v = reflect(-lightVec, normal);
		float specFactor = pow(max(dot(v, toEye), 0.0f), SpecularPower);

		diffuse = diffuseFactor * DiffuseColor;
		spec = specFactor * float4(SpecularColor,1);
	}
}


float4 PixelShaderFunction(VertexShaderOutput pin) : SV_Target
{
	// Interpolating normal can unnormalize it, so normalize it.
	pin.NormalW = normalize(pin.NormalW);

	float3 toEyeW = normalize(CameraPosition - pin.Position);

	// Start with a sum of zero. 
	float4 ambient = float4(0.0f, 0.0f, 0.0f, 0.0f);
	float4 diffuse = float4(0.0f, 0.0f, 0.0f, 0.0f);
	float4 spec = float4(0.0f, 0.0f, 0.0f, 0.0f);

	// Sum the light contribution from each light source.
	float4 A, D, S;

	ComputeDirectionalLight(pin.NormalW, toEyeW, A, D, S);
	ambient += A;
	diffuse += D;
	spec += S;

	float4 litColor = Texture.Sample(TextureSampler, pin.UV)*(ambient + diffuse + spec);

	// Common to take alpha from diffuse material.
	litColor.a = DiffuseColor.a;

	return litColor;
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
