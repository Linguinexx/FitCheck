Shader "FitCheck/Printed City Signs"
{
    Properties { _MainTex("Printed lettering",2D)="white"{} _Emission("Sign light",Float)=0 _CityExterior("Outdoor haze",Float)=0 }
    SubShader {
        Tags {"Queue"="AlphaTest" "RenderType"="TransparentCutout"}
        Pass {
            Cull Back ZWrite On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "CityLighting.cginc"
            sampler2D _MainTex;float _Emission,_CityExterior;
            struct vf {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float distance:TEXCOORD1;fixed4 color:COLOR;};
            vf vert(appdata_full v){vf o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord;o.color=v.color;o.distance=length(mul(unity_ObjectToWorld,v.vertex).xyz-_WorldSpaceCameraPos);return o;}
            fixed4 frag(vf i):SV_Target {
                fixed4 ink=tex2D(_MainTex,i.uv);clip(ink.a-.22);
                float3 color=i.color.rgb*lerp(.78,1.16,_Emission);
                return fixed4(CityHaze(color,i.distance,_CityExterior),1);
            }
            ENDCG
        }
    }
}
