Shader "uapp/debug/ShowNormalMap" {
	Properties {
        _NormalMap ("Normal Map", 2D) = "bump" {}
    }
	SubShader {
    
    	Tags { "RenderType" = "Opaque" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#pragma surface surf Lambert
    	
    	struct Input {
    		float3 viewDir;
    		float3 worldNormal;
    		float2 uv_NormalMap;
    	};
    	
    	sampler2D _NormalMap;
      	void surf (Input IN, inout SurfaceOutput o) {
      		float3 N = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap)).rgb;
      		o.Albedo = N * 0.5 + 0.5;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}
