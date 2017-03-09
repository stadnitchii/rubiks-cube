using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Model
    {
        public ShaderProgram Shader { get; protected set; }
        public VAO Vao { get; protected set; }
        public Texture Texture { get; protected set; }

        public Vector3 Position { get; set; }

        public double lr = 0;

        public Vector3 pickColor;
        public bool picked = false;

        bool l, f, r, ba, t, bo;

        public Model(VAO _vao, Texture t, ShaderProgram defaultShader)
        {
            this.Shader = defaultShader;
            this.Vao = _vao;
            this.Texture = t;
            this.Position = new Vector3(0);
            this.Transform = Matrix4.Identity;
            this.Rotation = Matrix4.Identity;
            this.Scale = 1f;
            this.createTransform();
            this.pickColor = new Vector3(1, 0, 0);
        }

        public void setColors(bool _l, bool _f, bool _r, bool _ba, bool _t, bool _bo)
        {
            l = _l;
            f = _f;
            r = _r;
            ba = _ba;
            t = _t;
            bo = _bo;
        }

        public virtual void draw(Matrix4 view)
        {
            if (picked)
            {
                //this.Scale = .75f;
            }

            GL.BindTexture(TextureTarget.Texture2D, this.Texture.Id);
            Shader.Use();
            Shader.setUniform("model", this.transform);
            Shader.setUniform("view", view);
            Shader.setUniform("picked", picked);

            Shader.setUniform("l", l);
            Shader.setUniform("f", f);
            Shader.setUniform("r", r);
            Shader.setUniform("ba", ba);
            Shader.setUniform("t", t);
            Shader.setUniform("bo", bo);

            Vao.Bind();

            GL.DrawArrays(PrimitiveType.Triangles, 0, Vao.Lenth);

            Vao.Unbind();

            if (picked)
            {
                picked = false;
                //this.Scale = 1.0f;
            }
        }

        public void drawToPickBuffer(ShaderProgram shader, Matrix4 view)
        {
            shader.Use();
            shader.setUniform("model", this.transform);
            shader.setUniform("view", view);
            shader.setUniform("pickColor", this.pickColor);

            Vao.Bind();

            GL.DrawArrays(PrimitiveType.Triangles, 0, Vao.Lenth);

            Vao.Unbind();
        }

        public virtual void update(double delta)
        {

        }

        public void setRotation(Vector3 axis, double angle)
        {
            rotation = Matrix4.CreateFromAxisAngle(axis, (float)angle);
            createTransform();
        }

        public void Rotate(Vector3 axis, double angle)
        {
            rotation = Matrix4.Mult(rotation, Matrix4.CreateFromAxisAngle(axis, (float)angle));
            createTransform();
        }

        public void setPosition(double x, double y, double z)
        {
            this.Position = new Vector3((float)x, (float)y, (float)z);
            createTransform();
        }

        protected void createTransform()
        {
            Transform = Matrix4.Mult(Matrix4.CreateScale(Scale), Matrix4.Mult(rotation, Matrix4.CreateTranslation(Position)));
        }

        #region Properties
        protected float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; createTransform(); }
        }


        protected Matrix4 transform;
        public Matrix4 Transform
        {
            get { return transform; }
            protected set { this.transform = value; }
        }

        protected Matrix4 rotation;
        public Matrix4 Rotation
        {
            get { return rotation; }
            protected set { this.rotation = value; createTransform(); }
        }
        #endregion
    }
}
