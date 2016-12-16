Shader "uapp/debug/ShowReflects" {
	SubShader {
    
    	Tags { "RenderType" = "Opaque" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#pragma surface surf Lambert
    	
    	struct Input {
    		float3 viewDir;
    		float3 worldNormal;
    	};
    	
      	void surf (Input IN, inout SurfaceOutput o) {
      		float3 R = reflect(-IN.viewDir, IN.worldNormal);
      		o.Albedo = R * 0.5 + 0.5;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}
