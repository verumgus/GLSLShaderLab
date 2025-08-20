#version 330 core
out vec4 FragColor;
uniform float iTime;
uniform vec2 iResolution;

void main()
{
    vec2 uv = gl_FragCoord.xy / iResolution.xy;

    float wave = (cos(iTime + uv.x)*.2)+.2;
     wave = cos(iTime + uv.x * 4.0) * 0.8;

    float fs=2.0 + wave * 1.8;

    

    if(uv.y > (0.1*wave+0.2)){
    FragColor = vec4(.8,.0,.0, 1.0)*fs;
    }if(uv.y > (0.1* wave + 0.4)){
     FragColor = vec4(1.00, 0.82, 0.00, 1.0)*fs;
    }
    if(uv.y > (0.1 * wave + 0.7)){
    FragColor = vec4(0.78, 0.00, 0.00, 1.0)*fs;
    }


    //corte vertical
    if(uv.y < (0.1 * wave + 0.2)){
        
        FragColor = vec4(0);
    }

    
    if(uv.y > (0.1 * wave + 0.9)){
        FragColor = vec4(0);
    }
    //vertical cut 
    if(uv.x < 0.1){
     FragColor = vec4(0);
    }
    if(uv.x > 0.9){
     FragColor = vec4(0);
    }
    
}
