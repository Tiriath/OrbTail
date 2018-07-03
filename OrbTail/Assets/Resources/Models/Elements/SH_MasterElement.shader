Shader "Custom/SH_MasterElement" {
    Properties {
        _Albedo ("Color", Color) = (1,1,1,1)
        _Roughness("Roughness", Range(0,1)) = 0.0
        _Metalness("Metalness", Range(0,1)) = 1.0
        _Emissivity("Emissivity", Range(0,10)) = 2.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_Diffuse;
        };

        fixed4 _Albedo;

        half _Roughness;
        half _Metalness;
        half _Emissivity;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Albedo.rgb;
            o.Smoothness = 1.0f - _Roughness;
            o.Metallic = _Metalness;
            o.Emission = _Albedo * _Emissivity;

            o.Alpha = _Albedo.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
