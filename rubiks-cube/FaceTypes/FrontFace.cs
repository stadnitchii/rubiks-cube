using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube.FacesTypes
{
    class FrontFace : CubeFace
    {
        Section left_temp;
        Section right_temp;
        Section above_temp;
        Section below_temp;

        public FrontFace(CubeState _parent)
            : base(_parent)
        {
            this.Color = 'F';
            this.Colors = new char[9];
            for (int i = 0; i < 9; i++)
                Colors[i] = Color;
        }

        public override void setNeightbors()
        {
            this.above = parent.up;
            this.left = parent.left;
            this.right = parent.right;
            this.below = parent.down;
        }

        public override void rotateClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getBottomSection();
            below_temp = below.getTopSection();

            above.putIntoBottomSection(left_temp, false);
            right.putIntoLeftSection(above_temp, true);
            below.putIntoTopSection(right_temp, false);
            left.putIntoRightSection(below_temp, true);

            rotateFaceClockwise();
        }

        public override void rotateCounterClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getBottomSection();
            below_temp = below.getTopSection();

            above.putIntoBottomSection(right_temp, true);
            right.putIntoLeftSection(below_temp, false);
            below.putIntoTopSection(left_temp, true);
            left.putIntoRightSection(above_temp, false);

            rotateFaceCounterclockwise();
        }
    }
}
