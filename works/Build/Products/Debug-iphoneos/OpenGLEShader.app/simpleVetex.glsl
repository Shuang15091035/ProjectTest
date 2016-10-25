attribute vec4 Position;//attribute表示shader 会接收一个传入变量，varying表示一个传出变量，vet4表示由四部分组成的矢量
attribute vec4 SourceColor;

varying vec4 DestinationColor;//varying”关键字表示，依据顶点的颜色，平滑计算出顶点之间每个像素的颜色。

void main(){
    DestinationColor = SourceColor;
    gl_Position = Position;//没做任何逻辑运算
}