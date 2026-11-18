Shader "FitCheck/Preview/Ink Fabric"
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
        UsePass "FitCheck/Blender Garment Fabric/PAINT"
        UsePass "FitCheck/Preview/Character Ink/INK_PREVIEW"
    }
}
