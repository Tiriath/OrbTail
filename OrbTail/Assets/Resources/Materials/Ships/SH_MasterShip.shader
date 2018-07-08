Shader "Custom/SH_MasterShip" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Diffuse ("Diffuse (RGB)", 2D) = "white" {}
        _PBR("PBR (Roughness, Metalness, Emissivity)", 2D) = "white" {}
        _Roughness("Roughness", Range(0,1)) = 1.0
        _Metalness("Metalness", Range(0,1)) = 1.0
        _Emissivity("Emissivity", Range(0,10)) = 2.0
        _RimColor("RimColor", Color) = (1, 0.95, 0.8, 1)
        _RimPower("RimPower", Range(0, 100)) = 2.0
        _RimScale("RimScale", Range(0, 10)) = 0.0
        _Desaturate("Desaturate", Range(0,1)) = 0.0
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
            float3 viewDir;
        };

        fixed4 _Color;
        fixed4 _RimColor;

        half _Roughness;
        half _Metalness;
        half _Emissivity;
        half _Desaturate;
        half _RimPower;
        half _RimScale;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 diffuse = tex2D(_Diffuse, IN.uv_Diffuse);
            fixed4 pbr = tex2D(_PBR, IN.uv_Diffuse);

            fixed3 desaturated_diffuse = dot(diffuse, fixed3(0.299f, 0.587f, 0.114f));

            // Base PBR

            o.Albedo = lerp(diffuse.rgb, desaturated_diffuse, _Desaturate);
            o.Smoothness = 1.0f - pbr.r * _Roughness;
            o.Metallic = pbr.g * _Metalness;
            o.Emission = lerp(_Color, 1, _Desaturate) * pbr.b * _Emissivity;
            o.Alpha = diffuse.a;

            // VFX

            half rim = pow(1.0f - saturate(dot(normalize(IN.viewDir), o.Normal)), _RimPower);

            o.Emission += _RimColor * rim *_RimScale;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
