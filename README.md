# GLSLShaderLab

Um laboratório interativo para experimentar com shaders GLSL, perfeito para estudantes aprenderem programação de shaders.

## 🚀 Como Usar

1. **Execute o programa**dotnet run
2. **Selecione um shader**
   - O programa irá mostrar uma lista de todos os shaders disponíveis
   - Digite o número correspondente ao shader que deseja testar
   - Digite 'r' para recarregar a lista caso adicione novos shaders

3. **Experimente!**
   - A janela abrirá com o shader selecionado em execução
   - O título da janela mostrará qual shader está sendo usado

## 📁 Estrutura de Shaders

Os shaders devem estar organizados no diretório `Shaders/` e seguir a convenção:
- `nomeDoShader.vert` - Vertex shader
- `nomeDoShader.frag` - Fragment shader

### Shaders Incluídos

1. **shader** - Shader colorido animado básico
2. **waves** - Efeito de ondas animadas

## 🎨 Criando Seus Próprios Shaders

Para criar um novo shader:

1. Crie dois arquivos no diretório `Shaders/`:
   - `meuShader.vert`
   - `meuShader.frag`

2. O vertex shader básico pode ser:#version 330 core
layout(location = 0) in vec3 aPos;
void main()
{
    gl_Position = vec4(aPos, 1.0);
}
3. No fragment shader você tem acesso a:
   - `uniform float iTime` - Tempo em segundos
   - `uniform vec2 iResolution` - Resolução da tela
   - `gl_FragCoord` - Coordenadas do pixel atual

4. Execute o programa novamente ou use 'r' para recarregar

## 🛠️ Uniforms Disponíveis

- `iTime` - Tempo decorrido em segundos (útil para animações)
- `iResolution` - Resolução da janela (vec2)

## 📚 Exemplos de Fragment Shaders

### Gradiente Simples#version 330 core
out vec4 FragColor;
uniform vec2 iResolution;

void main()
{
    vec2 uv = gl_FragCoord.xy / iResolution.xy;
    FragColor = vec4(uv, 0.5, 1.0);
}
### Círculo Animado#version 330 core
out vec4 FragColor;
uniform float iTime;
uniform vec2 iResolution;

void main()
{
    vec2 uv = (gl_FragCoord.xy - 0.5 * iResolution.xy) / iResolution.y;
    float dist = length(uv);
    float circle = smoothstep(0.3, 0.25, dist);
    vec3 color = vec3(circle) * (0.5 + 0.5 * sin(iTime));
    FragColor = vec4(color, 1.0);
}
## 🎯 Dicas para Estudantes

1. **Comece simples** - Modifique os shaders existentes antes de criar novos
2. **Use iTime** para criar animações
3. **Normalize coordenadas** com `gl_FragCoord.xy / iResolution.xy`
4. **Experimente funções** como `sin()`, `cos()`, `smoothstep()`, `mix()`
5. **Use cores HSV** para transições de cor mais suaves

## 🔧 Requisitos

- .NET 7 ou superior
- OpenTK 4.7.6
