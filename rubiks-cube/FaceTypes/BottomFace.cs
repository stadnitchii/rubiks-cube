using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube.FacesTypes
{
    class BottomFace : CubeFace
    {
        Section left_temp;
        Section right_temp;
        Section above_temp;
        Section below_temp;

        public BottomFace(CubeState _parent)
            : base(_parent)
        {
            this.Color = 'D';
            this.Colors = new char[9];
            for (int i = 0; i < 9; i++)
                Colors[i] = Color;
        }

        public override void setNeightbors()
        {
            this.above = parent.front;
            this.left = parent.left;
            this.right = parent.right;
            this.below = parent.back;
        }

        public override void rotateClockwise()
        {
            left_temp = left.getBottomSection();
            right_temp = right.getBottomSection();
            above_temp = above.getBottomSection();
            below_temp = below.getBottomSection();

            above.putIntoBottomSection(left_temp, true);
            right.putIntoBottomSection(above_temp, true);
            below.putIntoBottomSection(right_temp, true);
            left.putIntoBottomSection(below_temp, true);

            rotateFaceClockwise();
        }

        public override void rotateCounterClockwise()
        {
            left_temp = left.getBottomSection();
            right_temp = right.getBottomSection();
            above_temp = above.getBottomSection();
            below_temp = below.getBottomSection();

            above.putIntoBottomSection(right_temp, true);
            right.putIntoBottomSection(below_temp, true);
            below.putIntoBottomSection(left_temp, true);
            left.putIntoBottomSection(above_temp, true);

            rotateFaceCounterclockwise();
        }
    }
}
