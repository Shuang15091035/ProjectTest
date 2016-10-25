// 一般用于设置颜色像素

uniform highp vec4 color;
//      颜色使用高精度

//varying mediump vec2 v_uv; // 用于接收的uv

uniform sampler2D texture; // 贴图 sampler2D

void main() {
    //    vec4 t = texture2D(texture, v_uv);
    //    if (t.a < 0.1) {
    //        discard; // 如果执行了discard 后面的不会再执行
    //    }
//    gl_FragColor = color; // 贴图上添加颜色
    gl_FragColor = texture2D(texture,)
    //    gl_FragData = ;
}