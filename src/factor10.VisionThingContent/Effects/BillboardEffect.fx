//------- XNA interface --------
float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;

float ShadowFarPlane = 100;

float3 xAllowedRotDir;

//------- Texture Samplers --------
Texture Texture;
sampler textureSampler = sampler_state { texture = <Texture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = CLAMP; AddressV = CLAMP;};

struct VertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord	: TEXCOORD0;
	float4 WorldPosition: TEXCOORD1;
	float4 PositionCopy : TEXCOORD2;
};


VertexToPixel VSStandard(float4 inPos: POSITION0, float2 inTexCoord: TEXCOORD0)
{
	VertexToPixel output = (VertexToPixel)0;	

	float4 center = mul(inPos, World);
	float3 eyeVector = center - CameraPosition;	
	
	float3 upVector = xAllowedRotDir;
	upVector = normalize(upVector);
	float3 sideVector = cross(eyeVector,upVector);
	sideVector = normalize(sideVector);
	
	float3 finalPosition = center;
	finalPosition += (inTexCoord.x-0.5f)*sideVector;
	finalPosition += (1.5f-inTexCoord.y*1.5f)*upVector;	
	
	output.WorldPosition = float4(finalPosition, 1);
		
	float4x4 preViewProjection = mul (View, Projection);
	output.Position = output.PositionCopy = mul(output.WorldPosition, preViewProjection);
	
	output.TexCoord = inTexCoord;
	
	return output;
}

float4 PSStandard(VertexToPixel input) : COLOR0
{
	float4 color = tex2D(textureSampler, input.TexCoord);
	clip(color.a - 0.7843f);
	return color;
}

float4 PSClipPlane(VertexToPixel input) : COLOR0
{
	clip(dot(input.WorldPosition, ClipPlane));
	float4 color = tex2D(textureSampler, input.TexCoord);
	clip(color.a - 0.7843f);
	return color;
}

float4 PSDepthMap(VertexToPixel input) : COLOR0
{
	float4 color = tex2D(textureSampler, input.TexCoord);
	clip(color.a - 0.7843f);

	// Determine the depth of this vertex / by the far plane distance, limited to [0, 1]
    float depth = clamp(input.PositionCopy.z / ShadowFarPlane, 0, 1);
    depth = 0;  //testar lite här...

	// Return only the depth value
    return float4(depth, depth * depth, 0, 1);
}

technique TechStandard
{
	pass Pass0
    {
    	VertexShader = compile vs_2_0 VSStandard();
        PixelShader  = compile ps_2_0 PSStandard();        
    }
}

technique TechdClipPlane
{
	pass Pass0
    {
    	VertexShader = compile vs_2_0 VSStandard();
        PixelShader  = compile ps_2_0 PSClipPlane();        
    }
}

technique TechDepthMap
{
	pass Pass0
    {
    	VertexShader = compile vs_2_0 VSStandard();
        PixelShader  = compile ps_2_0 PSDepthMap();        
    }
}