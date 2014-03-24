float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;

texture CubeMap;

samplerCUBE CubeMapSampler = sampler_state {
	texture = <CubeMap>;
	minfilter = anisotropic;
	magfilter = anisotropic;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float3 WorldPosition : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);

	output.WorldPosition = worldPosition;
	output.Position = mul(worldPosition, mul(View, Projection)).xyww;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 viewDirection = normalize(input.WorldPosition - CameraPosition);

	if ( viewDirection.y < 0.001 )
	{
		float3 normalVector = float3(0,1,0);
		float3 LightDirection = float3(11, -2, -6);
	    float3 reflectionVector = -reflect(LightDirection, normalVector);
		float specular = dot(normalize(reflectionVector), viewDirection);
		specular = pow(abs(specular), 256);  

		viewDirection.y = abs(viewDirection.y);
		float4 dullColor = float4(0.1, 0.1, 0.3, 1.0f);
		return lerp( texCUBE(CubeMapSampler, viewDirection), dullColor, 0.4 ) + specular;
	}

	viewDirection.y = abs(viewDirection.y);
	return texCUBE(CubeMapSampler, viewDirection);

}

float4 PixelShaderFunctionClipPlane(VertexShaderOutput input) : COLOR0
{
	clip(dot(float4(input.WorldPosition, 1), ClipPlane));
	return PixelShaderFunction(input);
}

technique TechStandard
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();

        CullMode = None;
		ZFunc = Always;
		StencilEnable = true;
		StencilFunc = Always;
		StencilPass = Replace;
		StencilRef = 0;
    }
}

technique TechClipPlane
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunctionClipPlane();

        CullMode = None;
		ZFunc = Always;
		StencilEnable = true;
		StencilFunc = Always;
		StencilPass = Replace;
		StencilRef = 0;
    }
}
