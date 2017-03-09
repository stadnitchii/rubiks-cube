using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube.FacesTypes
{
    class BackFace : CubeFace
    {
         Section left_temp;
        Section right_temp;
        Section above_temp;
        Section below_temp;

        public BackFace(CubeState _parent)
            : base(_parent)
        {
            this.Color = 'B';
            this.Colors = new char[9];
            for (int i = 0; i < 9; i++)
                Colors[i] = Color;
        }

        public override void setNeightbors()
        {
            this.above = parent.up;
            this.left = parent.right;
            this.right = parent.left;
            this.below = parent.down;
        }

        //works clean
        public override void rotateClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getTopSection();
            below_temp = below.getBottomSection();

            above.putIntoTopSection(left_temp, true);
            right.putIntoLeftSection(above_temp, false);
            below.putIntoBottomSection(right_temp, true);
            left.putIntoRightSection(below_temp, false);

            rotateFaceClockwise();
        }

        public override void rotateCounterClockwise()
        {
            left_temp = left.getRightSection();
            right_temp = right.getLeftSection();
            above_temp = above.getTopSection();
            below_temp = below.getBottomSection();

            above.putIntoTopSection(right_temp, false);
            right.putIntoLeftSection(below_temp, true);
            below.putIntoBottomSection(left_temp, false);
            left.putIntoRightSection(above_temp, true);

            rotateFaceCounterclockwise();
        }
    }
}
