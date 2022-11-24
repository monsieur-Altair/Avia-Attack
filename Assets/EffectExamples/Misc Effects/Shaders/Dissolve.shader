Shader "Custom/Dissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		[HDR]_Emission ("Emission", Color) = (0,0,0,0)
		_MainTex ("Albedo", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_MetallicGlossMap ("Metallic (RGB) Smooth (A)", 2D) = "white" {}
		_AO ("AO", 2D) = "white" {}
		[HDR]_EdgeColor1 ("Edge Color", Color) = (1,1,1,1)
		_Noise ("Noise", 2D) = "white" {}
		[Toggle] _Use_Gradient ("Use Gradient?", Float) = 1
		_Gradient ("Gradient", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_MetallicValue ("Metallic", Range(0,1)) = 0.0
		[PerRendererData]_Cutoff ("Cutoff", Range(0,1)) = 0.0
		_EdgeSize ("EdgeSize", Range(0,1)) = 0.2
		_NoiseStrength ("Noise Strength", Range(0,1)) = 0.4
		_DisplaceAmount ("Displace Amount", Float) = 1.5
		_cutoff ("cutoff", Range(0,1)) = 0.0

		
	}
	SubShader {
		Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True" }
		Cull Off

		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow 

		#pragma target 3.0
		#pragma multi_compile __ _USE_GRADIENT_ON

		sampler2D _MainTex;
		sampler2D _Noise;
		sampler2D _Gradient;
		sampler2D _BumpMap;
		sampler2D _MetallicGlossMap;
		sampler2D _AO;

		struct Input {
			float2 uv_Noise;
			float2 uv_MainTex;
			fixed4 color : COLOR0;
			float3 worldPos;
		};


		half _Glossiness, _MetallicValue, _Cutoff, _EdgeSize, _NoiseStrength, _DisplaceAmount;
		half _cutoff;
		half4 _Color, _EdgeColor1, _Emission;


		void vert (inout appdata_full v, out Input o) {
        	UNITY_INITIALIZE_OUTPUT(Input,o);
    	  }

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half3 Noise = tex2D (_Noise, IN.uv_Noise);
			Noise.r = lerp(0, 1, Noise.r);
			half4 MetallicSmooth = tex2D (_MetallicGlossMap, IN.uv_MainTex);
			_cutoff  = lerp(0, _cutoff + _EdgeSize, _cutoff);
			half Edge = smoothstep(_cutoff + _EdgeSize, _cutoff, clamp(Noise.r, _EdgeSize, 1));
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed3 EmissiveCol = c.a * _Emission;

			o.Albedo = c;
			o.Occlusion = tex2D (_AO, IN.uv_MainTex);
			o.Emission = EmissiveCol + _EdgeColor1 * Edge;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Metallic = MetallicSmooth.r * _MetallicValue;
			o.Smoothness = MetallicSmooth.a * _Glossiness;
			clip(Noise - _cutoff);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
