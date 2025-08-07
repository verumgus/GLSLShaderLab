using System;

namespace GLSLShaderLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var shaderSelector = new ShaderSelector();
            var selectedShader = shaderSelector.SelectShader();
            
            if (selectedShader != null)
            {
                using (var window = new Window(800, 600, "GLSL Shader Lab", selectedShader))
                {
                    window.Run();
                }
            }
            else
            {
                Console.WriteLine("Nenhum shader selecionado. Encerrando programa.");
            }
        }
    }
}
