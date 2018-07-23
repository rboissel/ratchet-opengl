using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleTriangle
{
    public partial class Window : Form
    {
        Ratchet.Drawing.OpenGL.WGL.Context _WGLContext;
        Ratchet.Drawing.OpenGL.glContext _Context;
        Ratchet.Drawing.OpenGL.glBuffer _VertexBuffer;
        Ratchet.Drawing.OpenGL.glBuffer _IndexBuffer;
        Ratchet.Drawing.OpenGL.glProgram _Shader;


        string vertexShaderSource = @"
#version 330 core

in vec4 in_Position;
in vec4 in_Color;
smooth out vec4 out_Color;

void main(void)
{             
    gl_Position = vec4(in_Position);
    out_Color = in_Color;
}
";

        string fragmentShaderSource = @"
#version 330 core

in vec4 out_Color;

void main(void)
{             
    gl_FragColor = out_Color;  
}
";

        public Window()
        {

            InitializeComponent();

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;
                baseParams.ClassStyle |= (int)(Ratchet.Drawing.OpenGL.WGL.WindowClassStyle.OWNDC | Ratchet.Drawing.OpenGL.WGL.WindowClassStyle.VREDRAW | Ratchet.Drawing.OpenGL.WGL.WindowClassStyle.HREDRAW);
                return baseParams;
            }
        }


        private void Window_Load(object sender, EventArgs e)
        {
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
            float[] triangle = new float[] { -0.5f, -0.5f, 0.0f, 1.0f, // Vertex 1 Position
                                              1.0f,  0.0f, 0.0f, 1.0f, // Vertex 1 Color (red)

                                              0.5f, -0.5f, 0.0f, 1.0f, // Vertex 2 Position
                                              0.0f,  1.0f, 0.0f, 1.0f, // Vertex 2 Color (green)

                                              0.0f,  0.5f, 0.0f, 1.0f, // Vertex 3 Position
                                              0.0f,  0.0f, 1.0f, 1.0f, // Vertex 3 Color (blue)
            };
            _VertexBuffer.BufferData(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER, sizeof(float) * triangle.Length, triangle, Ratchet.Drawing.OpenGL.glBuffer.Usage.GL_STATIC_DRAW);

            _IndexBuffer = _Context.GenBuffer();
            _IndexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER);
            _IndexBuffer.BufferData(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * 3, new int[] { 0, 1, 2 }, Ratchet.Drawing.OpenGL.glBuffer.Usage.GL_STATIC_DRAW);
            _Context.Enable(Ratchet.Drawing.OpenGL.glContext.Capability.GL_VERTEX_ARRAY);

            var vertexShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_VERTEX_SHADER, vertexShaderSource);
            var fragmentShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_FRAGMENT_SHADER, fragmentShaderSource);
            _Shader = _Context.CreateProgram();
            _Shader.AttachShader(vertexShader);
            _Shader.AttachShader(fragmentShader);
            _Shader.BindAttribLocation(0, "in_Position");
            _Shader.BindAttribLocation(1, "in_Color");
            _Shader.LinkProgram();

            _Context.Viewport(0, 0, Width, Height);

        }

        private void Window_Paint(object sender, PaintEventArgs e)
        {
            _Context.MakeCurrent();


            int positionAttribIndex = _Shader.GetAttribLocation("in_Position");
            int colorAttribIndex = _Shader.GetAttribLocation("in_Color");


            _Context.ClearColor(0.1f, 0.7f, 1.0f, 1.0f);
            _Context.Clear(0x4000);
            _Shader.UseProgram();
            _VertexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER);
            _IndexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER);
            _Context.EnableVertexAttribArray(positionAttribIndex);
            _Context.VertexAttribPointer(0, 4, Ratchet.Drawing.OpenGL.glContext.VertexAttributeType.GL_FLOAT, false, 8 * sizeof(float), new IntPtr(0));

            _Context.EnableVertexAttribArray(1);
            _Context.VertexAttribPointer(1, 4, Ratchet.Drawing.OpenGL.glContext.VertexAttributeType.GL_FLOAT, false, 8 * sizeof(float), new IntPtr(4 * sizeof(float)));

            _Context.DrawElements(Ratchet.Drawing.OpenGL.glContext.PrimitivesType.GL_TRIANGLES, 3, Ratchet.Drawing.OpenGL.glContext.IndiceType.GL_UNSIGNED_INT);
            _Context.SwapBuffers();
        }

        private void Window_SizeChanged(object sender, EventArgs e)
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
