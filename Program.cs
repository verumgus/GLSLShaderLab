using System;

namespace GLSLShaderLab
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new Window(800, 600, "GLSL Shader Lab"))
            {
                window.Run();
            }
        }
    }
}
