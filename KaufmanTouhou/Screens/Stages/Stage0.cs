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
    /// small stage that takes up roughly 1-2 minutes to introduce the player to
    /// the various buttons and mechanics.
    /// </summary>
    public class Stage0 : Stage
    {
        private SpriteFont font, tFont;
        private Texture2D sinEnemyTexture, blank; // Temporary
        private float spawnTimer; // temporary
        private Dialogue currentDialogue;
        private List<Dialogue> dialogues;
        private bool isOver;
        private int stage; // maybe temporary
        private float stageTimer; // maybe temporary
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
            dialogues = new List<Dialogue>();
            rand = new Random();
            isOver = false;
            spawnedBoss = false;
        }

        public void SetStage(int stage)
        {
            this.stage = stage;
            stageTimer = 0;
        }


        public override void Initialize()
        {
            base.Initialize();
            tFont = Content.Load<SpriteFont>("TransitionFont");
            font = Content.Load<SpriteFont>("DialogueFont");

            foreach(Player p in Players)
            {
                p?.SetBullets(Bullets);
            }
            sinEnemyTexture = Content.Load<Texture2D>("Enemy");
            blank = Content.Load<Texture2D>("Blank");
            snailChan = Content.Load<Song>("SnailchanAdventure");

            SetStage(1);

            InitializeDialogues();
        }

        /// <summary>
        /// Spawns a sinpathenemy
        /// </summary>
        public void SpawnSinEnemy(Vector2 initialPosition, bool goingRight)
        {
            SinPathEnemy e = new SinPathEnemy(Players, goingRight)
            {
                Position = initialPosition,
                BulletTexture = blank,
                Texture = sinEnemyTexture,
                Color = Color.White,
                Size = new Point(sinEnemyTexture.Width * Sprite.SCALE, sinEnemyTexture.Height * Sprite.SCALE),
            };
            e.SetBullets(Bullets);
            AddEnemy(e);
        }

        /// <summary>
        /// Initializes the pre-scripted dialogues for the game.
        /// </summary>
        public void InitializeDialogues()
        {
            // dialogue 0
            Texture2D te = Content.Load<Texture2D>("Test");

            Dialogue d0 = new Dialogue(te, te, font);
            d0.AddText("Hello there!", 1000f, true);
            d0.AddText("Use the left stick to move your character around!", 3000f, false);
            d0.AddText("and press the right trigger to shoot!", 2500f, true);
            d0.AddText("Dodge the bullets and shoot the enemies!", 3000f, false);
            d0.AddText("Good luck!", 2500f, true);
            dialogues.Add(d0);

            // dialogue 1
            Dialogue d1 = new Dialogue(te, te, font);
            d1.AddText("Sir, there is an enemy force converging on \nour position.", 3500f, true);
            d1.AddText("Battle stations, we must push through!", 2200f, false);
            dialogues.Add(d1);

            // dialogue 2
            Dialogue d2 = new Dialogue(te, te, font);
            d2.AddText("Sir, we've managed to push back the enemies, but\nreinforcements are on their way.", 3500f, true);
            d2.AddText("Seems we need some more defense...", 2200f, false);
            d2.AddText("Player! Press the A button to use your shields.", 2500f, false);
            d2.AddText("Be careful, your shields need to recharge after use.", 2200f, false);
            dialogues.Add(d2);

            // dialogue 3
            Dialogue d3 = new Dialogue(te, te, font);
            d3.AddText("More enemy reinforcements!", 2300f, true);
            d3.AddText("We need a more aggressive stance against them...", 2200f, false);
            d3.AddText("Player! Press hold the left trigger down to fire your rockets!", 2200f, false);
            d3.AddText("They fire less frequently, but do more damage and affect an area.", 4200f, false);
            dialogues.Add(d3);

            // dialogue 4
            Dialogue d4 = new Dialogue(te, te, font);
            d4.AddText("They've stopped coming... I have a bad feeling about this.", 3000f, true);
            d4.AddText("I agree...", 1000f, false);
            d4.AddText("Player, keep watch for anything suspicious!", 2000f, false);
            dialogues.Add(d4);

            // dialogue 5
            Dialogue d5 = new Dialogue(te, te, font);
            d5.AddText("Sir, the Corrupted Sun is here, we're in trouble!", 3500f, true);
            d5.AddText("Take it down quick!", 2000f, false);
            dialogues.Add(d5);

            // dialogue 6
            Dialogue d6 = new Dialogue(te, null, font);
            d6.AddText("Player, he's almost destroyed, keep it up!", 2000f, true);
            dialogues.Add(d6);

            Dialogue d7 = new Dialogue(te, te, font);
            d7.AddText("Somehow we did it...", 2000f, true);
            d7.AddText("Good work player, return to base to debrief.", 3000f, false);
            d7.AddText("We have much to talk about...", 2500f, false);
            dialogues.Add(d7);
        }

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

            spawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            stageTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            switch (stage)
            {
                case 0:
                    if (stageTimer > 5000f)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 1:
                    currentDialogue = dialogues[0];
                    if (currentDialogue.FinishedPlaying)
                    {
                        SetStage(stage + 1);
                        MediaPlayer.Volume = 0.1f;
                        MediaPlayer.Play(snailChan);
                    }

                    break;
                case 2:
                    if (stageTimer > 5000f)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 3:
                    currentDialogue = dialogues[1];

                    if (currentDialogue.FinishedPlaying)
                    {
                        SetStage(stage + 1);
                    }

                    break;
                case 4: // 18 seconds
                    if (spawnTimer > 1000f && stageTimer < 14000f)
                    {
                        spawnTimer = 0;
                        SpawnSinEnemy(new Vector2(-100, 70), true);
                    }

                    if (stageTimer > 18000f)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 5:
                    currentDialogue = dialogues[2];
                    if (currentDialogue.FinishedPlaying)
                    {
                        SetStage(stage + 1);
                    }

                    break;
                case 6:
                    if (spawnTimer > 1000f)
                    {
                        spawnTimer = 0;
                        SpawnSinEnemy(new Vector2(-100, 70), true);
                    }

                    if (stageTimer > 18000f)
                    {
                        currentDialogue = dialogues[3];
                        if (currentDialogue.FinishedPlaying)
                        {
                            SetStage(stage + 1);
                        }
                    }
                    break;
                case 7:
                    if (spawnTimer > 1000f && stageTimer < 25000f)
                    {
                        spawnTimer = 0;
                        SpawnSinEnemy(new Vector2(-100, 70), true);
                        if (rand.NextDouble() < 0.8)
                        {
                            SpawnSinEnemy(new Vector2(ScreenManager.GetInstance().Width + 100,
                                rand.Next(0, ScreenManager.GetInstance().Height / 2)), false);
                        }
                    }

                    if (stageTimer > 30000f)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 8:
                    currentDialogue = dialogues[4];
                    if (currentDialogue.FinishedPlaying)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 9:
                    if (stageTimer > 7000f)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 10:
                    currentDialogue = dialogues[5];
                    if (currentDialogue.FinishedPlaying)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 11:
                    // Boss fight
                    if (!spawnedBoss)
                    {
                        Texture2D bossTexture = Content.Load<Texture2D>("TestBoss");
                        boss = new CorruptedSunBoss(Players, 100)
                        {
                            Texture = bossTexture,
                            Size = new Point(bossTexture.Width * 5, bossTexture.Height * 5),
                            BulletTexture = blank,
                            Color = Color.White,
                            vortexEnemyTexture = Content.Load<Texture2D>("Vortex"),
                            Blank = blank,
                            sunBullet = Content.Load<Texture2D>("SunBullet"),
                        };
                        boss.SetBullets(Bullets);
                        AddEnemy(boss);
                        spawnedBoss = true;
                    }
                    else
                    {
                        if ((float)boss.Health / boss.MAX_HEALTH < 0.5f)
                        {
                            currentDialogue = dialogues[6];
                        }
                    }

                    if (Enemies.Count == 0)
                    {
                        SetStage(stage + 1);
                    }
                    break;
                case 12:
                    currentDialogue = dialogues[7];

                    if (currentDialogue.FinishedPlaying)
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

                        }
                    }
                    break;
            }

            if (currentDialogue != null)
            {
                currentDialogue.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            float textHeight = tFont.MeasureString("H").Y;
            float opacity = 1f;
            switch (stage)
            {
                case 0:

                    if (stageTimer < 600f)
                    {
                        opacity = stageTimer / 600f;
                    }
                    else if (stageTimer > 4400f)
                    {
                        opacity = (5000f - stageTimer) / 600f;
                    }

                    spriteBatch.Draw(blank, new Rectangle(ScreenManager.GetInstance().Width / 2, ScreenManager.GetInstance().Height / 2, ScreenManager.GetInstance().Width, 400)
                        , null, Color.Green * opacity * 0.8f, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                    if (opacity == 1)
                    {
                        spriteBatch.DrawString(tFont, "Tutorial", new Vector2(300 + 700 * stageTimer / 5000f,
                            ScreenManager.GetInstance().Height / 2 - textHeight / 2), Color.White, 0f, new Vector2(0, 0),
                            1f, SpriteEffects.None, 0f);
                    }
                    break;
                case 1:
                    break;
                case 2:

                    if (stageTimer < 600f)
                    {
                        opacity = stageTimer / 600f;
                    }
                    else if (stageTimer > 4400f)
                    {
                        opacity = (5000f - stageTimer) / 600f;
                    }

                    spriteBatch.Draw(blank, new Rectangle(ScreenManager.GetInstance().Width / 2, ScreenManager.GetInstance().Height / 2, ScreenManager.GetInstance().Width, 400)
                        , null, Color.Green * opacity * 0.8f, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                    if (opacity == 1)
                    {
                        spriteBatch.DrawString(tFont, "Game Start!", new Vector2(300 + 700 * stageTimer / 5000f,
                            ScreenManager.GetInstance().Height / 2 - textHeight / 2), Color.White, 0f, new Vector2(0, 0),
                            1f, SpriteEffects.None, 0f);
                    }
                    break;
            }

            currentDialogue?.Draw(spriteBatch);
        }
    }
}
