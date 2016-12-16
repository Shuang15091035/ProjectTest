Shader "uapp/debug/ShowViewDir" {
	SubShader {
    
    	Tags { "RenderType" = "Opaque" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#pragma surface surf Lambert
    	
    	struct Input {
    		float3 viewDir;
    	};
    	
      	void surf (Input IN, inout SurfaceOutput o) {
      		o.Albedo = IN.viewDir * 0.5 + 0.5;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}
