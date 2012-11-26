float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;

bool DoShadowMapping = true;
float4x4 ShadowViewProjection;
float3 ShadowLightPosition;
//float ShadowFarPlane = 100;
float ShadowMult = 0.3f;
float ShadowBias = 0.001f;
texture2D ShadowMap;
sampler2D shadowSampler = sampler_state {
	texture = <ShadowMap>;
	minfilter = point;
	magfilter = point;
	mipfilter = point;
};


texture Texture;
texture BumpMap;

sampler TextureSampler = sampler_state {
	texture = <Texture>;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

sampler BumpMapSampler = sampler_state {
	texture = <BumpMap>;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

float3 DiffuseColor = float3(1, 1, 1);
float3 AmbientColor = float3(0.3, 0.3, 0.3);
float3 LightDirection = float3(-10, 20, 5);
float3 LightColor = float3(0.8, 0.8, 0.8);
float SpecularPower = 32;
float3 SpecularColor = float3(1, 1, 1);

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position		 : POSITION0;
    float2 UV			 : TEXCOORD0;
    float3 ViewDirection : TEXCOORD1;
	float3 WorldPosition : TEXCOORD2;
	float4 ShadowScreenPosition : TEXCOORD3;
	float4 PositionCopy  : TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    float4 worldPosition = mul(input.Position, World);
    float4x4 viewProjection = mul(View, Projection);
    
    output.WorldPosition = worldPosition;
    output.Position = output.PositionCopy = mul(worldPosition, viewProjection);

    output.UV = input.UV;

    output.ViewDirection = worldPosition - CameraPosition;

	output.ShadowScreenPosition = mul(worldPosition, ShadowViewProjection);

    return output;

}

float2 sampleShadowMap(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return float2(1, 1);

	return tex2D(shadowSampler, UV).rg;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 color = DiffuseColor * tex2D(TextureSampler, input.UV);

	// Start with ambient lighting
	float3 lighting = AmbientColor;

	float3 lightDir = normalize(LightDirection);

	float3 normal = normalize( tex2D(BumpMapSampler, input.UV).rgb * 2 - 1 );

	// Add lambertian lighting
	lighting += saturate(dot(lightDir, normal)) * LightColor;

	float3 refl = reflect(lightDir, normal);
	float3 view = normalize(input.ViewDirection);
	
	// Add specular highlights
	lighting += pow(saturate(dot(refl, view)), SpecularPower) * SpecularColor;

	if (DoShadowMapping)
	{
		float realDepth = input.ShadowScreenPosition.z / input.ShadowScreenPosition.w - ShadowBias;

		if (realDepth < 1)
		{
			// Variance shadow mapping code below from the variance shadow
			// mapping demo code @ http://www.punkuser.net/vsm/

			// Sample from depth texture
			float2 screenPos = input.ShadowScreenPosition.xy / input.ShadowScreenPosition.w;
			float2 shadowTexCoord = 0.5f * (float2(screenPos.x, -screenPos.y) + 1);

			float2 moments = sampleShadowMap(shadowTexCoord);

			// Check if we're in shadow
			float lit_factor = (realDepth <= moments.x);
    
			// Variance shadow mapping
			float E_x2 = moments.y;
			float Ex_2 = moments.x * moments.x;
			float variance = min(max(E_x2 - Ex_2, 0.0) + 1.0f / 10000.0f, 1.0);
			float m_d = (moments.x - realDepth);
			float p = variance / (variance + m_d * m_d);

			color *= clamp(max(lit_factor, p), ShadowMult, 1.0f);
		}
	}

	// Calculate final color
	float3 output = saturate(lighting) * color;

    return float4(output, 1);
}


float4 PixelShaderFunctionClipPlane(VertexShaderOutput input) : COLOR0
{
	clip(dot(float4(input.WorldPosition,1), ClipPlane));
	return PixelShaderFunction(input);
}


float4 PixelShaderFunctionDepthMap(VertexShaderOutput input) : COLOR0
{
	// Determine the depth of this vertex / by the far plane distance,
	// limited to [0, 1]
    float depth = clamp(input.PositionCopy.z / input.PositionCopy.w, 0, 1);
    
	// Return only the depth value
    return float4(depth, depth * depth, 0, 1);
}


technique TechNormal
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique TechClip
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunctionClipPlane();
    }
}

technique TectShadowDepth
{
	pass Pass0
    {   
    	VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader  = compile ps_3_0 PixelShaderFunctionDepthMap();
    }
}

