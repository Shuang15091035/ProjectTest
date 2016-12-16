#ifndef UAPP_INCLUDE
#define UAPP_INCLUDE

#define UAPP_Pi 3.1415926536
#define UAPP_2Pi 6.2831853072
#define UAPP_Pi2 1.5707963268

inline float3x3 mat4to3(float4x4 m) {
	return float3x3(
								m[0][0], m[0][1], m[0][2],
								m[1][0], m[1][1], m[1][2],
								m[2][0], m[2][1], m[2][2]);
}

// sphere map
inline float4 refl2DS(sampler2D _Reflective2D, float3 _EyeDir, float3 _Normal) {
	float3 R = reflect(_EyeDir, _Normal);
	
	// m = 2 * sqrt(Rx^2 + Ry^2 + (Rz + 1)^2)
	R.z += 1.0;
	float m = 2.0 * sqrt(dot(R, R));
	
	// u = Rx / m + 0.5
	// v = Ry / m + 0.5
	float2 uv = R.xy / m + float2(0.5, 0.5);
	
	float4 reflective = tex2D(_Reflective2D, uv);
	reflective.a = 0.0;
	return reflective;
}

// Latitude/Longitude map
// TODO 这种方式在Texture中如果设置了生成Minmap，则会出现边缘锯齿问题，估暂以取消Minmap解决
inline float4 refl2DL(sampler2D _Reflective2D, float3 _EyeDir, float3 _Normal) {
	float3 R = normalize(reflect(_EyeDir, _Normal));
	
	float u = atan2(R.x, R.z) / UAPP_2Pi + 0.5;
	float v = asin(R.y) / UAPP_Pi + 0.5;
	float2 uv = float2(u, v);
	
	float4 reflective = tex2D(_Reflective2D, uv);
	reflective.a = 0.0;
	return reflective;
}

inline float4 refl2DL2(sampler2D _Reflective2D, float3 _Reflect) {
	float3 R = _Reflect;
	
	float u = atan2(R.x, R.z) / UAPP_2Pi + 0.5;
	float v = asin(R.y) / UAPP_Pi + 0.5;
	float2 uv = float2(u, v);
	
	float4 reflective = tex2D(_Reflective2D, uv);
	reflective.a = 0.0;
	return reflective;
}

// cube map
inline float4 refl3D(samplerCUBE _Reflective3D, float3 _EyeDir, float3 _Normal) {
	float3 R = reflect(_EyeDir, _Normal);
	float4 reflective = texCUBE(_Reflective3D, R);
	reflective.a = 0.0;
	return reflective;
}

#endif