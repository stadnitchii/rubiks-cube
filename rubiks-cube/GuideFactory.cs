using OpenGL;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    class GuideFactory
    {
        Dictionary<Vector3, Model> models;
        Vector3 color;
        int increment =50;

        public GuideFactory()
        {
            models = new Dictionary<Vector3, Model>();
            color = new Vector3(increment, 0, 0);
        }

        public Model createModel(VAO _vao, Texture _t, ShaderProgram _shader)
        {
            Model m = new Model(_vao, _t, _shader);
            m.pickColor = color;
            models.Add(color, m);
            color = increaseColor(color);
            return m;
        }

        public Model getModel(Vector3 c)
        {
            if(models.ContainsKey(c))
                return models[c];
            return null;
        }

        public void dumpKeys()
        {
            foreach(var k in models.Keys)
                Console.WriteLine(k);
        }

        private Vector3 increaseColor(Vector3 color)
        {
            Vector3 nc = new Vector3(color.X += increment, color.Y, color.Z);
            if (nc.X > 255)
            {
                nc.X = 0;
                nc.Y += increment;
            }
            if(nc.Y > 255)
            {
                nc.Y = 0;
                nc.Z += increment;
            }
            return nc;
        }
    }
}
