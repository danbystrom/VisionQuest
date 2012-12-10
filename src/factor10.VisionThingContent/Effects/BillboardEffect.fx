//------- XNA interface --------
float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;

// Parameters controlling the wind effect.
float3 WindDirection = float3(0, 0, 1);
float WindWaveSize = 0.1;
float WindRandomness = 2;
float WindSpeed = 0.8;
float WindAmount = 0.15;
float WindTime;

// 1 means we should only accept opaque pixels.
// -1 means only accept transparent pixels.
float AlphaTestDirection = 1;
float AlphaTestThreshold = 0.95;

// Parameters describing the billboard itself.
float BillboardWidth = 2;
float BillboardHeight = 2;

float3 AllowedRotDir;

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
	
	float3 upVector = AllowedRotDir;
	//upVector = normalize(upVector);
	float3 sideVector = cross(eyeVector,upVector);
	sideVector = normalize(sideVector);
	
	float3 finalPosition = center;
	finalPosition += (inTexCoord.x-0.5f)*sideVector*BillboardWidth;
	finalPosition += (1.5f-inTexCoord.y*1.5f)*upVector*BillboardHeight;	
	
	// Work out how this vertex should be affected by the wind effect.
    float waveOffset = dot(finalPosition, WindDirection) * WindWaveSize;
    
    //waveOffset += input.Random * WindRandomness;
    waveOffset += (frac(inPos.x) + frac(inPos.y)) * WindRandomness;
    
    // Wind makes things wave back and forth in a sine wave pattern.
    float wind = sin(WindTime * WindSpeed + waveOffset) * WindAmount;
    
    // But it should only affect the top two vertices of the billboard!
    wind *= (1 - inTexCoord.y);
    
	output.WorldPosition = float4(finalPosition + WindDirection*wind, 1);
	
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
    clip((color.a - AlphaTestThreshold) * AlphaTestDirection);
	return color;
}

float4 PSDepthMap(VertexToPixel input) : COLOR0
{
	float4 color = tex2D(textureSampler, input.TexCoord);
    clip((color.a - AlphaTestThreshold) * AlphaTestDirection);

	// Determine the depth of this vertex / by the far plane distance, limited to [0, 1]
    float depth = clamp(input.PositionCopy.z / input.PositionCopy.w, 0, 1);

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

technique TechClipPlane
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
