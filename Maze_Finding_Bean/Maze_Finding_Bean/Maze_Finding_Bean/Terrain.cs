using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class Terrain : VisibleGameEntity
    {
        public Terrain(float left, float top, List<Texture2D> textures)
        {
            // TODO: Complete member initialization
            _Model = new My2DSprite(left, top, textures);
            ((My2DSprite)_Model).Depth = 1;
        }
    }
}
