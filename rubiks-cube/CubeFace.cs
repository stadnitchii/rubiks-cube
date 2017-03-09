using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    class CubeFace
    {
        public char Color { get; protected set; }
        public char[] Colors { get; protected set; }
        
        protected CubeState parent { get; private set; }

        protected CubeFace left, right, above, below;

        //enum FaceSection
        //{
        //    left, top, right, bottom
        //}

        //enum Orientation
        //{
        //    bottom_to_top,
        //    top_to_bottom,
        //    left_to_right,
        //    right_to_left,
        //}

        public CubeFace(CubeState _parent)
        {
            this.parent = _parent;
        }

        public CubeFace(char _color)
        {
            this.Color = _color;
            this.Colors = new char[9];
            for (int i = 0; i < 9; i++)
                Colors[i] = _color;
        }

        public virtual void setNeightbors() { }

        public virtual void rotateClockwise() { }

        public virtual void rotateCounterClockwise() { }

        public void putIntoLeftSection(Section sec, bool topToBottom)
        {
            if(topToBottom)
            {
                Colors[0] = sec.x;
                Colors[3] = sec.y;
                Colors[6] = sec.z;
            } 
            else
            {
                Colors[6] = sec.x;
                Colors[3] = sec.y;
                Colors[0] = sec.z;
            }
        }
        public void putIntoRightSection(Section sec, bool topToBottom)
        {
            if (topToBottom)
            {
                Colors[2] = sec.x;
                Colors[5] = sec.y;
                Colors[8] = sec.z;
            }
            else
            {
                Colors[8] = sec.x;
                Colors[5] = sec.y;
                Colors[2] = sec.z;
            }
        }
        public void putIntoTopSection(Section sec, bool leftToRight)
        {
            if (leftToRight)
            {
                Colors[0] = sec.x;
                Colors[1] = sec.y;
                Colors[2] = sec.z;
            }
            else
            {
                Colors[2] = sec.x;
                Colors[1] = sec.y;
                Colors[0] = sec.z;
            }
        }
        public void putIntoBottomSection(Section sec, bool leftToRight)
        {
            if (leftToRight)
            {
                Colors[6] = sec.x;
                Colors[7] = sec.y;
                Colors[8] = sec.z;
            }
            else
            {
                Colors[8] = sec.x;
                Colors[7] = sec.y;
                Colors[6] = sec.z;
            }
        }

        public void rotateFaceClockwise()
        {
            var temp = createTempFace();

            Colors[0] = temp[6];
            Colors[1] = temp[3];
            Colors[2] = temp[0];

            Colors[3] = temp[7];
            //mid stays the same
            Colors[5] = temp[1];

            Colors[6] = temp[8];
            Colors[7] = temp[5];
            Colors[8] = temp[2];
        }

        public void rotateFaceCounterclockwise()
        {
            var temp = createTempFace();

            Colors[0] = temp[2];
            Colors[1] = temp[5];
            Colors[2] = temp[8];

            Colors[3] = temp[1];
            //mid stays the same
            Colors[5] = temp[7];

            Colors[6] = temp[0];
            Colors[7] = temp[3];
            Colors[8] = temp[6];
        }

        private char[] createTempFace()
        {
            char[] temp = new char[9];
            for (int i = 0; i < Colors.Length; i++)
                temp[i] = Colors[i];
            return temp;
        }

        public Section getBottomSection()
        {
            return new Section(Colors[6], Colors[7], Colors[8]);
        }

        public Section getRightSection()
        {
            return new Section(Colors[2], Colors[5], Colors[8]);
        }

        public Section getLeftSection()
        {
            return new Section(Colors[0], Colors[3], Colors[6]);
        }

        public Section getTopSection()
        {
            return new Section(Colors[0], Colors[1], Colors[2]);
        }

        public Section pGetTop()
        {
            return new Section(Colors[0], Colors[1], Colors[2]);
        }

        public Section pGetMid()
        {
            return new Section(Colors[3], Colors[4], Colors[5]);
        }

        public Section pGetBottom()
        {
            return new Section(Colors[6], Colors[7], Colors[8]);
        }

        public void getString(StringBuilder sb)
        {
            for (int i = 0; i < 9; i++)
                sb.Append(Colors[i]);
        }
    }
}
