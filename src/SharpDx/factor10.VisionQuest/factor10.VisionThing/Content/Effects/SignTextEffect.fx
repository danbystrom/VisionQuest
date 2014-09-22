float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;
float3 SunlightDirection = float3(-10, 20, 5);

Texture2D Texture;
SamplerState TextureSampler;

float3 RotateAxis = float3(0,-1,0);

bool DoShadowMapping = true;
float4x4 ShadowViewProjection;
float ShadowMult = 0.3f;
float ShadowBias = 0.001f;
texture2D ShadowMap;
sampler2D shadowSampler = sampler_state {
	texture = <ShadowMap>;
	minfilter = point;
	magfilter = point;
	mipfilter = point;
};

float4 DiffuseColor = float4(240, 70, 20, 1);

struct VertexShaderInput
{
	float4 Position : SV_Position;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
 	float4 Position : SV_Position;
	float2 UV : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    /*
	float3 viewDirection = normalize(cross(RotateAxis, normalize(input.Position - CameraPosition)));
	float4x4 constrainedBillboard = 
	{
		float4(viewDirection,0),
		float4(RotateAxis,0),
		float4(normalize(cross(viewDirection, RotateAxis)),0),
		float4(input.Position.xyz,1)
	};
	*/
	float4 worldPosition = mul(input.Position, World);
    float4x4 viewProjection = mul(View, Projection);
    
    output.WorldPosition = worldPosition;
    output.Position = mul(worldPosition, viewProjection);

	output.UV = input.UV;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
	return Texture.Sample(TextureSampler, input.UV) * 0.8; // * DiffuseColor;
}


float4 PixelShaderFunctionClipPlane(VertexShaderOutput input) : SV_Target
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	return PixelShaderFunction(input);
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
