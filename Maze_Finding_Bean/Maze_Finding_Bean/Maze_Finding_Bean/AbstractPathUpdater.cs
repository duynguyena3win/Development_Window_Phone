using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public abstract class AbstractPathUpdater
    {
        public virtual Vector3 GetCurrentPosition()
        {
            return Vector3.Zero;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
