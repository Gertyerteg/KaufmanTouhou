using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace KaufmanTouhou.Screens.Stages
{
    /// <summary>
    /// The first level of the game.
    /// </summary>
    public class Stage1 : Stage
    {
        private float spawnTimer, vortexSpawnTimer;
        private bool isOver;
        private Random rand;

        private Song cosmoFunk;
        private Texture2D blank;
        private Texture2D[] asteroidTextures;
        private AsteroidBoss aBoss;
        private SpinnerBoss sBoss;

        /// <summary>
        /// Creates a new instance of Stage1.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="players"></param>
        public Stage1(ContentManager content, Player[] players) : base(content, players)
        {
            rand = new Random();
            SetStage(0);
            asteroidTextures = new Texture2D[5];
            for (int i = 0; i < 5; i++)
            {
                asteroidTextures[i] = content.Load<Texture2D>("Asteroid" + i);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            cosmoFunk = Content.Load<Song>("CosmoFunk");
            InitializeDialogue();
            blank = Content.Load<Texture2D>("Blank");
        }

        public void InitializeDialogue()
        {
            Dialogues = new List<Dialogue>();
            Texture2D bunny = Content.Load<Texture2D>("RightBunny");
            Texture2D panda = Content.Load<Texture2D>("LeftPanda");
            SpriteFont dFont = Content.Load<SpriteFont>("DialogueFont");

            // dialogue 0
            Dialogue d0 = new Dialogue(panda, bunny, dFont);
            d0.AddText("Player, we've tracked the Evil Queen to this Silent Field.", 3000f, true);
            d0.AddText("We must be getting close to her!", 2000, true);
            d0.AddText("", 2000f, true);
            d0.AddText("It's quiet... Too quiet...", 2000f, false);
            Dialogues.Add(d0);

            // dialogue 1
            Dialogue d1 = new Dialogue(panda, null, dFont);
            d1.AddText("Player, the robots have active camoflauge!", 1500f, true);
            d1.AddText("Track their movements by where their bullets are coming from!", 4000f, true);
            Dialogues.Add(d1);

            // dialogue 2
            Dialogue d2 = new Dialogue(panda, null, dFont);
            d2.AddText("There's too many of them!", 1500f, true);
            d2.AddText("Player, move into that asteroid field.", 2200f, true);
            Dialogues.Add(d2);

            // dialogue 3
            Dialogue d3 = new Dialogue(panda, bunny, dFont);
            d3.AddText("We've exited the asteroid field, but where's the Evil Queen?", 3000f, true);
            d3.AddText("Our radar is showing that the Asteroid-Bot is here!", 3000f, false);
            d3.AddText("The Asteroid-Bot? Quickly Player, take it down!", 2000f, true);
            Dialogues.Add(d3);

            // dialogue 4
            Dialogue d4 = new Dialogue(panda, bunny, dFont);
            d4.AddText("That was almost too easy. Interesting...", 3000f, true);
            d4.AddText("", 1000f, true);
            Dialogues.Add(d4);

            // dialogue 5 (ADD SPINNER TEXTURE)
            Dialogue d5 = new Dialogue(Content.Load<Texture2D>("Test"), null, dFont);
            d5.AddText("You fools!", 1000f, true);
            d5.AddText("It was me all this time, the Evil Spinner!", 3000f, true);
            Dialogues.Add(d5);

            // dialogue 6
            Dialogue d6 = new Dialogue(panda, bunny, dFont);
            d6.AddText("Watch out! We have another henchmen approaching on our radar!", 3000f, false);
            d6.AddText("It's the Evil Spinner! Dodge those attacks!", 2000f, true);
            Dialogues.Add(d6);

            // dialogue 7
            Dialogue d7 = new Dialogue(panda, bunny, dFont);
            d7.AddText("Excellant work, Player.", 2500f, false);
            d7.AddText("We've finally tracked down the Evil Queen.", 3000f, true);
            d7.AddText("Very good. Player, let's get the Princess back!", 3000f, false);
            Dialogues.Add(d7);

        }

        /// <summary>
        /// Updates the stage.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            StageTimer += dt;
            spawnTimer += dt;
            vortexSpawnTimer += dt;
            CurrentDialogue?.Update(gameTime);

            if (!isOver)
                base.Update(gameTime);
            else
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    if (Players[i] != null)
                    {
                        Players[i].Position += new Vector2(0, -5);
                    }
                }
            }

            switch (StageNumber)
            {
                case 1:
                    if (StageTimer >= 5000f)
                    {
                        SetStage(StageNumber + 1);
                        MediaPlayer.Play(cosmoFunk);
                    }
                    break;
                case 0:
                    CurrentDialogue = Dialogues[0];

                    if (CurrentDialogue.FinishedPlaying)
                        SetStage(StageNumber + 1);
                    break;
                case 2:
                    CurrentDialogue = Dialogues[1];

                    if (CurrentDialogue.FinishedPlaying)
                        SetStage(StageNumber + 1);
                    break;
                case 3:
                    // spawn robots with invis
                    if (spawnTimer > 700f)
                    {
                        spawnTimer = 0;
                        if (rand.NextDouble() > .50 || StageTimer < 7000f)
                        {
                            SpawnInvisSinEnemy(new Vector2(-100, rand.Next(100, 500)), true);
                        }
                        else if (StageTimer > 7000f)
                        {
                            SpawnInvisSinEnemy(new Vector2(ScreenManager.GetInstance().Width + 100, rand.Next(100, 500)), false);
                        }

                        if (vortexSpawnTimer > 9000 && StageTimer > 18000f)
                        {
                            vortexSpawnTimer = 0;
                            SpawnVortexEnemy(new Vector2(-100, -100), new Vector2(200, 200), 7000f);
                            SpawnVortexEnemy(new Vector2(ScreenManager.GetInstance().Width + 100, -100), new Vector2(-200, 200), 7000f);
                        }

                        if (StageTimer > 36000f)
                        {
                            SetStage(StageNumber + 1);
                        }
                    }

                    break;
                case 4:
                    CurrentDialogue = Dialogues[2];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 5:
                    if (StageTimer >= 5000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 6:
                    // summon asteroid hell upon the players
                    if (spawnTimer > 70 && StageTimer < 30000)
                    {
                        spawnTimer = 0;
                        Vector2 pos = new Vector2(rand.Next(-100, ScreenManager.GetInstance().Width + 100), -90);
                        float scale = (float)(rand.NextDouble() * (Sprite.SCALE - 3) + 3f);
                        float speed = 1000 / (float)Math.Sqrt(scale);
                        Texture2D aster = asteroidTextures[rand.Next(0, 5)];
                        float xSpeed = (float)((rand.NextDouble() - 0.5f) * 100f);
                        Asteroid a = new Asteroid(Bullet.EntitySide.ENEMY, 7000f, new Vector2(xSpeed, speed))
                        {
                            Texture = aster,
                            Position = pos,
                            Size = new Point((int)(aster.Width * scale), (int)(aster.Height * scale)),
                            Health = (int)(Math.Ceiling(scale / 2)),
                            Color = Color.White,
                            InitVelocity = new Vector2(xSpeed, speed),
                        };
                        Bullets.Add(a);
                    }

                    if (StageTimer > 33000)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 7:
                    CurrentDialogue = Dialogues[3];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        Texture2D asteroidBoss = Content.Load<Texture2D>("AsteroidBoss");
                        aBoss = new AsteroidBoss(Players, 70 + GetPlayerCount() * 30)
                        {
                            Texture = asteroidBoss,
                            Position = new Vector2(ScreenManager.GetInstance().Width / 2, -300),
                            Size = new Point(asteroidBoss.Width * Sprite.SCALE, asteroidBoss.Height * Sprite.SCALE),
                            BulletTexture = asteroidTextures[3],
                        };

                        AddEnemy(aBoss);
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 8:
                    if (StageTimer >= 5000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 9:
                    if (aBoss != null && !aBoss.IsActive)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 10:
                    CurrentDialogue = Dialogues[4];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 11:
                    CurrentDialogue = Dialogues[5];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    CurrentDialogue = Dialogues[5];
                    break;
                case 12:
                    CurrentDialogue = Dialogues[6];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 13:
                    if (StageTimer >= 5000f)
                    {
                        SetStage(StageNumber + 1);
                    }

                    break;
                case 14:
                    sBoss = new SpinnerBoss(Players, 100 + GetPlayerCount() * 50)
                    {
                        Position = new Vector2(100, 100),
                    };
                    AddEnemy(sBoss);
                    SetStage(StageNumber + 1);
                    break;
                case 15:
                    // SPAWN SPINNER BOSS
                    if (Enemies.Count == 0)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 16:
                    CurrentDialogue = Dialogues[7];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        isOver = true;
                        bool isReallyOver = false;
                        for (int i = 0; i < Players.Length; i++)
                        {
                            if (Players[i] != null && Players[i].Position.Y > 0)
                            {
                                isReallyOver = false;
                                break;
                            }
                            else
                                isReallyOver = true;
                        }

                        // now end level
                        if (isReallyOver)
                        {
                            ScreenManager.GetInstance().ChangeScreen(ScreenState.AFTER);
                            AfterScreen s = (AfterScreen)ScreenManager.GetInstance().CurrentScreen;
                            s.Players = Players;
                            s.Stage = 1;
                            MediaPlayer.Stop();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws the contents of the stage to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            float opacity = 1f;
            float textHeight = tFont.MeasureString("H").Y;
            switch (StageNumber)
            {
                case 1:
                    DrawSplash(spriteBatch, "Game Start!");
                    //if (StageTimer < 600f)
                    //{
                    //    opacity = StageTimer / 600f;
                    //}
                    //else if (StageTimer > 4400f)
                    //{
                    //    opacity = (5000f - StageTimer) / 600f;
                    //}

                    //spriteBatch.Draw(blank, new Rectangle(ScreenManager.GetInstance().Width / 2, ScreenManager.GetInstance().Height / 2, ScreenManager.GetInstance().Width, 400)
                    //    , null, Color.Green * opacity * 0.8f, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                    //if (opacity == 1)
                    //{
                    //    spriteBatch.DrawString(tFont, "Game Start!", new Vector2(300 + 700 * StageTimer / 5000f,
                    //        ScreenManager.GetInstance().Height / 2 - textHeight / 2), Color.White, 0f, new Vector2(0, 0),
                    //        1f, SpriteEffects.None, 0f);
                    //}
                    break;
                case 5:

                    DrawSplash(spriteBatch, "Avoid the Asteroids!");
                    //if (StageTimer < 600f)
                    //{
                    //    opacity = StageTimer / 600f;
                    //}
                    //else if (StageTimer > 4400f)
                    //{
                    //    opacity = (5000f - StageTimer) / 600f;
                    //}

                    //spriteBatch.Draw(blank, new Rectangle(ScreenManager.GetInstance().Width / 2, ScreenManager.GetInstance().Height / 2, ScreenManager.GetInstance().Width, 400)
                    //    , null, Color.Green * opacity * 0.8f, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                    //if (opacity == 1)
                    //{
                    //    spriteBatch.DrawString(tFont, "Avoid the Asteroids!", new Vector2(300 + 700 * StageTimer / 5000f,
                    //        ScreenManager.GetInstance().Height / 2 - textHeight / 2), Color.White, 0f, new Vector2(0, 0),
                    //        1f, SpriteEffects.None, 0f);
                    //}
                    break;
                case 8:
                    DrawSplash(spriteBatch, "Boss Fight: AsteroidBot");
                    break;
                case 13:
                    DrawSplash(spriteBatch, "Boss Fight: The Evil Spinner");
                    break;

            }
            CurrentDialogue?.Draw(spriteBatch);
        }
    }
}
