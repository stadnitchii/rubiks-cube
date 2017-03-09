using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenGL
{
    class Texture
    {
        public int Id { get; private set; }

        public Vector2 Size { get; private set; }

        public Texture(string path)
        {
            this.Id = 0;
            this.loadFromFile(path);
        }

        public Texture()
        {
            this.Id = 0;
        }

        public void loadFromFile(string path)
        {
            using (Bitmap img = new Bitmap(path))
            {
                img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                BitmapData bits = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, Id);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, img.Width, img.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


                float maxAniso;
                GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAniso);
                GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAniso);

                img.UnlockBits(bits);
            }
        }

        public void loadAsCubeMap(string direcotory, string ext = ".png")
        {
            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, Id);

            string[] paths = new string[] { direcotory + "/right" + ext, direcotory + "/left" + ext, direcotory + "/top" + ext,
            direcotory + "/bottom" + ext, direcotory + "/front" + ext, direcotory + "/back" + ext};

            for (int i = 0; i < 6; i++)
            {
                using (Bitmap img = new Bitmap(paths[i]))
                {
                    //img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    BitmapData bits = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, img.Width, img.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);
                    img.UnlockBits(bits);
                }
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        }
    }
}
