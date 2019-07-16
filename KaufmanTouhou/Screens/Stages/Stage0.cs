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
    /// Stage zero is the tutorial stage for the player. It is meant to be a
    /// small stage that takes up roughly 3-4 minutes to introduce the player to
    /// the various buttons and mechanics.
    /// </summary>
    public class Stage0 : Stage
    {
        private SpriteFont font;
        private Texture2D blank; // Temporary
        private float spawnTimer, spawnTimer2, vortexSpawnTimer; // temporary
        private bool isOver;
        private Random rand;
        private bool spawnedBoss;
        private CorruptedSunBoss boss;
        private Song snailChan;

        /// <summary>
        /// Creates a new instance of <c>Stage0</c>.
        /// </summary>
        /// <param name="content"></param>
        public Stage0(ContentManager content, Player[] players) : base(content, players)
        {
            Dialogues = new List<Dialogue>();
            rand = new Random();
            isOver = false;
            spawnedBoss = false;
        }

        
        public override void Initialize()
        {
            base.Initialize();
            font = Content.Load<SpriteFont>("DialogueFont");

            blank = Content.Load<Texture2D>("Blank");
            snailChan = Content.Load<Song>("SnailchanAdventure");

            SetStage(0);

            InitializeDialogues();
        }

        /// <summary>
        /// Initializes the pre-scripted dialogues for the game.
        /// </summary>
        public void InitializeDialogues()
        {
            // dialogue 0
            Texture2D bunny = Content.Load<Texture2D>("RightBunny");
            Texture2D panda = Content.Load<Texture2D>("LeftPanda");

            Dialogue d0 = new Dialogue(panda, bunny, font);
            d0.AddText("Hello there!", 1000f, true);
            d0.AddText("Use the left stick to move your character around!", 3000f, false);
            d0.AddText("and press the right trigger to shoot!", 2500f, true);
            d0.AddText("Dodge the bullets and shoot the enemies!", 3000f, false);
            d0.AddText("Good luck!", 2500f, true);
            Dialogues.Add(d0);

            // dialogue 1
            Dialogue d1 = new Dialogue(panda, bunny, font);
            d1.AddText("We must rescue the Princess!", 3500f, true);
            d1.AddText("The Evil Queen went this way, hurry!", 2200f, false);
            Dialogues.Add(d1);

            // dialogue 2
            Dialogue d2 = new Dialogue(panda, bunny, font);
            d2.AddText("We've managed to push back the enemies, but\nreinforcements are on their way.", 3500f, true);
            d2.AddText("Seems we need some more defense...", 2200f, false);
            d2.AddText("Player! Press the A button to use your shields.", 2500f, false);
            d2.AddText("Be careful, your shields need to recharge after use.", 2200f, false);
            Dialogues.Add(d2);

            // dialogue 3
            Dialogue d3 = new Dialogue(panda, bunny, font);
            d3.AddText("More enemy reinforcements!", 2300f, true);
            d3.AddText("We need a more aggressive stance against them...", 2200f, false);
            d3.AddText("Player! Press hold the left trigger down to fire your rockets!", 2200f, false);
            d3.AddText("They fire less frequently, but do more damage and affect an area.", 4200f, false);
            Dialogues.Add(d3);

            // dialogue 4
            Dialogue d4 = new Dialogue(panda, bunny, font);
            d4.AddText("They've stopped coming... I have a bad feeling about this.", 3000f, true);
            d4.AddText("I agree...", 1000f, false);
            d4.AddText("Player, keep watch for anything suspicious!", 2000f, false);
            Dialogues.Add(d4);

            // dialogue 5
            Dialogue d5 = new Dialogue(panda, bunny, font);
            d5.AddText("Oh no! The Corrupted Sun is here, we're in trouble!", 3500f, true);
            d5.AddText("Take it down quick!", 2000f, false);
            Dialogues.Add(d5);

            // dialogue 6
            Dialogue d6 = new Dialogue(panda, null, font);
            d6.AddText("Player, he's almost destroyed, keep it up!", 2000f, true);
            Dialogues.Add(d6);

            // dialogue 7
            Dialogue d7 = new Dialogue(panda, bunny, font);
            d7.AddText("Somehow we did it...", 2000f, true);
            d7.AddText("Good work player, we've tracked the Evil Queen down.", 3000f, false);
            d7.AddText("Head to these coordinates...", 2500f, false);
            Dialogues.Add(d7);
        }

        /// <summary>
        /// Updates the logic of the stage 0.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
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
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            spawnTimer += dt;
            spawnTimer2 += dt;
            StageTimer += dt;
            vortexSpawnTimer += dt;

            switch (StageNumber)
            {
                case 0:
                    if (StageTimer > 5000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 1:
                    CurrentDialogue = Dialogues[0];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                        MediaPlayer.Play(snailChan);
                    }

                    break;
                case 2:
                    if (StageTimer > 5000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 3:
                    CurrentDialogue = Dialogues[1];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }

                    break;
                case 4: // 18 seconds
                    if (StageTimer < 14000f)
                    {
                        if (spawnTimer > 1500f)
                        {
                            spawnTimer = 0;
                            SpawnSinEnemy(new Vector2(-100, 70), true);
                        }

                        if (spawnTimer2 > 1500f)
                        {
                            spawnTimer2 = 0;
                            SpawnSinEnemy(new Vector2(-100, 500), true);
                        }
                    }

                    if (StageTimer > 18000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 5:
                    CurrentDialogue = Dialogues[2];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }

                    break;
                case 6:
                    if (spawnTimer > 1500f)
                    {
                        spawnTimer = 0;
                        SpawnSinEnemy(new Vector2(-100, 70), true);
                        if (rand.NextDouble() > 0.8f)
                        {
                            SpawnSinEnemy(new Vector2(ScreenManager.GetInstance().Width + 100, 100), false);
                        }
                    }

                    if (spawnTimer2 > 1500f)
                    {
                        spawnTimer2 = 0;
                        SpawnSinEnemy(new Vector2(-100, 500), true);
                    }

                    if (StageTimer > 18000f)
                    {
                        CurrentDialogue = Dialogues[3];
                        if (CurrentDialogue.FinishedPlaying)
                        {
                            SetStage(StageNumber + 1);
                        }
                    }
                    break;
                case 7:
                    if (StageTimer < 25000f)
                    {
                        if (spawnTimer > 1500f)
                        {
                            spawnTimer = 0;
                            SpawnSinEnemy(new Vector2(-100, 70), true);
                            if (rand.NextDouble() > 0.8f)
                            {
                                SpawnSinEnemy(new Vector2(ScreenManager.GetInstance().Width + 100, 100), false);
                            }
                        }

                        if (spawnTimer2 > 1500f)
                        {
                            spawnTimer2 = 0;
                            SpawnSinEnemy(new Vector2(-100, 500), true);
                        }

                        if (vortexSpawnTimer > 3000f)
                        {
                            SpawnVortexEnemy(new Vector2(-100, ScreenManager.GetInstance().Height / 2), new Vector2(300, 0), 8000f);
                            vortexSpawnTimer = 0;
                        }
                    }

                    if (StageTimer > 30000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 8:
                    CurrentDialogue = Dialogues[4];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 9:
                    if (StageTimer > 7000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 10:
                    CurrentDialogue = Dialogues[5];
                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 11:
                    if (StageTimer > 5000f)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 12:
                    // Boss fight
                    if (!spawnedBoss)
                    {
                        Texture2D bossTexture = Content.Load<Texture2D>("TestBoss");
                        boss = new CorruptedSunBoss(Players, 100 + GetPlayerCount() * 50)
                        {
                            Texture = bossTexture,
                            Size = new Point(bossTexture.Width * 5, bossTexture.Height * 5),
                            BulletTexture = blank,
                            Color = Color.White,
                            vortexEnemyTexture = Content.Load<Texture2D>("Vortex"),
                            Blank = blank,
                            sunBullet = Content.Load<Texture2D>("SunBullet"),
                        };
                        AddEnemy(boss);
                        spawnedBoss = true;
                    }
                    else
                    {
                        if ((float)boss.Health / boss.MAX_HEALTH < 0.5f)
                        {
                            CurrentDialogue = Dialogues[6];
                        }
                    }

                    if (Enemies.Count == 0)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 13:
                    CurrentDialogue = Dialogues[7];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        // end level
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
                            s.Stage = 0;
                            MediaPlayer.Stop();
                        }
                    }
                    break;
            }

            if (CurrentDialogue != null)
            {
                CurrentDialogue.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            float textHeight = tFont.MeasureString("H").Y;
            float opacity = 1f;
            switch (StageNumber)
            {
                case 0:
                    DrawSplash(spriteBatch, "Tutorial");
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
                    //    spriteBatch.DrawString(tFont, "Tutorial", new Vector2(300 + 700 * StageTimer / 5000f,
                    //        ScreenManager.GetInstance().Height / 2 - textHeight / 2), Color.White, 0f, new Vector2(0, 0),
                    //        1f, SpriteEffects.None, 0f);
                    //}
                    break;
                case 1:
                    break;
                case 2:
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
                case 11:
                    DrawSplash(spriteBatch, "Boss Fight: The Corrupted Sun");
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
                    //    spriteBatch.DrawString(tFont, "Boss Fight: The Corrupted Sun", new Vector2(300 + 700 * StageTimer / 5000f,
                    //        ScreenManager.GetInstance().Height / 2 - textHeight / 2), Color.White, 0f, new Vector2(0, 0),
                    //        1f, SpriteEffects.None, 0f);
                    //}
                    break;
            }

            CurrentDialogue?.Draw(spriteBatch);
        }
    }
}
