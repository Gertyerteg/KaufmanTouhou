using KaufmanTouhou.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou
{
    /// <summary>
    /// Creates a new instance of the <c>ScreenManager</c>.
    /// </summary>
    public class ScreenManager
    {
        #region Singleton
        private static ScreenManager inst;

        /// <summary>
        /// Returns the instance of the <c>ScreenManager</c>.
        /// </summary>
        /// <returns></returns>
        public static ScreenManager GetInstance()
        {
            if (inst == null)
                inst = new ScreenManager();
            return inst;
        }
        #endregion

        /// <summary>
        /// The current screen of the game.
        /// </summary>
        public Screen CurrentScreen { get; private set; }

        public int Width
        {
            get { return device.PreferredBackBufferWidth; }
        }

        public int Height
        {
            get { return device.PreferredBackBufferHeight; }
        }

        private GameWindow window;
        private ContentManager Content;
        private GraphicsDeviceManager device;

        /// <summary>
        /// Special param.
        /// </summary>
        public const bool DEV_MODE = false;

        /// <summary>
        /// Creates a new instance of the <c>ScreenManager</c>.
        /// </summary>
        public ScreenManager()
        {
        }

        /// <summary>
        /// Performs first time initialization on the <c>ScreenManager</c>.
        /// </summary>
        /// <param name="Content"></param>
        public void Initialize(ContentManager Content, 
            GraphicsDeviceManager device, GameWindow window)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            this.device = device;
            this.window = window;

            if (DEV_MODE)
                ChangeScreenSize(1762, 1013);
            else
            {
                ChangeScreenSize(1920, 1080);
                SetFullScreen(true);
            }
            ChangeScreen(ScreenState.MENU);
        }

        /// <summary>
        /// Changes the screen's width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ChangeScreenSize(int width, int height)
        {
            device.PreferredBackBufferWidth = width;
            device.PreferredBackBufferHeight = height;
            device.ApplyChanges();
        }

        /// <summary>
        /// Sets the game to borderless or not. If it is to be borderless, the game window is
        /// set to (0, 0) on the screen.
        /// </summary>
        /// <param name="fs"></param>
        public void SetFullScreen(bool fs)
        {
           window.IsBorderless = fs;

            if (fs)
            {
                window.Position = new Point(0, 0);
            }
        }

        /// <summary>
        /// Changes the <c>Screen</c> to the specified one.
        /// </summary>
        public void ChangeScreen(ScreenState state)
        {
            Unload();

            switch (state)
            {
                case ScreenState.GAME:
                    CurrentScreen = new GameScreen();
                    break;
                case ScreenState.MENU:
                    CurrentScreen = new MenuScreen();
                    break;
                case ScreenState.READY:
                    CurrentScreen = new PlayerReadyScreen();
                    break;
                case ScreenState.AFTER:
                    CurrentScreen = new AfterScreen();
                    break;
            }

            CurrentScreen.LoadContent(Content);
        }

        /// <summary>
        /// Unloads the current instance of the <c>Screen</c>.
        /// </summary>
        public void Unload()
        {
            CurrentScreen?.Unload();
        }

        /// <summary>
        /// Updates the logic and conditional checking for the current instance of the <c>Screen</c>.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            CurrentScreen?.Update(gameTime);
        }

        /// <summary>
        /// Draws the instance of the <c>Screen</c> to the screen.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            CurrentScreen?.Draw(spriteBatch, graphics);
        }

    }
}
