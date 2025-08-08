using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace GLSLShaderLab
{
    public class Window : GameWindow
    {
        private Shader _shader;
        private Model _model;
        private float _time;
        private ShaderSelector.ShaderInfo _selectedShader;
        private ModelSelector.ModelInfo _selectedModel;
        private List<ShaderSelector.ShaderInfo> _availableShaders;
        private List<ModelSelector.ModelInfo> _availableModels;
        private int _currentShaderIndex;
        private int _currentModelIndex;
        private bool _showHelp;

        // Camera properties
        private Vector3 _cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
        private Vector3 _cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 _cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
        private float _yaw = -90.0f;
        private float _pitch = 0.0f;
        private float _fov = 45.0f;
        private bool _firstMove = true;
        private Vector2 _lastPos;

        // Model transformation
        private float _rotationY = 0.0f;

        public Window(int width, int height, string title, ShaderSelector.ShaderInfo selectedShader, ModelSelector.ModelInfo selectedModel)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = $"{title} - {selectedShader.Name} - {selectedModel.Name}" })
        {
            _selectedShader = selectedShader;
            _selectedModel = selectedModel;
            
            // Load available shaders and models
            var shaderSelector = new ShaderSelector();
            var modelSelector = new ModelSelector();
            _availableShaders = shaderSelector.GetAvailableShaders();
            _availableModels = modelSelector.GetAvailableModels();
            
            _currentShaderIndex = _availableShaders.FindIndex(s => s.Name == selectedShader.Name);
            if (_currentShaderIndex == -1) _currentShaderIndex = 0;
            
            _currentModelIndex = _availableModels.FindIndex(m => m.Name == selectedModel.Name);
            if (_currentModelIndex == -1) _currentModelIndex = 0;

            CursorState = CursorState.Grabbed; // Capture mouse for camera control
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            LoadShader(_selectedShader);
            LoadModel(_selectedModel);
            
            Console.WriteLine("Controles:");
            Console.WriteLine("  WASD    : Mover câmera");
            Console.WriteLine("  Mouse   : Olhar ao redor");
            Console.WriteLine("  Scroll  : Zoom");
            Console.WriteLine("  Q/E     : Trocar shaders");
            Console.WriteLine("  Z/X     : Trocar modelos");
            Console.WriteLine("  R       : Resetar câmera");
            Console.WriteLine("  H       : Mostrar/ocultar ajuda");
            Console.WriteLine("  ESC     : Sair");
            Console.WriteLine();
        }

        private void LoadShader(ShaderSelector.ShaderInfo shaderInfo)
        {
            try
            {
                _shader?.Dispose();
                _shader = new Shader(shaderInfo.VertexPath, shaderInfo.FragmentPath);
                _shader.Use();
                Title = $"GLSL Shader Lab - {shaderInfo.Name} - {_selectedModel.Name}";
                Console.WriteLine($"Shader carregado: {shaderInfo.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar shader {shaderInfo.Name}: {ex.Message}");
            }
        }

        private void LoadModel(ModelSelector.ModelInfo modelInfo)
        {
            try
            {
                _model?.Dispose();
                _model = new Model(modelInfo.FilePath);
                Title = $"GLSL Shader Lab - {_selectedShader.Name} - {modelInfo.Name}";
                Console.WriteLine($"Modelo carregado: {modelInfo.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar modelo {modelInfo.Name}: {ex.Message}");
            }
        }

        private void UpdateCamera()
        {
            var front = Vector3.Zero;
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_pitch));
            front.Z = (float)Math.Sin(MathHelper.DegreesToRadians(_yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(_pitch));
            _cameraFront = Vector3.Normalize(front);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Keys.Escape:
                    Close();
                    break;
                    
                case Keys.Q:
                    if (_availableShaders.Count > 1)
                    {
                        _currentShaderIndex = (_currentShaderIndex - 1 + _availableShaders.Count) % _availableShaders.Count;
                        _selectedShader = _availableShaders[_currentShaderIndex];
                        LoadShader(_selectedShader);
                    }
                    break;
                    
                case Keys.E:
                    if (_availableShaders.Count > 1)
                    {
                        _currentShaderIndex = (_currentShaderIndex + 1) % _availableShaders.Count;
                        _selectedShader = _availableShaders[_currentShaderIndex];
                        LoadShader(_selectedShader);
                    }
                    break;
                    
                case Keys.Z:
                    if (_availableModels.Count > 1)
                    {
                        _currentModelIndex = (_currentModelIndex - 1 + _availableModels.Count) % _availableModels.Count;
                        _selectedModel = _availableModels[_currentModelIndex];
                        LoadModel(_selectedModel);
                    }
                    break;
                    
                case Keys.X:
                    if (_availableModels.Count > 1)
                    {
                        _currentModelIndex = (_currentModelIndex + 1) % _availableModels.Count;
                        _selectedModel = _availableModels[_currentModelIndex];
                        LoadModel(_selectedModel);
                    }
                    break;
                    
                case Keys.R:
                    // Reset camera
                    _cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
                    _yaw = -90.0f;
                    _pitch = 0.0f;
                    _fov = 45.0f;
                    UpdateCamera();
                    break;
                    
                case Keys.H:
                    _showHelp = !_showHelp;
                    break;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            var input = KeyboardState;
            var cameraSpeed = 2.5f * (float)args.Time;

            if (input.IsKeyDown(Keys.W))
                _cameraPos += _cameraFront * cameraSpeed;
            if (input.IsKeyDown(Keys.S))
                _cameraPos -= _cameraFront * cameraSpeed;
            if (input.IsKeyDown(Keys.A))
                _cameraPos -= Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * cameraSpeed;
            if (input.IsKeyDown(Keys.D))
                _cameraPos += Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * cameraSpeed;

            _rotationY += (float)args.Time * 30.0f; // Rotate model slowly
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (_firstMove)
            {
                _lastPos = new Vector2(e.X, e.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = e.X - _lastPos.X;
                var deltaY = e.Y - _lastPos.Y;
                _lastPos = new Vector2(e.X, e.Y);

                _yaw += deltaX * 0.1f;
                _pitch -= deltaY * 0.1f;

                _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);

                UpdateCamera();
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _fov -= e.OffsetY;
            _fov = Math.Clamp(_fov, 1.0f, 90.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            _time += (float)args.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_shader != null && _model != null)
            {
                _shader.Use();

                // Set transformation matrices
                var model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotationY));
                var view = Matrix4.LookAt(_cameraPos, _cameraPos + _cameraFront, _cameraUp);
                var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov), Size.X / (float)Size.Y, 0.1f, 100.0f);

                _shader.SetMatrix4("model", model);
                _shader.SetMatrix4("view", view);
                _shader.SetMatrix4("projection", projection);

                // Set shader uniforms
                _shader.SetFloat("iTime", _time);
                _shader.SetVector2("iResolution", new Vector2(Size.X, Size.Y));
                _shader.SetVector3("viewPos", _cameraPos);

                _model.Render();
            }

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            _shader?.Dispose();
            _model?.Dispose();
            base.OnUnload();
        }
    }
}
