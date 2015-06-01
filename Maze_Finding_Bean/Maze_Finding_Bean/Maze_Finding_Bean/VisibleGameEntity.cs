using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public abstract class VisibleGameEntity
    {
        protected MyAbstractModel _Model;

        public MyAbstractModel Model
        {
            get { return _Model; }
            set { _Model = value; }
        }

        public virtual void Update(GameTime gameTime)
        {
            _Model.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, object obj)
        {
            _Model.Draw(gameTime, obj);
        }

        public virtual bool IsSelected(object obj)
        {
            return _Model.IsSelected(obj);
        }

        public virtual void Select(bool bSelected)
        {
            _Model.Select(bSelected);
        }
    }
}
