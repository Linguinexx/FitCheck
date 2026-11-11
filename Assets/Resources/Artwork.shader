Shader "FitCheck/Artwork"
{
    Properties{_MainTex("User supplied artwork",2D)="white"{}}
    SubShader{Tags{"RenderType"="Opaque"} Pass{Cull Back ZWrite On
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"
    sampler2D _MainTex;
    struct vf{float4 pos:SV_POSITION;float2 uv:TEXCOORD0;};
    vf vert(appdata_base v){vf o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord;return o;}
    fixed4 frag(vf i):SV_Target{return fixed4(tex2D(_MainTex,i.uv).rgb*.94,1);}
    ENDCG
    }}
}
