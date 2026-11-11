Shader "FitCheck/Artwork"
{
    Properties{_MainTex("User supplied artwork",2D)="white"{} _CityExterior("Outdoor haze",Float)=0}
    SubShader{Tags{"RenderType"="Opaque"} Pass{Cull Back ZWrite On
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"
    #include "CityLighting.cginc"
    sampler2D _MainTex;float _CityExterior;
    struct vf{float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float distance:TEXCOORD1;};
    vf vert(appdata_base v){vf o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord;o.distance=length(mul(unity_ObjectToWorld,v.vertex).xyz-_WorldSpaceCameraPos);return o;}
    fixed4 frag(vf i):SV_Target{return fixed4(CityHaze(tex2D(_MainTex,i.uv).rgb*.94,i.distance,_CityExterior),1);}
    ENDCG
    }}
}
