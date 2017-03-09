using System;
using System.Collections.Generic;
using System.IO;

namespace OpenGL
{
    class Polygon
    {
        public float[] Verts { get; private set; }
        public int Size { get; private set; }

        public Polygon(float[] verts, int size)
        {
            this.Verts = verts;
            this.Size = size;
        }
    }

    class ObjLoader
    {
        //data
        private List<float> verts;
        private List<float> normals;
        private List<float> txtCoords;
        private List<Polygon> faces;

        public ObjLoader()
        {
            verts = new List<float>();
            normals = new List<float>();
            txtCoords = new List<float>();
        }

        public List<Polygon> load(string path)
        {
            this.faces = new List<Polygon>();
            this.verts.Clear();
            this.normals.Clear();
            this.txtCoords.Clear();
            loadFromObj(path);
            return this.faces;
        }

        public VAO loadVBOS(string path, int _verts = 3)
        {
            this.faces = new List<Polygon>();
            this.verts.Clear();
            this.normals.Clear();
            this.txtCoords.Clear();
            loadFromObj(path);
            
            List<float> verts = new List<float>();
            List<float> uvs = new List<float>();
            List<float> normals = new List<float>();
            List<uint> elements = new List<uint>();

            uint e = 0;
            foreach(Polygon p in faces)
            {
                for(int i = 0; i< _verts * 8; i+=8)
                {
                    verts.Add(p.Verts[i]);
                    verts.Add(p.Verts[i + 1]);
                    verts.Add(p.Verts[i + 2]);

                    uvs.Add(p.Verts[i + 3]);
                    uvs.Add(p.Verts[i + 4]);

                    normals.Add(p.Verts[i + 5]);
                    normals.Add(p.Verts[i + 6]);
                    normals.Add(p.Verts[i + 7]);
                    elements.Add(e);
                    e++;
                }
            }

            return new VAO(verts.ToArray(), uvs.ToArray(), normals.ToArray());
        }

        private void loadFromObj(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            string[] lines = File.ReadAllLines(path);

            foreach (string str in lines)
            {
                if (!string.IsNullOrWhiteSpace(str) && str.Length > 2)
                {
                    switch (str.Substring(0, 2))
                    {
                        case "v ":
                            parseVert(str);
                            break;

                        case "vn":
                            parseNormal(str);
                            break;

                        case "vt":
                            parseTexture(str);
                            break;

                        case "f ":
                            parseFace(str);
                            break;
                    }
                }
            }

            Console.WriteLine("Loaded file {0}, with {1} faces", path, faces.Count);
        }

        private void parseVert(string str)
        {
            string[] parts = str.Split(new string[] { " ", "  ", "   " }, StringSplitOptions.RemoveEmptyEntries);
            verts.Add(float.Parse(parts[1]));
            verts.Add(float.Parse(parts[2]));
            verts.Add(float.Parse(parts[3]));
        }

        private void parseTexture(string str)
        {
            string[] parts = str.Split(new string[] { " ", "  ", "   " }, StringSplitOptions.RemoveEmptyEntries);
            txtCoords.Add(float.Parse(parts[1]));
            txtCoords.Add(float.Parse(parts[2]));
        }

        private void parseNormal(string str)
        {
            string[] parts = str.Split(new string[] { " ", "  ", "   " }, StringSplitOptions.RemoveEmptyEntries);
            normals.Add(float.Parse(parts[1]));
            normals.Add(float.Parse(parts[2]));
            normals.Add(float.Parse(parts[3]));
        }

        private void parseFace(string str)
        {
            List<string> verticies = new List<string>(str.Split(new string[] { " ", "  ", "   " }, StringSplitOptions.RemoveEmptyEntries));
            verticies.RemoveAt(0);

            //ading faces
            float[] data = new float[verticies.Count * 8];
            int offset = 0;

            foreach (string s in verticies)
            {
                // 1/2/3 = vertex 1 / textureCoord 2 / normal 3
                string[] parts = s.Split(new string[] { "/" }, StringSplitOptions.None);

                int vertNumber = int.Parse(parts[0]);
                vertNumber = (vertNumber - 1) * 3;

                int txtNumber = int.Parse(parts[1]);
                txtNumber = (txtNumber - 1) * 2;

                int normalNumber = int.Parse(parts[2]);
                normalNumber = (normalNumber - 1) * 3;


                data[0 + offset] = verts[vertNumber];
                data[1 + offset] = verts[vertNumber + 1];
                data[2 + offset] = verts[vertNumber + 2];

                data[3 + offset] = txtCoords[txtNumber];
                data[4 + offset] = txtCoords[txtNumber + 1];

                data[5 + offset] = normals[normalNumber];
                data[6 + offset] = normals[normalNumber + 1];
                data[7 + offset] = normals[normalNumber + 2];

                offset += 8;
            }

            this.faces.Add(new Polygon(data, verticies.Count));

        }
    }
}
