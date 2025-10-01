#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;
in vec3 WorldPos;

uniform float iTime;
uniform vec2 iResolution;
uniform vec3 viewPos;
uniform sampler2D texture0;
uniform sampler2D texture1; 

vec3 lightDir = vec3(0.2,-1.0,-0.2);
float ambient = 0.2;

void main()
{
    float brightness = clamp(dot(Normal, -lightDir), 0.0, 1.0);
    vec4 texColor = texture(texture1, TexCoord); // <- usa texture1
    vec4 texColor1 = texture(texture0, TexCoord); // <- usa texture1

    FragColor = texColor * (brightness + ambient);

    FragColor = mix(texColor, texColor1,1 - cos(brightness))* (brightness+ambient);
}
