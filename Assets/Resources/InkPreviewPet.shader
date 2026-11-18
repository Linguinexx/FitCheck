Shader "FitCheck/Preview/Ink Pet"
{
    Properties
    {
        _Color("Fur color",Color)=(1,1,1,1)
        _MainTex("Short fur detail",2D)="gray"{}
        _FurMean("Texture luminance",Float)=.5
        _TextureWeight("Fur detail",Range(0,1))=.35
        _StripeWeight("Tabby markings",Range(0,1))=0
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" "Queue"="Geometry"}
        UsePass "FitCheck/Cat Fur/PAINT"
        UsePass "FitCheck/Preview/Character Ink/INK_PREVIEW"
    }
}
