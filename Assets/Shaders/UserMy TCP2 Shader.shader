// Toony Colors Pro+Mobile 2
// (c) 2014-2022 Jean Moreno

Shader "Toony Colors Pro 2/User/My TCP2 Shader"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[MainTexture] _MainTex ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularShadowAttenuation ("Specular Shadow Attenuation", Float) = 0.25
		_SpecularRoughnessPBR ("Roughness", Range(0,1)) = 0.5
		[TCP2Separator]

		[TCP2HeaderHelp(Reflections)]
		[Toggle(TCP2_REFLECTIONS)] _UseReflections ("Enable Reflections", Float) = 0
		[TCP2ColorNoAlpha] _ReflectionColor ("Color", Color) = (1,1,1,1)
		_PlanarNormalsInfluence ("Reflection Normal Influence", Range(0,1)) = 0.1
		[HideInInspector] _ReflectionTex ("Planar Reflection RenderTexture", 2D) = "white" {}
		[TCP2Separator]
		
		[TCP2HeaderHelp(Normal Mapping)]
		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
		[TCP2Separator]
		
		// Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"IgnoreProjectors"="True"
		}

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Texture/Sampler abstraction
		#define TCP2_TEX2D_WITH_SAMPLER(tex)						UNITY_DECLARE_TEX2D(tex)
		#define TCP2_TEX2D_NO_SAMPLER(tex)							UNITY_DECLARE_TEX2D_NOSAMPLER(tex)
		#define TCP2_TEX2D_SAMPLE(tex, samplertex, coord)			UNITY_SAMPLE_TEX2D_SAMPLER(tex, samplertex, coord)
		#define TCP2_TEX2D_SAMPLE_LOD(tex, samplertex, coord, lod)	UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex, samplertex, coord, lod)

		// Shader Properties
		TCP2_TEX2D_WITH_SAMPLER(_BumpMap);
		TCP2_TEX2D_WITH_SAMPLER(_MainTex);
		
		// Shader Properties
		float4 _MainTex_ST;
		fixed4 _Color;
		float _RampThreshold;
		float _RampSmoothing;
		fixed4 _HColor;
		fixed4 _SColor;
		float _SpecularRoughnessPBR;
		float _SpecularShadowAttenuation;
		fixed4 _SpecularColor;
		float _PlanarNormalsInfluence;
		fixed4 _ReflectionColor;

		sampler2D _ReflectionTex;

		//Specular help functions (from UnityStandardBRDF.cginc)
		inline half3 SpecSafeNormalize(half3 inVec)
		{
			half dp3 = max(0.001f, dot(inVec, inVec));
			return inVec * rsqrt(dp3);
		}
		
		//GGX
		#define TCP2_PI			3.14159265359
		#define TCP2_INV_PI		0.31830988618f
		#if defined(SHADER_API_MOBILE)
			#define TCP2_EPSILON 1e-4f
		#else
			#define TCP2_EPSILON 1e-7f
		#endif
		inline half GGX(half NdotH, half roughness)
		{
			half a2 = roughness * roughness;
			half d = (NdotH * a2 - NdotH) * NdotH + 1.0f;
			return TCP2_INV_PI * a2 / (d * d + TCP2_EPSILON);
		}

		ENDCG

		// Main Surface Shader
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		Zwrite on

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha nolightmap nofog nolppv keepalpha alpha:blend
		#pragma target 3.0

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature_local_fragment TCP2_SPECULAR
		#pragma shader_feature_local_fragment TCP2_REFLECTIONS

		//================================================================
		// STRUCTS

		// Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			half4 tangent : TANGENT;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			half3 viewDir;
			half3 tangent;
			float4 screenPosition;
			float2 texcoord0;
		};

		//================================================================

		// Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;
			float3 normalTS;

			Input input;

			// Shader Properties
			float __rampThreshold;
			float __rampSmoothing;
			float3 __highlightColor;
			float3 __shadowColor;
			float __ambientIntensity;
			float __specularRoughnessPbr;
			float __specularShadowAttenuation;
			float3 __specularColor;
			float __planarNormalsInfluence;
			float3 __reflectionColor;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			// Texture Coordinates
			output.texcoord0.xy = v.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;

			float4 clipPos = UnityObjectToClipPos(v.vertex);

			// Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			output.screenPosition = screenPos;

			output.tangent = mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0)).xyz;

		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			// Shader Properties Sampling
			float4 __normalMap = ( TCP2_TEX2D_SAMPLE(_BumpMap, _BumpMap, input.texcoord0.xy).rgba );
			float4 __albedo = ( TCP2_TEX2D_SAMPLE(_MainTex, _MainTex, input.texcoord0.xy).rgba );
			float4 __mainColor = ( _Color.rgba );
			float __alpha = ( __albedo.a * __mainColor.a );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__ambientIntensity = ( 1.0 );
			output.__specularRoughnessPbr = ( _SpecularRoughnessPBR );
			output.__specularShadowAttenuation = ( _SpecularShadowAttenuation );
			output.__specularColor = ( _SpecularColor.rgb );
			output.__planarNormalsInfluence = ( _PlanarNormalsInfluence );
			output.__reflectionColor = ( _ReflectionColor.rgb );

			output.input = input;

			half4 normalMap = __normalMap;
			output.Normal = UnpackNormal(normalMap);
			output.normalTS = output.Normal;

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;

			output.Albedo *= __mainColor.rgb;

		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, half3 viewDir, UnityGI gi)
		{

			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				// extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			half3 ramp;
			
			#define		RAMP_THRESHOLD	surface.__rampThreshold
			#define		RAMP_SMOOTH		surface.__rampSmoothing
			ndl = saturate(ndl);
			ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);

			// Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;

			// Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			// Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

			#if defined(TCP2_SPECULAR)
			//Specular: GGX
			half3 halfDir = SpecSafeNormalize(lightDir + viewDir);
			half roughness = surface.__specularRoughnessPbr*surface.__specularRoughnessPbr;
			half nh = saturate(dot(normal, halfDir));
			half spec = GGX(nh, saturate(roughness));
			spec *= TCP2_PI * 0.05;
			#ifdef UNITY_COLORSPACE_GAMMA
				spec = max(0, sqrt(max(1e-4h, spec)));
				half surfaceReduction = 1.0 - 0.28 * roughness * surface.__specularRoughnessPbr;
			#else
				half surfaceReduction = 1.0 / (roughness*roughness + 1.0);
			#endif
			spec = max(0, spec * ndl);
			spec *= surfaceReduction;
			spec *= saturate(atten * ndl + surface.__specularShadowAttenuation);
			
			//Apply specular
			color.rgb += spec * lightColor.rgb * surface.__specularColor;
			#endif

			half3 reflections = half3(0, 0, 0);
			#if defined(TCP2_REFLECTIONS)
			float2 planarReflectionCoords = (surface.input.screenPosition.xy + (surface.normalTS.xy * surface.__planarNormalsInfluence)) / surface.input.screenPosition.w;
			reflections.rgb += tex2D(_ReflectionTex, planarReflectionCoords).rgb;
			#endif
			reflections *= surface.__reflectionColor;
			color.rgb += reflections;

			return color;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			// GI without reflection probes
			gi = UnityGlobalIllumination(data, 1.0, normal); // occlusion is applied in the lighting function, if necessary

			surface.atten = data.atten; // transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb; // remove attenuation

		}

		ENDCG

	}

	Fallback "Diffuse"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(ver:"2.9.2";unity:"2021.3.4f1";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2019_4","UNITY_2020_1","UNITY_2021_1","BUMP","CULLING","ALPHA_BLENDING","SHADER_BLENDING","SPEC_PBR_GGX","SPECULAR","SPECULAR_NO_ATTEN","SPECULAR_SHADER_FEATURE","PLANAR_REFLECTION","REFLECTION_SHADER_FEATURE"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0"];shaderProperties:list[];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 4e5508fdb3ae7fdc497784f15cbf8179 */
