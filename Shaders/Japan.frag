#version 330 core
out vec4 FragColor;
uniform float iTime;
uniform vec2 iResolution;
#define PI 3.1415926535

#define ROT(x) mat2(cos(x), -sin(x), sin(x), cos(x))

void main()
{
    

    vec2 uv = gl_FragCoord.xy / iResolution.xy;
    uv-=.5;
    uv.x*=iResolution.x/iResolution.y;
    float w = sin((uv.x + uv.y - iTime * .75 + sin(1.5 * uv.x + 4.5 * uv.y) * PI * .3) * PI * .6);

    uv*= 1. + (.036 - .036 * w);

    float fs=2.7 + w * 1.8;
    float d = length(uv);
    float c = d;
    
    if(d<.3)c=0.1;else c=1.0;
    FragColor = vec4(1.,vec2(c),1.0)* fs ;


    //corte vertical b
    if(uv.y < (0.1 * w -0.5)){
        
        FragColor = vec4(0);
    }

    
    if(uv.y > (0.1 * w + 0.5)){
        FragColor = vec4(0);
    }
    //vertical cut 
    if(uv.x < -0.8){
     FragColor = vec4(0);
    }
    if(uv.x > 0.8){
     FragColor = vec4(0);
    }
    
}
