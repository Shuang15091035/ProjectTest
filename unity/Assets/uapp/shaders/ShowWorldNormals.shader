Shader "uapp/debug/ShowWorldNormals" {
	SubShader {
    
    	Tags { "RenderType" = "Opaque" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#pragma surface surf Lambert
    	
    	struct Input {
    		float3 worldNormal;
    	};
    	
      	void surf (Input IN, inout SurfaceOutput o) {
      		o.Albedo = IN.worldNormal * 0.5 + 0.5;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}
