using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube.FacesTypes
{
    class TopFace : CubeFace
    {
        Section left_temp;
        Section right_temp;
        Section above_temp;
        Section below_temp;

        public TopFace(CubeState _parent)
            : base(_parent)
        {
            this.Color = 'U';
            this.Colors = new char[9];
            for (int i = 0; i < 9; i++)
                Colors[i] = Color;
        }

        public override void setNeightbors()
        {
            this.above = parent.back;
            this.left = parent.left;
            this.right = parent.right;
            this.below = parent.front;
        }

        //
        public override void rotateClockwise()
        {
            left_temp = left.getTopSection();
            right_temp = right.getTopSection();
            above_temp = above.getTopSection();
            below_temp = below.getTopSection();

            above.putIntoTopSection(left_temp, true);
            right.putIntoTopSection(above_temp, true);
            below.putIntoTopSection(right_temp, true);
            left.putIntoTopSection(below_temp, true);

            rotateFaceClockwise();
        }

        public override void rotateCounterClockwise()
        {
            left_temp = left.getTopSection();
            right_temp = right.getTopSection();
            above_temp = above.getTopSection();
            below_temp = below.getTopSection();

            above.putIntoTopSection(right_temp, true);
            right.putIntoTopSection(below_temp, true);
            below.putIntoTopSection(left_temp, true);
            left.putIntoTopSection(above_temp, true);

            rotateFaceCounterclockwise();
        }
    }
}
