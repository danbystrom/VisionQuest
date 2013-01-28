float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;
float Time;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
	float A : TEXCOORD00;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
    float4 PositionCopy : TEXCOORD00;
	float3 WorldPosition : TEXCOORD1;
	float A : TEXCOORD02;
	float D : TEXCOORD03;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float4 worldPosition = mul(input.Position, World);
    float4x4 viewProjection = mul(View, Projection);
    
    output.WorldPosition = worldPosition;
    output.Position = output.PositionCopy = mul(worldPosition, viewProjection);

	output.Color = input.Color;

	output.A = input.A + Time;
	output.D = sqrt(distance(worldPosition,CameraPosition));

    return output;
}


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float c = 0.5 + sin(input.A)/ input.D;
    return input.Color + sin(input.A)/ input.D;
}


float4 PixelShaderFunctionClipPlane(VertexShaderOutput input) : COLOR0
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	float c = 0.5 + sin(input.A)/ input.D;
    return float4(c,c,c,1);
}


float4 PixelShaderFunctionDepthMap(VertexShaderOutput input) : COLOR0
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
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique TechClipPlane
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunctionClipPlane();
    }
}

technique TechDepthMap
{
	pass Pass0
    {   
    	VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader  = compile ps_2_0 PixelShaderFunctionDepthMap();
    }
}
