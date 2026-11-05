Shader "FitCheck/Soft street light halos"
{
    SubShader{
        Tags{"Queue"="Transparent" "RenderType"="Transparent"}
        Pass{
            Blend SrcAlpha One ZWrite Off Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct input{float4 vertex:POSITION;float3 normal:NORMAL;float2 uv:TEXCOORD0;fixed4 color:COLOR;};
            struct output{float4 pos:SV_POSITION;float2 uv:TEXCOORD0;fixed4 color:COLOR;};
            output vert(input v){
                output o;float3 center=mul(unity_ObjectToWorld,v.vertex).xyz;
                float2 offset=(v.uv*2-1)*v.normal.x;
                center+=UNITY_MATRIX_V[0].xyz*offset.x+UNITY_MATRIX_V[1].xyz*offset.y;
                o.pos=mul(UNITY_MATRIX_VP,float4(center,1));o.uv=v.uv;o.color=v.color;return o;
            }
            fixed4 frag(output i):SV_Target{
                float2 p=i.uv*2-1;float radius=dot(p,p);
                float soft=pow(saturate(1-radius),3)*.30;
                float core=pow(saturate(1-radius*12),3)*.48;
                return fixed4(i.color.rgb,(soft+core)*i.color.a);
            }
            ENDCG
        }
    }
}
