#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;
in vec3 WorldPos;

uniform float iTime;
uniform vec2 iResolution;
uniform vec3 viewPos;

vec3 lightDir = vec3(0.45,-1.0,0.0);// ROt, UP DOWN, Power;

void main()
{

    float brightness = dot(Normal, lightDir);
    float darkness = clamp(dot(Normal,-lightDir),0,1) ;
    // Convert from pixel coordinates to normalized coordinates
    // vec2 uv = gl_FragCoord.xy / iResolution.xy;
    vec2 uv = TexCoord;


    // Calclulate the aspect ratio
    float prop=iResolution.x/iResolution.y;

    // Adjust the UV coordinates to maintain aspect ratio and center the pattern
    float x=  length(vec2(uv.x*prop,uv.y)-vec2(prop/2.0,0.5));

    // white background
    FragColor = vec4(1,1,1, 1.0);

    // red sprehe in center and 0.3 radius of the sphere
    if(x < 0.3) {
        FragColor = vec4(1,0,0, 1.0)*darkness;
    }
   
}
