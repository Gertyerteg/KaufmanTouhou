using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KaufmanTouhou.Screens
{
    /// <summary>
    /// The players ready up in this screen. This serves as the initialization for the
    /// game screen.
    /// </summary>
    public class PlayerReadyScreen : Screen
    {
        private bool[] playerReady;
        private Texture2D blank, playerTexture;
        private ContentManager Content;
        private SpriteFont titleFont, subFont;
        private float timer;
        /// <summary>
        /// Creates a new instance of the <c>PlayerReadyScreen</c>.
        /// </summary>
        public PlayerReadyScreen()
        {
            playerReady = new bool[4];
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            blank = this.Content.Load<Texture2D>("Blank");
            playerTexture = this.Content.Load<Texture2D>("Player");
            titleFont = Content.Load<SpriteFont>("TransitionFont");
            subFont = Content.Load<SpriteFont>("DialogueFont");
        }

        /// <summary>
        /// Updates the logic and conditional checking for the <c>PlayerReadyScreen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputManager im = InputManager.Instance;
            ScreenManager s = ScreenManager.GetInstance();
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            for (int i = 0; i < 4; i++)
            {
                if (im.IsButtonDown(Buttons.A, i))
                {
                    playerReady[i] = true;
                }
                else if (im.IsButtonDown(Buttons.B, i))
                {
                    playerReady[i] = false;
                }
                // player starts the game
                else if (playerReady[i] && im.IsButtonPressed(Buttons.Start, i))
                {
                    int PlayerCount = GetPlayerCount();
                    List<SoundEffect> hurtEffects = new List<SoundEffect>();
                    hurtEffects.Add(Content.Load<SoundEffect>("Hit_Hurt"));
                    hurtEffects.Add(Content.Load<SoundEffect>("Hit_Hurt2"));
                    hurtEffects.Add(Content.Load<SoundEffect>("Hit_Hurt3"));
                    Player[] players = new Player[4];
                    Texture2D pointerTexture = Content.Load<Texture2D>("Pointer");

                    for (int j = 0; j < 4; j++)
                    {
                        if (playerReady[j])
                        {
                            players[j] = new Player((PlayerIndex)j, pointerTexture, blank, hurtEffects)
                            {
                                shootSound = Content.Load<SoundEffect>("Laser_Shoot"),
                                Position = new Vector2(s.Width / (PlayerCount + 1) * (j + 1), s.Height - 100),
                                Velocity = Vector2.Zero,
                                Texture = playerTexture,
                                BulletTexture = blank,
                                RocketTexture = Content.Load<Texture2D>("Missile"),
                                rocketImpact = Content.Load<SoundEffect>("Rocket_Impact"),
                                rocketLaunch = Content.Load<SoundEffect>("Rocket_Launch"),
                                Explosion = Content.Load<Texture2D>("Explosion"),
                            };
                        }
                    }
                    ScreenManager.GetInstance().ChangeScreen(ScreenState.GAME);
                    GameScreen gs = (GameScreen)ScreenManager.GetInstance().CurrentScreen;
                    gs.StageNumber = 0;
                    gs.Players = players;
                    gs.Initialize();
                }
            }
        }
        public override void Unload()
        {
        }

        private int GetPlayerCount()
        {
            int pl = 0;
            foreach (bool p in playerReady)
            {
                if (p)
                    pl++;
            }

            return pl;
        }

        /// <summary>
        /// Draws the gui to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphics"></param>
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            int border = 32;
            int length = (ScreenManager.GetInstance().Width - border * 5) / 4;
            graphics.Clear(Color.Black);
            base.Draw(spriteBatch, graphics);

            spriteBatch.Begin();

            for (int i = 0; i < 4; i++)
            {
                int x = border * (i + 1) + length * i + length / 2;
                Color c = (playerReady[i]) ? Color.Green : Color.Green * 0.2f;
                spriteBatch.Draw(blank, new Rectangle(x, ScreenManager.GetInstance().Height / 2, length, length),
                    null, c, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);

                if (playerReady[i])
                {

                    string playerText = "Player " + (i + 1);
                    Vector2 playerTextSize = subFont.MeasureString(playerText);
                    spriteBatch.DrawString(subFont, playerText, new Vector2(x, 
                        ScreenManager.GetInstance().Height / 2 + 130), Color.White, 0f, playerTextSize / 2, 1f, SpriteEffects.None, 0f);
                }

            }
            string text = "Players, Ready Up!";
            Vector2 textSize = titleFont.MeasureString(text);
            string text2 = "Press A to ready up";
            Vector2 text2Size = titleFont.MeasureString(text2);
            float rotation = (float)Math.Sin(timer / 250f) * 0.08f;
            float opacity = (float)Math.Sin(timer / 1000) * 0.2f + 0.3f;
            spriteBatch.DrawString(titleFont, text, new Vector2(ScreenManager.GetInstance().Width / 2, textSize.Y),
                Color.White, rotation, textSize / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(titleFont, text2, new Vector2(ScreenManager.GetInstance().Width / 2, 
                ScreenManager.GetInstance().Height - text2Size.Y), Color.White * opacity, 0f, text2Size / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
