float4x4 View;
float4x4 Projection;
float4x4 World;
float3 CameraPosition;
float4 ClipPlane;
float3 LightingDirection = float3(0.7,0.7,0.7);
float4 TexOffsetAndScale = float4(0,0,1,1);

bool DoShadowMapping = true;
float ShadowMult = 0.3f;
float ShadowBias = 0.001f;
float4x4 ShadowViewProjection;
texture2D ShadowMap;
sampler2D shadowSampler = sampler_state {
	texture = <ShadowMap>;
	minfilter = point;
	magfilter = point;
	mipfilter = point;
};

float Ambient = 0.2;

//------- Texture Samplers --------

Texture TextureA;
Texture TextureB;
Texture TextureC;
Texture TextureD;
Texture TextureE;
Texture TextureF;
Texture TextureG;
Texture TextureH;

Texture HeightsMap;
Texture NormalsMap;
Texture WeightsMap1;
Texture WeightsMap2;

sampler TextureSamplerA = sampler_state { texture = <TextureA> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSamplerB = sampler_state { texture = <TextureB> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSamplerC = sampler_state { texture = <TextureC> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSamplerD = sampler_state { texture = <TextureD> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSamplerE = sampler_state { texture = <TextureE> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSamplerF = sampler_state { texture = <TextureF> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSamplerG = sampler_state { texture = <TextureG> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSamplerH = sampler_state { texture = <TextureH> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

sampler HeightsSampler = sampler_state { texture = <HeightsMap> ; magfilter = POINT; minfilter = POINT; mipfilter=POINT; AddressU = mirror; AddressV = mirror;};
sampler NormalsSampler = sampler_state { texture = <NormalsMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler WeightsSampler1 = sampler_state { texture = <WeightsMap1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler WeightsSampler2 = sampler_state { texture = <WeightsMap2> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};


struct MTVertexToPixel
{
    float4 Position         : POSITION;
    float3 WorldPosition    : TEXCOORD0;
    float2 TextureCoords    : TEXCOORD1;
	float  Depth            : TEXCOORD2;
    float4 PositionCopy     : TEXCOORD3;
    float4 ShadowScreenPosition : TEXCOORD4;
};

struct MTPixelToFrame
{
    float4 Color : COLOR0;
};

MTVertexToPixel MultiTexturedVS( float4 inPos : POSITION, float2 inTexCoords: TEXCOORD0)
{
    MTVertexToPixel output;

    float4 worldPosition = mul(inPos, World);
    float4x4 viewProjection = mul(View, Projection);

    output.TextureCoords = float2(
	  TexOffsetAndScale.x + inTexCoords.x * TexOffsetAndScale.z,
	  TexOffsetAndScale.y + inTexCoords.y * TexOffsetAndScale.w );
    
	worldPosition.y += tex2Dlod(HeightsSampler, float4(output.TextureCoords, 0.0f, 0.0f)).r;

    output.Position = output.PositionCopy = mul(worldPosition, viewProjection);
    output.WorldPosition = worldPosition;
    
	output.Depth = output.Position.z / output.Position.w;

	output.ShadowScreenPosition = mul(worldPosition, ShadowViewProjection);

    return output;    
}


float2 sampleShadowMap(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return float2(1, 1);

	return tex2D(shadowSampler, UV).rg;
}


MTPixelToFrame MultiTexturedPS(MTVertexToPixel input)
{
    MTPixelToFrame Output = (MTPixelToFrame)0;        
    
	float3 normal = tex2D(NormalsSampler, input.TextureCoords).xyz - float3(0.5,0.5,0.5);
    float lightingFactor = saturate(Ambient+dot(normal, -LightingDirection));

    float blendDistance = 0.99f;
	float blendWidth = 0.005f;
	float blendFactor = clamp((input.Depth-blendDistance)/blendWidth, 0, 1);

	float4 textureWeights1 = tex2D(WeightsSampler1, input.TextureCoords);
	float4 textureWeights2 = tex2D(WeightsSampler2, input.TextureCoords);

	float4 farColor;
	float2 farTextureCoords = input.TextureCoords*3;
	farColor  = tex2D(TextureSamplerA, farTextureCoords)*textureWeights1.x;
	farColor += tex2D(TextureSamplerB, farTextureCoords)*textureWeights1.y;
	farColor += tex2D(TextureSamplerC, farTextureCoords)*textureWeights1.z;
	farColor += tex2D(TextureSamplerD, farTextureCoords)*textureWeights1.w;
	farTextureCoords *= 40;
	farColor += tex2D(TextureSamplerE, farTextureCoords)*textureWeights2.x;
	farColor += tex2D(TextureSamplerF, farTextureCoords)*textureWeights2.y;
	farColor += tex2D(TextureSamplerG, farTextureCoords)*textureWeights2.z;
	farColor += tex2D(TextureSamplerH, farTextureCoords)*textureWeights2.w;
    
	float4 nearColor;
	float2 nearTextureCoords = input.TextureCoords*9;
	nearColor  = tex2D(TextureSamplerA, nearTextureCoords)*textureWeights1.x;
	nearColor += tex2D(TextureSamplerB, nearTextureCoords)*textureWeights1.y;
	nearColor += tex2D(TextureSamplerC, nearTextureCoords)*textureWeights1.z;
	nearColor += tex2D(TextureSamplerD, nearTextureCoords)*textureWeights1.w;
	nearTextureCoords = farTextureCoords;
	nearColor += tex2D(TextureSamplerE, nearTextureCoords)*textureWeights2.x;
	nearColor += tex2D(TextureSamplerF, nearTextureCoords)*textureWeights2.y;
	nearColor += tex2D(TextureSamplerG, nearTextureCoords)*textureWeights2.z;
	nearColor += tex2D(TextureSamplerH, nearTextureCoords)*textureWeights2.w;

	if (DoShadowMapping)
	{
		float realDepth = input.ShadowScreenPosition.z / input.ShadowScreenPosition.w - ShadowBias;
		if (realDepth < 1)
		{
			float2 screenPos = input.ShadowScreenPosition.xy / input.ShadowScreenPosition.w;
			float2 shadowTexCoord = 0.5f * (float2(screenPos.x, -screenPos.y) + 1);

			// Variance shadow mapping code below from the variance shadow
			// mapping demo code @ http://www.punkuser.net/vsm/

			// Sample from depth texture
			float2 moments = sampleShadowMap(shadowTexCoord);

			// Check if we're in shadow
			float lit_factor = (realDepth <= moments.x);
    
			// Variance shadow mapping
			float E_x2 = moments.y;
			float Ex_2 = moments.x * moments.x;
			float variance = min(max(E_x2 - Ex_2, 0.0) + 1.0f / 10000.0f, 1.0);
			float m_d = (moments.x - realDepth);
			float p = variance / (variance + m_d * m_d);

			lightingFactor *= clamp(max(lit_factor, p), ShadowMult, 1.0f);
		}
	}

	Output.Color = lerp(nearColor, farColor, blendFactor);
	Output.Color *= lightingFactor;

    return Output;
}

float4 MultiTexturedPSClipPlane(MTVertexToPixel input) : COLOR0
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	return MultiTexturedPS(input).Color;
}

float4 MultiTexturedPSDepth(MTVertexToPixel input) : COLOR0
{
	// Determine the depth of this vertex / by the far plane distance,
	// limited to [0, 1]
    float depth = clamp(input.PositionCopy.z / input.ShadowScreenPosition.w, 0, 1);
    
	// Return only the depth value
    return float4(depth, depth * depth, 0, 1);
}

technique TechStandard
{
	pass Pass0
    {   
    	VertexShader = compile vs_3_0 MultiTexturedVS();
        PixelShader  = compile ps_3_0 MultiTexturedPS();
    }
}

technique TechClipPlane
{
	pass Pass0
    {   
    	VertexShader = compile vs_3_0 MultiTexturedVS();
        PixelShader  = compile ps_3_0 MultiTexturedPSClipPlane();
    }
}

technique TechDepthMap
{
	pass Pass0
    {   
    	VertexShader = compile vs_3_0 MultiTexturedVS();
        PixelShader  = compile ps_3_0 MultiTexturedPSDepth();
    }
}

