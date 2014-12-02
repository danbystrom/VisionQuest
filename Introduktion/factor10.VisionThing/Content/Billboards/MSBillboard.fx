float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;
float3 LightDirection;

//------- Texture Samplers --------
Texture2D Texture;
SamplerState TextureSampler;

// Lighting parameters.
float3 LightColor = 0.8;
float3 AmbientColor = 0.4;

// Parameters controlling the wind effect.
float3 WindDirection = float3(1, 0, 0);
float WindWaveSize = 0.001;
float WindRandomness = 1;
float WindSpeed = 2;
float WindAmount = 0.05;
float WindTime;


// 1 means we should only accept opaque pixels.
// -1 means only accept transparent pixels.
float AlphaTestDirection = 1;
float AlphaTestThreshold = 0.95;


// Parameters describing the billboard itself.
float BillboardWidth = 10;
float BillboardHeight = 10;

struct VertexToPixel
{
    float4 Position : SV_Position;
    float2 TexCoord : TEXCOORD0;
    float4 Color : COLOR0;
	float4 WorldPosition: TEXCOORD1;
	float4 PositionCopy : TEXCOORD2;
};

VertexToPixel VSStandard(
  float4 inPosition : SV_Position,
  float3 inNormal: NORMAL0,
  float2 inTexCoord : TEXCOORD0,
  float2 inRandom: TEXCOORD1
  )
{
	VertexToPixel output;

	inPosition.x += inTexCoord.x;
	inPosition.y += inTexCoord.y;

	output.WorldPosition = inPosition;
    output.Position = output.PositionCopy = mul(mul(inPosition, View), Projection);

    output.TexCoord = inTexCoord;
    
    output.Color.rgb = float3(1,1,1);
    output.Color.a = 1;
    
    return output;
}

/*
VertexToPixel VSStandard(
  float4 inPosition : SV_Position,
  float3 inNormal: NORMAL0,
  float2 inTexCoord : TEXCOORD0,
  float2 inRandom: TEXCOORD1
  )
{
    // Apply a scaling factor to make some of the billboards
    // shorter and fatter while others are taller and thinner.
    float squishFactor = 0.75 + abs(inRandom) / 2;

    float width = BillboardWidth * squishFactor;
    float height = BillboardHeight / squishFactor;

    // Flip half of the billboards from left to right. This gives visual variety
    // even though we are actually just repeating the same texture over and over.
    //if (inRandom < 0)
    //    width = -width;
	//width = width * sign(inRandom.x);

    // Work out what direction we are viewing the billboard from.
    float3 viewDirection = View._m02_m12_m22;

    float3 rightVector = normalize(cross(viewDirection, inNormal));

    // Calculate the position of this billboard vertex.
    float3 position = inPosition;

    // Offset to the left or right.
    position += rightVector * (inTexCoord.x - 0.5) * width;
    
    // Offset upward if we are one of the top two vertices.
    position += inNormal * (1 - inTexCoord.y) * height;

    // Work out how this vertex should be affected by the wind effect.
    float waveOffset = dot(position, WindDirection) * WindWaveSize;
    
    waveOffset += inRandom.x * WindRandomness;
    
    // Wind makes things wave back and forth in a sine wave pattern.
    float wind = sin(WindTime * WindSpeed + waveOffset) * WindAmount;
    
    // But it should only affect the top two vertices of the billboard!
    wind *= (1 - inTexCoord.y);
    
    position += WindDirection * wind;

    // Apply the camera transform.
    float4 viewPosition = mul(float4(position, 1), View);

	VertexToPixel output;

	output.WorldPosition = inPosition;
    output.Position = output.PositionCopy = mul(viewPosition, Projection);

    output.TexCoord = inTexCoord;
    
    // Compute lighting.
    float diffuseLight = max(-dot(inNormal, LightDirection), 0);
    
    output.Color.rgb = diffuseLight * LightColor + AmbientColor;
    output.Color.a = 1;
    
    return output;
}
*/

float4 PSStandard(VertexToPixel input) : SV_Target
{
	float4 color = float4(1, 1, 1, 1); // input.Color * Texture.Sample(TextureSampler, input.TexCoord);
    //clip((color.a - AlphaTestThreshold) * AlphaTestDirection);
    return color;
}


float4 PSClipPlane(VertexToPixel input) : SV_Target
{
	clip(dot(input.WorldPosition, ClipPlane));
	float4 color = input.Color * Texture.Sample(TextureSampler, input.TexCoord);
	clip((color.a - AlphaTestThreshold) * AlphaTestDirection);
	return color;
}

float4 PSDepthMap(VertexToPixel input) : SV_Target
{
    float4 color = input.Color * Texture.Sample(TextureSampler, input.TexCoord);
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
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VSStandard()));
		SetPixelShader(CompileShader(ps_4_0, PSStandard()));
	}
}

technique TechClipPlane
{
	pass Pass0
    {
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VSStandard()));
		SetPixelShader(CompileShader(ps_4_0, PSClipPlane()));
	}
}

technique TechDepthMap
{
	pass Pass0
	{
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VSStandard()));
		SetPixelShader(CompileShader(ps_4_0, PSDepthMap()));
	}
}