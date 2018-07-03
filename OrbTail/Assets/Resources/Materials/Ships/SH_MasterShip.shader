Shader "Custom/SH_MasterShip" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Diffuse ("Diffuse (RGB)", 2D) = "white" {}
        _PBR("PBR (Roughness, Metalness, Emissivity)", 2D) = "white" {}
        _Roughness("Roughness", Range(0,1)) = 1.0
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

        sampler2D _Diffuse;
        sampler2D _PBR;

        struct Input
        {
            float2 uv_Diffuse;
        };

        fixed4 _Color;

        half _Roughness;
        half _Metalness;
        half _Emissivity;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 diffuse = tex2D(_Diffuse, IN.uv_Diffuse);
            fixed4 pbr = tex2D(_PBR, IN.uv_Diffuse);

            o.Albedo = diffuse.rgb;
            o.Smoothness = 1.0f - pbr.r * _Roughness;
            o.Metallic = pbr.g * _Metalness;
            o.Emission = _Color * pbr.b * _Emissivity;
            o.Alpha = diffuse.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
