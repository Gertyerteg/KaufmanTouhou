using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KaufmanTouhou.Screens
{
    public class MenuScreen : Screen
    {
        private float timer;
        private SpriteFont font;
        private ScrollingStarBackground bg;
        private Texture2D titleScreen, title, logan;
        private Song backgroundSong;
        public MenuScreen()
        {
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            logan = Content.Load<Texture2D>("Logan");
            font = Content.Load<SpriteFont>("DialogueFont");
            bg = new ScrollingStarBackground(Content);
            title = Content.Load<Texture2D>("Title");
            titleScreen = Content.Load<Texture2D>("TitleScreen");
            backgroundSong = Content.Load<Song>("BeautifulDead");
            MediaPlayer.Play(backgroundSong);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            bg.Update(gameTime);

            //if (ScreenManager.DEV_MODE && Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    ScreenManager.GetInstance().ChangeScreen(ScreenState.READY);
            //}

            for (int i = 0; i < 4; i++)
            {
                if (InputManager.Instance.IsButtonPressed(Buttons.A, i))
                {
                    ScreenManager.GetInstance().ChangeScreen(ScreenState.GAME);
                    GameScreen gs = (GameScreen)ScreenManager.GetInstance().CurrentScreen;
                    gs.StageNumber = -1;
                    //gs.Players = players;
                    gs.Initialize();
                }
            }
        }

        /// <summary>
        /// Draws the menu to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphics"></param>
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            base.Draw(spriteBatch, graphics);
            graphics.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointWrap, null, null, null, null);
            spriteBatch.Draw(logan, new Rectangle(0, 0, ScreenManager.GetInstance().Width, ScreenManager.GetInstance().Height), Color.White * 0.05f);
            bg.Draw(spriteBatch);
            Point screenDim = new Point(ScreenManager.GetInstance().Width, ScreenManager.GetInstance().Height);
            spriteBatch.Draw(titleScreen, new Rectangle(screenDim.X / 2, screenDim.Y / 2, screenDim.X, screenDim.Y), null, Color.White, 0f,
                new Vector2(titleScreen.Width / 2f, titleScreen.Height / 2f), SpriteEffects.None, 0f);
            spriteBatch.Draw(title, new Vector2(screenDim.X - 340, 160), null, Color.White, -MathHelper.PiOver4 / 4, new Vector2(title.Width / 2f,
                title.Height / 2f), 2.5f + 0.2f * (float)Math.Sin(timer * 2), SpriteEffects.None, 0f);
            float opacity = (float)Math.Cos(timer * 4) * 0.6f + 0.65f;
            string text = "Press the A button to continue";
            Vector2 origin = font.MeasureString(text) / 2;

            spriteBatch.DrawString(font, text, new Vector2(ScreenManager.GetInstance().Width / 2, 
                ScreenManager.GetInstance().Height / 2 + 300), Color.White * opacity, 0f, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
