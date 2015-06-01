using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class MenuItem : VisibleGameEntity
    {
        public MenuItem(float left, float top, List<Texture2D> background)
        {
            // TODO: Complete member initialization
            _Model = new My2DSprite(left, top, background);
            ((My2DSprite)_Model).Depth = 0.5f;
            ((My2DSprite)_Model).Type = Object_Unit.MenuItem;
        }
        public int ProcessEvent(object obj)
        {
            return 0;
        }

        public delegate int MenuEventHandler(object obj);
        public event MenuEventHandler eventHandler;
    }
}
