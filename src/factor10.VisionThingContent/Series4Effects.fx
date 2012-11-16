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

float3 LightDirection;
float Ambient;
bool EnableLighting;

//------- Texture Samplers --------

Texture Texture0;
Texture Texture1;
Texture Texture2;
Texture Texture3;

sampler TextureSampler0 = sampler_state { texture = <Texture0> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};
sampler TextureSampler1 = sampler_state { texture = <Texture1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};
sampler TextureSampler2 = sampler_state { texture = <Texture2> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler TextureSampler3 = sampler_state { texture = <Texture3> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};


struct MTVertexToPixel
{
    float4 Position         : POSITION;    
    //float4 Color           : COLOR0;
    float3 Normal           : TEXCOORD0;
    float2 TextureCoords    : TEXCOORD1;
    float4 LightDirection   : TEXCOORD2;
    float4 TextureWeights   : TEXCOORD3;
	float  Depth            : TEXCOORD4;
};

struct MTPixelToFrame
{
    float4 Color : COLOR0;
};

MTVertexToPixel MultiTexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0, float4 inTexWeights: TEXCOORD1)
{    
    MTVertexToPixel Output = (MTVertexToPixel)0;
    float4x4 preViewProjection = mul (View, Projection);
    float4x4 preWorldViewProjection = mul (World, preViewProjection);
    
    Output.Position = mul(inPos, preWorldViewProjection);
    Output.Normal = mul(normalize(inNormal), World);
    Output.TextureCoords = inTexCoords;
    Output.LightDirection.xyz = -LightDirection;
    Output.LightDirection.w = 1;    
    Output.TextureWeights = inTexWeights;
    
	Output.Depth = Output.Position.z/Output.Position.w;

    return Output;    
}


MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
    MTPixelToFrame Output = (MTPixelToFrame)0;        
    
    float lightingFactor = 1;
    if (EnableLighting)
        lightingFactor = saturate(saturate(dot(PSIn.Normal, PSIn.LightDirection)) + Ambient);

    float blendDistance = 0.99f;
	float blendWidth = 0.005f;
	float blendFactor = clamp((PSIn.Depth-blendDistance)/blendWidth, 0, 1);

	float4 farColor;
	farColor = tex2D(TextureSampler0, PSIn.TextureCoords)*PSIn.TextureWeights.x;
	farColor += tex2D(TextureSampler1, PSIn.TextureCoords)*PSIn.TextureWeights.y;
	farColor += tex2D(TextureSampler2, PSIn.TextureCoords)*PSIn.TextureWeights.z;
	farColor += tex2D(TextureSampler3, PSIn.TextureCoords)*PSIn.TextureWeights.w;
    
	float4 nearColor;
	float2 nearTextureCoords = PSIn.TextureCoords*3;
	nearColor = tex2D(TextureSampler0, nearTextureCoords)*PSIn.TextureWeights.x;
	nearColor += tex2D(TextureSampler1, nearTextureCoords)*PSIn.TextureWeights.y;
	nearColor += tex2D(TextureSampler2, nearTextureCoords)*PSIn.TextureWeights.z;
	nearColor += tex2D(TextureSampler3, nearTextureCoords)*PSIn.TextureWeights.w;

	Output.Color = lerp(nearColor, farColor, blendFactor);
	Output.Color *= lightingFactor;

    return Output;
}

technique MultiTextured
{
	pass Pass0
    {   
    	VertexShader = compile vs_2_0 MultiTexturedVS();
        PixelShader  = compile ps_2_0 MultiTexturedPS();
    }
}


float4x4 ReflectionView;

float WaveLength;
float WaveHeight;

float Time;
float WindForce;
float3 WindDirection;
float Overcast;

Texture WaterBumpMap;
Texture RefractionMap;
Texture ReflectionMap;

sampler WaterBumpMapSampler = sampler_state { texture = <WaterBumpMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
//sampler RefractionSampler = sampler_state { texture = <RefractionMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
sampler ReflectionSampler = sampler_state { texture = <ReflectionMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

struct WVertexToPixel
{
    float4 Position                 : POSITION;
    float4 ReflectionMapSamplingPos : TEXCOORD1;
    float2 BumpMapSamplingPos       : TEXCOORD2;
    //float4 RefractionMapSamplingPos : TEXCOORD3;
    float4 Position3D               : TEXCOORD4;
};
 
 struct WPixelToFrame
 {
     float4 Color : COLOR0;
 };
 
 WVertexToPixel WaterVS(float4 inPos : POSITION, float2 inTex: TEXCOORD)
{    
    WVertexToPixel Output = (WVertexToPixel)0;

    float4x4 preViewProjection = mul (View, Projection);
    float4x4 preWorldViewProjection = mul (World, preViewProjection);
    float4x4 preReflectionViewProjection = mul (ReflectionView, Projection);
    float4x4 preWorldReflectionViewProjection = mul (World, preReflectionViewProjection);

    Output.Position = mul(inPos, preWorldViewProjection);
    Output.ReflectionMapSamplingPos = mul(inPos, preWorldReflectionViewProjection);    
    //Output.RefractionMapSamplingPos = mul(inPos, preWorldViewProjection);
    Output.Position3D = mul(inPos, World);        
    
    float3 windDir = normalize(WindDirection);    
    float3 perpDir = cross(WindDirection, float3(0,1,0));
    float ydot = dot(inTex, WindDirection.xz);
    float xdot = dot(inTex, perpDir.xz);
    float2 moveVector = float2(xdot, ydot);
    moveVector.y += Time*WindForce;    
    Output.BumpMapSamplingPos = moveVector/WaveLength;    

    
    return Output;
}

WPixelToFrame WaterPS(WVertexToPixel PSIn)
{
    WPixelToFrame Output = (WPixelToFrame)0;        
    
    float4 bumpColor = tex2D(WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
    float2 perturbation = WaveHeight*(bumpColor.rg - 0.5f)*2.0f;
    
    float2 ProjectedTexCoords;
    ProjectedTexCoords.x = PSIn.ReflectionMapSamplingPos.x/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
    ProjectedTexCoords.y = -PSIn.ReflectionMapSamplingPos.y/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;        
    float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;
    float4 reflectiveColor = tex2D(ReflectionSampler, perturbatedTexCoords);
    
    //float2 ProjectedRefrTexCoords;
    //ProjectedRefrTexCoords.x = PSIn.RefractionMapSamplingPos.x/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
    //ProjectedRefrTexCoords.y = -PSIn.RefractionMapSamplingPos.y/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;    
    //float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;    
    float4 refractiveColor = float4(0.01,0.01,0.2,1); // tex2D(RefractionSampler, perturbatedRefrTexCoords);
    
    float3 eyeVector = normalize(CameraPosition - PSIn.Position3D);
    float3 normalVector = (bumpColor.rbg-0.5f)*2.0f;
    
    float fresnelTerm = dot(eyeVector, normalVector);    
    float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm);
    
    float4 dullColor = float4(0.3f, 0.3f, 0.5f, 1.0f);
    
    Output.Color = lerp(combinedColor, dullColor, 0.2f);    
    
    float3 reflectionVector = -reflect(LightDirection, normalVector);
    float specular = dot(normalize(reflectionVector), normalize(eyeVector));
    specular = pow(abs(specular), 256);        
    Output.Color.rgb += specular;
    
    return Output;
}
 
 technique Water
 {
     pass Pass0
     {
         VertexShader = compile vs_2_0 WaterVS();
         PixelShader = compile ps_2_0 WaterPS();
     }
 }
