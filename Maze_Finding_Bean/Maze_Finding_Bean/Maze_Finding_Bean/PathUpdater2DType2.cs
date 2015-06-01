using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class PathUpdater2DType2 : PathUpdater2D
    {
        private float X0, Y0;
        private float A, W, Phi,dA;

        public PathUpdater2DType2(float x0, float y0, float a, float w, float phi, float da)
        {
            X0 = x0;
            Y0 = y0;
            A = a;
            W = w;
            Phi = phi;
            dA = da;
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
            X = X0;

            Y =(float)( Y0 + A * Math.Sin(W * t + Phi));
            if (dA != 0)
            {
                A -= dA;
                if (Math.Abs(A) < epsilon)
                {
                    dA = 0;
                    A = 0;
                }
            }
        }
    }
}
