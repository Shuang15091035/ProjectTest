Shader "uapp/VertexColor" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
    }
    SubShader {
    
    	Tags { "RenderType" = "Opaque" }
    	LOD 200
    	
    	Lighting Off
    	
    	CGPROGRAM
    	#pragma surface surf Lambert
    	struct Input {
    		float4 color : COLOR;
    	};
    	
    	float4 _Color;
      	void surf (Input IN, inout SurfaceOutput o) {
      		o.Albedo = _Color.rgb * IN.color;
      	}
      	ENDCG
  }
  Fallback "Diffuse"
}