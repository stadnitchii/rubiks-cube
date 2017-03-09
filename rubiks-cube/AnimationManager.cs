using OpenGL;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    sealed class AnimationManager
    {
        public float UpdateRate { get; set; }
        public float AnimationDuration { get; set; }
        public bool IsAnimating { get; private set; }
        public float CubeDistance { get; set; }

        public event Action AnimationFinished;
        public event Action AnimationSequenceFinished;

        private IEnumerable<Model> parts;
        private IEnumerable<Model> animatedParts;

        //animation info
        private Matrix4d RotatinMatrix { get; set; }
        private Vector3 RotationVector { get; set; }
        private double Pheta { get; set; }
        private int TicksRemaining { get; set; }

        private List<string> animationSequence;

        private CubeState states;

        public enum Axis
        {
            X, Y, Z
        }

        public AnimationManager(float _updateRate, float _animationDuration, float _cubeDistance, IEnumerable<Model> _parts, CubeState _states)
        {
            this.UpdateRate = _updateRate;
            this.IsAnimating = false;
            this.AnimationDuration = _animationDuration;
            this.CubeDistance = _cubeDistance;
            this.parts = _parts;
            this.states = _states;
        }

        public void init(int fps, int animationDuration)
        {
            int totalTicks = fps * animationDuration;
            double pheta = Math.PI / 2.0 / totalTicks;
        }

        public void Animate(String moveString)
        {
            if (IsAnimating)
                return;

            TicksRemaining = (int)(UpdateRate * AnimationDuration);
            Pheta = Math.PI / 2.0 / TicksRemaining;

            setAnimationInfo(moveString);

            states.Move(moveString);

            IsAnimating = true;
        }

        public void AnimateSequence(string[] sequence)
        {
            animationSequence = sequence.ToList();
        }

        private void setAnimationInfo(String moveString)
        {
            if (moveString.Length > 1)// R' R R2
            {
                if (moveString[1] == '\'') Pheta *= -1;
                if (moveString[1] == '2') Pheta *= 2;
            }

            switch (moveString[0])
            {
                case 'L':
                    Pheta *= -1;
                    animatedParts = from part in this.parts where part.Position.X == -CubeDistance select part;
                    RotatinMatrix = getXRotationMatrix(Pheta);
                    RotationVector = new Vector3(1, 0, 0);
                    break;
                case 'R':
                    animatedParts = from part in this.parts where part.Position.X == CubeDistance select part;
                    RotatinMatrix = getXRotationMatrix(Pheta);
                    RotationVector = new Vector3(1, 0, 0);
                    break;
                case 'F':
                    animatedParts = from part in this.parts where part.Position.Z == CubeDistance select part;
                    RotatinMatrix = getZRotationMatrix(Pheta);
                    RotationVector = new Vector3(0, 0, 1);
                    break;
                case 'B':
                    Pheta *= -1;
                    animatedParts = from part in this.parts where part.Position.Z == -CubeDistance select part;
                    RotatinMatrix = getZRotationMatrix(Pheta);
                    RotationVector = new Vector3(0, 0, 1);
                    break;
                case 'U':
                    animatedParts = from part in this.parts where part.Position.Y == CubeDistance select part;
                    RotatinMatrix = getYRotationMatrix(Pheta);
                    RotationVector = new Vector3(0, 1, 0);
                    break;
                case 'D':
                    Pheta *= -1;
                    animatedParts = from part in this.parts where part.Position.Y == -CubeDistance select part;
                    RotatinMatrix = getYRotationMatrix(Pheta);
                    RotationVector = new Vector3(0, 1, 0);
                    break;
                default: Console.WriteLine(string.Format("Face {0}, not recognized}", moveString[0])); break;
            }
        }

        public void Update()
        {
            if (TicksRemaining > 0)
            {
                foreach (var part in animatedParts)
                {
                    float x = part.Position.X;
                    float y = part.Position.Y;
                    float z = part.Position.Z;


                    Vector3d newPost = Vector3d.Transform(new Vector3d(part.Position.X, part.Position.Y, part.Position.Z), RotatinMatrix);

                    part.setPosition(newPost.X, newPost.Y, newPost.Z);
                    part.Rotate(RotationVector, -Pheta);
                }

                TicksRemaining--;
                if (TicksRemaining == 0)
                {
                    //because sometmes we get 9.70093128E-9 instead of 0 we need to round up or down so
                    //out LINQ can properly select the top,front, back, etc parts
                    //after a 90 degree turn all parts in ALL axis shoul be in either -2, 0, 2 (or CubeDistance vairable)
                    foreach (var part in parts)
                    {
                        part.Position = new Vector3((float)Math.Round(part.Position.X), (float)Math.Round(part.Position.Y), (float)Math.Round(part.Position.Z));
                    }

                    IsAnimating = false;
                    if (AnimationFinished != null)
                        AnimationFinished();
                }
            }

            if (!IsAnimating && animationSequence != null)
            {
                Animate(animationSequence[0]);
                animationSequence.RemoveAt(0);

                if (animationSequence.Count == 0)
                {
                    animationSequence = null;
                    if (AnimationSequenceFinished != null)
                        AnimationSequenceFinished();
                }
            }
        }

        #region RotationMatricies
        //http://inside.mines.edu/fs_home/gmurray/ArbitraryAxisRotation/

        private Matrix4d getXRotationMatrix(double pheta)
        {
            return new Matrix4d(new Vector4d(1, 0, 0, 0),
                                new Vector4d(0, Math.Cos(pheta), -Math.Sin(pheta), 0),
                                new Vector4d(0, Math.Sin(pheta), Math.Cos(pheta), 0),
                                new Vector4d(0, 0, 0, 1));

        }

        private Matrix4d getYRotationMatrix(double pheta)
        {
            return new Matrix4d(new Vector4d(Math.Cos(pheta), 0, Math.Sin(pheta), 0),
                                new Vector4d(0, 1, 0, 0),
                                new Vector4d(-Math.Sin(pheta), 0, Math.Cos(pheta), 0),
                                new Vector4d(0, 0, 0, 1));
        }

        private Matrix4d getZRotationMatrix(double pheta)
        {
            return new Matrix4d(new Vector4d(Math.Cos(pheta), -Math.Sin(pheta), 0, 0),
                                new Vector4d(Math.Sin(pheta), Math.Cos(pheta), 0, 0),
                                new Vector4d(0, 0, 1, 0),
                                new Vector4d(0, 0, 0, 1));
        }

        //testing
        //private Matrix3d getXRotationMatrix(double pheta)
        //{
        //    return new Matrix3d(new Vector3d(1, 0, 0),
        //                        new Vector3d(0, Math.Cos(pheta), -Math.Sin(pheta)),
        //                        new Vector3d(0, Math.Sin(pheta), Math.Cos(pheta)));

        //}

        //private Matrix3d getYRotationMatrix(double pheta)
        //{
        //    return new Matrix3d(new Vector3d(Math.Cos(pheta), 0, Math.Sin(pheta)),
        //                        new Vector3d(0, 1, 0),
        //                        new Vector3d(-Math.Sin(pheta), 0, Math.Cos(pheta)));
        //}

        //private Matrix3d getZRotationMatrix(double pheta)
        //{
        //    return new Matrix3d(new Vector3d(Math.Cos(pheta), -Math.Sin(pheta), 0),
        //                        new Vector3d(Math.Sin(pheta), Math.Cos(pheta), 0),
        //                        new Vector3d(0, 0, 1));
        //}
        #endregion
    }
}
