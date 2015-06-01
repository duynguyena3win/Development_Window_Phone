using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class MenuNewGame : Menu
    {
        public MenuNewGame(float left, float top, List<Texture2D> background) : base(left,top,background)
        {
        }
        public override int ProcessEvent(object obj)
        {
            return 1;
        }
    }
}
