using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou.Screens
{
    public class AfterScreen : Screen
    {
        public Player[] Players;
        public int Stage;
        private Texture2D blank;
        private SpriteFont font, tFont;
        private float timer;

        private string stageText
        {
            get { return "Stage " + Stage + " Completed!"; }
        }

        public override void LoadContent(ContentManager Content)
        {
            tFont = Content.Load<SpriteFont>("TransitionFont");
            font = Content.Load<SpriteFont>("DialogueFont");
            blank = Content.Load<Texture2D>("Blank");
            base.LoadContent(Content);
        }

        /// <summary>
        /// Updates the logic and conditional checking for the <c>AfterScreen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < 4; i++)
            {
                if (InputManager.Instance.IsButtonPressed(Buttons.A, i))
                {

                    for (int j = 0; j < 4; j++)
                    {
                        if (Players[j] != null)
                        {
                            Players[j].Health = 0;
                            Players[j].Position = new Vector2(Players[j].Position.X, ScreenManager.GetInstance().Height - 100);
                        }
                    }

                    ScreenManager.GetInstance().ChangeScreen(ScreenState.GAME);
                    GameScreen sc = (GameScreen)ScreenManager.GetInstance().CurrentScreen;
                    sc.StageNumber = Stage + 1;
                    sc.Players = Players;
                    sc.Initialize();
                }
            }
        }

        /// <summary>
        /// Draws the after screen to the game window.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphics"></param>
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            base.Draw(spriteBatch, graphics);
            int border = 32;
            int length = (ScreenManager.GetInstance().Width - border * 5) / 4;
            graphics.Clear(Color.Black);

            spriteBatch.Begin();

            for (int i = 0; i < 4; i++)
            {
                int x = border * (i + 1) + length * i + length / 2;
                Color c = (Players[i] != null) ? Color.Green : Color.Green * 0.2f;
                spriteBatch.Draw(blank, new Rectangle(x, ScreenManager.GetInstance().Height / 2, length, length),
                    null, c, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);

                if (Players[i] != null)
                {
                    string text = "You were hit " + -Players[i].Health + " times";
                    Vector2 textSize = font.MeasureString(text);
                    spriteBatch.DrawString(font, text, new Vector2(x, 
                        ScreenManager.GetInstance().Height / 2 + 100), Color.White, 
                        0f, textSize / 2, 1f, SpriteEffects.None, 0f);
                }
            }
            Vector2 stageTextSize = tFont.MeasureString(stageText);
            spriteBatch.DrawString(tFont, stageText, new Vector2(ScreenManager.GetInstance().Width / 2f,
                120), Color.White, (float)Math.Sin(timer * MathHelper.Pi * 3 / 4)
                    * MathHelper.PiOver4 / 5f, stageTextSize / 2f, 1f + (float)Math.Sin(timer * MathHelper.TwoPi) * 0.02f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }
}
