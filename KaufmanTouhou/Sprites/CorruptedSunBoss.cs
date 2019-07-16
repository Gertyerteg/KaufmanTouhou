using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaufmanTouhou.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static KaufmanTouhou.Sprites.Bullet;

namespace KaufmanTouhou.Sprites
{
    /// <summary>
    /// A test boss. Purely temporary.
    /// </summary>
    public class CorruptedSunBoss : Enemy
    {
        private float stageTimer, bulletTimer, sunBeamChargeTimer, sunBeamTimer, sunBeamSafeTimer, sunSpawnTimer;
        private Random rand;
        public Texture2D Blank, vortexEnemyTexture, sunBullet;
        public readonly int MAX_HEALTH;
        private const float SUN_BEAM_SAFE_TIMER = 5000f;
        private const float SUN_BEAM_FIRE_TIMER = 5000f;
        private bool isVertical;
        private const float SUN_BEAM_CHARGE_TIMER = 3500f;
        /// <summary>
        /// Creates a new instance of the test boss.
        /// </summary>
        /// <param name="players"></param>
        public CorruptedSunBoss(Player[] players, int health) : base(players)
        {
            rand = new Random();
            isVertical = rand.Next(0, 1) == 0;
            MAX_HEALTH = health;
            Health = MAX_HEALTH;
            sunBeamSafeTimer = SUN_BEAM_SAFE_TIMER / 10;
            sunBeamTimer = SUN_BEAM_FIRE_TIMER;
            sunBeamChargeTimer = SUN_BEAM_CHARGE_TIMER;
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
            sunBeamSafeTimer -= dt;
            float theta = stageTimer / 1000f;
            //Velocity = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * 100f;

            if (sunBeamSafeTimer < 0 && stageTimer > 5000f)
            {
                sunBeamChargeTimer -= dt;

                if (sunBeamChargeTimer <= 0)
                {
                    // fire
                    sunBeamTimer -= dt;
                    
                    Vector2 v = (isVertical) ? new Vector2(0, -400f) : new Vector2(400, 0);

                    for (int i = 1; i <= 4; i++)
                    {
                        Vector2 pos = (isVertical) ? new Vector2(i * ScreenManager.GetInstance().Width / 5, ScreenManager.GetInstance().Height) :
                        new Vector2(0, i * ScreenManager.GetInstance().Height / 5);
                        LinearBullet b = new LinearBullet(EntitySide.ENEMY, 7000f, v)
                        {
                            Texture = BulletTexture,
                            Position = pos,
                            Size = new Point(30, 50),
                            Color = Color.LightGoldenrodYellow,
                        };
                        CurrentStage.AddBullet(b);
                    }

                    if (sunBeamTimer <= 0)
                    {
                        sunBeamSafeTimer = SUN_BEAM_SAFE_TIMER;
                        sunBeamTimer = SUN_BEAM_FIRE_TIMER;
                        sunBeamChargeTimer = SUN_BEAM_CHARGE_TIMER;
                        isVertical = rand.Next(0, 2) == 0;
                    }
                }
            }

            if (stageTimer > 12000f)
            {
                sunSpawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (sunSpawnTimer > 5000)
                {
                    sunSpawnTimer = 0f;
                    int rad = 100;

                    for (int i = 0; i < CurrentStage.GetPlayerCount(); i++)
                    {
                        Vector2 newPos = new Vector2(Position.X + rand.Next(-rad, rad), Position.Y + rand.Next(-rad, rad));

                        MiniSun s = new MiniSun(Players)
                        {
                            BulletTexture = sunBullet,
                            Position = newPos,
                            Health = 3,
                            Color = Color.White,
                            Texture = Texture,
                            Size = new Point(Texture.Width * SCALE / 3, Texture.Height * SCALE / 3),
                        };
                        CurrentStage.AddEnemy(s);
                    }
                }
            }

            if (bulletTimer > 400f)
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
                            Texture = sunBullet,
                            Position = Position,
                            Size = new Point(32, 32),
                            Color = Color.White,
                        };

                        CurrentStage.AddBullet(b);
                    }
                }
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
            if (sunBeamSafeTimer <= 0 && stageTimer > 5000f)
            {
                float opacity = (float)(-Math.Cos(sunBeamSafeTimer / 900f) + 1) / 2 * 0.8f;
                int sWidth = ScreenManager.GetInstance().Width;
                int sHeight = ScreenManager.GetInstance().Height;
                for (int i = 0; i < 4; i++)
                {
                    if (isVertical)
                    {
                        spriteBatch.Draw(Blank, new Rectangle(sWidth / 5 * (i + 1), sHeight / 2, 30, sHeight), null,
                            Color.White * opacity, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                    }
                    else
                        spriteBatch.Draw(Blank, new Rectangle(sWidth / 2, sHeight / 5 * (i + 1), sWidth, 30), null,
                            Color.White * opacity, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);

                }
            }

            // draws the health bar
            float prog = (float)(Health) / MAX_HEALTH;
            int width = (int)(ScreenManager.GetInstance().Width * 3 / 4 * prog);
            Rectangle healthRect = new Rectangle(ScreenManager.GetInstance().Width / 2, 32, width, 32);
            spriteBatch.Draw(Blank, healthRect, null, Color.Red, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }
    }
}
