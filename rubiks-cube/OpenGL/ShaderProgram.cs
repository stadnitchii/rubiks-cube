using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenGL
{
    public class AttributeInfo
    {
        public String name = "";
        public int address = -1;
        public ActiveAttribType type;
    }

    public class UniformInfo
    {
        public String name = "";
        public int address = -1;
        public ActiveUniformType type;
    }

    class Shader
    {
        public int Pointer { get; private set; }
        public string InfoLog { get; private set; }

        public Shader(string source, OpenTK.Graphics.OpenGL.ShaderType type)
        {
            this.Pointer = GL.CreateShader(type);
            GL.ShaderSource(Pointer, source);
            GL.CompileShader(Pointer);
            InfoLog = GL.GetShaderInfoLog(Pointer);
        }
    }

    public class ShaderProgram
    {
        public int Pointer { get; private set; }
        public string ProgramInfoLog { get; private set; }
        public string ShaderInfoLog { get; private set; }
        public string Name { get; private set; }
        public List<string> WarningLogs { get; private set; }
        public Dictionary<string, AttributeInfo> Attributes { get; private set; }
        public Dictionary<string, UniformInfo> Uniforms { get; private set; }
        public bool LogWarnings { get; set; }

        public ShaderProgram(string name) :
            this(name + ".vert", name + ".frag", name)
        { }

        public ShaderProgram(string vertexShader, string fragmentShader, string name, bool fromFile = true, string geometryShader = null)
        {
            if (fromFile)
            {
                vertexShader = File.ReadAllText(vertexShader);
                fragmentShader = File.ReadAllText(fragmentShader);
                if (geometryShader != null) geometryShader = File.ReadAllText(geometryShader);
            }

            Shader vertex = new Shader(vertexShader, ShaderType.VertexShader);
            this.ShaderInfoLog += vertex.InfoLog;
            Shader fragment = new Shader(fragmentShader, ShaderType.FragmentShader);
            this.ShaderInfoLog += fragment.InfoLog;
            Shader geometry = null;
            if (geometryShader != null)
            {
                geometry = new Shader(geometryShader, ShaderType.GeometryShader);
                this.ShaderInfoLog += geometry.InfoLog;
            }

            this.Attributes = new Dictionary<string, AttributeInfo>();
            this.Uniforms = new Dictionary<string, UniformInfo>();
            this.WarningLogs = new List<string>();
            this.Name = name;

            Pointer = GL.CreateProgram();
            GL.AttachShader(Pointer, vertex.Pointer);
            GL.AttachShader(Pointer, fragment.Pointer);
            if (geometry != null) GL.AttachShader(Pointer, geometry.Pointer);
            GL.LinkProgram(Pointer);
            ProgramInfoLog = GL.GetProgramInfoLog(Pointer);

            int count = 0;
            GL.GetProgram(Pointer, GetProgramParameterName.ActiveAttributes, out count);

            int size = 0;

            for (int i = 0; i < count; i++)
            {
                AttributeInfo info = new AttributeInfo();
                info.name = GL.GetActiveAttrib(Pointer, i, out size, out info.type);
                info.address = GL.GetAttribLocation(Pointer, info.name);
                Attributes.Add(info.name, info);
            }

            GL.GetProgram(Pointer, GetProgramParameterName.ActiveUniforms, out count);

            for (int i = 0; i < count; i++)
            {
                UniformInfo uInfo = new UniformInfo();
                uInfo.name = GL.GetActiveUniform(Pointer, i, out size, out uInfo.type);
                uInfo.address = GL.GetUniformLocation(Pointer, uInfo.name);
                Uniforms.Add(uInfo.name, uInfo);
            }

            if (!string.IsNullOrEmpty(ShaderInfoLog))
            {
                Console.WriteLine("\n-----" + name + "-----");
                Console.WriteLine(ShaderInfoLog);
            }
        }

        public void writeAllInfoLogs()
        {
            Console.WriteLine(Name);
            Console.WriteLine(this.ProgramInfoLog);
            Console.WriteLine(this.AttributeInfo());
            Console.WriteLine(this.UnifromInfo());
        }

        public string AttributeInfo()
        {
            string info = "";
            foreach (var e in Attributes)
                info += string.Format("AttributeInfo [ Name: {0}, Type: {1}, Location: {2} ]\n", e.Value.name, e.Value.type, e.Value.address);
            return info;
        }

        public string UnifromInfo()
        {
            string info = "";
            foreach (var e in Uniforms)
                info += string.Format("UnifromInfo [ Name: {0}, Type: {1}, Location: {2} ]\n", e.Value.name, e.Value.type, e.Value.address);
            return info;
        }

        //public void bindBufferToShaderAttrib<T>(VBO<T> buffer, string attributeName)
        //    where T : struct
        //{
        //    if (Attributes.ContainsKey(attributeName))
        //    {
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Pointer);
        //        GL.EnableVertexAttribArray(Attributes[attributeName].address);
        //        GL.VertexAttribPointer(Attributes[attributeName].address, 3, VertexAttribPointerType.Float, true, 0, 0);
        //    }
        //    else
        //        logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", attributeName, Name));
        //}

        //public void bindBufferToShaderAttrib<T>(VBO<T> buffer, string attributeName, ushort count)
        //     where T : struct
        //{
        //    if (Attributes.ContainsKey(attributeName))
        //    {
        //        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Pointer);
        //        GL.VertexAttribPointer(Attributes[attributeName].address, count, VertexAttribPointerType.Float, true, 0, 0);
        //        GL.EnableVertexAttribArray(Attributes[attributeName].address);
        //    }
        //    else
        //        logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", attributeName, Name));
        //}


        public void Use()
        {
            GL.UseProgram(Pointer);
        }

        #region setUniform
        public void setUniform(string name, bool value)
        {
            if (Uniforms.ContainsKey(name))
                GL.Uniform1(Uniforms[name].address, (value) ? 1 : 0);
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, int value)
        {
            if (Uniforms.ContainsKey(name))
                GL.Uniform1(Uniforms[name].address, value);
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, float value)
        {
            if (Uniforms.ContainsKey(name))
                GL.Uniform1(Uniforms[name].address, value);
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, float value, float value2)
        {
            if (Uniforms.ContainsKey(name))
                GL.Uniform2(Uniforms[name].address, value, value2);
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, float value, float value2, float value3)
        {
            if (Uniforms.ContainsKey(name))
                GL.Uniform3(Uniforms[name].address, value, value2, value3);
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, float value, float value2, float value3, float value4)
        {
            if (Uniforms.ContainsKey(name))
                GL.Uniform4(Uniforms[name].address, value, value2, value3, value4);
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, Matrix4 mat)
        {
            if (Uniforms.ContainsKey(name))
                GL.UniformMatrix4(Uniforms[name].address, false, ref mat);
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, Matrix4[] mat)
        {
            if (Uniforms.ContainsKey("{name}[0]"))
            {
                for(int i = 0; i< mat.Length; i++)
                {
                    int loc = GL.GetUniformLocation(Pointer, "{name}[{i}]");
                    GL.UniformMatrix4(loc, false, ref mat[i]);
                }
            }
            else
                logWarning(string.Format("Warning: \"{0}\" not found in shader \"{1}\"", name, Name));
        }

        public void setUniform(string name, Vector2 vec)
        {
            setUniform(name, vec.X, vec.Y);
        }

        public void setUniform(string name, Vector3 vec)
        {
            setUniform(name, vec.X, vec.Y, vec.Z);
        }

        public void setUniform(string name, Vector4 vec)
        {
            setUniform(name, vec.X, vec.Y, vec.Z, vec.W);
        }

        private void logWarning(string log)
        {
            if (LogWarnings)
            {
                bool contains = false;
                foreach (string s in WarningLogs)
                    if (WarningLogs.Contains(log))
                        contains = true;
                if (!contains)
                {
                    Console.WriteLine(log);
                    WarningLogs.Add(log);
                }
            }
        }
        #endregion
    }
}
