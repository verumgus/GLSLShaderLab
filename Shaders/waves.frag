#version 330 core
out vec4 FragColor;
uniform float iTime;
uniform vec2 iResolution;

void main()
{
    vec2 uv = (gl_FragCoord.xy - 0.5 * iResolution.xy) / iResolution.y;
    
    float wave1 = sin(uv.x * 10.0 + iTime * 2.0) * 0.1;
    float wave2 = sin(uv.x * 15.0 + iTime * 3.0) * 0.05;
    float wave3 = sin(uv.x * 8.0 + iTime * 1.5) * 0.08;
    
    float waves = wave1 + wave2 + wave3;
    
    float mask = smoothstep(0.02, 0.0, abs(uv.y - waves));
    
    vec3 waveColor = vec3(0.2, 0.6, 1.0);
    vec3 backgroundColor = vec3(0.05, 0.05, 0.2);
    
    vec3 color = mix(backgroundColor, waveColor, mask);
    
    FragColor = vec4(color, 1.0);
}