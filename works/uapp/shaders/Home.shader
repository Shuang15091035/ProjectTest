Shader "uapp/Home" { // NOTE for mobile
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		//_ReflectiveColor ("Reflective Color", Color) = (0.5, 0.5, 0.5, 1)
		_ReflAmount ("Reflection Amount", Range(0, 1)) = 0.5
		_RimColor ("Rim Color", Color) = (0.26, 0.19, 0.16, 0.0)
        _RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0
        
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("LightMap (RGB)", 2D) = "white" {}
		_Reflective2D ("Reflective (RGB)", 2D) = "black" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _AO ("Ambient Occlusion Texture", 2D) = "white" {}
    }
    
    SubShader {
    	
        Tags { "RenderType" = "Opaque" }
    	LOD 400
        
        Lighting Off
        
        CGPROGRAM
        #include "uapp.cginc"
        #pragma surface surf BlinnPhong
        #pragma target 3.0
        
        float4 _Color;
        half _Shininess;
        //float4 _ReflectiveColor;
        float _ReflAmount;
        float4 _RimColor;
        float _RimPower;
        
        sampler2D _MainTex;
        sampler2D _LightMap;
        sampler2D _Reflective2D;
        sampler2D _BumpMap;
        sampler2D _AO;

        struct Input {
        	float2 uv_MainTex;
        	float2 uv2_LightMap;
        	float2 uv_BumpMap;
            float2 uv_AO;
            float3 worldPos;
            float3 worldNormal;
            float3 worldRefl;
            float3 viewDir;
            INTERNAL_DATA
        };

        void surf(Input IN, inout SurfaceOutput o) {            
            half4 mt = tex2D(_MainTex, IN.uv_MainTex);
            mt *= _Color;
            half4 lm = tex2D(_LightMap, IN.uv2_LightMap);
            mt *= lm;
            half4 ao = tex2D(_AO, IN.uv_AO);
            mt *= ao;
            float3 normals = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)).rgb;
            o.Normal = normals;
            
            //float4 rf = refl2DL(_Reflective2D, -IN.viewDir, normals);
            float4 rf = refl2DL2(_Reflective2D, WorldReflectionVector(IN, o.Normal));
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), normals));
            o.Albedo = mt.rgb;
            o.Specular = _Shininess;
            o.Gloss = mt.a;
            o.Emission = rf.rgb * _ReflAmount + _RimColor.rgb * pow(rim, _RimPower);
        }
        ENDCG
    }
    FallBack "Specular"
}
