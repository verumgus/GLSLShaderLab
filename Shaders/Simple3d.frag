#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;
in vec3 WorldPos;

uniform float iTime;
uniform vec2 iResolution;
uniform vec3 viewPos;

vec3 lightDir = vec3(0.45,1.0,0.0);// ROt, UP DOWN, Power;

void main()
{
    float brightness = dot(Normal, lightDir);
    float darkness = clamp(dot(Normal,-lightDir),0,1) ;

    FragColor = vec4(1.0,1.0,1.0,1.0)*darkness;
}