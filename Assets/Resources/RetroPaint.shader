Shader "FitCheck/Retro Painted"
{
    Properties
    {
        _Color ("Pigment",Color)=(1,1,1,1)
        _NightCity("City night",Float)=0
        _FabricTex("Fabric weave",2D)="gray"{}
        _FabricWeight("Fabric weave weight",Float)=0
        _HairFlow("Broad hair strands",Float)=0
        _MainTex ("Four hand painted frames",2D)="white"{}
        _TexScale ("Pattern scale",Float)=1
        _Motion ("Animated paint",Range(0,1))=1
        _Glow ("Printed light",Range(0,1))=0
        _Outline ("Ink edge",Range(0,0.025))=0.006
        _SmoothLight ("Rounded surface lighting",Range(0,1))=0
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" "Queue"="Geometry"}
        Pass
        {
            Name "INK"
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _Outline;
            float4 vert(appdata_base v):SV_POSITION
            {
                float3 world=mul(unity_ObjectToWorld,v.vertex).xyz;
                world+=UnityObjectToWorldNormal(v.normal)*_Outline;
                return mul(UNITY_MATRIX_VP,float4(world,1));
            }
            fixed4 frag():SV_Target{return fixed4(.075,.045,.10,1);}
            ENDCG
        }
        Pass
        {
            Name "PAINT"
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            sampler2D _MainTex,_FabricTex;float4 _Color,_MainTex_TexelSize;float _TexScale,_Motion,_Glow,_PaintFrame,_SmoothLight,_NightCity,_HairFlow,_FabricWeight;
            struct v2f{float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float3 normal:TEXCOORD1;float3 world:TEXCOORD2;};
            v2f vert(appdata_base v)
            {v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord.xy;o.normal=UnityObjectToWorldNormal(v.normal);o.world=mul(unity_ObjectToWorld,v.vertex).xyz;return o;}
            fixed4 frag(v2f i):SV_Target
            {
                float phase=_PaintFrame*_Motion;
                float frame=fmod(floor(phase),4);
                float nextFrame=fmod(frame+1,4);
                float blend=smoothstep(0,1,frac(phase));
                // Keep every sample inside its own atlas frame, including UV seams.
                float2 margin=min(float2(.49,.49),_MainTex_TexelSize.xy*float2(2,.5));
                float2 hairWarp=float2(i.uv.y*.72+sin(i.uv.y*7+i.uv.x*4)*.05,0)*_HairFlow;
                float2 uv=lerp(margin,1-margin,frac(i.uv*_TexScale+hairWarp));
                float3 paintA=tex2D(_MainTex,float2((uv.x+frame)*.25,uv.y)).rgb;
                float3 paintB=tex2D(_MainTex,float2((uv.x+nextFrame)*.25,uv.y)).rgb;
                float flow=.88+.14*smoothstep(-.6,.8,sin((i.uv.x+i.uv.y*.72)*25.1327+sin(i.uv.y*7+i.uv.x*4)*1.35));
                paintA*=lerp(1,flow,_HairFlow);paintB*=lerp(1,flow,_HairFlow);
                float weave=dot(tex2D(_FabricTex,i.uv*.65).rgb,float3(.299,.587,.114));
                float fabric=lerp(1,clamp(1+(weave-.3554)*5,.70,1.22),_FabricWeight);
                paintA*=fabric;paintB*=fabric;
                float lit=saturate(dot(normalize(i.normal),normalize(float3(-.35,.65,-.75))));
                float shade=.79+lerp(floor(lit*3.0)/3.0,lit,_SmoothLight)*.23;
                // Quantize endpoints, then blend. Quantizing after interpolation
                // would reintroduce visible time steps in these subtle textures.
                float3 colorA=floor(saturate(_Color.rgb*paintA*shade+_Color.rgb*_Glow*.55)*31+.5)/31;
                float3 colorB=floor(saturate(_Color.rgb*paintB*shade+_Color.rgb*_Glow*.55)*31+.5)/31;
                float2 p=i.world.xz-float2(200,0);float localLight=.55+.25*exp(-dot(p-float2(-17,3),p-float2(-17,3))/30)+.20*exp(-dot(p-float2(10,1),p-float2(10,1))/70);
                return fixed4(lerp(colorA,colorB,blend)*lerp(1,localLight,_NightCity),1);
            }
            ENDCG
        }
    }
    Fallback "Unlit/Color"
}
