using System;
using System.Collections.Generic;
using KaufmanTouhou.Screens.Stages;
using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KaufmanTouhou.Screens
{
    /// <summary>
    /// The <c>Screen</c> in which most of the gameplay will take place.
    /// </summary>
    public class GameScreen : Screen
    {
        private ScrollingStarBackground ssb;
        private ContentManager Content;
        private Texture2D blank, playerTexture;
        private SpriteFont font;
        public Player[] Players
        {
            get;
            set;
        }

        public int PlayerCount;
        private Color spaceColor;
        private bool isPlaying;

        public int StageNumber;

        public static Stage CurrentStage;

        /// <summary>
        /// Creates a new instance of the <c>GameScreen</c>.
        /// </summary>
        public GameScreen()
        {
            spaceColor = new Color(10, 10, 35);
            isPlaying = true;
        }

        /// <summary>
        /// Loads the content of the <c>GameScreen</c>.
        /// </summary>
        /// <param name="Content"></param>
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            ssb = new ScrollingStarBackground(this.Content);
            blank = this.Content.Load<Texture2D>("Blank");
            ScreenManager s = ScreenManager.GetInstance();
            playerTexture = this.Content.Load<Texture2D>("Player");
            font = this.Content.Load<SpriteFont>("TransitionFont");

            // comment out if you are initializing players through the player ready screen
            //InitializePlayers();
        }

        public void Initialize()
        {
            switch (StageNumber)
            {
                case -1:
                    ChangeStage(new Prestage(Content, Players));
                    break;
                case 0:
                    ChangeStage(new Stage0(Content, Players));
                    break;
                case 1:
                    ChangeStage(new Stage1(Content, Players));
                    break;
                case 2:
                    ChangeStage(new Stage2(Content, Players));
                    break;
            }
        }

        public void InitializePlayers()
        {
            ScreenManager s = ScreenManager.GetInstance();
            Texture2D pointerTexture = Content.Load<Texture2D>("Pointer");
            List<SoundEffect> hurtEffects = new List<SoundEffect>
            {
                Content.Load<SoundEffect>("Hit_Hurt"),
                Content.Load<SoundEffect>("Hit_Hurt2"),
                Content.Load<SoundEffect>("Hit_Hurt3")
            };

            for (int i = 0; i < PlayerCount; i++)
            {
                Players[i] = new Player((PlayerIndex)i, pointerTexture, blank, hurtEffects)
                {
                    shootSound = Content.Load<SoundEffect>("Laser_Shoot"),
                    Position = new Vector2(s.Width / (PlayerCount + 1) * (i + 1), s.Height - 100),
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

        /// <summary>
        /// Unloads the contentmanger for the game screen.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
            Content.Unload();
        }

        /// <summary>
        /// Updates the logic of the <c>GameScreen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ssb.Update(gameTime);

            if (isPlaying)
                CurrentStage.Update(gameTime);

            InputManager im = InputManager.Instance;

            for (int i = 0; i < 4; i++)
            {
                if (im.IsButtonPressed(Buttons.Start, i))
                {
                    CurrentStage.Pause();
                    isPlaying = !isPlaying;
                    break;
                }
            }
        }

        public void ChangeStage(Stage stage)
        {
            CurrentStage?.Unload();
            CurrentStage = stage;
            CurrentStage.Initialize();
        }

        /// <summary>
        /// Draws the contents of the <c>Screen</c> to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphics"></param>
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            base.Draw(spriteBatch, graphics);
            graphics.Clear(spaceColor);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointWrap, null, null, null, null);
            ssb.Draw(spriteBatch);

            CurrentStage.Draw(spriteBatch);

            if (!isPlaying)
            {
                string text = "PAUSED - PRESS START TO CONTINUE";
                Vector2 pos = new Vector2(ScreenManager.GetInstance().Width / 2, ScreenManager.GetInstance().Height / 2);
                pos -= font.MeasureString(text) / 2;
                spriteBatch.DrawString(font, text, pos, Color.White);
            }
            
            spriteBatch.End();
        }
    }

    /// <summary>
    /// A background with slowly scrolling stars.
    /// </summary>
    public class ScrollingStarBackground
    {
        private class Star : Sprite
        {
            public int size;

            public void Update(GameTime gameTime)
            {
                Position += Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, new Rectangle(Position.ToPoint(), new Point(size)), Color.White);
            }
        }

        private Star[] stars;
        private Random rand;
        private Texture2D texture;

        /// <summary>
        /// Creates a new instance of the <c>ScrollingStarBackground</c>.
        /// </summary>
        public ScrollingStarBackground(ContentManager content)
        {
            texture = content.Load<Texture2D>("Blank");
            rand = new Random();
            stars = new Star[70];

            for (int i = 0; i < stars.Length; i++)
            {
                int size = rand.Next(3, 9);
                Star s = new Star()
                {
                    Velocity = new Vector2(0, 1f / (size + 10)),
                    size = size,
                    Position = new Vector2(rand.Next(0, ScreenManager.GetInstance().Width), 
                        rand.Next(0, ScreenManager.GetInstance().Height)),
                    Texture = texture,
                };

                stars[i] = s;
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].Update(gameTime);

                if (stars[i].Position.Y > ScreenManager.GetInstance().Height)
                {
                    stars[i] = getRandomStar();
                    stars[i].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Star s in stars)
            {
                s.Draw(spriteBatch);
            }
        }

        private Star getRandomStar()
        {
            int size = rand.Next(2, 10);
            Star s = new Star()
            {
                Velocity = new Vector2(0, 1f / size),
                size = size,
                Position = new Vector2(rand.Next(0, ScreenManager.GetInstance().Width), -size),
                Texture = texture,
            };

            return s;
        }
    }

}
