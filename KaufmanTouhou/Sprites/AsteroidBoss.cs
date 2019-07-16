using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KaufmanTouhou.Sprites
{
    public class AsteroidBoss : Enemy
    {
        public readonly float MAX_HEALTH;
        private float timer, spawnTimer, spawnTimer2;
        private Random rand;
        private enum AsteroidBossStage
        {
            APPEARING,
            FIRING,
            HIDING,
            DYING,
        }

        private AsteroidBossStage stage;
        private Texture2D blank;
        private Texture2D[] asteroidTextures;

        /// <summary>
        /// Creates a new instance of an <c>AsteroidBoss</c>.
        /// </summary>
        /// <param name="players"></param>
        /// <param name="health"></param>
        public AsteroidBoss(Player[] players, int health) : base(players)
        {
            asteroidTextures = new Texture2D[5];
            for (int i = 0; i < asteroidTextures.Length; i++)
            {
                asteroidTextures[i] = CurrentStage.Content.Load<Texture2D>("Asteroid" + i);
            }
            stage = AsteroidBossStage.APPEARING;
            blank = CurrentStage.Content.Load<Texture2D>("Blank");
            Health = health;
            MAX_HEALTH = health;
            rand = new Random();
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            timer += dt;
            spawnTimer += dt;
            spawnTimer2 += dt;

            if (Health <= 0)
            {
                stage = AsteroidBossStage.DYING;
            }

            switch (stage)
            {
                case AsteroidBossStage.APPEARING:
                    if (Position.Y < 200)
                    {
                        Velocity = new Vector2(0, 200);
                    }
                    else
                    {
                        timer = 0;
                        stage = AsteroidBossStage.FIRING;
                    }
                    break;

                case AsteroidBossStage.FIRING:

                    if (spawnTimer > 300f) {
                        spawnTimer = 0;
                        for (int i = 0; i < Players.Length; i++)
                        {
                            if (Players[i] != null)
                            {
                                float scale = (float)(rand.NextDouble() * (SCALE - 3) + 3f);
                                float speed = 1600 / (float)Math.Sqrt(scale);
                                float angle = GetAngleBetweenSprite(Players[i]);
                                Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                                Texture2D aster = asteroidTextures[rand.Next(0, 5)];
                                Asteroid a = new Asteroid(Bullet.EntitySide.ENEMY, 7000f, dir * speed)
                                {
                                    Texture = aster,
                                    Position = Position,
                                    Size = new Point((int)(aster.Width * scale), (int)(aster.Height * scale)),
                                    Health = (int)(Math.Ceiling(scale / 2)),
                                    Color = Color.White,
                                    InitVelocity = dir * speed,
                                };
                                CurrentStage.AddBullet(a);
                            }
                        }

                        if (timer > 10000)
                        {
                            timer = 0;
                            stage = AsteroidBossStage.HIDING;
                        }
                    }
                    break;

                case AsteroidBossStage.HIDING:
                    if (Position.Y > -300)
                    {
                        Velocity = new Vector2(0, -200);
                    }
                    else
                    {
                        if (spawnTimer > 60f)
                        {
                            spawnTimer = 0;
                            Vector2 pos = new Vector2(rand.Next(-100, ScreenManager.GetInstance().Width + 100), -90);
                            float scale = (float)(rand.NextDouble() * (SCALE - 3) + 3f);
                            float speed = 1000 / (float)Math.Sqrt(scale);
                            Texture2D aster = asteroidTextures[rand.Next(0, 5)];
                            float xSpeed = (float)((rand.NextDouble() - 0.5f) * 100f);
                            float iRot = (float)(rand.NextDouble() * MathHelper.TwoPi);
                            Asteroid a = new Asteroid(Bullet.EntitySide.ENEMY, 7000f, new Vector2(xSpeed, speed))
                            {
                                Texture = aster,
                                Position = pos,
                                Size = new Point((int)(aster.Width * scale), (int)(aster.Height * scale)),
                                Health = (int)(Math.Ceiling(scale / 2)),
                                Color = Color.White,
                                InitVelocity = new Vector2(xSpeed, speed),
                                Rotation = iRot,
                            };
                            Bullets.Add(a);
                        }
                        if (timer > 14000)
                        {
                            timer = 0;
                            stage = AsteroidBossStage.APPEARING;
                        }
                    }
                    break;

                case AsteroidBossStage.DYING:
                    Velocity = new Vector2(0, -100);

                    if (Position.Y < -200)
                        IsActive = false;
                    break;
            }

            if (Health <= 0)
            {
                stage = AsteroidBossStage.DYING;
            }

            if (stage != AsteroidBossStage.HIDING && Position.Y > 0)
                CheckBulletCollision();


            if (Health / MAX_HEALTH < 0.4f)
            {
                if (spawnTimer2 > 800)
                {
                    spawnTimer2 = 0;
                    Vector2 pos = new Vector2(rand.Next(-100, ScreenManager.GetInstance().Width + 100), -90);
                    float scale = (float)(rand.NextDouble() * (SCALE - 3) + 3f);
                    float speed = 1000 / (float)Math.Sqrt(scale);
                    Texture2D aster = asteroidTextures[rand.Next(0, 5)];
                    float xSpeed = (float)((rand.NextDouble() - 0.5f) * 100f);
                    Asteroid a = new Asteroid(Bullet.EntitySide.ENEMY, 7000f, new Vector2(xSpeed, speed))
                    {
                        Texture = aster,
                        Position = pos,
                        Size = new Point((int)(aster.Width * scale), (int)(aster.Height * scale)),
                        Health = (int)Math.Ceiling(scale / 2),
                        Color = Color.White,
                        InitVelocity = new Vector2(xSpeed, speed),
                    };
                    Bullets.Add(a);

                }
            }

            Rotation += dt / 10000 * MathHelper.TwoPi;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Draws the asteroid boss.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y,
                Size.X, Size.Y), null, Color.White, Rotation, Origin, SpriteEffects.None, 0f);
            // draws the health bar
            float prog = Health / MAX_HEALTH;
            int width = (int)(ScreenManager.GetInstance().Width * 3 / 4 * prog);
            Rectangle healthRect = new Rectangle(ScreenManager.GetInstance().Width / 2, 32, width, 32);
            spriteBatch.Draw(blank, healthRect, null, Color.Red, 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }
    }
}
