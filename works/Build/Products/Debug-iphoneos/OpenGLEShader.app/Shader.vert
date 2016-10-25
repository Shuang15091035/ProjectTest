//一般用于设置顶点

attribute mediump vec3 position; // 设置顶点
//        精准度  还有 highp lowp
//attribute mediump vec2 uv; // 系统给出的uv
//
//uniform mediump mat4 u_modelview; // 定义矩阵

//varying mediump vec2 v_uv; // 用于传出的uv

void main() {
    gl_Position =  vec4(position, 1.0); // 输出结果
    //    v_uv = uv; // 赋值 传出uv
}