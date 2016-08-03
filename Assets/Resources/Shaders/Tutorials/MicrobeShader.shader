Shader "Blink Studios/Microbe" {
	Properties
	{
		_MainTex("Base (RGB) RefStrGloss (A)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5,0.5,0.5,1)
		_Shininess("Shininess", Range(0.01, 1)) = 0.078125
		_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
		_FresnelColor("Fresnel Color", Color) = (1,1,1,0.5)
		_Fresnel("Fresnel Exponent",Range(0.001,16)) = 1
		_Cube("Reflection Cubemap", Cube) = "" { TexGen CubeReflect }
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Boost("Fresnel Boost", Float) = 1

	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 400
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;

		fixed4 _Color;
		fixed4 _ReflectColor;
		half _Shininess;
		half _Fresnel;
		half _Boost;
		fixed4 _FresnelColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		};

		inline float Schlick2(float Rzero,float3 lightDir,float3 normal,float exponent)
		{
			return Rzero + (1 - Rzero) * pow((1 - dot(lightDir,normal)), exponent);
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c = tex * _Color;
			o.Albedo = c.rgb;
			o.Gloss = tex.a;
			o.Specular = _Shininess;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			half3 worldRefl = WorldReflectionVector(IN, o.Normal);
			fixed4 reflcol = texCUBE(_Cube, worldRefl);
			reflcol *= tex.a;
			half fresnel = Schlick2(0,normalize(IN.viewDir),o.Normal,_Fresnel);
			reflcol *= fresnel;
			o.Emission = reflcol.rgb * _ReflectColor.rgb;
			o.Emission += fresnel * _Boost * _FresnelColor;
		}
		ENDCG
	}
	FallBack "Reflective/Bumped Diffuse"
}