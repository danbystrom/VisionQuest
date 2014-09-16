float4x4 World;
float4x4 View;
float4x4 Projection;

texture tex1;
texture tex2;

sampler Sampler1 = 
sampler_state
{
  Texture = <tex1>;
  MipFilter = LINEAR;
  MinFilter = LINEAR;
  MagFilter = LINEAR;
  AddressU = wrap; 
  AddressV = wrap;
};
sampler Sampler2 = 
sampler_state
{
  Texture = <tex2>;
  MipFilter = LINEAR;
  MinFilter = LINEAR;
  MagFilter = LINEAR;
  AddressU = wrap; 
  AddressV = wrap;
};



float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR0
{
    return float4(1, 0, 0, 1);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
