using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.IO;

namespace GLSLShaderLab
{
    public class Window : GameWindow
    {
        private int _vao;
        private Shader _shader;
        private float _time;

        public Window(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            float[] vertices = {
                -1f, -1f, 0f,
                 1f, -1f, 0f,
                 1f,  1f, 0f,
                -1f,  1f, 0f,
            };

            uint[] indices = {
                0, 1, 2,
                2, 3, 0
            };

            _vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            _time += (float)args.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();
            _shader.SetFloat("iTime", _time);
            _shader.SetVector2("iResolution", new Vector2(Size.X, Size.Y));

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }
    }
}
