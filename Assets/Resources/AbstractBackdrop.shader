Shader "FitCheck/Abstract Descending Backdrop"
{
    Properties { _MainTex ("Seamless abstraction",2D)="black"{} _BackdropColor ("Pattern",Color)=(.58,.08,.91,1) _BackdropBase ("Background",Color)=(.025,.008,.045,1) }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;float _BackdropTime;fixed4 _BackdropColor,_BackdropBase;
            struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;};
            v2f vert(appdata_base v){v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord;return o;}
            fixed4 frag(v2f i):SV_Target
            {
                // Increasing the sampled V moves every visible shape downward.
                float tone=tex2D(_MainTex,float2(i.uv.x,frac(i.uv.y+_BackdropTime*.065))).r;
                return fixed4(lerp(_BackdropBase.rgb,_BackdropColor.rgb,saturate(tone*2.5)),1);
            }
            ENDCG
        }
    }
    Fallback "Unlit/Texture"
}
