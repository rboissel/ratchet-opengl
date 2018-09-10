using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleBlur
{
    partial class Windows : Form
    {
        Ratchet.Drawing.OpenGL.WGL.Context _WGLContext;
        Ratchet.Drawing.OpenGL.glContext _Context;
        Ratchet.Drawing.OpenGL.glBuffer _VertexBuffer;
        Ratchet.Drawing.OpenGL.glBuffer _IndexBuffer;
        Ratchet.Drawing.OpenGL.glProgram _Shader;
        Ratchet.Drawing.OpenGL.glProgram _BlurShader;
        Ratchet.Drawing.OpenGL.glTexture _Texture;
        Ratchet.Drawing.OpenGL.glTexture _FramebufferColorBuffer;
        Ratchet.Drawing.OpenGL.glFramebuffer _Framebuffer;


        string vertexShaderSource = @"
#version 330 core

in vec4 in_Position;
in vec2 in_textureCoord;
smooth out vec2 out_textureCoord;

void main(void)
{             
    gl_Position = vec4(in_Position);
    out_textureCoord = in_textureCoord;
}
";

        string fragmentShaderSource = @"
#version 330 core

in vec2 out_textureCoord;
uniform sampler2D textureSampler;
layout(location = 0) out vec4 color;

void main(void)
{             
    color = texture(textureSampler, out_textureCoord); 

}
";

        string blurShaderSource = @"
#version 330 core

in vec2 out_textureCoord;
uniform sampler2D textureSampler;

void main(void)
{             
    // Ok this is really 'ratchet' don't do that if you want to implement a nice blur
    // But this is just for the demo
    gl_FragColor = texture(textureSampler, out_textureCoord + vec2(0.005, 0.00)) / 4.0 +
                   texture(textureSampler, out_textureCoord + vec2(0.00, 0.005)) / 4.0 +
                   texture(textureSampler, out_textureCoord + vec2(-0.005, 0.00)) / 4.0 +
                   texture(textureSampler, out_textureCoord + vec2(0.00, -0.005)) / 4.0; 
}
";

        public Windows()
        {
            InitializeComponent();
        }

        static unsafe void Swap_BGRA_RGBA(IntPtr scan0, int size)
        {
            // .NET Stores image for 32RGBA as (B|G|R|A LE)
            // Since we are asking openGL to use GL_RGBA which expect (R|G|B|A LE)
            // We could play with glPixelStore but we are just going to do a swap here
            uint* ptr = (uint*)scan0.ToPointer();
            for (int n = 0; n < size; n++)
            {
                *ptr = ((*ptr & 0xFF000000)) /*A*/ | ((*ptr & 0x00FF0000) >> 16)  /*R->B*/ | ((*ptr & 0x0000FF00)) /*G*/ | ((*ptr & 0x000000FF) << 16) /*B->R*/;
                ptr++;
            }
        }

        void RegenerateFramebuffer()
        {

            // Create our renderbuffer

            _FramebufferColorBuffer = _Context.GenTexture();
            _FramebufferColorBuffer.BindTexture(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D);
            _FramebufferColorBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_MIN_FILTER, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_NEAREST);
            _FramebufferColorBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_MAG_FILTER, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_NEAREST);
            _FramebufferColorBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_WRAP_S, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_CLAMP_TO_BORDER);
            _FramebufferColorBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_WRAP_T, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_CLAMP_TO_BORDER);
            _FramebufferColorBuffer.TexImage2D(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, 0, Ratchet.Drawing.OpenGL.glTexture.InternalFormat.GL_RGBA, Width, Height, 0, Ratchet.Drawing.OpenGL.glTexture.Format.GL_RGBA, Ratchet.Drawing.OpenGL.glTexture.Type.GL_UNSIGNED_BYTE, null);

            Ratchet.Drawing.OpenGL.glTexture depthBuffer = _Context.GenTexture();
            depthBuffer.BindTexture(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D);
            depthBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_MIN_FILTER, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_NEAREST);
            depthBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_MAG_FILTER, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_NEAREST);
            depthBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_WRAP_S, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_CLAMP_TO_BORDER);
            depthBuffer.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_WRAP_T, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_CLAMP_TO_BORDER);
            depthBuffer.TexImage2D(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, 0, Ratchet.Drawing.OpenGL.glTexture.InternalFormat.GL_DEPTH_COMPONENT, Width, Height, 0, Ratchet.Drawing.OpenGL.glTexture.Format.GL_DEPTH_COMPONENT, Ratchet.Drawing.OpenGL.glTexture.Type.GL_UNSIGNED_BYTE, null);


            if (_Framebuffer != null) { _Framebuffer.Dispose(); }
            _Framebuffer = _Context.GenFramebuffer();
            _Framebuffer.BindFramebuffer(Ratchet.Drawing.OpenGL.glFramebuffer.BindTarget.GL_FRAMEBUFFER);
            _Context.Viewport(0, 0, Width, Height);
            _Framebuffer.AttachTexture(Ratchet.Drawing.OpenGL.glFramebuffer.BindTarget.GL_FRAMEBUFFER, Ratchet.Drawing.OpenGL.glFramebuffer.Attachment.GL_COLOR_ATTACHMENT0, _FramebufferColorBuffer, 0);
            _Framebuffer.AttachTexture(Ratchet.Drawing.OpenGL.glFramebuffer.BindTarget.GL_FRAMEBUFFER, Ratchet.Drawing.OpenGL.glFramebuffer.Attachment.GL_DEPTH_ATTACHMENT, depthBuffer, 0);
            _Context.DrawBuffers(new Ratchet.Drawing.OpenGL.glContext.DrawBufferMode[] { Ratchet.Drawing.OpenGL.glContext.DrawBufferMode.GL_COLOR_ATTACHMENT0 });
            _Framebuffer.UnbindFramebuffer(Ratchet.Drawing.OpenGL.glFramebuffer.BindTarget.GL_FRAMEBUFFER);
        }

        private void Windows_Load(object sender, EventArgs e)
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
            float[] triangle = new float[] { -0.5f, -0.5f, 0.0f, 1.0f, // Vertex 1 Position
                                              1.0f,  1.0f,

                                              0.5f, -0.5f, 0.0f, 1.0f, // Vertex 2 Position
                                              0.0f,  1.0f,

                                              0.5f,  0.5f, 0.0f, 1.0f, // Vertex 3 Position
                                              0.0f,  0.0f,

                                              -0.5f,  0.5f, 0.0f, 1.0f, // Vertex 4 Position
                                              1.0f,  0.0f,
            };
            _VertexBuffer.BufferData(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER, sizeof(float) * triangle.Length, triangle, Ratchet.Drawing.OpenGL.glBuffer.Usage.GL_STATIC_DRAW);

            _IndexBuffer = _Context.GenBuffer();
            _IndexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER);
            _IndexBuffer.BufferData(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * 6, new int[] { 0, 1, 2, 0, 2, 3 }, Ratchet.Drawing.OpenGL.glBuffer.Usage.GL_STATIC_DRAW);

            System.Drawing.Bitmap image = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"./ratchet.png");

            // Create a new texture
            _Texture = _Context.GenTexture();
            // Bind the texture we have created to Texture 2D in sampler 0 in order to configure it
            // At that point configuring the sampler to use sampler 0 is not needed but we do it for consistency
            _Context.ActiveTexture(0);
            _Texture.BindTexture(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D);
            var lockRegion = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Swap_BGRA_RGBA(lockRegion.Scan0, image.Width * image.Height);
            _Texture.TexImage2D(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, 0, Ratchet.Drawing.OpenGL.glTexture.InternalFormat.GL_RGBA, image.Width, image.Height, 0, Ratchet.Drawing.OpenGL.glTexture.Format.GL_RGBA, Ratchet.Drawing.OpenGL.glTexture.Type.GL_UNSIGNED_BYTE, lockRegion.Scan0);
            image.UnlockBits(lockRegion);
            // Filters needs to be set so the sampler can sample the texture correctly.
            _Texture.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_MIN_FILTER, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_NEAREST);
            _Texture.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_MAG_FILTER, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_NEAREST);
            _Texture.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_WRAP_S, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_CLAMP_TO_BORDER);
            _Texture.TexParameter(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D, Ratchet.Drawing.OpenGL.glTexture.Parameter.GL_TEXTURE_WRAP_T, Ratchet.Drawing.OpenGL.glTexture.ParameterValue.GL_CLAMP_TO_BORDER);

            {
                var vertexShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_VERTEX_SHADER, vertexShaderSource);
                var fragmentShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_FRAGMENT_SHADER, fragmentShaderSource);
                _Shader = _Context.CreateProgram();
                _Shader.AttachShader(vertexShader);
                _Shader.AttachShader(fragmentShader);
                _Shader.BindAttribLocation(0, "in_Position");
                _Shader.BindAttribLocation(1, "in_textureCoord");
                _Shader.LinkProgram();
            }

            {
                var vertexShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_VERTEX_SHADER, vertexShaderSource);
                var fragmentShader = _Context.CreateShader(Ratchet.Drawing.OpenGL.glShader.Type.GL_FRAGMENT_SHADER, blurShaderSource);
                _BlurShader = _Context.CreateProgram();
                _BlurShader.AttachShader(vertexShader);
                _BlurShader.AttachShader(fragmentShader);
                _BlurShader.BindAttribLocation(0, "in_Position");
                _BlurShader.BindAttribLocation(1, "in_textureCoord");
                _BlurShader.LinkProgram();
            }
            

            _Context.Viewport(0, 0, Width, Height);
            RegenerateFramebuffer();
        }

        private void Windows_Paint(object sender, PaintEventArgs e)
        {
            _Context.MakeCurrent();

            {
                _Context.DrawBuffers(new Ratchet.Drawing.OpenGL.glContext.DrawBufferMode[] { Ratchet.Drawing.OpenGL.glContext.DrawBufferMode.GL_COLOR_ATTACHMENT0 });

                _Framebuffer.BindFramebuffer(Ratchet.Drawing.OpenGL.glFramebuffer.BindTarget.GL_DRAW_FRAMEBUFFER);

                _Context.DrawBuffers(new Ratchet.Drawing.OpenGL.glContext.DrawBufferMode[] { Ratchet.Drawing.OpenGL.glContext.DrawBufferMode.GL_COLOR_ATTACHMENT0 });

                int positionAttribIndex = _BlurShader.GetAttribLocation("in_Position");
                int texCoordAttribIndex = _BlurShader.GetAttribLocation("in_textureCoord");
                int texSampler = _BlurShader.GetUniformLocation("textureSampler");

                _Context.ClearColor(0.1f, 0.7f, 1.0f, 1.0f);
                _Context.Clear(Ratchet.Drawing.OpenGL.glContext.ClearTarget.GL_COLOR_BUFFER);

                // Activate Sampler 0 and bind our texture to it
                _Context.ActiveTexture(0);
                _Texture.BindTexture(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D);


                _Shader.UseProgram();
                _VertexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER);
                _IndexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER);
                _Context.EnableVertexAttribArray(positionAttribIndex);
                _Context.VertexAttribPointer(0, 4, Ratchet.Drawing.OpenGL.glContext.VertexAttributeType.GL_FLOAT, false, 6 * sizeof(float), new IntPtr(0));

                _Context.EnableVertexAttribArray(1);
                _Context.VertexAttribPointer(1, 2, Ratchet.Drawing.OpenGL.glContext.VertexAttributeType.GL_FLOAT, false, 6 * sizeof(float), new IntPtr(4 * sizeof(float)));

                // Link the uniform "textureSampler" that is expecting a textureSampler to be bound to it
                // to 0 which is the sampler currently associated to our texture
                _Context.SetUniform(texSampler, 0);


                _Context.DrawElements(Ratchet.Drawing.OpenGL.glContext.PrimitivesType.GL_TRIANGLES, 6, Ratchet.Drawing.OpenGL.glContext.IndiceType.GL_UNSIGNED_INT);
                _Context.Flush();
                _Framebuffer.UnbindFramebuffer(Ratchet.Drawing.OpenGL.glFramebuffer.BindTarget.GL_DRAW_FRAMEBUFFER);
            }

            // Be lazy and redraw the same center square but this time using the previous render as a texture
            {
                int positionAttribIndex = _BlurShader.GetAttribLocation("in_Position");
                int texCoordAttribIndex = _BlurShader.GetAttribLocation("in_textureCoord");
                int texSampler = _BlurShader.GetUniformLocation("textureSampler");

                _Context.ClearColor(0.1f, 0.7f, 1.0f, 1.0f);
                _Context.Clear(Ratchet.Drawing.OpenGL.glContext.ClearTarget.GL_COLOR_BUFFER);

                // Activate Sampler 0 and bind our texture to it
                _Context.ActiveTexture(0);
                _FramebufferColorBuffer.BindTexture(Ratchet.Drawing.OpenGL.glTexture.BindTarget.GL_TEXTURE_2D);


                _BlurShader.UseProgram();
                _VertexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ARRAY_BUFFER);
                _IndexBuffer.BindBuffer(Ratchet.Drawing.OpenGL.glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER);
                _Context.EnableVertexAttribArray(positionAttribIndex);
                _Context.VertexAttribPointer(0, 4, Ratchet.Drawing.OpenGL.glContext.VertexAttributeType.GL_FLOAT, false, 6 * sizeof(float), new IntPtr(0));

                _Context.EnableVertexAttribArray(1);
                _Context.VertexAttribPointer(1, 2, Ratchet.Drawing.OpenGL.glContext.VertexAttributeType.GL_FLOAT, false, 6 * sizeof(float), new IntPtr(4 * sizeof(float)));

                // Link the uniform "textureSampler" that is expecting a textureSampler to be bound to it
                // to 0 which is the sampler currently associated to our texture
                _Context.SetUniform(texSampler, 0);

                _Context.DrawElements(Ratchet.Drawing.OpenGL.glContext.PrimitivesType.GL_TRIANGLES, 6, Ratchet.Drawing.OpenGL.glContext.IndiceType.GL_UNSIGNED_INT);
            }

            _Context.SwapBuffers();
        }

        private void Windows_Resize(object sender, EventArgs e)
        {
            if (_Context != null)
            {
                _Context.MakeCurrent();
                RegenerateFramebuffer();
                _Context.Viewport(0, 0, Width, Height);
                Invalidate();
            }
        }

        private void Windows_Load_1(object sender, EventArgs e)
        {

        }
    }
}
