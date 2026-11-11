#ifndef FITCHECK_CITY_LIGHTING
#define FITCHECK_CITY_LIGHTING
sampler2D _CityLightField;
float4 _CityLightRect;
float _CityLightActive;
float3 CityLightAt(float3 world)
{
    float2 uv=(world.xz-float2(200,0)-_CityLightRect.xy)*_CityLightRect.zw;
    float inside=step(0,uv.x)*step(uv.x,1)*step(0,uv.y)*step(uv.y,1);
    return tex2D(_CityLightField,saturate(uv)).rgb*inside*_CityLightActive*lerp(1,.20,saturate(world.y/20));
}
float3 CityMoonDirection(){return normalize(float3(-.7,.55,.65));}
float3 CityHaze(float3 color,float distance,float exterior)
{
    float mist=smoothstep(16,106,distance)*.58*saturate(exterior);
    return lerp(color,float3(.095,.102,.096),mist);
}
#endif
