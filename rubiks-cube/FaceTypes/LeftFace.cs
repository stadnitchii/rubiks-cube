using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube.FacesTypes
{
    class LeftFace : CubeFace
    {
        Section left_temp;
        Section right_temp;
        Section above_temp;
        Section below_temp;

        public LeftFace(CubeState _parent)
            : base(_parent)
        {
            this.Color = 'L';
            this.Colors = new char[9];
            for (int i = 0; i < 9; i++)
                Colors[i] = Color;
        }

        public override void setNeightbors()
        {
            this.above = parent.up;
            this.left = parent.back;
            this.right = parent.front;
            this.below = parent.down;
        }

        public override void rotateClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getLeftSection();
            below_temp = below.getLeftSection();

            above.putIntoLeftSection(left_temp, false);
            right.putIntoLeftSection(above_temp, true);
            below.putIntoLeftSection(right_temp, true);
            left.putIntoRightSection(below_temp, false);

            rotateFaceClockwise();
        }

        public override void rotateCounterClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getLeftSection();
            below_temp = below.getLeftSection();

            above.putIntoLeftSection(right_temp, true);
            right.putIntoLeftSection(below_temp, true);
            below.putIntoLeftSection(left_temp, false);
            left.putIntoRightSection(above_temp, false);

            rotateFaceCounterclockwise();
        }
    }
}
