#version 330 core
out vec4 FragColor;
uniform float iTime;
uniform vec2 iResolution;
uniform vec2 iMouse;



// Funcao para desenhar circulo com borda suave
vec4 DrawCircle(vec2 uv, vec3 color, vec2 pos, float radius, float edge) {
    vec2 posn = pos / iResolution.xy;
    float dist = distance(uv, posn);
    float alpha = smoothstep(radius, radius - edge, dist);
    return vec4(color, alpha);
}

void main()
{
    vec2 uv = gl_FragCoord.xy / iResolution.xy;
    vec2 mouseNorm = iMouse.xy ;
    vec3 baseColor = 0.5 + 0.5 * cos(iTime + uv.xyx + vec3(0,2,4));

    vec4 circle = DrawCircle(uv, vec3(0.0, 0.0, 1.0), mouseNorm, 0.1, 0.01);

    // Mistura a cor base com o circulo usando o alpha do circulo
    FragColor = mix(vec4(baseColor, 1.0), circle, circle.a);

}


