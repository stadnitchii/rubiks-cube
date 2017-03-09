using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube.FacesTypes
{
    class RightFace : CubeFace
    {
        Section left_temp;
        Section right_temp;
        Section above_temp;
        Section below_temp;

        public RightFace(CubeState _parent)
            : base(_parent)
        {
            this.Color = 'R';
            this.Colors = new char[9];
            for (int i = 0; i < 9; i++)
                Colors[i] = Color;
        }

        public override void setNeightbors()
        {
            this.above = parent.up;
            this.left = parent.front;
            this.right = parent.back;
            this.below = parent.down;
        }

        //works clean
        public override void rotateClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getRightSection();
            below_temp = below.getRightSection();

            above.putIntoRightSection(left_temp, true);
            right.putIntoLeftSection(above_temp, false);
            below.putIntoRightSection(right_temp, false);
            left.putIntoRightSection(below_temp, true);

            rotateFaceClockwise();
        }

        public override void rotateCounterClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getRightSection();
            below_temp = below.getRightSection();

            above.putIntoRightSection(right_temp, false);
            right.putIntoLeftSection(below_temp, false);
            below.putIntoRightSection(left_temp, true);
            left.putIntoRightSection(above_temp, true);

            rotateFaceCounterclockwise();
        }
    }
}
