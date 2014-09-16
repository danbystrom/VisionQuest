float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;
float3 LightingDirection = float3(-3, 1, 0);

float4 DiffuseColor = float4(1, 1, 1, 0.1);
float4 AmbientColor = float4(0.2, 0.2, 0.2, 0.5);
float4 LightColor = float4(0.8, 0.8, 0.8, 0.5);
float SpecularPower = 32;
float4 SpecularColor = float4(1, 1, 1, 1);

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float3 Normal : TEXCOORD0;
	float3 ViewDirection : TEXCOORD1;
	float3 WorldPosition : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float4 worldPosition = mul(input.Position, World);
    float4x4 viewProjection = mul(View, Projection);
    
    output.WorldPosition = worldPosition;
    output.Position = mul(worldPosition, viewProjection);

	output.Normal = mul(input.Normal, World);

	output.ViewDirection = worldPosition - CameraPosition;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Start with diffuse color
	float4 color = DiffuseColor;

	// Start with ambient lighting
	float4 lighting = AmbientColor;

	float3 normal = normalize(input.Normal);

	// Add lambertian lighting
	lighting += saturate(dot(LightingDirection, normal)) * LightColor;

	float3 refl = reflect(LightingDirection, normal);
	float3 view = normalize(input.ViewDirection);

	// Add specular highlights
	lighting += pow(saturate(dot(refl, view)), SpecularPower) * SpecularColor;

	// Calculate final color
	return saturate(lighting) * color;
}


float4 PixelShaderFunctionClipPlane(VertexShaderOutput input) : COLOR0
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	return PixelShaderFunction(input);
}


technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique Technique2
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunctionClipPlane();
    }
}
