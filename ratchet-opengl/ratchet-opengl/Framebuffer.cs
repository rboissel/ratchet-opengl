using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Drawing.OpenGL
{
    public class glFramebuffer : IDisposable
    {
        public enum BindTarget
        {
            GL_FRAMEBUFFER = 0x8D40,
            GL_READ_FRAMEBUFFER = 0x8CA8,
            GL_DRAW_FRAMEBUFFER = 0x8CA9,
        }

        public enum Attachment
        {
            GL_COLOR_ATTACHMENT0 = 0x8CE0,
            GL_DEPTH_ATTACHMENT = 0x8D00,
        }


        int _Handle;
        public int Handle { get { return _Handle; } }

        glContext _Parent;
        public glContext Context { get { return _Parent; } }


        internal glFramebuffer(glContext Context, int Handle) { _Parent = Context; _Handle = Handle; }

        public void BindFramebuffer(BindTarget Target)
        {
            _Parent.BindFramebuffer(this, Target);
        }

        public void UnbindFramebuffer(BindTarget Target)
        {
            _Parent.BindFramebuffer(null, Target);
        }

        public void AttachTexture(BindTarget Target, Attachment Attachment, glTexture texture, int Level)
        {
            _Parent.FramebufferTexture(this, Target, Attachment, texture, Level);
        }

        public void Dispose() { if (_Handle != 0) { _Parent.DeleteFramebuffer(this); _Handle = 0; } }
        ~glFramebuffer() { Dispose(); }
    }
}
