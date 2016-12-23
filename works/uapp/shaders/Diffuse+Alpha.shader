Shader "uapp/Diffuse+Alpha" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
    
    	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#pragma surface surf Lambert alpha:blend
    	struct Input {
    		float2 uv_MainTex;
    	};
    	
    	float4 _Color;
      	sampler2D _MainTex;
      	void surf (Input IN, inout SurfaceOutput o) {
      		float4 mt = tex2D(_MainTex, IN.uv_MainTex);
      		o.Albedo = mt.rgb * _Color.rgb;
      		o.Alpha = mt.a * _Color.a;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}