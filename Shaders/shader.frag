#version 330 core
out vec4 FragColor;
uniform float iTime;
uniform vec2 iResolution;

void main()
{
    vec2 uv = gl_FragCoord.xy / iResolution.xy;
    vec3 color = 0.5 + 0.5 * cos(iTime + uv.xyx + vec3(0,2,4));
    FragColor = vec4(color, 1.0);
}
