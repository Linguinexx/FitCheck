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
            float hash(float3 p){return frac(sin(dot(p,float3(127.1,311.7,74.7)))*43758.5453);}
            float noise(float3 p){
                float3 a=floor(p),f=frac(p);f=f*f*(3-2*f);
                return lerp(lerp(lerp(hash(a),hash(a+float3(1,0,0)),f.x),lerp(hash(a+float3(0,1,0)),hash(a+float3(1,1,0)),f.x),f.y),lerp(lerp(hash(a+float3(0,0,1)),hash(a+float3(1,0,1)),f.x),lerp(hash(a+float3(0,1,1)),hash(a+float3(1,1,1)),f.x),f.y),f.z);
            }
            fixed4 frag(vf i):SV_Target {
                float3 d=normalize(i.ray);
                float h=saturate(d.y);
                float3 sky=lerp(float3(.105,.085,.057),float3(.007,.014,.016),smoothstep(0,.65,h));
                float3 p=d*5+float3(_CityTime*.012,0,_CityTime*.007);
                float cloud=noise(p)*.57+noise(p*2.03+7)*.28+noise(p*4.01-11)*.15;
                float veil=smoothstep(.43,.70,cloud)*smoothstep(.015,.16,h)*(1-smoothstep(.78,.98,h));
                float moonDot=dot(d,normalize(float3(-.7,.55,.65)));
                float halo=pow(saturate(moonDot),110)*.16;
                sky+=halo*float3(.60,.55,.39);
                float disk=smoothstep(.99936,.99951,moonDot);
                float shadow=smoothstep(.99939,.99954,dot(d,normalize(float3(-.683,.557,.665))));
                sky=lerp(sky,float3(.87,.83,.67)*(1-shadow*.82),disk);
                sky=lerp(sky,lerp(float3(.10,.115,.10),float3(.25,.24,.19),halo),veil*.84);
                float3 stars=floor(d*420);float seed=hash(stars);
                float pin=pow(saturate(1-length(frac(d*420)-.5)*2.3),4)*step(.9965,seed)*smoothstep(.12,.38,h);
                sky+=pin*(.65+.35*sin(_CityTime*(.5+seed)+seed*90))*(1-veil)*float3(.7,.69,.57);
                return fixed4(sky,1);
            }
            ENDCG
        }
    }
}
