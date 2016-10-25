

varying lowp vec4 DestinationColor; //fragment shader中，必须给出一个计算的精度。出于性能考虑，总使用最低精度是一个好习惯。这里就是设置成最低的精度。如果你需要，也可以设置成medp或者highp.

void main(void){
    
    gl_FragColor = DestinationColor;
    
}