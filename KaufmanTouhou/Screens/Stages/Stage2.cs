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
    public class Stage2 : Stage
    {
        private Random rand;
        private bool isOver;
        private LichBoss boss;
        private Texture2D evilQueenFace;
        private Vector2 evilQueenFacePos;
        private SpriteFont font;
        private Song charmPoint;

        /// <summary>
        /// Creates a new instance of the <c>Stage2</c>.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="players"></param>
        public Stage2(ContentManager content, Player[] players) : base(content, players)
        {
            evilQueenFacePos = new Vector2(ScreenManager.GetInstance().Width / 2f, ScreenManager.GetInstance().Height + 400);
            Dialogues = new List<Dialogue>();
            rand = new Random();
            isOver = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            font = Content.Load<SpriteFont>("DialogueFont");
            charmPoint = Content.Load<Song>("CharmPoint");
            evilQueenFace = Content.Load<Texture2D>("EvilQueenFace");
            SetStage(5);

            InitializeDialogues();
        }

        public void InitializeDialogues()
        {
            Texture2D bunny = Content.Load<Texture2D>("RightBunny");
            Texture2D panda = Content.Load<Texture2D>("LeftPanda");
            Texture2D lich = Content.Load<Texture2D>("EvilQueen");
            // 0
            Dialogue d0 = new Dialogue(panda, bunny, font);
            d0.AddText("Player, we've finally tracked the Evil Queen.", 2000f, true);
            d0.AddText("She's not getting away this time!", 2000f, false);
            Dialogues.Add(d0);

            // 1
            Dialogue d1 = new Dialogue(lich, null, font);
            d1.AddText("HAHAHAHAHAHA!", 2000f, true);
            d1.AddText("You absolute baffoons!", 2000f, true);
            d1.AddText("Now that the Princess is away from the Kingdom.", 2400f, true);
            d1.AddText("I am free to invade that area whenever I want!", 2400f, true);
            Dialogues.Add(d1);

            // 2
            Dialogue d2 = new Dialogue(panda, bunny, font);
            d2.AddText("Not if Player can help it!", 1500, false);
            d2.AddText("Yeah, you won't get away this!", 1700f, true);
            Dialogues.Add(d2);

            // 3
            Dialogue d3 = new Dialogue(lich, null, font);
            d3.AddText("If you think I'll be as easy to defeat as my henchmen...", 3000f, true);
            d3.AddText("T H I N K  A G A I N !", 1700f, true);
            Dialogues.Add(d3);
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

            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            StageTimer += dt;

            switch (StageNumber)
            {
                case 0:
                    if (StageTimer > 5000f)
                    {
                        SetStage(StageNumber + 1);
                        MediaPlayer.Play(charmPoint);
                    }
                    break;
                case 1:
                    CurrentDialogue = Dialogues[0];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 2:
                    CurrentDialogue = Dialogues[1];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 3:
                    CurrentDialogue = Dialogues[2];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 4:
                    CurrentDialogue = Dialogues[3];

                    if (CurrentDialogue.FinishedPlaying)
                    {
                        SetStage(StageNumber + 1);
                    }
                    break;
                case 5:
                    // evil queen entrance
                    evilQueenFacePos += new Vector2(0, -1100) * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (evilQueenFacePos.Y < -400)
                    {
                        SetStage(StageNumber + 1);
                        boss = new LichBoss(Players, 200);
                        Enemies.Add(boss);
                    }
                    break;
                case 6:
                    // first stage of evil queen
                    break;
                case 7:
                    // king launches a nuke
                    break;
                case 8:
                    // 2nd stage
                    break;
                case 9:
                    // 3rd stage
                    break;
                case 10:
                    // end game
                    break;
            }

            CurrentDialogue?.Update(gameTime);
        }

        /// <summary>
        /// Draws the contents of the stage to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            switch (StageNumber)
            {
                case 0:
                    DrawSplash(spriteBatch, "Game Start!");
                    break;
                case 5:
                    spriteBatch.Draw(evilQueenFace, evilQueenFacePos, null, Color.White, 0f, new Vector2(evilQueenFace.Width / 2f, evilQueenFace.Height / 2f), Sprite.SCALE, SpriteEffects.None, 0f);
                    break;
            }

            CurrentDialogue?.Draw(spriteBatch);
        }
    }
}
