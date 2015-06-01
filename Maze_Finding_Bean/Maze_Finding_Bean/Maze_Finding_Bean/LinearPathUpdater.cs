using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class LinearPathUpdater : PathUpdater2D
    {
        private float X0, Y0;
        private float Vx, Vy;
        public LinearPathUpdater(float x0, float y0, float vx, float vy)
        {
            X0 = x0;
            Y0 = y0;
            Vx = vx;
            Vy = vy;
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
            X = X0 + t * Vx;
            Y = Y0 + t * Vx;
        }
    }
}
