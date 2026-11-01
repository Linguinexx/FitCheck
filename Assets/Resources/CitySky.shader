Shader "FitCheck/Bairro Sky"
{
    Properties { _CityTime("Wind time",Float)=0 }
    SubShader {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _CityTime;
            struct vf { float4 pos:SV_POSITION; float3 ray:TEXCOORD0; };
            vf vert(appdata_base v) { vf o;o.pos=UnityObjectToClipPos(v.vertex);o.ray=v.vertex.xyz;return o; }
            float hash(float2 p){return frac(sin(dot(p,float2(127.1,311.7)))*43758.5453);}
            fixed4 frag(vf i):SV_Target {
                float3 d=normalize(i.ray);
                float h=saturate(d.y);
                float3 sky=lerp(float3(.075,.045,.050),float3(.008,.007,.012),smoothstep(0,.7,h));
                // Rounded, elongated cloud banks. Continuous drift; no frame stepping.
                float2 q=float2(atan2(d.x,d.z)*4.5+_CityTime*.012,d.y*6);
                float cloud=0;
                for(int layer=0;layer<2;layer++){
                    float2 p=q*float2(.8,1.2)+float2(layer*3.7,layer*2.2);
                    float2 cell=floor(p),f=frac(p);
                    float seed=hash(cell);
                    float shape=length((f-float2(.5,.46+seed*.08))*float2(1,1.8));
                    shape=min(shape,length(f-float2(.39,.57))+.14);
                    shape=min(shape,length(f-float2(.62,.56))+.16);
                    cloud=max(cloud,(1-smoothstep(.36,.40,shape))*step(.68,seed));
                }
                cloud*=smoothstep(.06,.16,d.y)*(1-smoothstep(.55,.75,d.y));
                sky=lerp(sky,float3(.12,.095,.105),cloud*.40);
                float moon=smoothstep(.998,.9984,dot(d,normalize(float3(-.7,.48,.65))));
                sky=lerp(sky,float3(.84,.77,.64),moon);
                float2 starUV=float2(atan2(d.x,d.z)*130,d.y*170);
                float star=step(.994,hash(floor(starUV)))*(1-smoothstep(.03,.09,length(frac(starUV)-.5)))*smoothstep(.1,.35,d.y);
                sky+=star*float3(.55,.48,.40);
                return fixed4(sky,1);
            }
            ENDCG
        }
    }
}
