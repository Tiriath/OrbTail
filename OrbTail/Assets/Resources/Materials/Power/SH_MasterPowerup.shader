Shader "Custom/SH_MasterPowerup"
{
    Properties
    {
        _RimColor("RimColor", Color) = (1, 0.95, 0.8, 1)
        _RimPower("RimPower", Range(0, 100)) = 2.0
        _RimScale("RimScale", Range(0, 10)) = 0.0
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
#pragma surface surf Standard fullforwardshadows alpha:fade
#pragma target 3.0

        sampler2D _Diffuse;
        sampler2D _PBR;

        struct Input
        {
            float2 uv_Diffuse;
            float3 viewDir;
        };

        fixed4 _RimColor;

        half _RimPower;
        half _RimScale;
        half _RimWobble;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half rim = pow(1.0f - saturate(dot(normalize(IN.viewDir), o.Normal)), _RimPower);

            o.Albedo = 0.0f;
            o.Alpha = rim;

            // Rim effect

            o.Emission = _RimColor * rim *_RimScale;
        }
         ENDCG
    }

    FallBack "Diffuse"
}
