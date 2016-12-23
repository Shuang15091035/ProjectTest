Shader "uapp/PBR" { // NOTE for pc
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _AO ("Ambient Occlusion Texture", 2D) = "white" {}
        _MetallicGlossMap ("Metallic (R) and Smoothness (A)", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float4 _Color;
        float _Glossiness;
        
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _AO;
        sampler2D _MetallicGlossMap;

        struct Input {
        	float2 uv_MainTex;
        	float2 uv_BumpMap;
            float2 uv_AO;
            float2 uv_MetallicGlossMap;
            INTERNAL_DATA
        };

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 mt = tex2D(_MainTex, IN.uv_MainTex);
            mt *= _Color;
            half4 ao = tex2D(_AO, IN.uv_AO);
            mt *= ao;
            half4 mm = tex2D(_MetallicGlossMap, IN.uv_MetallicGlossMap);
            float3 normals = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)).rgb;
            o.Normal = normals;

            o.Albedo = mt.rgb;
            o.Metallic = mm.r;
			o.Smoothness = mm.a;
			o.Alpha = mt.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
