using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class VAO
    {
        public int Id { get; set; }
        public int Lenth { get; set; }

        private List<int> Buffers { get; set; }

        public VAO(float[] _verts, float[] _uvs, float[] _normals)
        {
            Buffers = new List<int>();
            this.Lenth = _verts.Length / 3;
            this.Id = GL.GenVertexArray();

            this.addAttribute(_verts, 3);
            this.addAttribute(_uvs, 2);
            this.addAttribute(_normals, 3);
        }

        public VAO(float[] _verts)
        {
            Buffers = new List<int>();
            this.Lenth = _verts.Length / 3;
            this.Id = GL.GenVertexArray();

            this.addAttribute(_verts, 3);
        }

        public int addAttribute(float[] bufferData, int size)
        {
            GL.BindVertexArray(Id);

            //generate buffer add it and enable atribute array
            int bufferID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(bufferData.Length * sizeof(float)), bufferData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Buffers.Count, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(Buffers.Count);
            Buffers.Add(bufferID);
            
            //unbind our buffers
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return Buffers.Count;
        }

        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            foreach (var bufferID in Buffers)
                GL.DeleteBuffer(bufferID);

            GL.DeleteVertexArray(Id);
        }
    }
}
