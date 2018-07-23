using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Drawing.OpenGL
{
    public unsafe class glContext
    {
        WGL.Context _WGLContext;

        const int GL_VENDOR = 0x1F00;
        const int GL_MAJOR_VERSION = 0x821B;
        const int GL_MINOR_VERSION = 0x821C;

        delegate void glEnableFunc(int cap);
        glEnableFunc _glEnable;


        delegate byte* glGetStringFunc(int glEnum);
        glGetStringFunc glGetString;

        delegate void glGetIntegervFunc(int glEnum, int* p);
        glGetIntegervFunc glGetIntegerv;

        delegate void glGetBooleanvFunc(int glEnum, int* p);
        glGetBooleanvFunc glGetBooleanv;

        delegate void glGetFloatvFunc(int glEnum, float* p);
        glGetFloatvFunc glGetFloatv;

        delegate void glGetDoublevFunc(int glEnum, float* p);
        glGetDoublevFunc glGetDoublev;

        delegate void glGenBuffersFunc(int n, int* buffers);
        glGenBuffersFunc _glGenBuffers;

        delegate void glBufferDataFunc(int target, IntPtr size, void* data, int usage);
        glBufferDataFunc _glBufferData;

        delegate void glBindBufferFunc(int target, int buffer);
        glBindBufferFunc _glBindBuffer;

        delegate void glClearFunc(uint Target);
        glClearFunc _glClear;

        delegate void glClearColorFunc(float R, float G, float B, float A);
        glClearColorFunc _glClearColor;

        delegate int glCreateShaderFunc(int ShaderType);
        glCreateShaderFunc _glCreateShader;

        delegate void glShaderSourceFunc(int ShaderHandle, IntPtr Count, IntPtr ppString, IntPtr pLength);
        glShaderSourceFunc _glShaderSource;


        delegate void glCompileShaderFunc(int ShaderHandle);
        glCompileShaderFunc _glCompileShader;

        delegate void glGetShaderivFunc(int ShaderHandle, int pName, IntPtr pParams);
        glGetShaderivFunc _glGetShaderiv;


        delegate void glGetShaderInfoLogFunc(int ShaderHandle, IntPtr MaxLength, IntPtr pLength, IntPtr pInfoLog);
        glGetShaderInfoLogFunc _glGetShaderInfoLog;

        delegate int glCreateProgramFunc();
        glCreateProgramFunc _glCreateProgram;

        delegate void glUseProgramFunc(int ProgramHandle);
        glUseProgramFunc _glUseProgram;

        delegate void glAttachShaderFunc(int ProgramHandle, int ShaderHandle);
        glAttachShaderFunc _glAttachShader;

        delegate void glLinkProgramFunc(int ProgramHandle);
        glLinkProgramFunc _glLinkProgram;

        delegate int glGetAttribLocationFunc(int ProgramHandle, IntPtr pName);
        glGetAttribLocationFunc _glGetAttribLocation;

        delegate int glGetUniformLocationFunc(int ProgramHandle, IntPtr pName);
        glGetUniformLocationFunc _glGetUniformLocation;

        delegate int glBindAttribLocationFunc(int ProgramHandle, int index, IntPtr pName);
        glBindAttribLocationFunc _glBindAttribLocation;

        delegate void glGetProgramivFunc(int ShaderHandle, int pName, IntPtr pParams);
        glGetProgramivFunc _glGetProgramiv;

        delegate void glUniform1fFunc(int location, float value);
        glUniform1fFunc _glUniform1f;

        delegate void glUniform2fFunc(int location, float value0, float value1);
        glUniform2fFunc _glUniform2f;

        delegate void glUniform3fFunc(int location, float value0, float value1, float value2);
        glUniform3fFunc _glUniform3f;

        delegate void glUniform4fFunc(int location, float value0, float value1, float value2, float value3);
        glUniform4fFunc _glUniform4f;

        delegate void glUniform1iFunc(int location, int value);
        glUniform1iFunc _glUniform1i;

        delegate void glUniform2iFunc(int location, int value0, int value1);
        glUniform2iFunc _glUniform2i;

        delegate void glUniform3iFunc(int location, int value0, int value1, int value2);
        glUniform3iFunc _glUniform3i;

        delegate void glUniform4iFunc(int location, int value0, int value1, int value2, int value3);
        glUniform4iFunc _glUniform4i;

        delegate void glGetProgramInfoLogFunc(int ProgramHandle, IntPtr MaxLength, IntPtr pLength, IntPtr pInfoLog);
        glGetProgramInfoLogFunc _glGetProgramInfoLog;

        delegate void glDrawElementsFunc(int mode, IntPtr count, int type, IntPtr indices);
        glDrawElementsFunc _glDrawElements;

        delegate void glEnableVertexAttribArrayFunc(int index);
        glEnableVertexAttribArrayFunc _glEnableVertexAttribArray;

        delegate void glVertexAttribPointerFunc(int index, int size, int type, int normalized, int stride, IntPtr pointer);
        glVertexAttribPointerFunc _glVertexAttribPointer;

        delegate void glDebugMessageCallbackFunc(IntPtr callback, IntPtr userParam);
        glDebugMessageCallbackFunc _glDebugMessageCallback;

        delegate void glGenTexturesFunc(int n, int* buffers);
        glGenTexturesFunc _glGenTextures;

        delegate void glBindTextureFunc(int target, int texture);
        glBindTextureFunc _glBindTexture;

        delegate void glTexImage2DFunc(int target, int level, int internalFormat, int width, int height, int border, int format, int type, IntPtr data);
        glTexImage2DFunc _glTexImage2D;

        delegate void glActiveTextureFunc(int texture);
        glActiveTextureFunc _glActiveTexture;

        delegate void glTexParameteriFunc(int target, int parameter, int value);
        glTexParameteriFunc _glTexParameteri;

        delegate void glViewportFunc(int x, int y, int width, int height);
        glViewportFunc _glViewport;

        Version _Version;

        internal glContext(WGL.Context WGLContext)
        {
            _WGLContext = WGLContext;
            _WGLContext.MakeCurrent();

            glGetString = WGL.GetProcAddress<glGetStringFunc>("glGetString");
            glGetIntegerv = WGL.GetProcAddress<glGetIntegervFunc>("glGetIntegerv");
            glGetBooleanv = WGL.GetProcAddress<glGetBooleanvFunc>("glGetBooleanv");
            glGetFloatv = WGL.GetProcAddress<glGetFloatvFunc>("glGetFloatv");
            glGetDoublev = WGL.GetProcAddress<glGetDoublevFunc>("glGetDoublev");

            if (glGetString == null || glGetIntegerv == null || glGetFloatv == null || glGetDoublev == null) { throw new Exception("OpenGL 1.0 is not supported"); }
            int major, minor;
            glGetIntegerv(GL_MAJOR_VERSION, &major);
            glGetIntegerv(GL_MINOR_VERSION, &minor);
            _Version = new Version(major, minor);

            _glEnable = WGL.GetProcAddress<glEnableFunc>("glEnable");

            _glBufferData = WGL.GetProcAddress<glBufferDataFunc>("glBufferData");
            _glGenBuffers = WGL.GetProcAddress<glGenBuffersFunc>("glGenBuffers");
            _glBindBuffer = WGL.GetProcAddress<glBindBufferFunc>("glBindBuffer");

            _glClearColor = WGL.GetProcAddress<glClearColorFunc>("glClearColor");
            _glClear = WGL.GetProcAddress<glClearFunc>("glClear");

            _glCreateShader = WGL.GetProcAddress<glCreateShaderFunc>("glCreateShader");
            _glShaderSource = WGL.GetProcAddress<glShaderSourceFunc>("glShaderSource");
            _glCompileShader = WGL.GetProcAddress<glCompileShaderFunc>("glCompileShader");
            _glGetShaderiv = WGL.GetProcAddress<glGetShaderivFunc>("glGetShaderiv");
            _glGetShaderInfoLog = WGL.GetProcAddress<glGetShaderInfoLogFunc>("glGetShaderInfoLog");

            _glCreateProgram = WGL.GetProcAddress<glCreateProgramFunc>("glCreateProgram");
            _glAttachShader = WGL.GetProcAddress<glAttachShaderFunc>("glAttachShader");
            _glLinkProgram = WGL.GetProcAddress<glLinkProgramFunc>("glLinkProgram");
            _glLinkProgram = WGL.GetProcAddress<glLinkProgramFunc>("glLinkProgram");
            _glGetAttribLocation = WGL.GetProcAddress<glGetAttribLocationFunc>("glGetAttribLocation");
            _glGetUniformLocation = WGL.GetProcAddress<glGetUniformLocationFunc>("glGetUniformLocation");
            _glBindAttribLocation = WGL.GetProcAddress<glBindAttribLocationFunc>("glBindAttribLocation");
            _glGetProgramInfoLog = WGL.GetProcAddress<glGetProgramInfoLogFunc>("glGetProgramInfoLog");
            _glGetProgramiv = WGL.GetProcAddress<glGetProgramivFunc>("glGetProgramiv");
            _glUseProgram = WGL.GetProcAddress<glUseProgramFunc>("glUseProgram");
            _glUniform1f = WGL.GetProcAddress<glUniform1fFunc>("glUniform1f");
            _glUniform2f = WGL.GetProcAddress<glUniform2fFunc>("glUniform2f");
            _glUniform3f = WGL.GetProcAddress<glUniform3fFunc>("glUniform3f");
            _glUniform4f = WGL.GetProcAddress<glUniform4fFunc>("glUniform4f");
            _glUniform1i = WGL.GetProcAddress<glUniform1iFunc>("glUniform1i");
            _glUniform2i = WGL.GetProcAddress<glUniform2iFunc>("glUniform2i");
            _glUniform3i = WGL.GetProcAddress<glUniform3iFunc>("glUniform3i");
            _glUniform4i = WGL.GetProcAddress<glUniform4iFunc>("glUniform4i");

            _glGenTextures = WGL.GetProcAddress<glGenTexturesFunc>("glGenTextures");
            _glBindTexture = WGL.GetProcAddress<glBindTextureFunc>("glBindTexture");
            _glTexImage2D = WGL.GetProcAddress<glTexImage2DFunc>("glTexImage2D");
            _glActiveTexture = WGL.GetProcAddress<glActiveTextureFunc>("glActiveTexture");
            _glTexParameteri = WGL.GetProcAddress<glTexParameteriFunc>("glTexParameteri");

            _glDrawElements = WGL.GetProcAddress<glDrawElementsFunc>("glDrawElements");
            _glEnableVertexAttribArray = WGL.GetProcAddress<glEnableVertexAttribArrayFunc>("glEnableVertexAttribArray");
            _glVertexAttribPointer = WGL.GetProcAddress<glVertexAttribPointerFunc>("glVertexAttribPointer");

            _glDebugMessageCallback = WGL.GetProcAddress<glDebugMessageCallbackFunc>("glDebugMessageCallback");

            _glViewport = WGL.GetProcAddress<glViewportFunc>("glViewport");

            _ActiveTextureUnit = new TextureUnitTracker(this);
            _TextureUnitTrackers.Add(_ActiveTextureUnitIndex, _ActiveTextureUnit);
        }

        public void Viewport(int x, int y, int width, int height)
        {
            _glViewport(x, y, width, height);
        }

        public enum Capability
        {
            GL_VERTEX_ARRAY = 0x8074,
            GL_DEBUG_OUTPUT = 0x92E0,
            GL_TEXTURE_2D = 0x0DE1,
        }

        public void Enable(Capability Capability)
        {
            _glEnable((int)Capability);
        }

        internal delegate void DebugMessageCallbackInternalFunc(int source, int type, uint id, int severity, IntPtr length, IntPtr message, IntPtr userparam);
        internal void DebugMessageCallback(DebugMessageCallbackInternalFunc Callback)
        {
            _glDebugMessageCallback(System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(Callback), new IntPtr(null));
        }

        public delegate void DebugMessageCallbackFunc(int source, int type, int id, int severity, string Message);
        public void DebugMessageCallback(DebugMessageCallbackFunc Callback)
        {
            DebugMessageCallbackInternalFunc adapter = (int source, int type, uint id, int severity, IntPtr length, IntPtr message, IntPtr userparam)
                =>
            {
                int msgLength = 0;
                if (length.ToInt64() > int.MaxValue - 1) { msgLength = int.MaxValue - 1; }
                else { msgLength = (int)length.ToInt64(); }

                byte* pMessage = (byte*)message.ToPointer();
                byte[] data = new byte[msgLength];
                for (int n = 0; n < data.Length && *pMessage != 0; n++)
                {
                    data[n] = *pMessage;
                    pMessage++;
                }
                string strMessage = System.Text.Encoding.UTF8.GetString(data);
                Callback(source, type, (int)id, severity, strMessage);
            };

            DebugMessageCallback(adapter);
        }

        public void ClearColor(float R, float G, float B, float A)
        {
            _glClearColor(R, G, B, A);
        }

        public void Clear(uint Target)
        {
            _glClear(Target);
        }

        public void MakeCurrent()
        {
            if (_WGLContext != null) { _WGLContext.MakeCurrent(); }
            //else if () { }
        }

        public void SwapBuffers()
        {
            if (_WGLContext != null) { _WGLContext.SwapBuffers(); }
            //else if () { }
        }

        public glTexture GenTexture()
        {
            int textureHandle;
            _glGenBuffers(1, &textureHandle);
            return new glTexture(this, textureHandle);
        }

        public glTexture[] GenTextures(int numTextures)
        {
            int[] textureHandle = new int[numTextures];
            fixed (int* pTextureHandles = &textureHandle[0])
            {
                _glGenTextures(numTextures, pTextureHandles);
            }
            glTexture[] textures = new glTexture[numTextures];
            for (int n = 0; n < numTextures; n++)
            {
                textures[n] = new glTexture(this, textureHandle[n]);
            }
            return textures;
        }

        internal static int GlTargetTotextureTargetIndex(glTexture.BindTarget Target)
        {
            switch (Target)
            {
                case glTexture.BindTarget.GL_TEXTURE_1D: return 0;
                case glTexture.BindTarget.GL_TEXTURE_1D_ARRAY: return 1;
                case glTexture.BindTarget.GL_TEXTURE_2D: return 2;
                case glTexture.BindTarget.GL_TEXTURE_2D_ARRAY: return 3;
                case glTexture.BindTarget.GL_TEXTURE_2D_MULTISAMPLE: return 4;
                case glTexture.BindTarget.GL_TEXTURE_2D_MULTISAMPLE_ARRAY: return 5;
                case glTexture.BindTarget.GL_TEXTURE_3D: return 6;
                case glTexture.BindTarget.GL_TEXTURE_BUFFER: return 7;
                case glTexture.BindTarget.GL_TEXTURE_CUBE_MAP: return 8;
                case glTexture.BindTarget.GL_TEXTURE_CUBE_MAP_ARRAY: return 9;
                case glTexture.BindTarget.GL_TEXTURE_RECTANGLE: return 10;

            }
            return 0;
        }

        class TextureUnitTracker
        {
            glContext _Context;
            public TextureUnitTracker(glContext Context)
            {
                _Context = Context;
            }

            internal glTexture[] _Textures = new glTexture[GlTargetTotextureTargetIndex(glTexture.BindTarget.GL_TEXTURE_RECTANGLE) + 1];
            internal void BindTexture(glTexture Texture, glTexture.BindTarget Target)
            {
                int indexTarget = GlTargetTotextureTargetIndex(Target);

                lock (_Textures)
                {
                    if (Texture == null)
                    {
                        _Context._glBindTexture((int)Target, 0);
                        _Textures[indexTarget] = null;
                    }
                    else
                    {
                        _Context._glBindTexture((int)Target, Texture.Handle);
                        _Textures[indexTarget] = Texture;
                    }
                }
            }
        }
        Dictionary<int, TextureUnitTracker> _TextureUnitTrackers = new Dictionary<int, TextureUnitTracker>();
        int _ActiveTextureUnitIndex = 0;
        TextureUnitTracker _ActiveTextureUnit;

        internal void BindTexture(glTexture Texture, glTexture.BindTarget Target)
        {
            lock (_TextureUnitTrackers)
            {
                _ActiveTextureUnit.BindTexture(Texture, Target);
            }
        }

        internal void TexImage2D(glTexture Texture, glTexture.BindTarget Target, int Level, glTexture.InternalFormat InternalFormat, int Width, int Height, glTexture.Format Format, glTexture.Type Type, IntPtr Data)
        {
            int indexTarget = GlTargetTotextureTargetIndex(Target);

            lock (_TextureUnitTrackers)
            {
                if (_ActiveTextureUnit._Textures[indexTarget] != Texture) { throw new Exception("The texture is not bound correctly"); }
                _glTexImage2D((int)Target, Level, (int)InternalFormat, Width, Height, 0, (int)Format, (int)Type, Data);
            }
        }

        internal void TexParameter(glTexture Texture, glTexture.BindTarget Target, int Parameter, int Value)
        {
            int indexTarget = GlTargetTotextureTargetIndex(Target);

            lock (_TextureUnitTrackers)
            {
                if (_ActiveTextureUnit._Textures[indexTarget] != Texture) { throw new Exception("The texture is not bound correctly"); }
                _glTexParameteri((int)Target, Parameter, Value);
            }
        }

        public void ActiveTexture(int textureUnitIndex)
        {
            const int GL_TEXTURE0 = 0x84C0;
            lock (_TextureUnitTrackers)
            {
                if (!_TextureUnitTrackers.ContainsKey(textureUnitIndex)) { _TextureUnitTrackers.Add(textureUnitIndex, new TextureUnitTracker(this)); }
                _ActiveTextureUnit = _TextureUnitTrackers[textureUnitIndex];
                _glActiveTexture(textureUnitIndex + GL_TEXTURE0);
            }
        }

        public glBuffer GenBuffer()
        {
            int bufferHandle;
            _glGenBuffers(1, &bufferHandle);
            return new glBuffer(this, bufferHandle);
        }

        public glBuffer[] GenBuffers(int numBuffers)
        {
            int[] bufferHandle = new int[numBuffers];
            fixed (int* pbufferHandles = &bufferHandle[0])
            {
                _glGenBuffers(numBuffers, pbufferHandles);
            }
            glBuffer[] buffers = new glBuffer[numBuffers];
            for (int n = 0; n < numBuffers; n++)
            {
                buffers[n] = new glBuffer(this, bufferHandle[n]);
            }
            return buffers;
        }

        internal static int GlTargetTobufferTargetIndex(glBuffer.BindTarget Target)
        {
            switch (Target)
            {
                case glBuffer.BindTarget.GL_ARRAY_BUFFER: return 0;
                case glBuffer.BindTarget.GL_ELEMENT_ARRAY_BUFFER: return 1;
                case glBuffer.BindTarget.GL_PIXEL_PACK_BUFFER: return 2;
                case glBuffer.BindTarget.GL_PIXEL_UNPACK_BUFFER: return 3;
            }
            return 0;
        }

        glBuffer[] _Buffers = new glBuffer[GlTargetTobufferTargetIndex(glBuffer.BindTarget.GL_PIXEL_UNPACK_BUFFER) + 1];
        internal void BindBuffer(glBuffer Buffer, glBuffer.BindTarget Target)
        {
            int indexTarget = GlTargetTobufferTargetIndex(Target);

            lock (_Buffers)
            {
                if (Buffer == null)
                {
                    _glBindBuffer((int)Target, 0);
                    _Buffers[indexTarget] = null;
                }
                else
                {
                    _glBindBuffer((int)Target, Buffer.Handle);
                    _Buffers[indexTarget] = Buffer;
                }
            }
        }

        internal void BufferData(glBuffer Buffer, glBuffer.BindTarget Target, int Size, IntPtr Data, glBuffer.Usage Usage)
        {
            int indexTarget = GlTargetTobufferTargetIndex(Target);
            if (Size < 0) { throw new Exception("Negative buffer size are not allowed"); }
            lock (_Buffers)
            {
                if (_Buffers[indexTarget] != Buffer) { throw new Exception("Trying to set buffer data while the buffer is not bound to the specified target"); }
                _glBufferData((int)Target, new IntPtr(Size), Data.ToPointer(), (int)Usage);
            }
        }

        public glShader CreateShader(glShader.Type Type, string Source)
        {
            int handle = _glCreateShader((int)Type);
            ShaderSource(handle, Source);
            CompileShader(handle);
            return new glShader(this, handle);
        }

        void ShaderSource(int Shader, string Source)
        {
            int Length = Source.Length;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(Source);
            fixed (byte* pData = &data[0])
            {
                IntPtr[] pDataArray = new IntPtr[1];
                pDataArray[0] = new IntPtr(pData);
                fixed (IntPtr* ppDataArray = &pDataArray[0])
                {
                    _glShaderSource(Shader, new IntPtr(1), new IntPtr(ppDataArray), new IntPtr(&Length));
                }
            }
        }

        void CompileShader(int Shader)
        {
            _glCompileShader(Shader);
            int compileStatus = 0;
            _glGetShaderiv(Shader, (int)glShader.SymbolicName.GL_COMPILE_STATUS, new IntPtr(&compileStatus));
            if (compileStatus == 0)
            {
                int logSize = 0;
                _glGetShaderiv(Shader, (int)glShader.SymbolicName.GL_INFO_LOG_LENGTH, new IntPtr(&logSize));
                if (logSize <= 0) { throw new Exception("Failed to compile the shader"); }
                IntPtr infoLogBuffer = System.Runtime.InteropServices.Marshal.AllocHGlobal(logSize);
                if (infoLogBuffer.ToInt64() == 0) { throw new Exception("Failed to compile the shader"); }
                string log = "";
                try
                {
                    IntPtr sizeTLogSize = new IntPtr(logSize);
                    _glGetShaderInfoLog(Shader, sizeTLogSize, new IntPtr(&sizeTLogSize), infoLogBuffer);
                    if (sizeTLogSize.ToInt32() > 0)
                    {
                        byte* pLog = (byte*)infoLogBuffer.ToPointer();
                        byte[] data = new byte[sizeTLogSize.ToInt32()];
                        for (int n = 0; n < data.Length && *pLog != 0; n++)
                        {
                            data[n] = *pLog;
                            pLog++;
                        }
                        log = System.Text.Encoding.UTF8.GetString(data);
                    }
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(infoLogBuffer);
                }

                throw new Exception("Failed to compile the shader: " + log);
            }
        }

        public glProgram CreateProgram()
        {
            int handle = _glCreateProgram();
            return new glProgram(this, handle);
        }

        internal void AttachShader(glProgram Program, glShader Shader)
        {
            _glAttachShader(Program.Handle, Shader.Handle);
        }

        internal void LinkProgram(glProgram Program)
        {
            _glLinkProgram(Program.Handle);
            int linkStatus = 0;
            _glGetProgramiv(Program.Handle, (int)glProgram.SymbolicName.GL_LINK_STATUS, new IntPtr(&linkStatus));
            if (linkStatus == 0)
            {
                int logSize = 0;
                _glGetProgramiv(Program.Handle, (int)glProgram.SymbolicName.GL_INFO_LOG_LENGTH, new IntPtr(&logSize));
                if (logSize <= 0) { throw new Exception("Failed to link the program"); }
                IntPtr infoLogBuffer = System.Runtime.InteropServices.Marshal.AllocHGlobal(logSize);
                if (infoLogBuffer.ToInt64() == 0) { throw new Exception("Failed to link the program"); }
                string log = "";
                try
                {
                    IntPtr sizeTLogSize = new IntPtr(logSize);
                    _glGetProgramInfoLog(Program.Handle, sizeTLogSize, new IntPtr(&sizeTLogSize), infoLogBuffer);
                    if (sizeTLogSize.ToInt32() > 0)
                    {
                        byte* pLog = (byte*)infoLogBuffer.ToPointer();
                        byte[] data = new byte[sizeTLogSize.ToInt32()];
                        for (int n = 0; n < data.Length && *pLog != 0; n++)
                        {
                            data[n] = *pLog;
                            pLog++;
                        }
                        log = System.Text.Encoding.UTF8.GetString(data);
                    }
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(infoLogBuffer);
                }

                throw new Exception("Failed to link the program: " + log);
            }
        }

        public void SetUniform(int location, float value) { _glUniform1f(location, value); }
        public void SetUniform(int location, float value0, float value1) { _glUniform2f(location, value0, value1); }
        public void SetUniform(int location, float value0, float value1, float value2) { _glUniform3f(location, value0, value1, value2); }
        public void SetUniform(int location, float value0, float value1, float value2, float value3) { _glUniform4f(location, value0, value1, value2, value3); }
        public void SetUniform(int location, int value) { _glUniform1i(location, value); }
        public void SetUniform(int location, int value0, int value1) { _glUniform2i(location, value0, value1); }
        public void SetUniform(int location, int value0, int value1, int value2) { _glUniform3i(location, value0, value1, value2); }
        public void SetUniform(int location, int value0, int value1, int value2, int value3) { _glUniform4i(location, value0, value1, value2, value3); }

        public enum PrimitivesType : int
        {
            GL_POINTS             = 0x0000,
            GL_LINES              = 0x0001,
            GL_LINE_LOOP          = 0x0002,
            GL_LINE_STRIP         = 0x0003,
            GL_TRIANGLES          = 0x0004,
            GL_TRIANGLE_STRIP     = 0x0005,
            GL_TRIANGLE_FAN       = 0x0006,
            GL_QUADS              = 0x0007,
            GL_QUAD_STRIP         = 0x0008,
            GL_POLYGON            = 0x0009
        }

        public enum Type : int
        {
            GL_BYTE          = 0x1400,
            GL_UNSIGNED_BYTE = 0x1401,
            GL_SHORT = 0x1402,
            GL_UNSIGNED_SHORT = 0x1403,
            GL_INT = 0x1404,
            GL_UNSIGNED_INT = 0x1405,
            GL_FLOAT = 0x1406,
            GL_FIXED = 0x140C,
        }

        public enum IndiceType : int
        {
            GL_UNSIGNED_BYTE = 0x1401,
            GL_UNSIGNED_SHORT = 0x1403,
            GL_UNSIGNED_INT = 0x1405,
        }

        internal void DrawElements(PrimitivesType mode, IntPtr count, IndiceType type, IntPtr indices)
        {
            _glDrawElements((int)mode, count, (int)type, indices);
        }

        public void DrawElements(PrimitivesType mode, int count, IndiceType type)
        {
            if (count < 0) { throw new Exception("Invalid element count"); }
            DrawElements(mode, new IntPtr(count), type, new IntPtr(0));
        }

        public void DrawElements(PrimitivesType mode, int count, IndiceType type, IntPtr indices)
        {
            if (count < 0) { throw new Exception("Invalid element count"); }
            DrawElements(mode, new IntPtr(count), type, indices);
        }

        public void DrawElements(PrimitivesType mode, int count, uint[] indices, int offset)
        {
            if (count < 0) { throw new Exception("Invalid element count"); }
            if (count < 0) { throw new Exception("Invalid offset"); }
            fixed (uint* pIndices = &indices[0])
            {
                DrawElements(mode, new IntPtr(count), IndiceType.GL_UNSIGNED_INT, new IntPtr(pIndices + offset));
            }
        }

        public void DrawElements(PrimitivesType mode, uint[] indices)
        {
            DrawElements(mode, indices.Length, indices, 0);
        }

        public void EnableVertexAttribArray(int Index)
        {
            if (Index < 0) { throw new Exception("Invalid Index"); }
            _glEnableVertexAttribArray(Index);
        }

        public enum VertexAttributeType
        {
            GL_BYTE = 0x1400,
            GL_UNSIGNED_BYTE = 0x1401,
            GL_SHORT = 0x1402,
            GL_UNSIGNED_SHORT = 0x1403,
            GL_INT = 0x1404,
            GL_UNSIGNED_INT = 0x1405,
            GL_FLOAT = 0x1406,
            GL_DOUBLE = 0x140A,
        }

        public void VertexAttribPointer(int index, int size, VertexAttributeType type, bool normalize, int stride, IntPtr pointer)
        {
            if (index < 0) { throw new Exception("Invalid Index"); }
            if (size < 1 || size > 4) { throw new Exception("Size must be a value between 1 and 4"); }
            if (stride < 0) { throw new Exception("Invalid stride value"); }
            _glVertexAttribPointer(index, size, (int)(type), normalize ? 1 : 0, stride, pointer);
        }

        internal void UseProgram(glProgram Program)
        {
            _glUseProgram(Program.Handle);
        }

        internal int GetAttribLocation(glProgram Program, string Name)
        {
            byte[] NameData = System.Text.UTF8Encoding.UTF8.GetBytes(Name);
            byte[] ZeroTerminated = new byte[NameData.Length + 1];
            for (int n = 0; n < NameData.Length; n++) { ZeroTerminated[n] = NameData[n]; }
            ZeroTerminated[ZeroTerminated.Length - 1] = 0;
            fixed (byte* pNameData = &ZeroTerminated[0])
            {
                return _glGetAttribLocation(Program.Handle, new IntPtr(pNameData));
            }
        }

        internal void BindAttribLocation(glProgram Program, int Index, string Name)
        {
            byte[] NameData = System.Text.UTF8Encoding.UTF8.GetBytes(Name);
            byte[] ZeroTerminated = new byte[NameData.Length + 1];
            for (int n = 0; n < NameData.Length; n++) { ZeroTerminated[n] = NameData[n]; }
            ZeroTerminated[ZeroTerminated.Length - 1] = 0;
            fixed (byte* pNameData = &ZeroTerminated[0])
            {
                _glBindAttribLocation(Program.Handle, Index, new IntPtr(pNameData));
            }
        }

        internal int GetUniformLocation(glProgram Program, string Name)
        {
            byte[] NameData = System.Text.UTF8Encoding.UTF8.GetBytes(Name);
            byte[] ZeroTerminated = new byte[NameData.Length + 1];
            for (int n = 0; n < NameData.Length; n++) { ZeroTerminated[n] = NameData[n]; }
            ZeroTerminated[ZeroTerminated.Length - 1] = 0;
            fixed (byte* pNameData = &ZeroTerminated[0])
            {
                return _glGetUniformLocation(Program.Handle, new IntPtr(pNameData));
            }
        }

        public static glContext CreateContextFromWGLContext(WGL.Context WGLContext)
        {
            return new glContext(WGLContext);
        }


        public static glContext CreateContextFromGLXContext(object GLXContext)
        {
            return null;
        }
    }
}
