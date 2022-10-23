Shader "Bullet"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BotEmissionColor ("Bot Emission Color", Color) = (1,1,1,1)
        _TopEmissionColor ("Top Emission Color", Color) = (1,1,1,1)
        _Coefficient ("Coefficient", Range(-1,1)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma vertex vert

        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float4 pos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _BotEmissionColor;
        fixed4 _TopEmissionColor;
        fixed _Coefficient;

        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.pos = v.vertex;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            const fixed height = IN.pos.z;  
            const fixed t = saturate(height - _Coefficient);

            o.Albedo = _Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a;
            o.Emission = lerp(_BotEmissionColor, _TopEmissionColor, t);
        }
        ENDCG
    }
    FallBack "Diffuse"
}