using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ratchet.Math;

namespace SimpleCube
{
    public partial class SimpleCube : Form
    {
        Ratchet.Drawing.OpenGL.WGL.Context _WGLContext;
        Ratchet.Drawing.OpenGL.glContext _Context;
        Ratchet.Drawing.OpenGL.glBuffer _VertexBuffer;
        Ratchet.Drawing.OpenGL.glBuffer _IndexBuffer;
        Ratchet.Drawing.OpenGL.glProgram _Shader;

        mat4 _Projection;
        mat4 _Lookat;



        string vertexShaderSource = @"
#version 330 core

in vec4 in_Position;
out vec4 world_Position;

uniform mat4 projectionMatrix;

void main(void)
{             
    world_Position = in_Position;
    gl_Position = projectionMatrix * vec4(in_Position);
}
";

        string fragmentShaderSource = @"
#version 330 core

in vec4 world_Position;

void main(void)
{             
    gl_FragColor = vec4(world_Position.x + 0.5, world_Position.y + 0.5, world_Position.z + 0.5, 1.0); 

}
";

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;
                baseParams.ClassStyle |= (int)(Ratchet.Drawing.OpenGL.WGL.WindowClassStyle.OWNDC | Ratchet.Drawing.OpenGL.WGL.WindowClassStyle.VREDRAW | Ratchet.Drawing.OpenGL.WGL.WindowClassStyle.HREDRAW);
                return baseParams;
            }
        }

        public SimpleCube()
        {
            InitializeComponent();

        }

        private void SimpleCube_Load(object sender, EventArgs e)
        {
            DoubleBuffered = false;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            SetStyle(ControlStyles.Opaque, true);


            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Size = new Size(800, 600);

            _WGLContext = Ratchet.Drawing.OpenGL.WGL.CreateContext(this);
            _Context = Ratchet.Drawing.OpenGL.glContext.CreateContextFromWGLContext(_WGLContext);
            _Context.MakeCurrent();
            _Context.Enable(Ratchet.Drawing.OpenGL.glContext.Capability.GL_DEBUG_OUTPUT);
            _Context.DebugMessageCallback((int source, int id, int type, int severity, string message) =>
            {
                Console.WriteLine(message);
            });
            _VertexBuffer = _Context.GenBuffer();
            _VertexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER);
            float[] triangle = new float[] { -0.5f, -0.5f, -0.5f, 1.0f, // Vertex 0 Position
                                              1.0f,  1.0f,

                                              0.5f, -0.5f, -0.5f, 1.0f, // Vertex 1 Position
                                              0.0f,  1.0f,

                                              0.5f,  0.5f, -0.5f, 1.0f, // Vertex 2 Position
                                              0.0f,  0.0f,

                                              -0.5f,  0.5f, -0.5f, 1.0f, // Vertex 3 Position
                                              1.0f,  0.0f,

                                              -0.5f, -0.5f, 0.5f, 1.0f, // Vertex 4 Position
                                              1.0f,  1.0f,

                                              0.5f, -0.5f, 0.5f, 1.0f, // Vertex 5 Position
                                              0.0f,  1.0f,

                                              0.5f,  0.5f, 0.5f, 1.0f, // Vertex 6 Position
                                              0.0f,  0.0f,

                                              -0.5f,  0.5f, 0.5f, 1.0f, // Vertex 7 Position
                                              1.0f,  0.0f,
            };
            _VertexBuffer.BufferData(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER, sizeof(float) * triangle.Length, triangle, Ratchet.Drawing.OpenGL.glBuffer.Usage.GL_STATIC_DRAW);

            _IndexBuffer = _Context.GenBuffer();
            _IndexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER);
            ///        7 --- 6
            ///       /     /|
            ///      3 --- 2 5
            ///      |     |/
            ///      0 --- 1
            _IndexBuffer.BufferData(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * 36, new int[] { 0, 1, 2, 0, 2, 3 ,
                                                                                                                                      4, 5, 6, 4, 6, 7 ,
                                                                                                                                      0, 1, 4, 1, 4, 5 ,
                                                                                                                                      2, 7, 3, 2, 7, 6 ,
                                                                                                                                      1, 6, 2, 1, 6, 5 ,
                                                                                                                                      0, 7, 3, 0, 7, 4 ,
                                                                                                                                      }, Ratchet.Drawing.OpenGL.glBuffer.Usage.GL_STATIC_DRAW);
            var vertexShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_VERTEX_SHADER, vertexShaderSource);
            var fragmentShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_FRAGMENT_SHADER, fragmentShaderSource);
            _Shader = _Context.CreateProgram();
            _Shader.AttachShader(vertexShader);
            _Shader.AttachShader(fragmentShader);
            _Shader.LinkProgram();

            _Context.Viewport(0, 0, Width, Height);

            _Projection = mat4.Perspective(2.8f, (float)Width / (float)Height, 0.01f, 10.0f);
            _Lookat = mat4.LookAt(new vec3(0.0f, 0.2f, -2.0f), new vec3(0.1f, 0.1f, 0.1f), new vec3(0.0f, 1.0f, 0.0f));

            _Context.Enable(Ratchet.Drawing.OpenGL.glContext.Capability.GL_DEPTH_TEST);
            _Context.DepthFunc(Ratchet.Drawing.OpenGL.glContext.DepthFuncion.GL_LEQUAL);
        }

        vec3 _CameraPosition = new vec3(0.0f, 0.7f, -2.0f);

        private unsafe void SimpleCube_Paint(object sender, PaintEventArgs e)
        {
            _Context.MakeCurrent();
            _Lookat = mat4.LookAt(_CameraPosition, new vec3(0.1f, 0.1f, 0.1f), new vec3(0.0f, 1.0f, 0.0f));


            int positionAttribIndex = _Shader.GetAttribLocation("in_Position");
            int texCoordAttribIndex = _Shader.GetAttribLocation("in_textureCoord");
            int projectionMatrix = _Shader.GetUniformLocation("projectionMatrix");

            _Context.DrawBuffer(Ratchet.Drawing.OpenGL.glContext.DrawBufferMode.GL_BACK);
            _Context.ClearDepth(1.0f);
            _Context.ClearColor(0.1f, 0.7f, 1.0f, 1.0f);
            _Context.Clear(Ratchet.Drawing.OpenGL.glContext.ClearTarget.GL_COLOR_BUFFER | Ratchet.Drawing.OpenGL.glContext.ClearTarget.GL_DEPTH_BUFFER);

            _Shader.UseProgram();
            mat4 matrix = _Projection * _Lookat;
            _Context.SetUniformMatrix4(projectionMatrix, false, new IntPtr(&matrix));

            _VertexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER);
            _IndexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER);
            _Context.EnableVertexAttribArray(positionAttribIndex);
            _Context.VertexAttribPointer(0, 4, Ratchet.Drawing.OpenGL.glContext.VertexAttributeType.GL_FLOAT, false, 6 * sizeof(float), new IntPtr(0));

            _Context.DrawElements(Ratchet.Drawing.OpenGL.glContext.PrimitivesType.GL_TRIANGLES, 36, Ratchet.Drawing.OpenGL.glContext.IndiceType.GL_UNSIGNED_INT);
            _Context.Flush();
            _Context.SwapBuffers();
        }

        double _TimeScale = 0.0;

        private void refresh_Tick(object sender, EventArgs e)
        {
            _TimeScale += 0.02f;
            _CameraPosition = new vec3(1.6f * (float)System.Math.Sin(_TimeScale), 0.7f, 1.6f * (float)System.Math.Cos(_TimeScale));
            this.Refresh();
        }

        private void SimpleCube_Resize(object sender, EventArgs e)
        {
            if (_Context != null)
            {
                _Context.MakeCurrent();
                _Context.Viewport(0, 0, Width, Height);
                Invalidate();
            }
        }
    }
}
