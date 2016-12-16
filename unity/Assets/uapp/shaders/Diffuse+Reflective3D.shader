Shader "uapp/Diffuse+Reflective3D" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ReflectiveColor ("Reflective Color", Color) = (0.5, 0.5, 0.5, 1)
		_Reflective3D ("Reflective (RGB)", CUBE) = "black" {}
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
    		float3 worldRefl;
    		float2 uv_MainTex;
    		INTERNAL_DATA
    	};
    	
    	float4 _Color;
    	float4 _ReflectiveColor;
      	sampler2D _MainTex;
      	samplerCUBE _Reflective3D;
      	void surf (Input IN, inout SurfaceOutput o) {
      		half4 mt = tex2D(_MainTex, IN.uv_MainTex);
      		//o.Normal = IN.worldNormal;
      		//float4 rf = texCUBE(_Reflective3D, IN.worldRefl);
      		float4 rf = refl3D(_Reflective3D, -IN.viewDir, IN.worldNormal);
      		
      		o.Albedo = mt.rgb * _Color.rgb;
      		o.Emission = rf.rgb;
      		o.Alpha = mt.a;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}