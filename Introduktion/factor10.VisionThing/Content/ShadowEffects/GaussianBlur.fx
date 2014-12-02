Texture2D<float4> Texturex : register(t0);

// SpriteBatch expects that default texture sampler parameter will have name 'TextureSampler'
sampler TextureSamplerx : register(s0);

// SpriteBatch expects that default vertex transform parameter will have name 'MatrixTransform'
row_major float4x4 MatrixTransformx;

void VSMain(
	inout float4 color    : COLOR0,
	inout float2 texCoord : TEXCOORD0,
	inout float4 position : SV_Position)
{
	position = mul(position, MatrixTransformx);
}

// Precalculated weights and offsets
float weights[15] = { 0.1061154, 0.1028506, 0.1028506, 0.09364651, 0.09364651, 
	0.0801001, 0.0801001, 0.06436224, 0.06436224, 0.04858317, 0.04858317, 
	0.03445063, 0.03445063, 0.02294906, 0.02294906 };

float offsets[15] = { 0, 0.00125, -0.00125, 0.002916667, -0.002916667, 
	0.004583334, -0.004583334, 0.00625, -0.00625, 0.007916667, -0.007916667, 
	0.009583334, -0.009583334, 0.01125, -0.01125 };


// Blurs the input image horizontally
float4 PSBlurHorizontal(
	float4 color: COLOR,
	float2 UV : TEXCOORD0) : SV_Target
{
	return color;
}

// Blurs the input image vertically
float4 PSBlurVertical(
	float4 color: COLOR,
	float2 UV : TEXCOORD0) : SV_Target
{
	return color;
}


technique Tech
{
	pass Horizontal
	{
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VSMain()));
		SetPixelShader(CompileShader(ps_4_0, PSBlurHorizontal()));
	}

	pass Verical
	{
		SetGeometryShader(0);
		SetVertexShader(CompileShader(vs_4_0, VSMain()));
		SetPixelShader(CompileShader(ps_4_0, PSBlurVertical()));
	}
}