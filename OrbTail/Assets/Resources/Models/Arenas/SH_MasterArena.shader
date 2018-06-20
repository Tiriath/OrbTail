Shader "Custom/SH_MasterArena"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Diffuse("Diffuse (RGB)", 2D) = "white" {}
        _PBR("PBR (Roughness, Metalness, Emissivity)", 2D) = "white" {}
        _Normal("Normal", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags{ "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM

#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

        sampler2D _Diffuse;
        sampler2D _PBR;
        sampler2D _Normal;

        struct Input
        {
            float2 uv_Diffuse;
        };

        fixed4 _Color;

        half _Glossiness;
        half _Metallic;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 diffuse = tex2D(_Diffuse, IN.uv_Diffuse);
            fixed4 pbr = tex2D(_PBR, IN.uv_Diffuse);

            o.Albedo = diffuse.rgb;
            o.Smoothness = 1.0f - pbr.r;
            o.Metallic = pbr.g;
            o.Emission = _Color * pbr.b;

            o.Alpha = diffuse.a;
        }
        ENDCG
    }
        FallBack "Diffuse"
}
