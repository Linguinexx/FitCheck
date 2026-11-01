Shader "FitCheck/Abstract Descending Backdrop"
{
    Properties { _MainTex ("Seamless purple abstraction",2D)="black"{} }
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
            sampler2D _MainTex;float _BackdropTime;
            struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;};
            v2f vert(appdata_base v){v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord;return o;}
            fixed4 frag(v2f i):SV_Target
            {
                // Increasing the sampled V moves every visible shape downward.
                return tex2D(_MainTex,float2(i.uv.x,frac(i.uv.y+_BackdropTime*.065)));
            }
            ENDCG
        }
    }
    Fallback "Unlit/Texture"
}
