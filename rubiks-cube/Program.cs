using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using OpenGL;
using System.Diagnostics;
using Microsoft.Win32;

namespace RubiksCube
{
    class Program
    {
        public bool drawToPickBuffer = false;

        public float AnimaitonDuration = .5f;
        public int CubeDistance = 2;

        public GameWindow Window { get; set; }
        public List<Model> parts = new List<Model>();

        public ShaderProgram shader;
        public ShaderProgram pickShader;
        public SkyBox skybox;
        public Camera camera;

        public GuideFactory gf;

        public CubeState cubeState;

        public AnimationManager animationManager;

        private System.Diagnostics.Process p = null;

        //https://en.wikipedia.org/wiki/Optimal_solutions_for_Rubik%27s_Cube

        public static IntPtr SizeOf<T>(T[] array) where T : struct
        {
            return (IntPtr)(array.Length * System.Runtime.InteropServices.Marshal.SizeOf(default(T)));
        }

        public void WriteSystemInformation()
        {
            using (new GameWindow())
            {
                Console.WriteLine("<-------OpenGL Information------->");
                Console.WriteLine("Graphics Card: " + GL.GetString(StringName.Renderer));
                Console.WriteLine("Vendor: " + GL.GetString(StringName.Vendor));
                Console.WriteLine("OpenGL Version: " + GL.GetString(StringName.Version));
                Console.WriteLine("GLSL Version: " + GL.GetString(StringName.ShadingLanguageVersion));

                var device = OpenTK.DisplayDevice.Default;
                Console.WriteLine("\n<-------Display Information------->");
                Console.WriteLine(string.Format("({0}, {1}) {2}bpp {3}Hz\n\n", device.Width, device.Height, device.BitsPerPixel, device.RefreshRate));
            }

        }

        [STAThread]
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            WriteSystemInformation();

            Window = new GameWindow(1600, 900, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8), "Rubix Cube", GameWindowFlags.Default);
            Window.VSync = VSyncMode.Off;
            Window.Resize += Window_Resize;
            Window.TargetRenderFrequency = 60;
            Window.TargetUpdateFrequency = 60;
            Window.UpdateFrame += Window_UpdateFrame;
            Window.RenderFrame += Window_RenderFrame;
            Window.MouseDown += Window_MouseDown;
            Window.MouseUp += Window_MouseUp;
            Window.MouseMove += Window_MouseMove;
            Window.MouseWheel += Window_MouseWheel;
            Window.KeyDown += Window_KeyDown;

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            ObjLoader loader = new ObjLoader();

            var vao = loader.loadVBOS("Content/cube-part.obj", 3);

            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80), 108f / 72, .1f, 100);

            shader = new ShaderProgram("Shaders/shader.vert", "Shaders/shader.frag", "default");
            shader.Use();
            shader.setUniform("proj", proj);

            pickShader = new ShaderProgram("Shaders/pick.vert", "Shaders/pick.frag", "pick");
            pickShader.Use();
            pickShader.setUniform("proj", proj);

            Texture txt = new Texture("Content/txt.bmp");

            addCubes(vao, txt, shader);

            camera = new Camera(10);
            camera.Projection = proj;
            camera.setDepth(0, 20);

            cubeState = new CubeState();

            Texture cubeTexture = new Texture();
            cubeTexture.loadAsCubeMap("Content/Cubemap/");

            skybox = new SkyBox(cubeTexture.Id);

            animationManager = new AnimationManager(60, AnimaitonDuration, CubeDistance, parts, cubeState);
            animationManager.AnimationSequenceFinished += animationManager_AnimationSequenceFinished;
            animationManager.AnimationFinished += AnimationManager_AnimationFinished;      

            Window.Run(60);

        }

        private void Window_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Window.Width, Window.Height);
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80), Window.Width / (float) Window.Height, .1f, 100);
            camera.Projection = proj;

            pickShader.Use();
            pickShader.setUniform("proj", proj);

            shader.Use();
            shader.setUniform("proj", proj);
        }

        private void AnimationManager_AnimationFinished()
        {
            cubeState.printState();
        }

        void animationManager_AnimationSequenceFinished()
        {
            //cubeState.printState();
        }

        #region AddCubes
        void addCubes(VAO vao, Texture txt, ShaderProgram shader)
        {
            //starting with bottom-back-left block
            //then filling to the right, then filling forward, then filling up
            float x = -CubeDistance;
            float y = -CubeDistance;
            float z = -CubeDistance;

            gf = new GuideFactory();
            

            Model model = null;

            #region Bottom9
            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, false, false, true, false, true);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, false, true, false, true);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, true, true, false, true);
            parts.Add(model);
            x = -CubeDistance;
            z += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, false, false, false, false, true);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, false, false, false, true);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, true, false, false, true);
            parts.Add(model);
            x = -CubeDistance;
            z += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, true, false, false, false, true);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, true, false, false, false, true);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, true, true, false, false, true);
            parts.Add(model);
            x = -CubeDistance;
            z = -CubeDistance;
            y += CubeDistance;
            #endregion

            #region Middle8
            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, false, false, true, false, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, false, true, false, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, true, true, false, false);
            parts.Add(model);
            x = -CubeDistance;
            z += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, false, false, false, false, false);
            parts.Add(model);
            x += CubeDistance;
            x += CubeDistance; // no middle cube so no need to place it there

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, true, false, false, false);
            parts.Add(model);
            x = -CubeDistance;
            z += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, true, false, false, false, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, true, false, false, false, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, true, true, false, false, false);
            parts.Add(model);
            x = -CubeDistance;
            z = -CubeDistance;
            y += CubeDistance;
            #endregion

            #region Top9
            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, false, false, true, true, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, false, true, true, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, true, true, true, false);
            parts.Add(model);
            x = -CubeDistance;
            z += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, false, false, false, true, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, false, false, true, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, false, true, false, true, false);
            parts.Add(model);
            x = -CubeDistance;
            z += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(true, true, false, false, true, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, true, false, false, true, false);
            parts.Add(model);
            x += CubeDistance;

            model = gf.createModel(vao, txt, shader);
            model.setPosition(x, y, z);
            model.setColors(false, true, true, false, true, false);
            parts.Add(model);
            x = -CubeDistance;
            z = -CubeDistance;
            y += CubeDistance;
            #endregion
        }
        #endregion

        void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            //color picking
            GL.ClearColor(1f, 1f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var c in parts)
                c.drawToPickBuffer(pickShader, camera.Transfrom);

            byte[] pixel = new byte[4];
            GL.ReadPixels(Window.Mouse.X, Window.Height - Window.Mouse.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);
            var m = gf.getModel(new Vector3(pixel[0], pixel[1], pixel[2]));
            if (m != null)
            {
                m.picked = true;
            }

            if (!drawToPickBuffer)
            {
                GL.ClearColor(.5f, .5f, .2f, 1f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                foreach (var c in parts)
                    c.draw(camera.Transfrom);

                skybox.draw(camera);
            }

            Window.SwapBuffers();
        }

        void Window_MouseWheel(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            camera.mouseWheel(e.Delta);
        }

        void Window_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            camera.mouseMove(e.X, e.Y);
        }

        void Window_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            camera.mouseUp();
        }

        void Window_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            camera.mouseDown(e.X, e.Y);

            byte[] pixel = new byte[4];
            GL.ReadPixels(e.X, Window.Height - e.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);
            Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", pixel[0], pixel[1], pixel[2], pixel[3]));
        }

        void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            #region Cube Rotation
            //left
            if (!animationManager.IsAnimating)
            {
                if (e.Key == OpenTK.Input.Key.A)
                {
                    string move = "L";
                    if (e.Shift)
                        move = move + "'";

                    animationManager.Animate(move);
                }

                //right
                if (e.Key == OpenTK.Input.Key.D)
                {
                    string move = "R";
                    if (e.Shift)
                        move = move + "'";

                    animationManager.Animate(move);
                }

                //top
                if (e.Key == OpenTK.Input.Key.W)
                {
                    string move = "U";
                    if (e.Shift)
                        move = move + "'";

                    animationManager.Animate(move);
                }

                //bottom
                if (e.Key == OpenTK.Input.Key.X)
                {
                    string move = "D";
                    if (e.Shift)
                        move = move + "'";

                    animationManager.Animate(move);
                }

                //front
                if (e.Key == OpenTK.Input.Key.S)
                {
                    string move = "F";
                    if (e.Shift)
                        move = move + "'";

                    animationManager.Animate(move);
                }

                //back
                if (e.Key == OpenTK.Input.Key.F)
                {
                    string move = "B";
                    if (e.Shift)
                        move = move + "'";

                    animationManager.Animate(move);
                }
            }

            if (p == null && e.Key == OpenTK.Input.Key.M)
            {

                RegistryKey rk = Registry.LocalMachine;
                RegistryKey subKey = rk.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");

                string currentVerion = subKey?.GetValue("CurrentVersion").ToString();

                if(string.IsNullOrWhiteSpace(currentVerion))
                {
                    Console.WriteLine("Java must be installed");
                    return;
                }

                string state = cubeState.getStateString();

                p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += p_OutputDataReceived;
                p.StartInfo.FileName = "java";
                p.StartInfo.Arguments = "-jar solver.jar " + state;

                p.Start();
                p.BeginOutputReadLine();
            }
            #endregion

            if (e.Key == OpenTK.Input.Key.P)
                drawToPickBuffer = !drawToPickBuffer;
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            string[] moves = e.Data.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            animationManager.AnimateSequence(moves);

            p = null;
        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            camera.update();
            animationManager.Update();
        }
    }
}
