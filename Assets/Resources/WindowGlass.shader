Shader "FitCheck/Textured Window"
{
    Properties{_MainTex("Surface wear",2D)="gray"{} _Interior("Indoor",Float)=0}
    SubShader{Tags{"RenderType"="Opaque"} Pass{Cull Off
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"
    #include "CityLighting.cginc"
    sampler2D _MainTex;float _Interior;
    struct v2f{float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float3 world:TEXCOORD1;float3 normal:TEXCOORD2;fixed4 color:COLOR;};
    v2f vert(appdata_full v){v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord;o.world=mul(unity_ObjectToWorld,v.vertex).xyz;o.normal=UnityObjectToWorldNormal(v.normal);o.color=v.color;return o;}
    fixed4 frag(v2f i):SV_Target{
        float2 uv=i.uv;float wear=tex2D(_MainTex,uv*.5).r;
        float3 view=normalize(_WorldSpaceCameraPos-i.world);float fresnel=pow(1-abs(dot(normalize(i.normal),view)),3);
        float stripe=pow(saturate(.5+.5*sin((uv.x*.75+uv.y*.35+view.x*.2)*7)),12);
        float rib=.92+.08*sin(uv.x*120);float shade=.25+.40*saturate(uv.y*.24)+wear*.35;
        float3 room=i.color.rgb*shade*float3(1,.86,.69);
        float3 glass=lerp(room,float3(.19,.23,.22),.22+fresnel*.3)+stripe*float3(.13,.13,.10);
        glass+=CityLightAt(i.world)*.10*(1-_Interior);
        glass*=rib;glass=lerp(glass,float3(.035,.025,.03),saturate((distance(i.world,_WorldSpaceCameraPos)-35)/110)*.85);
        return fixed4(floor(saturate(glass)*63+.5)/63,1);
    }
    ENDCG
    }}
}
