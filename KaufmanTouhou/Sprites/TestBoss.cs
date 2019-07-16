using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static KaufmanTouhou.Sprites.Bullet;

namespace KaufmanTouhou.Sprites
{
    /// <summary>
    /// A test boss. Purely temporary.
    /// </summary>
    public class TestBoss : Enemy
    {
        private float stageTimer, bulletTimer, vortexSpawnTimer;
        public Texture2D Blank, vortexEnemyTexture;
        public readonly int MAX_HEALTH;

        /// <summary>
        /// Creates a new instance of the test boss.
        /// </summary>
        /// <param name="players"></param>
        public TestBoss(Player[] players, int health) : base(players)
        {
            MAX_HEALTH = health;
            Health = MAX_HEALTH;
        }

        /// <summary>
        /// Updates the logic and conditional checking for the test boss.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            stageTimer += dt;
            bulletTimer += dt;
            float theta = stageTimer / 1000f;
            //Velocity = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * 100f;

            if (bulletTimer > 500f)
            {
                bulletTimer = 0f;
                for (int i = 0; i < 4; i++)
                {
                    Player p = GetPlayer(i);

                    if (p != null)
                    {
                        float angle = GetAngleBetweenSprite(p);
                        Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 500f;
                        LinearBullet b = new LinearBullet(EntitySide.ENEMY, 7000f, vel)
                        {
                            Texture = BulletTexture,
                            Position = Position,
                            Size = new Point(15, 8),
                            Color = Color.YellowGreen,
                        };

                        CurrentStage.AddBullet(b);
                    }
                }
            }
            vortexSpawnTimer -= dt;
            if ((float)(Health) / MAX_HEALTH < 0.5f && vortexSpawnTimer <= 0)
            {
                vortexSpawnTimer = 3000f;
                int Swidth = ScreenManager.GetInstance().Width;
                int Sheight = ScreenManager.GetInstance().Height;
                VortexEnemy en = new VortexEnemy(CurrentStage.Players, 4, 4000f, 10000f)
                {
                    Velocity = new Vector2(0, -200f),
                    Position = new Vector2(200, Sheight),
                    Texture = vortexEnemyTexture,
                    BulletTexture = Blank,
                    Size = new Point(vortexEnemyTexture.Width * SCALE, vortexEnemyTexture.Height * SCALE),
                };
                VortexEnemy en2 = new VortexEnemy(CurrentStage.Players, 4, 4000f, 10000f)
                {
                    Velocity = new Vector2(0, -200f),
                    Position = new Vector2(Swidth - 200, Sheight),
                    Texture = vortexEnemyTexture,
                    BulletTexture = Blank,
                    Size = new Point(vortexEnemyTexture.Width * SCALE, vortexEnemyTexture.Height * SCALE),
                };
                en.SetBullets(Bullets);
                en2.SetBullets(Bullets);
                CurrentStage.AddEnemy(en);
                CurrentStage.AddEnemy(en2);
            }


            //Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Velocity = Vector2.Zero;
            // no velocity, position is fixed
            int width = ScreenManager.GetInstance().Width;
            int height = ScreenManager.GetInstance().Height;
            Position = new Vector2((float)(Math.Cos(theta) + 1) / 2 * width * 0.75f + 0.125f * width,
                (float)(Math.Sin(theta) + 1) / 2 * height / 4 + 200);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the boss the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Rectangle drawRect = new Rectangle(Position.ToPoint(), Size);
            spriteBatch.Draw(Texture, drawRect, null, Color.White * 0.98f, 0f, Origin, SpriteEffects.None, 0f);

            // draws the health bar
            float prog = (float)(Health) / MAX_HEALTH;
            int width = (int)(ScreenManager.GetInstance().Width * 3 / 4 * prog);
            Rectangle healthRect = new Rectangle(ScreenManager.GetInstance().Width / 2, 32, width, 32);
            spriteBatch.Draw(Blank, healthRect, null, Color.Red, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }
    }
}
