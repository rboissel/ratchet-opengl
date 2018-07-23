using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Drawing.OpenGL
{
    public class glBuffer
    {
        public enum BindTarget
        {
            GL_ARRAY_BUFFER         = 0x8892,
            GL_ELEMENT_ARRAY_BUFFER = 0x8893,
            GL_PIXEL_PACK_BUFFER    = 0x88EB,
            GL_PIXEL_UNPACK_BUFFER  = 0x88EC
        }

        public enum Usage : int
        {
            GL_STREAM_DRAW = 0x88E0,
            GL_STREAM_READ = 0x88E1,
            GL_STREAM_COPY = 0x88E2,

            GL_STATIC_DRAW = 0x88E4,
            GL_STATIC_READ = 0x88E5,
            GL_STATIC_COPY = 0x88E6,

            GL_DYNAMIC_DRAW = 0x88E8,
            GL_DYNAMIC_READ = 0x88E9,
            GL_DYNAMIC_COPY = 0x88EA
        }

        int _Handle;
        public int Handle { get { return _Handle; } }

        glContext _Parent;
        public glContext Context { get { return _Parent; } }


        internal glBuffer(glContext Context, int Handle) { _Parent = Context; _Handle = Handle; }

        public void BindBuffer(BindTarget Target)
        {
            _Parent.BindBuffer(this, Target);
        }

        public void UnbindBuffer(BindTarget Target)
        {
            _Parent.BindBuffer(null, Target);
        }

        public unsafe void BufferData(BindTarget Target, int Size, byte[] Data, Usage Usage)
        {
            if (Data == null) { _Parent.BufferData(this, Target, Size, new IntPtr(0), Usage); }
            else
            {
                fixed (byte* pData = &Data[0]) { _Parent.BufferData(this, Target, Size, new IntPtr(pData), Usage); }
            }
        }

        public unsafe void BufferData(BindTarget Target, int Size, float[] Data, Usage Usage)
        {
            if (Data == null) { _Parent.BufferData(this, Target, Size, new IntPtr(0), Usage); }
            else
            {
                fixed (float* pData = &Data[0]) { _Parent.BufferData(this, Target, Size, new IntPtr(pData), Usage); }
            }
        }

        public unsafe void BufferData(BindTarget Target, int Size, int[] Data, Usage Usage)
        {
            if (Data == null) { _Parent.BufferData(this, Target, Size, new IntPtr(0), Usage); }
            else
            {
                fixed (int* pData = &Data[0]) { _Parent.BufferData(this, Target, Size, new IntPtr(pData), Usage); }
            }
        }

        public void BufferData(BindTarget Target, int Size, IntPtr Data, Usage Usage)
        {
            _Parent.BufferData(this, Target, Size, Data, Usage);
        }

        public IntPtr MapBuffer()
        {
            return new IntPtr(0);
        }

        public void UnmapBuffer()
        {

        }
    }
}
