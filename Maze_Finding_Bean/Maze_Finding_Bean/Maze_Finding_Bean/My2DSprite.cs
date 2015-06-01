using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class My2DSprite : MyAbstractModel
    {
        // private variable:
        float _left;
        Object_Unit _type;
        float _top;
        int _width;
        int _height;
        List<Texture2D> _textures;
        int _ntextures;
        float _animation;
        int _itexture;
        bool change_Animation = false;
        float Scale = 1;
        int FarmAnimation = 0;
        int _state = 0;
        private float _depth;

        // Matrix animation:
        Matrix matrix;
        float start_top = -62;
        float transY = 0;
        float A = 10;
        float W = 5;
        float t = 0, Phi = 0;
        float dt = 0.1f;
        //

        public float Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        public Object_Unit Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }
        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public List<Texture2D> Textures
        {
            get { return _textures; }
            set {
                if (value != null)
                {
                    _textures = value;
                    _ntextures = _textures.Count;
                    _itexture = 0;
                    _width = Textures[0].Width;
                    _height = Textures[0].Height;
                }
            }
        }
        public float Animation
        {
            get { return _animation; }
            set { _animation = value; }
        }
        public int nTextures
        {
            get { return _ntextures; }
            set { _ntextures = value; }
        }
        public int iTexture
        {
            get { return _itexture; }
            set { _itexture = value; }
        }
        public My2DSprite(float left, float top, List<Texture2D> textures)
        {
            Left = left;
            Top = top;
            Textures = textures;
        }
        
        public void InternalDraw(GameTime gameTime, SpriteBatch spritebatch)
        {
            if (State == 0)
            {
                switch (Type)
                {
                    case Object_Unit.Letter:
                        spritebatch.Draw(_textures[_itexture], new Vector2(_left, _top), null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, Depth);
                        break;
                    case Object_Unit.MenuItem:
                        spritebatch.Draw(_textures[_itexture], new Vector2(_left, start_top), null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, Depth);
                        break;
                    case Object_Unit.MrBean:
                        if (change_Animation == true)
                            spritebatch.Draw(_textures[_itexture], new Vector2(_left-50, _top-50), null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, Depth);
                        else
                            spritebatch.Draw(_textures[_itexture], new Vector2(_left, _top), null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, Depth);
                        break;
                    case Object_Unit.Menu:
                        break;
                    default:
                        spritebatch.Draw(_textures[_itexture], new Vector2(_left, _top), null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, Depth);
                        break;
                }

            }
            else
                spritebatch.Draw(_textures[_itexture], new Vector2(_left-8, _top-5), null, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, Depth);
        }
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
        public void Set_Size(int width, int height)
        {
            _width = width;
            _height = height;
        }


        public override bool IsSelected(object obj)
        {
            Vector2 pos = (Vector2) obj;
            if (pos.X >= _left && pos.X <= _left + _width &&
                pos.Y >= _top && pos.Y <= _top + _height)
                return true;
            return false;
        }
        public override void Update(GameTime gameTime)
        {
            switch (Type)
            {
                case Object_Unit.Letter:
                    Scale = 1;
                    _itexture = (_itexture + 1) % _ntextures;
                    break;
                case Object_Unit.Cloud:
                    _left += Animation;
                    if (_left >= 800)
                        _left = -200;
                    break;
                case Object_Unit.MrBean:
                    Update_MrBean();
                    break;
                case Object_Unit.MenuItem:
                    Update_MenuItem();
                    break;
                default:
                    _itexture = (_itexture + 1) % _ntextures;
                    break;
            }
        }
        public override void Draw(GameTime gameTime, object obj)
        {
            switch (Type)
            {
                case Object_Unit.MenuItem:
                    if (change_Animation == true)
                        ((SpriteBatch)obj).Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, matrix);
                    else
                        ((SpriteBatch)obj).Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    break;
                default:
                    ((SpriteBatch)obj).Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    break;
            }
            
            InternalDraw(gameTime, (SpriteBatch)obj);
            ((SpriteBatch)obj).End();
        }
        public override Object_Unit Get_Type()
        {
            return Type;
        }
        public override void Select(bool bSelected)
        {
            if (bSelected)
                State = 1;
            else
                State = 0;
        }

       
        private void Update_MrBean()
        {
            FarmAnimation++;
            if (FarmAnimation % Animation == 0)
            {
                if (_left <= 350)
                {
                    _left = 350;
                    Animation = 6;
                    if (change_Animation == false)
                    {
                        Scale = 2;
                        if (_itexture == 4)
                            change_Animation = true;
                        _itexture = (_itexture + 1) % 5;

                    }
                    else
                    {
                        Scale = 2.4f;
                        if (_itexture < 12 || _itexture >= 18)
                            _itexture = 11;
                        _itexture = (_itexture + 1) % 19;
                    }
                }
                else
                {
                    Scale = 2;
                    if (_itexture < 5 || _itexture >= 11)
                        _itexture = 4;
                    _left -= 3;
                    _itexture = (_itexture + 1) % 12;
                }
            }
        }
        private void Update_MenuItem()
        {
            Scale = 1;

            if (change_Animation == false)
            {

                if ((start_top+=3) >= _top)
                {
                    change_Animation = true;
                    start_top = _top;
                }
                else
                    start_top++;
            }
            else
            {
                //transX += dx;
                //transY += dy;

                t += dt;
                transY = (float)(A * Math.Sin(W * t + Phi));
                matrix = Matrix.CreateTranslation(0, transY, 0);

                if (Math.Abs(A) < 0.1f)
                    A = 0;
                else
                    A -= 0.1f;
            }
        }
    }
}
