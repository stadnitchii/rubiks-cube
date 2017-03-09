using RubiksCube.FacesTypes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    /*       | u u u |
     *       | u u u |
     *       | u u u |
     *       
     * l l l | f f f | r r r | b b b |
     * l l l | f f f | r r r | b b b |
     * l l l | f f f | r r r | b b b |
     *       
     *       | b b b |
     *       | b b b |
     *       | b b b |
     */
    struct Section
    {
        public char x, y, z;

        public Section(char _x, char _y, char _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }
    }

    class CubeState
    {
        public CubeFace left;
        public CubeFace front;
        public CubeFace right;
        public CubeFace back;
        public CubeFace up;
        public CubeFace down;

        public bool PrintColors { get; set; }

        public CubeState()
        {
            left = new LeftFace(this);
            front = new FrontFace(this);
            right = new RightFace(this);
            back = new BackFace(this);
            up = new TopFace(this);
            down = new BottomFace(this);

            left.setNeightbors();
            front.setNeightbors();
            right.setNeightbors();
            back.setNeightbors();
            up.setNeightbors();
            down.setNeightbors();

            PrintColors = true;
        }

        public enum RotationType
        {
            clockwise,
            c_clockwise,
            one_eighty,
        }

        public void Move(string move)
        {
            //face that is facing us
            CubeFace face = getFace(move);
            RotationType type = getRotationType(move);

            if (type == RotationType.clockwise)
                face.rotateClockwise();
            else if (type == RotationType.c_clockwise)
                face.rotateCounterClockwise();
            else if(type == RotationType.one_eighty)
            {
                face.rotateClockwise();
                face.rotateClockwise();
            }
        }

        private void rotateClockwi() { }

        public CubeFace getFace(string move)
        {
            switch(move[0])
            {
                case 'L': return left;
                case 'F': return front;
                case 'R': return right;
                case 'B': return back;
                case 'U': return up;
                case 'D': return down;
                default: Console.WriteLine(string.Format("Face {0}, not recognized}", move[0])); return null;
            }
        }

        public RotationType getRotationType(string move)
        {
            if (move.Length == 1) return RotationType.clockwise;
            else if (move[1] == '\'') return RotationType.c_clockwise;
            else return RotationType.one_eighty;
        }


        #region Print and getState
        public void printState()
        {
            printEmptySection(); printSection(up.pGetTop()); newLine();
            printEmptySection(); printSection(up.pGetMid()); newLine();
            printEmptySection(); printSection(up.pGetBottom()); newLine();

            newLine();

            printSection(left.pGetTop()); printSection(front.pGetTop()); printSection(right.pGetTop()); printSection(back.pGetTop()); newLine();
            printSection(left.pGetMid()); printSection(front.pGetMid()); printSection(right.pGetMid()); printSection(back.pGetMid()); newLine();
            printSection(left.pGetBottom()); printSection(front.pGetBottom()); printSection(right.pGetBottom()); printSection(back.pGetBottom()); newLine();

            newLine();

            printEmptySection(); printSection(down.pGetTop()); newLine();
            printEmptySection(); printSection(down.pGetMid()); newLine();
            printEmptySection(); printSection(down.pGetBottom()); newLine();

            newLine();
        }

        private void printSection(Section chars)
        {
            if (PrintColors) chars = toColors(chars);
            Console.Write( string.Format("{0,-3}{1,-3}{2, -3}  ", chars.x, chars.y, chars.z));
        }

        private Section toColors(Section chars)
        {
            Section s = new Section();
            s.x = getColor(chars.x);
            s.y = getColor(chars.y);
            s.z = getColor(chars.z);
            return s;
        }

        private char getColor(char section)
        {
            switch(section)
            {
                case 'L': return 'G';
                case 'F': return 'O';
                case 'R': return 'B';
                case 'U': return 'Y';
                case 'D': return 'W';
                case 'B': return 'R';
                default : return '0';
            }
        }

        private void printEmptySection()
        {
            Console.Write(string.Format("  {0,-3}{1,-3}{2, -3}", " ", " ", " "));
        }

        private void newLine()
        {
            Console.WriteLine();
        }

        public string getStateString()
        {
            StringBuilder sb = new StringBuilder();
            //this order is very important
            up.getString(sb);
            right.getString(sb);
            front.getString(sb);
            down.getString(sb);
            left.getString(sb);
            back.getString(sb);
            return sb.ToString();
        }
        #endregion
    }
}
