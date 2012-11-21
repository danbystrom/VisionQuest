//------- XNA interface --------
float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float4 ClipPlane;

float3 xAllowedRotDir;

//------- Texture Samplers --------
Texture Texture;
sampler textureSampler = sampler_state { texture = <Texture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = CLAMP; AddressV = CLAMP;};

struct BBVertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord	: TEXCOORD0;
};
struct BBPixelToFrame
{
    float4 Color 	: COLOR0;
};

//------- Technique: CylBillboard --------
BBVertexToPixel CylBillboardVS(float4 inPos: POSITION0, float2 inTexCoord: TEXCOORD0)
{
	BBVertexToPixel Output = (BBVertexToPixel)0;	

	float4 center = mul(inPos, World);
	float3 eyeVector = center - CameraPosition;	
	
	float3 upVector = xAllowedRotDir;
	upVector = normalize(upVector);
	float3 sideVector = cross(eyeVector,upVector);
	sideVector = normalize(sideVector)*2;
	
	float3 finalPosition = center;
	finalPosition += (inTexCoord.x-0.5f)*sideVector;
	finalPosition += (1.5f-inTexCoord.y*1.5f)*upVector;	
	
	float4 finalPosition4 = float4(finalPosition, 1);
		
	float4x4 preViewProjection = mul (View, Projection);
	Output.Position = mul(finalPosition4, preViewProjection);
	
	Output.TexCoord = inTexCoord;
	
	return Output;
}

BBPixelToFrame BillboardPS(BBVertexToPixel PSIn) : COLOR0
{
	BBPixelToFrame Output = (BBPixelToFrame)0;		
	Output.Color = tex2D(textureSampler, PSIn.TexCoord);

	clip(Output.Color.w - 0.7843f);

	return Output;
}

technique CylBillboard
{
	pass Pass0
    {          
    	VertexShader = compile vs_2_0 CylBillboardVS();
        PixelShader  = compile ps_2_0 BillboardPS();        
    }
}

technique CylBillboardClipPlane
{
	pass Pass0
    {          
    	VertexShader = compile vs_2_0 CylBillboardVS();
        PixelShader  = compile ps_2_0 BillboardPS();        
    }
}