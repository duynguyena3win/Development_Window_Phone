using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Maze_Finding_Bean
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public enum Object_Unit
    {
        MenuItem,       // 0.6f
        Letter,     // 0.7f
        MrBean,     // 0.8f
        Cloud,      // 0.8f
        Menu,       // 0.9f
        
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 50;

        // Camera
        List<Abstract_Camera> cameraHelper;
        int CurrentCamera = 0;
        // Mouse
        MouseEventHelper mouseEventHelper;

        // Sound 
        SoundEffect soundEngine, sound_chosen;
        SoundEffectInstance soundEngineInstance, sound_chosenInstance;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        List<VisibleGameEntity> entity;
        int nEntities;

        float aspectRatio;
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            this.IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here


            soundEngine = Content.Load<SoundEffect>(@"Audio\back_sound");
            soundEngineInstance = soundEngine.CreateInstance();

            sound_chosen = Content.Load<SoundEffect>(@"Audio\Tock");
            sound_chosenInstance = soundEngine.CreateInstance();
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            entity = new List<VisibleGameEntity>();
            nEntities = 0;

            entity.Add(CreateBackground(new Vector2(0,0)));
            entity.Add(CreateACloud(new Vector2(100, 10), 1));
            entity.Add(CreateACloud(new Vector2(500, 140), 2));
            entity.Add(CreateALetter(@"Letter_Sprite\M\Letter-M", new Vector2(10, 10)));
            entity.Add(CreateALetter(@"Letter_Sprite\A\Letter-A", new Vector2(128, 108)));
            entity.Add(CreateALetter(@"Letter_Sprite\Z\Letter-Z", new Vector2(10, 216)));
            entity.Add(CreateALetter(@"Letter_Sprite\E\Letter-E", new Vector2(128, 324)));
            entity.Add(CreateMrBean(new Vector2(800, 300), 2));
            entity.Add(CreateAMenu(new Vector2(0, 0)));

            nEntities = entity.Count;

            ///// Camera:
            //cameraHelper = new List<Abstract_Camera>();
            //cameraHelper.Add(new Camera2D(0.5f, 1, 0.1f, 0, 0));
            //cameraHelper.Add(new Camera2D(1f, 1, 0.1f, 0, 0));
            //cameraHelper[1].PathUpdater = new IdlePathUpdater(1, 0);
            ////cameraHelper[2].PathUpdater = new LinearPathUpdater(1, 0);
            //CurrentCamera = 0;
        }

        private VisibleGameEntity CreateAMenu(Vector2 vector2)
        {
            List<Texture2D> list = new List<Texture2D>();
            list.Add(Content.Load<Texture2D>(@"Background\Background"));
            VisibleGameEntity temp = new Menu(vector2.X, vector2.Y, list);

            List<Texture2D> start = new List<Texture2D>();
            start.Add(Content.Load<Texture2D>(@"Background\Menu1"));
            MenuItem menu_play = new MenuItem(550, 200, start);
            menu_play.eventHandler += Game1_eventHandler;
            ((Menu)temp).AddItem(menu_play);
            

            List<Texture2D> score = new List<Texture2D>();
            score.Add(Content.Load<Texture2D>(@"Background\Menu2"));
            ((Menu)temp).AddItem(550, 280, score);

            List<Texture2D> setting = new List<Texture2D>();
            setting.Add(Content.Load<Texture2D>(@"Background\Menu3"));
            ((Menu)temp).AddItem(550, 360, setting);
            
            return temp;
        }

        int Game1_eventHandler(object obj)
        {
            sound_chosenInstance.Play();
            return 1;
        }

        private VisibleGameEntity CreateMrBean(Vector2 vector2, int ani)
        {
            VisibleGameEntity temp;
            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 0; i <= 18; i++)
                textures.Add(Content.Load<Texture2D>(@"MrBean\MrBean" + i.ToString()));
            temp = new Terrain(vector2.X, vector2.Y, textures);
            ((My2DSprite)temp.Model).Type = Object_Unit.MrBean;
            ((My2DSprite)temp.Model).Depth = 0.8f;
            ((My2DSprite)temp.Model).Animation = ani;
            return temp;
        }

        private VisibleGameEntity CreateACloud(Vector2 vector2, float ani)
        {
            VisibleGameEntity temp;
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>(@"Background\Cloud"));
            temp = new Terrain(vector2.X, vector2.Y, textures);
            ((My2DSprite)temp.Model).Type = Object_Unit.Cloud;
            ((My2DSprite)temp.Model).Depth = 0.8f;
            ((My2DSprite)temp.Model).Animation = ani;
            return temp;
        }

        int mn_eventHandler(object obj)
        {
            return 0;
        }

        private VisibleGameEntity CreateALetter(string uri_textureVector2, Vector2 vector2)
        {
            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 0; i < 14; i++)
                textures.Add(Content.Load<Texture2D>(uri_textureVector2 + i.ToString()));

            VisibleGameEntity temp = new Unit(vector2.X, vector2.Y, textures);
            ((My2DSprite)temp.Model).Depth = 0.7f;
            ((My2DSprite)temp.Model).Type = Object_Unit.Letter;
            return temp;
        }

        private VisibleGameEntity CreateBackground(Vector2 vector2)
        {
            VisibleGameEntity temp;
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>(@"Background\Background"));
            temp = new Terrain(vector2.X, vector2.Y, textures);
            ((My2DSprite)temp.Model).Type = Object_Unit.Letter;
            ((My2DSprite)temp.Model).Depth = 0.9f;
            return temp;
        }

        Random rand = new Random();

        private Vector2 GetRandomPosition()
        {
            return new Vector2(rand.Next() % 500, rand.Next() % 300);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        bool bDrag = false;
        bool bFreeFloat = false;
        float vxFreeFloat = 0, vyFreeFloat = 0;
        float axFreeFloat = -0, ayFreeFloat = -0;
        private float epsilonFreeFloat = 0;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            soundEngineInstance.Play();

            //mouseEventHelper.ProcessNewState(Mouse.GetState());
            //if (mouseEventHelper.HasLeftButtonDownEvent())
            //{
            //    bDrag = true;
            //    bFreeFloat = false;
            //}
            //else if (mouseEventHelper.HasLeftButtonUpEvent())
            //{
            //    Vector2 trans = mouseEventHelper.GetMouseDifference();
            //    ((Camera2D)cameraHelper[CurrentCamera]).xTran += trans.X;
            //    ((Camera2D)cameraHelper[CurrentCamera]).yTran += trans.Y;

            //    bDrag = false;
            //    bFreeFloat = true;
            //    {
            //        vxFreeFloat = trans.X;
            //        vyFreeFloat = trans.Y;
            //        axFreeFloat = -0.05f * vxFreeFloat;
            //        ayFreeFloat = -0.05f * vyFreeFloat;
            //    }
            //}
            //else if (mouseEventHelper.IsLeftButtonDown())
            //{
            //    if (bDrag)
            //    {
            //        Vector2 trans = mouseEventHelper.GetMouseDifference();
            //        ((Camera2D)cameraHelper[CurrentCamera]).xTran += trans.X;
            //        ((Camera2D)cameraHelper[CurrentCamera]).yTran += trans.Y;
            //    }
            //}
            //else
            //{
            //    if (bFreeFloat)
            //    {
            //        ((Camera2D)cameraHelper[CurrentCamera]).xTran += vxFreeFloat;
            //        ((Camera2D)cameraHelper[CurrentCamera]).yTran += vyFreeFloat;
            //        vxFreeFloat += axFreeFloat;
            //        vyFreeFloat += ayFreeFloat;
            //        if (vxFreeFloat * vxFreeFloat + vyFreeFloat * vxFreeFloat < epsilonFreeFloat)
            //        {
            //            bFreeFloat = false;
            //        }
            //    }
            //}

            #region Xyly Mouse
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Point idx = GetSelectedSpriteIndex(new Vector2(mouse.X, mouse.Y));
                if (idx.Y != -1)
                {
                    SelectSprite(idx);
                }
            }
            #endregion
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                for (int i = 0; i < nEntities; i++)
                    entity[i].Update(gameTime);
            }

            //cameraHelper[CurrentCamera].Update(gameTime);
            base.Update(gameTime);
        }

        private void SelectSprite(Point idx)
        {
            for (int i = 0; i < ((Menu)entity[idx.X]).items.Count; i++)
                ((Menu)entity[idx.X]).items[i].Select(i == idx.Y);
        }

        private Point GetSelectedSpriteIndex(Vector2 vector2)
        {
            Point temp = new Point(0,-1);
            for (int i = nEntities-1; i >= 0; i--)
            {
                if (entity[i].Model.Get_Type() == Object_Unit.Menu)
                {
                    temp.X = i;
                    for(int j = ((Menu)entity[i]).items.Count-1;j>=0;j--)
                        if (((Menu)entity[i]).items[j].IsSelected(vector2))
                        {
                            temp.Y = j;
                            return temp;
                        }
                }
            }
            return temp;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            for (int i = 0; i < nEntities; i++)
                entity[i].Draw(gameTime, spriteBatch);
            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
