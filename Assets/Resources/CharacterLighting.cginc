#ifndef FITCHECK_CHARACTER_LIGHTING
#define FITCHECK_CHARACTER_LIGHTING
// Broad warm key and a restrained red edge, inspired by the cover. No extra
// lights, shadow maps, texture samples or material instances per character.
float3 CharacterPaintLight(float3 original,float3 albedo,float3 normal,float3 view,float enabled,float rimAllowed)
{
    float3 n=normalize(normal),v=normalize(view);
    float key=smoothstep(.02,.92,saturate(dot(n,normalize(float3(-.65,.70,-.65)))));
    float shade=.68+.42*key;
    float3 tint=lerp(float3(.90,.92,1),float3(1.18,1.02,.82),key);
    float edge=1-saturate(dot(n,v));edge=edge*edge*edge;
    float3 cameraRight=normalize(UNITY_MATRIX_V[0].xyz);
    edge*=smoothstep(.05,.85,dot(n,cameraRight))*rimAllowed;
    float3 lit=albedo*shade*tint+edge*(albedo*.40+float3(.34,.018,.008));
    return lerp(original,saturate(lit),saturate(enabled)*.82);
}
#endif
