using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou.Screens
{
    /// <summary>
    /// A level within the game that is indepedently updated and drawn.
    /// Think of it as a screen within a screen!
    /// </summary>
    public class Stage
    {
        /// <summary>
        /// The contentmanager of the stage.
        /// </summary>
        public ContentManager Content;

        /// <summary>
        /// The players within the game.
        /// </summary>
        public Player[] Players;

        public List<Enemy> Enemies;
        public List<Bullet> Bullets;
        public List<Rocket> Rockets;
        public List<Dialogue> Dialogues;
        public Dialogue CurrentDialogue;
        private List<Explosion> explosions;
        public float StageTimer;
        public int StageNumber;
        private Texture2D blank, sinEnemyTexture, vortexTexture, explosionTexture;
        private Random rand;
        public SpriteFont tFont;

        /// <summary>
        /// Creates a new instance of the <c>Stage</c>.
        /// </summary>
        /// <param name="content"></param>
        public Stage(ContentManager content, Player[] players)
        {
            rand = new Random();
            Players = players;
            Content = new ContentManager(content.ServiceProvider, "Content");
            tFont = content.Load<SpriteFont>("TransitionFont");
            Bullets = new List<Bullet>();
            Enemies = new List<Enemy>();
            Rockets = new List<Rocket>();
            explosions = new List<Explosion>();

            blank = content.Load<Texture2D>("Blank");
            sinEnemyTexture = content.Load<Texture2D>("Enemy");
            vortexTexture = content.Load<Texture2D>("Vortex");
            explosionTexture = content.Load<Texture2D>("Explosion");
        }

        public void AddExplosion(Explosion e)
        {
            explosions.Add(e);
        }

        public void AddRocket(Rocket r)
        {
            Rockets.Add(r);
        }

        /// <summary>
        /// Sets the stage of the game.
        /// </summary>
        /// <param name="stage"></param>
        public void SetStage(int stage)
        {
            StageNumber = stage;
            StageTimer = 0;
        }

        public void AddEnemy(Enemy e)
        {
            Enemies.Add(e);
        }

        public void AddBullet(Bullet b)
        {
            Bullets.Add(b);
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Player GetPlayer(int index)
        {
            return Players[index];
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Player GetPlayer(PlayerIndex index)
        {
            return GetPlayer((int)index);
        }

        public Player GetRandomPlayer()
        {
            Player p = GetPlayer(rand.Next(0, 4));

            if (p == null)
                return GetRandomPlayer();

            return p;
        }

        /// <summary>
        /// Indicates whether the stage is finished.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStageFinished()
        {
            return false;
        }

        /// <summary>
        /// Initializes the stage.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Unloads the contentmanager.
        /// </summary>
        public virtual void Unload()
        {
            Content.Unload();
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
            AddEnemy(e);

        }

        public void SpawnVortexEnemy(Vector2 initialPosition, Vector2 velocity, float ttl)
        {
            VortexEnemy en = new VortexEnemy(Players, 4, 3000f, ttl)
            {
                Texture = vortexTexture,
                Position = initialPosition,
                Velocity = velocity,
                Health = 6,
                Size = new Point(vortexTexture.Width * Sprite.SCALE / 2, vortexTexture.Height * Sprite.SCALE / 2),
                BulletTexture = blank,
            };
            Enemies.Add(en);
        }

        /// <summary>
        /// Spawns a sinpathenemy
        /// </summary>
        public void SpawnInvisSinEnemy(Vector2 initialPosition, bool goingRight)
        {
            InvisSinEnemy e = new InvisSinEnemy(Players, goingRight)
            {
                Position = initialPosition,
                BulletTexture = blank,
                Texture = sinEnemyTexture,
                Color = Color.White,
                Size = new Point(sinEnemyTexture.Width * Sprite.SCALE, sinEnemyTexture.Height * Sprite.SCALE),
            };
            AddEnemy(e);

        }

        /// <summary>
        /// The amount of players currently active in the game.
        /// </summary>
        /// <returns></returns>
        public int GetPlayerCount()
        {
            int pl = 0;
            foreach (Player p in Players)
            {
                if (p != null)
                    pl++;
            }

            return pl;
        }

        /// <summary>
        /// Updates the instance of the <c>Stage</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i]?.Update(gameTime);
            }

            for (int i = 0; i < Bullets.Count; i++)
            {
                if (!Bullets[i].IsActive)
                {
                    Bullets.RemoveAt(i--);
                }
                else
                    Bullets[i].Update(gameTime);
            }

            for (int i = 0; i < Rockets.Count; i++)
            {
                if (!Rockets[i].IsActive)
                {
                    Rockets.RemoveAt(i--);
                }
                else
                    Rockets[i].Update(gameTime);
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                if (!explosions[i].IsActive)
                {
                    explosions.RemoveAt(i--);
                }
                else
                    explosions[i].Update(gameTime);
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].IsActive)
                {
                    Enemies[i].Update(gameTime);
                }
                else
                {
                    Explosion e = new Explosion(400f)
                    {
                        Texture = explosionTexture,
                        Position = Enemies[i].Position,
                        Size = new Point(80, 80),
                    };
                    AddExplosion(e);
                    Enemies.RemoveAt(i--);
                }
            }


        }

        /// <summary>
        /// Pauses the stage.
        /// </summary>
        public virtual void Pause()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
            else if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        /// <summary>
        /// Draws the <c>Stage</c> to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in Bullets)
                b.Draw(spriteBatch);

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i]?.Draw(spriteBatch);
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw(spriteBatch);
            }

            for (int i = 0; i < Rockets.Count; i++)
            {
                Rockets[i].Draw(spriteBatch);
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }
        }

        public void DrawSplash(SpriteBatch spriteBatch, string text)
        {
            float textHeight = tFont.MeasureString("H").Y;

            float opacity = 1f;
            if (StageTimer < 600f)
            {
                opacity = StageTimer / 600f;
            }
            else if (StageTimer > 4400f)
            {
                opacity = (5000f - StageTimer) / 600f;
            }

            spriteBatch.Draw(blank, new Rectangle(ScreenManager.GetInstance().Width / 2, ScreenManager.GetInstance().Height / 2, ScreenManager.GetInstance().Width, 400)
                , null, Color.Green * opacity * 0.8f, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
            if (opacity == 1)
            {
                spriteBatch.DrawString(tFont, text, new Vector2(300 + 700 * StageTimer / 5000f,
                    ScreenManager.GetInstance().Height / 2 - textHeight / 2), Color.White, 0f, new Vector2(0, 0),
                    1f, SpriteEffects.None, 0f);
            }
        }
    }
}
