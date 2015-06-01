using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public class MouseEventHelper
    {
        private MouseState CurrentState, PrevState;

        public void ProcessNewState(MouseState mouseState)
        {
            PrevState = CurrentState;
            CurrentState = mouseState;
        }

        public bool HasLeftButtonDownEvent()
        {
            try
            {
                if (PrevState.LeftButton == ButtonState.Released && CurrentState.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public bool HasLeftButtonUpEvent()
        {
            try
            {
                if (PrevState.LeftButton == ButtonState.Pressed && CurrentState.LeftButton == ButtonState.Released)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public bool IsLeftButtonDown()
        {
            return CurrentState.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonUp()
        {
            return CurrentState.LeftButton == ButtonState.Released;
        }

        public Vector2 GetMouseDifference()
        {
            try
            {

                return new Vector2(CurrentState.X - PrevState.X, CurrentState.Y - PrevState.Y);
            }
            catch (Exception ex)
            {
            }
            return Vector2.Zero;
        }
    }
}
