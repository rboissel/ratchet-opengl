using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Drawing.OpenGL
{
    public class glShader : IDisposable
    {
        public enum Type : int
        {
            GL_FRAGMENT_SHADER         = 0x8B30,
            GL_VERTEX_SHADER           = 0x8B31,
            GL_COMPUTE_SHADER          = 0x91B9,
            GL_TESS_CONTROL_SHADER     = 0x8E88,
            GL_TESS_EVALUATION_SHADER  = 0x8E87,
            GL_GEOMETRY_SHADER         = 0x8DD9
        }

        internal enum SymbolicName : int
        {
            GL_SHADER_TYPE             = 0x8B4F,
            GL_DELETE_STATUS           = 0x8B80,
            GL_COMPILE_STATUS          = 0x8B81,
            GL_INFO_LOG_LENGTH         = 0x8B84,
            GL_SHADER_SOURCE_LENGTH    = 0x8B88
        }

        int _Handle;
        public int Handle { get { return _Handle; } }

        glContext _Parent;
        public glContext Context { get { return _Parent; } }

        internal glShader(glContext Context, int Handle) { _Parent = Context; _Handle = Handle; }

        public void Dispose() { if (_Handle != 0) { _Parent.DeleteShader(this); _Handle = 0; } }
        ~glShader() { Dispose(); }
    }
}
