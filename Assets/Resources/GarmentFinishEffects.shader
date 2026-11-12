Shader "FitCheck/Selective Garment Finishes"
{
    Properties
    {
        _GarmentFinish ("Clothing finish",Float)=0
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
        _CharacterLit ("Character cover lighting",Range(0,1))=0
        _PortraitRim ("Character edge light",Range(0,1))=1
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" "Queue"="Geometry"}
        Pass
        {
            Name "PAINT"
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            #include "CityLighting.cginc"
            #include "CharacterLighting.cginc"
            sampler2D _MainTex,_FabricTex;float4 _Color,_MainTex_TexelSize;float _GarmentFinishesEnabled;float _GarmentFinish;float _TexScale,_Motion,_Glow,_PaintFrame,_SmoothLight,_NightCity,_HairFlow,_FabricWeight,_CharacterLit,_PortraitRim;
            struct v2f{float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float3 normal:TEXCOORD1;float3 world:TEXCOORD2;float3 view:TEXCOORD3;};
            v2f vert(appdata_base v)
            {v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord.xy;o.normal=UnityObjectToWorldNormal(v.normal);o.world=mul(unity_ObjectToWorld,v.vertex).xyz;o.view=_WorldSpaceCameraPos-o.world;return o;}
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
                float3 paintA=tex2D(_MainTex,uv).rgb;
                float3 paintB=paintA;
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
                float3 localLight=float3(.52,.55,.60)+CityLightAt(i.world)*.66;
                float3 painted=CharacterPaintLight(lerp(colorA,colorB,blend),_Color.rgb*lerp(paintA,paintB,blend),i.normal,i.view,_CharacterLit,_PortraitRim);
                float3 result=painted*lerp(1,localLight,_NightCity);
                float3 normal=normalize(i.normal),view=normalize(i.view);float rim=1-saturate(dot(normal,view));
                if(_GarmentFinishesEnabled>.5&&_GarmentFinish>.5&&_GarmentFinish<1.5){
                    // Stylized foil only in the bright printed art; cotton stays matte.
                    float mask=smoothstep(.38,.70,max(paintA.r,max(paintA.g,paintA.b)));
                    mask*=smoothstep(.36,.39,uv.y)*(1-smoothstep(.64,.68,uv.y));
                    mask*=smoothstep(.32,.35,uv.x)*(1-smoothstep(.65,.68,uv.x));
                    float3 r=reflect(-view,normal);float band=smoothstep(.22,.80,.5+.5*sin(r.y*12+r.x*7+1.2));
                    float3 chrome=lerp(float3(.065,.075,.11),float3(.91,.94,1),band);
                    float glint=pow(saturate(dot(normal,normalize(view+float3(-.65,.5,-.6)))),48);
                    chrome+=glint*.55+rim*rim*float3(.10,.08,.16);result=lerp(result,chrome*lerp(1,localLight,_NightCity),mask*.98);
                }
                if(_GarmentFinishesEnabled>.5&&_GarmentFinish>1.5){
                    // Broad restrained nylon/satin highlight, no real-time reflection capture.
                    float broad=pow(saturate(dot(normal,normalize(view+float3(-.65,.5,-.6)))),14);
                    result+=(broad*.24+rim*rim*.08)*float3(.65,.78,1)*lerp(1,localLight,_NightCity);
                }
                return fixed4(saturate(result),1);
            }
            ENDCG
        }
    }
    Fallback "Unlit/Color"
}
