using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Drawing.OpenGL
{
    public class glProgram : IDisposable
    {
        internal enum SymbolicName : int
        {
            GL_DELETE_STATUS = 0x8B80,
            GL_LINK_STATUS = 0x8B82,
            GL_VALIDATE_STATUS = 0x8B83,
            GL_INFO_LOG_LENGTH = 0x8B84,
            GL_ATTACHED_SHADERS = 0x8B85,
            GL_ACTIVE_ATOMIC_COUNTER_BUFFERS = 0x92D9,
            GL_ACTIVE_ATTRIBUTES = 0x8B89,
            GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A,
            GL_ACTIVE_UNIFORMS = 0x8B86,
            GL_ACTIVE_UNIFORM_BLOCKS = 0x8A36,
            GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH = 0x8A35,
            GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87,
            GL_COMPUTE_WORK_GROUP_SIZE = 0x8267,
            GL_PROGRAM_BINARY_LENGTH = 0x8741,
            GL_TRANSFORM_FEEDBACK_BUFFER_MODE = 0x8C7F,
            GL_TRANSFORM_FEEDBACK_VARYINGS = 0x8C83,
            GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x8C76,
            GL_GEOMETRY_VERTICES_OUT = 0x8916,
            GL_GEOMETRY_INPUT_TYPE = 0x8917,
            GL_GEOMETRY_OUTPUT_TYPE = 0x8918
        }

        int _Handle;
        public int Handle { get { return _Handle; } }

        glContext _Parent;
        public glContext Context { get { return _Parent; } }

        internal glProgram(glContext Context, int Handle) { _Parent = Context; _Handle = Handle; }

        public void BindAttribLocation(int index, string name) { _Parent.BindAttribLocation(this, index, name); }
        public int GetAttribLocation(string name) { return _Parent.GetAttribLocation(this, name); }
        public int GetUniformLocation(string name) { return _Parent.GetUniformLocation(this, name); }
        public void AttachShader(glShader Shader) { _Parent.AttachShader(this, Shader); }
        public void LinkProgram() { _Parent.LinkProgram(this); }
        public void UseProgram() { _Parent.UseProgram(this); }

        public void Dispose() { if (_Handle != 0) { _Parent.DeleteProgram(this); _Handle = 0; } }
        ~glProgram() { Dispose(); }
    }
}
