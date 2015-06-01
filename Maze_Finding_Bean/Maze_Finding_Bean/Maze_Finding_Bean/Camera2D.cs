using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class Camera2D : Abstract_Camera
    {
        public float xScale, yScale, xTran, yTran, zRot;
        public Camera2D(float xscale, float yscale, float zrot, float xtran, float ytran)
        {
            xScale = xscale;
            yScale = yscale;
            zRot = zrot;
            xTran = xtran;
            yTran = ytran;
        }

        // Đối cảnh và camere chỉ cần đồi camera View và World;
        public override void Update(GameTime gameTime)
        {
            if (PathUpdater != null)
            {
                PathUpdater.Update(gameTime);
                Vector3 CurPos = PathUpdater.GetCurrentPosition();
                xTran = CurPos.X;
                yTran = CurPos.Y;
            }

            View = Matrix.CreateScale(xScale, yScale, 1)
                * Matrix.CreateRotationZ(zRot)
                * Matrix.CreateTranslation(xTran, yTran, 0);

        }
    }
}
