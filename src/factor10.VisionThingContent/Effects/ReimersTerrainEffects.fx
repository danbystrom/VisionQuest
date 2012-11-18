//----------------------------------------------------
//--                                                --
//--               www.riemers.net                  --
//--         Series 4: Advanced terrain             --
//--                 Shader code                    --
//--                                                --
//----------------------------------------------------

//------- Constants --------
float4x4 View;
float4x4 Projection;
float4x4 World;
float3 CameraPosition;
float4 ClipPlane;

float3 LightDirection = float3(0.7,0.7,0.7);
float Ambient = 0.4;
bool EnableLighting;

float dtex = 1/128;

//------- Texture Samplers --------

Texture Texture0;
Texture Texture1;
Texture Texture2;
Texture Texture3;

Texture HeightsMap;
Texture NormalsMap;
Texture WeightsMap;

sampler TextureSampler0 = sampler_state { texture = <Texture0> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSampler1 = sampler_state { texture = <Texture1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSampler2 = sampler_state { texture = <Texture2> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSampler3 = sampler_state { texture = <Texture3> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

sampler HeightsSampler = sampler_state { texture = <HeightsMap> ; magfilter = POINT; minfilter = POINT; mipfilter=POINT; AddressU = mirror; AddressV = mirror;};
sampler NormalsSampler = sampler_state { texture = <NormalsMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler WeightsSampler = sampler_state { texture = <WeightsMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};


struct MTVertexToPixel
{
    float4 Position         : POSITION;    
    float3 WorldPosition    : TEXCOORD0;
    float2 TextureCoords    : TEXCOORD1;
    float4 LightDirection   : TEXCOORD2;
	float  Depth            : TEXCOORD3;
};

struct MTPixelToFrame
{
    float4 Color : COLOR0;
};

MTVertexToPixel MultiTexturedVS( float4 inPos : POSITION, float2 inTexCoords: TEXCOORD0)
{
    float4 worldPosition = mul(inPos, World);
    float4x4 viewProjection = mul(View, Projection);
    
    float4x4 preViewProjection = mul (View, Projection);
    float4x4 preWorldViewProjection = mul (World, preViewProjection);
    
	worldPosition.y += tex2Dlod(HeightsSampler, float4(inTexCoords, 0.0f, 0.0f)).r;

    MTVertexToPixel Output = (MTVertexToPixel)0;
    Output.Position = mul(worldPosition, viewProjection);
    Output.WorldPosition = worldPosition;
    Output.TextureCoords = inTexCoords;
    Output.LightDirection.xyz = -LightDirection;
    Output.LightDirection.w = 1;
    
	Output.Depth = Output.Position.z/Output.Position.w;

    return Output;    
}


MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
    MTPixelToFrame Output = (MTPixelToFrame)0;        
    
    float lightingFactor = 1;
    //if (EnableLighting)
	//{
		float3 normal = tex2D(NormalsSampler, PSIn.TextureCoords).xyz - float3(0.5,0.5,0.5);
		normal = normalize(normal);
        lightingFactor = saturate(Ambient+dot(normal, normalize(PSIn.LightDirection)));
	//}

    float blendDistance = 0.99f;
	float blendWidth = 0.005f;
	float blendFactor = clamp((PSIn.Depth-blendDistance)/blendWidth, 0, 1);

	float4 textureWeights = tex2D(WeightsSampler, PSIn.TextureCoords);

	float4 farColor;
	float2 farTextureCoords = PSIn.TextureCoords*2;
	farColor = tex2D(TextureSampler0, farTextureCoords)*textureWeights.x;
	farColor += tex2D(TextureSampler1, farTextureCoords)*textureWeights.y;
	farColor += tex2D(TextureSampler2, farTextureCoords)*textureWeights.z;
	farColor += tex2D(TextureSampler3, farTextureCoords)*textureWeights.w;
    
	float4 nearColor;
	float2 nearTextureCoords = farTextureCoords*3;
	nearColor = tex2D(TextureSampler0, nearTextureCoords)*textureWeights.x;
	nearColor += tex2D(TextureSampler1, nearTextureCoords)*textureWeights.y;
	nearColor += tex2D(TextureSampler2, nearTextureCoords)*textureWeights.z;
	nearColor += tex2D(TextureSampler3, nearTextureCoords)*textureWeights.w;

	Output.Color = lerp(nearColor, farColor, blendFactor);
	Output.Color *= lightingFactor;

    return Output;
}

float4 MultiTexturedPSClipPlane(MTVertexToPixel input) : COLOR0
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	return MultiTexturedPS(input).Color;
}

technique MultiTextured
{
	pass Pass0
    {   
    	VertexShader = compile vs_3_0 MultiTexturedVS();
        PixelShader  = compile ps_3_0 MultiTexturedPS();
    }
}

technique MultiTexturedClip
{
	pass Pass0
    {   
    	VertexShader = compile vs_3_0 MultiTexturedVS();
        PixelShader  = compile ps_3_0 MultiTexturedPSClipPlane();
    }
}

