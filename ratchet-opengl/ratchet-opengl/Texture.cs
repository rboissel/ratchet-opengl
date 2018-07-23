using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.Drawing.OpenGL
{
    public class glTexture
    {
        public enum BindTarget
        {
            GL_TEXTURE_1D                   = 0x0DE0,
            GL_TEXTURE_2D                   = 0x0DE1,
            GL_TEXTURE_3D                   = 0x806F,
            GL_TEXTURE_1D_ARRAY             = 0x8C18,
            GL_TEXTURE_2D_ARRAY             = 0x8C1A,
            GL_TEXTURE_RECTANGLE            = 0x84F5,
            GL_TEXTURE_CUBE_MAP             = 0x8513,
            GL_TEXTURE_CUBE_MAP_ARRAY       = 0x9009,
            GL_TEXTURE_BUFFER               = 0x8C2A,
            GL_TEXTURE_2D_MULTISAMPLE       = 0x9100,
            GL_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102
        }

        public enum InternalFormat : int
        {
            GL_RED                                = 0x1903,
            GL_RG                                 = 0x8227,
            GL_RGB                                = 0x1907,
            GL_BGR                                = 0x80E0,
            GL_RGBA                               = 0x1908,
            GL_DEPTH_COMPONENT                    = 0x1902,
            GL_DEPTH_STENCIL                      = 0x84F9,
            GL_R8                                 = 0x8229,
            GL_R8_SNORM                           = 0x8F94,
            GL_R16                                = 0x822A,
            GL_R16_SNORM                          = 0x8F98,
            GL_RG8                                = 0x822B,
            GL_RG8_SNORM                          = 0x8F95,
            GL_RG16                               = 0x822C,
            GL_RG16_SNORM                         = 0x8F99,
            GL_R3_G3_B2                           = 0x2A10,
            GL_RGB4                               = 0x804F,
            GL_RGB5                               = 0x8050,
            GL_RGB8                               = 0x8051,
            GL_RGB8_SNORM                         = 0x8F96,
            GL_RGB10                              = 0x8052,
            GL_RGB12                              = 0x8053,
            GL_RGB16_SNORM                        = 0x8F9A,
            GL_RGBA2                              = 0x8055,
            GL_RGBA4                              = 0x8056,
            GL_RGB5_A1                            = 0x8057,
            GL_RGBA8                              = 0x8058,
            GL_RGBA8_SNORM                        = 0x8F97,
            GL_RGB10_A2                           = 0x8059,
            GL_RGB10_A2UI                         = 0x906F,
            GL_RGBA12                             = 0x805A,
            GL_RGBA16                             = 0x805B,
            GL_SRGB8                              = 0x8C41,
            GL_SRGB8_ALPHA8                       = 0x8C43,
            GL_R16F                               = 0x822D,
            GL_RG16F                              = 0x822F,
            GL_RGB16F                             = 0x881B,
            GL_RGBA16F                            = 0x881A,
            GL_R32F                               = 0x822E,
            GL_RG32F                              = 0x8230,
            GL_RGB32F                             = 0x8815,
            GL_RGBA32F                            = 0x8814,
            GL_R11F_G11F_B10F                     = 0x8C3A,
            GL_RGB9_E5                            = 0x8C3D,
            GL_R8I                                = 0x8231,
            GL_R8UI                               = 0x8232,
            GL_R16I                               = 0x8233,
            GL_R16UI                              = 0x8234,
            GL_R32I                               = 0x8235,
            GL_R32UI                              = 0x8236,
            GL_RG8I                               = 0x8237,
            GL_RG8UI                              = 0x8238,
            GL_RG16I                              = 0x8239,
            GL_RG16UI                             = 0x823A,
            GL_RG32I                              = 0x823B,
            GL_RG32UI                             = 0x823C,
            GL_RGB8I                              = 0x8D8F,
            GL_RGB8UI                             = 0x8D7D,
            GL_RGB16I                             = 0x8D89,
            GL_RGB16UI                            = 0x8D77,
            GL_RGB32I                             = 0x8D83,
            GL_RGB32UI                            = 0x8D71,
            GL_RGBA8I                             = 0x8D8E,
            GL_RGBA8UI                            = 0x8D7C,
            GL_RGBA16I                            = 0x8D88,
            GL_RGBA16UI                           = 0x8D76,
            GL_RGBA32I                            = 0x8D82,
            GL_RGBA32UI                           = 0x8D70,
            GL_COMPRESSED_RED                     = 0x8225,
            GL_COMPRESSED_RG                      = 0x8226,
            GL_COMPRESSED_RGB                     = 0x84ED,
            GL_COMPRESSED_RGBA                    = 0x84EE,
            GL_COMPRESSED_SRGB                    = 0x8C48,
            GL_COMPRESSED_SRGB_ALPHA              = 0x8C49,
            GL_COMPRESSED_RED_RGTC1               = 0x8DBB,
            GL_COMPRESSED_SIGNED_RED_RGTC1        = 0x8DBC,
            GL_COMPRESSED_RG_RGTC2                = 0x8DBD,
            GL_COMPRESSED_SIGNED_RG_RGTC2         = 0x8DBE,
            GL_COMPRESSED_RGBA_BPTC_UNORM         = 0x8E8C,
            GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM   = 0x8E8D,
            GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT   = 0x8E8E,
            GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT = 0x8E8F
        }

        public enum Format : int
        {
            GL_RED              = 0x1903,
            GL_RG               = 0x8227,
            GL_RGB              = 0x1907,
            GL_BGR              = 0x80E0,
            GL_RGBA             = 0x1908,
            GL_BGRA             = 0x80E1,
            GL_RED_INTEGER      = 0x8D94,
            GL_RG_INTEGER       = 0x8228,
            GL_RGB_INTEGER      = 0x8D98,
            GL_BGR_INTEGER      = 0x8D9A,
            GL_RGBA_INTEGER     = 0x8D99,
            GL_BGRA_INTEGER     = 0x8D9B,
            GL_STENCIL_INDEX    = 0x1901,
            GL_DEPTH_COMPONENT  = 0x1902,
            GL_DEPTH_STENCIL    = 0x84F9
        }

        public enum Type : int
        {
            GL_BYTE                          = 0x1400,
            GL_UNSIGNED_BYTE                 = 0x1401,
            GL_SHORT                         = 0x1402,
            GL_UNSIGNED_SHORT                = 0x1403,
            GL_INT                           = 0x1404,
            GL_UNSIGNED_INT                  = 0x1405,
            GL_FLOAT                         = 0x1406,
            GL_UNSIGNED_BYTE_3_3_2           = 0x8032,
            GL_UNSIGNED_BYTE_2_3_3_REV       = 0x8362,
            GL_UNSIGNED_SHORT_5_6_5          = 0x8363,
            GL_UNSIGNED_SHORT_5_6_5_REV      = 0x8364,
            GL_UNSIGNED_SHORT_4_4_4_4        = 0x8033,
            GL_UNSIGNED_SHORT_4_4_4_4_REV    = 0x8365,
            GL_UNSIGNED_SHORT_5_5_5_1        = 0x8034,
            GL_UNSIGNED_SHORT_1_5_5_5_REV    = 0x8366,
            GL_UNSIGNED_INT_8_8_8_8          = 0x8035,
            GL_UNSIGNED_INT_8_8_8_8_REV      = 0x8367,
            GL_UNSIGNED_INT_10_10_10_2       = 0x8036,
            GL_UNSIGNED_INT_2_10_10_10_REV   = 0x8368
        }

        public enum Parameter : int
        {
            GL_TEXTURE_BASE_LEVEL,
            GL_TEXTURE_COMPARE_FUNC,
            GL_TEXTURE_COMPARE_MODE,
            GL_TEXTURE_LOD_BIAS,
            GL_TEXTURE_MIN_FILTER = 0x2801,
            GL_TEXTURE_MAG_FILTER = 0x2800,
            GL_TEXTURE_MIN_LOD,
            GL_TEXTURE_MAX_LOD,
            GL_TEXTURE_MAX_LEVEL,
            GL_TEXTURE_SWIZZLE_R,
            GL_TEXTURE_SWIZZLE_G,
            GL_TEXTURE_SWIZZLE_B,
            GL_TEXTURE_SWIZZLE_A,
            GL_TEXTURE_WRAP_S = 0x2802,
            GL_TEXTURE_WRAP_T,
            GL_TEXTURE_WRAP_R,
        }

        public enum ParameterValue : int
        {
            GL_NEAREST = 0x2600,
            GL_LINEAR = 0x2601,
            GL_NEAREST_MIPMAP_NEAREST = 0x2700,
            GL_LINEAR_MIPMAP_NEAREST = 0x2701,
            GL_NEAREST_MIPMAP_LINEAR = 0x2702,
            GL_LINEAR_MIPMAP_LINEAR = 0x2703
        }

        int _Handle;
        public int Handle { get { return _Handle; } }

        glContext _Parent;
        public glContext Context { get { return _Parent; } }


        internal glTexture(glContext Context, int Handle) { _Parent = Context; _Handle = Handle; }

        public void BindTexture(BindTarget Target)
        {
            _Parent.BindTexture(this, Target);
        }

        public void UnbindTexture(BindTarget Target)
        {
            _Parent.BindTexture(null, Target);
        }

        public void TexImage2D(BindTarget Target, int Level, InternalFormat InternalFormat, int Width, int Height, int Border, Format Format, Type Type, IntPtr Data)
        {
            if (Border != 0) { throw new Exception("Border value must be 0"); }
            _Parent.TexImage2D(this, Target, Level, InternalFormat, Width, Height, Format, Type, Data);
        }

        public unsafe void TexImage2D(BindTarget Target, int Level, InternalFormat InternalFormat, int Width, int Height, int Border, Format Format, Type Type, byte[] Data)
        {
            fixed (byte* pData = &Data[0])
            {
                TexImage2D(Target, Level, InternalFormat, Width, Height, Border, Format, Type, new IntPtr(pData));
            }
        }

        public void TexParameter(BindTarget Target, Parameter Parameter, ParameterValue ParameterValue)
        {
            _Parent.TexParameter(this, Target, (int)Parameter, (int)ParameterValue);
        }
    }
}
