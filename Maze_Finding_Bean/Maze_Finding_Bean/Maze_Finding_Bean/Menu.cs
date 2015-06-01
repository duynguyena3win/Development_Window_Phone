using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class Menu : VisibleGameEntity
    {
        public List<MenuItem> items;
        public Menu(float left, float top, List<Texture2D> background)
        {
            // TODO: Complete member initialization
            _Model = new My2DSprite(left, top, background);
            ((My2DSprite)_Model).Depth =0.999f;
            items = new List<MenuItem>();
            ((My2DSprite)_Model).Type = Object_Unit.Menu;
        }

        public void AddItem(float left, float top, List<Texture2D> itemBackground)
        {
            items.Add(new MenuItem(left,top, itemBackground));
        }

        public void AddItem(MenuItem item)
        {
            items.Add(item);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, object obj)
        {
            _Model.Draw(gameTime, obj);
            for (int i = 0; i < items.Count; i++)
                items[i].Draw(gameTime, obj);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _Model.Update(gameTime);
            for (int i = 0; i < items.Count; i++)
                items[i].Update(gameTime);
        }

        public override bool IsSelected(object obj)
        {
            for (int i = 0; i < items.Count; i++)
                if (items[i].IsSelected(obj))
                    return true;
            return false;
        }

        public virtual int ProcessEvent(object obj)
        {
            return 0;
        }

        public delegate int MenuEventHandler(object obj);
        public event MenuEventHandler eventHandler;
    }

}
