Shader "FitCheck/Dust Particle" {
 Properties {_MainTex("Particle",2D)="white"{}}
 SubShader {Tags {"Queue"="Transparent" "RenderType"="Transparent"} Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off
 Pass {CGPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 sampler2D _MainTex;
 struct vf {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;fixed4 color:COLOR;};
 vf vert(appdata_full v){vf o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord.xy;o.color=v.color;return o;}
 fixed4 frag(vf i):SV_Target{return tex2D(_MainTex,i.uv)*i.color;}
 ENDCG}
 }
}
