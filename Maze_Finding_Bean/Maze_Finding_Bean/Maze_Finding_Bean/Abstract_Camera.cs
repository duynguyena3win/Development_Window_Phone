using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze_Finding_Bean
{
    public abstract class Abstract_Camera : InvisibleGameEntity
    {
        AbstractPathUpdater _PathUpdater;

        public AbstractPathUpdater PathUpdater
        {
            get { return _PathUpdater; }
            set { _PathUpdater = value; }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        protected Matrix _World;        // Mô tả sẵn, muốn phóng to vật thể lên - có thể tự biến đổi một cảnh, một vật (mẫu hình cho trước) với kích thước và hệ trục tọa độ của nó sang hệ tọa độ thế giới thực

        public Matrix World
        {
            get { return _World; }
            set { _World = value; }
        }
        protected  Matrix _View;         // Đổi tọa độ của thế giới thực sang góc nhìn camare.

        public Matrix View
        {
            get { return _View; }
            set { _View = value; }
        }
        protected Matrix _Projection;   // Tạo ra ảnh từ camera

        public Matrix Projection
        {
            get { return _Projection; }
            set { _Projection = value; }
        }

        public Matrix WVPMatrix
        {
            get
            {
                return World * View * Projection;
            }
        }
    }
}
