Shader "FitCheck/Cat Fur"
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
        Tags {"RenderType"="Opaque"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;float4 _Color;float _FurMean,_TextureWeight,_StripeWeight;
            struct appdata {float4 vertex:POSITION;float3 normal:NORMAL;float2 uv:TEXCOORD0;};
            struct v2f {float4 vertex:SV_POSITION;float3 normal:TEXCOORD0;float2 uv:TEXCOORD1;float3 local:TEXCOORD2;};
            v2f vert(appdata v){v2f o;o.vertex=UnityObjectToClipPos(v.vertex);o.normal=UnityObjectToWorldNormal(v.normal);o.uv=v.uv;o.local=v.vertex.xyz;return o;}
            fixed4 frag(v2f i):SV_Target
            {
                float detail=dot(tex2D(_MainTex,i.uv*2).rgb,float3(.299,.587,.114));
                float grain=lerp(1,clamp(1+(detail-_FurMean)*2.5,.75,1.22),_TextureWeight);
                float stripe=smoothstep(.72,.95,sin(i.local.x*23+sin(i.local.z*12)*.6))*saturate((i.local.y+.10)*6)*_StripeWeight;
                float3 pigment=lerp(_Color.rgb,_Color.rgb*float3(.46,.43,.39),stripe*.72);
                float light=saturate(dot(normalize(i.normal),normalize(float3(-.35,.65,-.75))));
                float3 paint=pigment*grain*(.78+light*.23);
                return fixed4(floor(saturate(paint)*31+.5)/31,1);
            }
            ENDCG
        }
    }
}
