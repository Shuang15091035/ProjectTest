Shader "uapp/Transparent+LightMap" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("LightMap (RGB)", 2D) = "white" {}
		_Transparent ("Transparent (A)", 2D) = "white" {}
    }
    SubShader {
    
    	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#pragma surface surf Lambert alpha:blend
    	struct Input {
    		float2 uv_MainTex;
    		float2 uv2_LightMap;
    	};
    	
    	float4 _Color;
      	sampler2D _MainTex;
      	sampler2D _LightMap;
      	sampler2D _Transparent;
      	void surf (Input IN, inout SurfaceOutput o) {
      		float4 mt = tex2D(_MainTex, IN.uv_MainTex);
      		float4 lm = tex2D(_LightMap, IN.uv2_LightMap);
      		float4 t = tex2D(_Transparent, IN.uv_MainTex);
      		
      		o.Albedo = (mt.rgb * _Color.rgb) * lm.rgb;
      		//o.Emission = lm.rgb;
      		o.Alpha = t.a;
      }
      ENDCG
  }
  Fallback "Diffuse"
}
