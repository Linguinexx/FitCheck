Shader "FitCheck/Preview/Character Ink"
{
    SubShader
    {
        Tags {"RenderType"="Opaque" "Queue"="Geometry"}
        Pass
        {
            Name "INK_PREVIEW"
            Cull Front
            ZWrite On
            ZTest LEqual
            Offset 1,1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _InkPreviewPixels,_InkPreviewWorldLimit,_InkEffectsDisabled;
            fixed4 _InkPreviewColor;
            struct v2f {float4 pos:SV_POSITION;float amount:TEXCOORD0;};
            v2f vert(appdata_base v){
                v2f o;o.pos=UnityObjectToClipPos(v.vertex);
                float3 view=UnityObjectToViewPos(v.vertex);
                float3 normal=mul((float3x3)UNITY_MATRIX_V,UnityObjectToWorldNormal(v.normal));
                float2 projection=float2(UNITY_MATRIX_P._m00,UNITY_MATRIX_P._m11);
                float2 direction=normal.xy*projection*_ScreenParams.xy;
                direction/=max(length(direction),.0001);
                float depth=max(.1,-view.z),fade=1-smoothstep(12,28,depth);
                float worldPixels=_InkPreviewWorldLimit*abs(projection.y)*_ScreenParams.y/(2*depth);
                float pixels=min(_InkPreviewPixels*_ScreenParams.y/720,worldPixels)*fade;
                o.pos.xy+=direction*(2*pixels/_ScreenParams.xy)*o.pos.w;
                o.amount=pixels;return o;
            }
            fixed4 frag(v2f i):SV_Target {clip(.5-_InkEffectsDisabled);clip(i.amount-.03);return _InkPreviewColor;}
            ENDCG
        }
    }
}
