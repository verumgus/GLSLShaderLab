using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GLSLShaderLab
{
    public class Shader
    {
        public int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexCode = File.ReadAllText(vertexPath);
            string fragmentCode = File.ReadAllText(fragmentPath);

            int vertex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertex, vertexCode);
            GL.CompileShader(vertex);
            Console.WriteLine(GL.GetShaderInfoLog(vertex));

            int fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragment, fragmentCode);
            GL.CompileShader(fragment);
            Console.WriteLine(GL.GetShaderInfoLog(fragment));

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertex);
            GL.AttachShader(Handle, fragment);
            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, vertex);
            GL.DetachShader(Handle, fragment);
            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);
        }

        public void Use() => GL.UseProgram(Handle);

        public void SetFloat(string name, float value) =>
            GL.Uniform1(GL.GetUniformLocation(Handle, name), value);

        public void SetVector2(string name, Vector2 value) =>
            GL.Uniform2(GL.GetUniformLocation(Handle, name), value);
    }
}
