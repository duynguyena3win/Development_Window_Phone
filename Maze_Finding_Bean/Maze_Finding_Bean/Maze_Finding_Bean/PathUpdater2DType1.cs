using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class PathUpdater2DType1 : PathUpdater2D
    {
        private float X0, Y0;
        private float Vx, Vy;
        private float Ax, Ay;
        public PathUpdater2DType1(float x0, float y0, float vx, float vy, float ax, float ay)
        {
            X0 = x0;
            Y0 = y0;
            Vx = vx;
            Vy = vy;
            Ax = ax;
            Ay = ay;
        }
        public override Vector3 GetCurrentPosition()
        {
            return new Vector3(X, Y, 0);
        }

        private float t = 0;
        private float X, Y;

        public override void Update(GameTime gameTime)
        {
            t++;    // to be refired later
            X = (float)(X0 + t * Vx + 0.5 * Ax * t * t);
            Y = (float)(Y0 + t * Vx + 0.5 * Ay * t * t);
        }
    }
}
