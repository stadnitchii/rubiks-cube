using System;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    public class SkyBox
    {
        private ShaderProgram Shader;
        private int Texture;

        private int vao;
        private int verts;
        private int elements;

        public SkyBox(int textureId)
        {
            Texture = textureId;

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            verts = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, verts);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (verts_f.Length * sizeof(float)), verts_f, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            elements = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr) (elems_u.Length * sizeof(uint)), elems_u, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            Shader = Shaders.SkyboxShader();
        }

        public void draw(Camera cam)
        {
            GL.DepthFunc(DepthFunction.Lequal);
            GL.CullFace(CullFaceMode.Front);

            GL.BindTexture(TextureTarget.TextureCubeMap, Texture);

            Shader.Use();
            Shader.setUniform("proj", cam.Projection);
            Shader.setUniform("view", cam.Transfrom);

            GL.BindVertexArray(vao);
            //GL.DrawArrays(PrimitiveType.Quads, 0, verts_f.Length);
            GL.DrawElements(PrimitiveType.Quads, elems_u.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);

            GL.DepthFunc(DepthFunction.Less);
            GL.CullFace(CullFaceMode.Back);
        }

        private readonly float[] verts_f = new float[]
        {
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, 1.0f,
            -1.0f, -1.0f, 1.0f,
            -1.0f, -1.0f, -1.0f,
            1.0f, 1.0f, -1.0f,
            1.0f, 1.0f, 1.0f,
            -1.0f, 1.0f, 1.0f,
            -1.0f, 1.0f, -1.0f
        };

        private readonly uint[] elems_u = new uint[]
        {
            0, 1, 2, 3,
            4, 7, 6, 5,
            0, 4, 5, 1,
            1, 5, 6, 2,
            2, 6, 7, 3,
            4, 0, 3, 7 
        };
    }
}
