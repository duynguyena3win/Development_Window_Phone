using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class IdlePathUpdater : PathUpdater2D
    {
        private float X0, Y0;
        public IdlePathUpdater(float x0, float y0)
        {
            X0 = x0;
            Y0 = y0;
        }
        public override Vector3 GetCurrentPosition()
        {
            return new Vector3(X0, Y0, 0);
        }
    }
}
