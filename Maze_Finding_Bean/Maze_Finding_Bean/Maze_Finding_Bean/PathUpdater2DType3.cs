using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class PathUpdater2DType3 : PathUpdater2D
    {
        private float X0, Y0;
        private float R, dt;

        public PathUpdater2DType3(float x0, float y0, float r, float dt)
        {
            X0 = x0;
            Y0 = y0;
            R = r;
            dt = dt;
        }
        public override Vector3 GetCurrentPosition()
        {
            return new Vector3(X, Y, 0);
        }

        private float t = 0;
        private float X, Y;
        private float epsilon;
        public override void Update(GameTime gameTime)
        {
            t++;    // to be refired later
            X = (float)(X0 + R * Math.Sin(t));

            Y = (float)(Y0 + R * Math.Cos(t));
        }
    }
}
