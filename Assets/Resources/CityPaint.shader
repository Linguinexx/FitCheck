Shader "FitCheck/City Painted"
{
    Properties { _DecorTint("Decoration tint",Color)=(1,1,1,1) _MainTex("Paint atlas",2D)="white"{} _SingleSurface("Imported surface",Float)=0 _SurfaceMean("Surface mean",Float)=.5 _SurfaceContrast("Surface contrast",Float)=3 _SurfaceTint("Surface tint",Color)=(1,1,1,1) _SurfaceTile("Urban atlas tile",Float)=0 _TexScale("Material scale",Float)=1 _Emission("Light",Float)=0 _Interior("Indoor lighting",Float)=0 _LeafWind("Leaves",Float)=0 _Motion("Paint movement",Float)=1 _TextureWeight("Paint detail",Float)=.35 _Wind("Banner wind",Float)=0 [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull",Float)=2 }
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
            sampler2D _MainTex;float _Motion,_PaintFrame,_TextureWeight,_Wind,_CityTime,_SurfaceTile,_TexScale,_Emission,_LeafWind,_Interior,_SingleSurface,_SurfaceMean,_SurfaceContrast;float4 _SurfaceTint,_DecorTint;
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
                float light=saturate(dot(normalize(i.normal),normalize(float3(-.45,.8,-.4))));
                float2 p=i.world.xz-float2(200,0);
                float pool=exp(-dot(p-float2(-1.75,-4),p-float2(-1.75,-4))/22)+exp(-dot(p-float2(18.25,-7),p-float2(18.25,-7))/24);
                float shop=exp(-pow((p.y-3.3)/3.6,2))*exp(-pow((p.x-7.5)/11,2));
                float cafe=exp(-pow((p.y-4.6)/3,2))*exp(-pow((p.x+17)/7,2));
                float westLamp=clamp(round((p.x+134)/18)*18-134,-134,-26);
                float westPool=exp(-dot(p-float2(westLamp+.75,-7.4),p-float2(westLamp+.75,-7.4))/28)*step(p.x,-24);
                float westFront=exp(-pow((p.y-5.2)/4,2))*step(-143,p.x)*step(p.x,-25);
                float court=exp(-dot((p-float2(-76,42))/float2(13,12),(p-float2(-76,42))/float2(13,12)));
                float civic=exp(-dot((p-float2(-97,33))/float2(15,8),(p-float2(-97,33))/float2(15,8)));
                float venue=exp(-dot((p-float2(-49,-20))/float2(12,6),(p-float2(-49,-20))/float2(12,6)));
                float shade=.30+floor(light*4)/4*.17+pool*.40+shop*.33+cafe*.29+westPool*.44+westFront*.28+court*.32+civic*.32+venue*.32;
                shade=lerp(shade,.64+floor(light*4)/4*.23,saturate(_Interior));
                shade=lerp(shade,1.18,saturate(_Emission));
                float3 colorA=floor(saturate(i.color.rgb*a*shade*_DecorTint.rgb)*63+.5)/63,colorB=floor(saturate(i.color.rgb*b*shade*_DecorTint.rgb)*63+.5)/63;
                float3 color=lerp(colorA,colorB,smoothstep(0,1,frac(phase)));
                float luminance=dot(color,float3(.299,.587,.114));color=saturate(lerp(luminance.xxx,color,1.27)*1.06);
                return fixed4(lerp(color,float3(.035,.025,.03),saturate((i.distance-35)/110)*.85),1);
            }
            ENDCG
        }
    }
    Fallback "Unlit/Color"
}
