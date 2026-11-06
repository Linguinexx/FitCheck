Shader "FitCheck/City Painted"
{
    Properties { _DecorTint("Decoration tint",Color)=(1,1,1,1) _MainTex("Paint atlas",2D)="white"{} _SingleSurface("Imported surface",Float)=0 _SurfaceMean("Surface mean",Float)=.5 _SurfaceContrast("Surface contrast",Float)=3 _SurfaceTint("Surface tint",Color)=(1,1,1,1) _SurfaceTile("Urban atlas tile",Float)=0 _TexScale("Material scale",Float)=1 _Emission("Light",Float)=0 _Interior("Indoor lighting",Float)=0 _LeafWind("Leaves",Float)=0 _Motion("Paint movement",Float)=1 _TextureWeight("Paint detail",Float)=.35 _Wind("Banner wind",Float)=0 [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull",Float)=2 [HideInInspector] _HomeOrigin("Home lighting origin",Vector)=(0,0,0,0) }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        Pass
        {
            Cull [_Cull]
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "CityLighting.cginc"
            sampler2D _MainTex;float _Motion,_PaintFrame,_TextureWeight,_Wind,_CityTime,_SurfaceTile,_TexScale,_Emission,_LeafWind,_Interior,_SingleSurface,_SurfaceMean,_SurfaceContrast;float4 _SurfaceTint,_DecorTint,_HomeOrigin;
            struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;float3 normal:TEXCOORD1;float distance:TEXCOORD2;float3 world:TEXCOORD3;fixed4 color:COLOR;};
            v2f vert(appdata_full v){v2f o;float flutter=saturate(1-v.texcoord.y/1.1)*_Wind;v.vertex.z+=sin(v.vertex.x*2.3+_CityTime*2.1)*flutter*.22; v.vertex.x+=sin(v.vertex.y*1.6+v.vertex.z+_CityTime*1.2)*_LeafWind*.09; o.pos=UnityObjectToClipPos(v.vertex);o.uv=v.texcoord;o.normal=UnityObjectToWorldNormal(v.normal);o.color=v.color;o.world=mul(unity_ObjectToWorld,v.vertex).xyz;o.distance=length(o.world-_WorldSpaceCameraPos);return o;}
            fixed4 frag(v2f i):SV_Target
            {
                float phase=_PaintFrame*_Motion,frame=fmod(floor(phase),4),next=fmod(frame+1,4);
                float2 uv=lerp(.009,.991,frac(i.uv*_TexScale));
                float3 a=tex2D(_MainTex,float2((uv.x+frame)*.25,uv.y)).rgb,b=tex2D(_MainTex,float2((uv.x+next)*.25,uv.y)).rgb;
                if(_SingleSurface>.5){
                    float3 surface=tex2D(_MainTex,i.uv*_TexScale).rgb;
                    float lum=dot(surface,float3(.299,.587,.114));
                    a=clamp(1+(lum-_SurfaceMean)*_SurfaceContrast,.45,1.35)*_SurfaceTint.rgb;b=a;
                }else if(_SurfaceTile>.5){
                    float2 offset=float2(_SurfaceTile==2||_SurfaceTile==4?.5:0,_SurfaceTile<2.5?.5:0);
                    float2 pixelUV=(floor(frac(i.uv*_TexScale)*128)+.5)/128;
                    a=tex2D(_MainTex,offset+lerp(.004,.496,pixelUV)).rgb;b=a;
                }
                a=lerp(1,a,_TextureWeight);b=lerp(1,b,_TextureWeight);
                float moon=saturate(dot(normalize(i.normal),CityMoonDirection()));
                float3 illumination=.29+floor(moon*4)/4*float3(.13,.17,.24)+CityLightAt(i.world)*.93;
                float indoorLight=saturate(dot(normalize(i.normal),normalize(float3(-.45,.8,-.4))));
                illumination=lerp(illumination,.64+floor(indoorLight*4)/4*.23,saturate(_Interior));
                if(_HomeOrigin.w>.5){
                    float3 p=i.world-_HomeOrigin.xyz;
                    float3 desk=p-float3(-4.62,2,-2.99),bed=p-float3(-5.01,1.79,3.03),window=p-float3(.7,2.75,4.10);
                    float warm=saturate(1-dot(desk,desk)/9)+saturate(1-dot(bed,bed)/9);
                    float cool=saturate(1-dot(window,window)/17);
                    illumination=illumination*.82+warm*float3(.35,.21,.08)+cool*float3(.055,.15,.22);
                }
                illumination=lerp(illumination,1.18,saturate(_Emission));
                float3 colorA=floor(saturate(i.color.rgb*a*illumination*_DecorTint.rgb)*63+.5)/63,colorB=floor(saturate(i.color.rgb*b*illumination*_DecorTint.rgb)*63+.5)/63;
                float3 color=lerp(colorA,colorB,smoothstep(0,1,frac(phase)));
                float luminance=dot(color,float3(.299,.587,.114));color=saturate(lerp(luminance.xxx,color,1.27)*1.06);
                return fixed4(lerp(color,float3(.035,.025,.03),saturate((i.distance-35)/110)*.85),1);
            }
            ENDCG
        }
    }
    Fallback "Unlit/Color"
}
