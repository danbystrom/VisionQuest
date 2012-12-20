float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;
float3 LightingDirection = float3(-3, 1, 0);

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 PositionCopy : TEXCOORD00;
	float3 ViewDirection : TEXCOORD1;
	float3 WorldPosition : TEXCOORD2;
	float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float4 worldPosition = mul(input.Position, World);
    float4x4 viewProjection = mul(View, Projection);
    
    output.WorldPosition = worldPosition;
    output.Position = output.PositionCopy = mul(worldPosition, viewProjection);

	output.ViewDirection = worldPosition - CameraPosition;
	output.Color = input.Color;

    return output;
}


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return input.Color;
}


float4 PixelShaderFunctionClipPlane(VertexShaderOutput input) : COLOR0
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
    return input.Color;
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
