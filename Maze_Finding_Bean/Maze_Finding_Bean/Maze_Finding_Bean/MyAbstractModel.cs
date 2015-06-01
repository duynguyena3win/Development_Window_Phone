using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public abstract class MyAbstractModel
    {
        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, object obj)
        {
        }

        public virtual bool IsSelected(object obj)
        {
            return false;
        }

        public virtual void Select(bool bSelected)
        {

        }

        public virtual Object_Unit Get_Type()
        {
            return Object_Unit.Menu;
        }
    }
}
