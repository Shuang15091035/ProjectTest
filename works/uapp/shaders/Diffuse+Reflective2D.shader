Shader "uapp/Diffuse+Reflective2D" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		//_ReflectiveColor ("Reflective Color", Color) = (0.5, 0.5, 0.5, 1)
		_Reflective2D ("Reflective (RGB)", 2D) = "black" {}
    }
    SubShader {
    
    	Tags { "RenderType" = "Opaque" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#include "./uapp.cginc"
    	#pragma surface surf Lambert

    	struct Input {
    		float3 viewDir;
    		float3 worldPos;
    		float3 worldNormal;
    		float2 uv_MainTex;
    	};

    	float4 _Color;
    	float4 _ReflectiveColor;
      	sampler2D _MainTex;
      	sampler2D _Reflective2D;
      	void surf (Input IN, inout SurfaceOutput o) {
      		half4 mt = tex2D(_MainTex, IN.uv_MainTex);
      		float4 rf = refl2DL(_Reflective2D, -IN.viewDir, IN.worldNormal);
      		
      		o.Albedo = mt.rgb * _Color.rgb;
      		o.Emission = rf.rgb;
      		o.Alpha = mt.a;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}