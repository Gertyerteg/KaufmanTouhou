using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KaufmanTouhou.Sprites.Bullet;

namespace KaufmanTouhou.Sprites
{
    class SpinnerBoss : Enemy
    {
        private float stageTimer, bulletTimer, vortexSpawnTimer, sinSpawner, sinSpawner2;
        public readonly int MAX_HEALTH;
        private Texture2D vortexEnemyTexture, Blank;
        private float opacity;

        /// <summary>
        /// Creates a new instance of the test boss.
        /// </summary>
        /// <param name="players"></param>
        public SpinnerBoss(Player[] players, int health) : base(players)
        {
            opacity = 0f;
            ContentManager Content = CurrentStage.Content;
            MAX_HEALTH = health;
            Health = MAX_HEALTH;
            BulletTexture = Blank = Content.Load<Texture2D>("Blank");
            Texture = Content.Load<Texture2D>("FidgetBoss");
            vortexEnemyTexture = Content.Load<Texture2D>("Vortext");
            Size = new Point(Texture.Width * 8, Texture.Height * 8);
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
            sinSpawner -= dt;
            sinSpawner2 -= dt;
            float theta = stageTimer / 1000f;
            //Velocity = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * 100f;

            if (bulletTimer > 300f)
            {
                bulletTimer = 0f;
                for (int i = 0; i < 4; i++)
                {
                    Player p = GetPlayer(i);

                    if (p == null)
                    {
                        p = CurrentStage.GetRandomPlayer();
                    }

                    float rot = MathHelper.PiOver2 * i + Rotation;
                    Vector2 offset = new Vector2((float)(Math.Cos(rot)), (float)(Math.Sin(rot))) * 220f;
                    Vector2 pos = Position + offset;
                    float angle = GetAngleBetweenSprite(p, pos);
                    Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 800f;
                    LinearBullet b = new LinearBullet(EntitySide.ENEMY, 7000f, vel)
                    {
                        Texture = BulletTexture,
                        Position = pos,
                        Size = new Point(18, 8),
                        Color = Color.OrangeRed,
                    };

                    CurrentStage.AddBullet(b);
                }
            }
            vortexSpawnTimer -= dt;
            if ((float)(Health) / MAX_HEALTH < 0.75f && vortexSpawnTimer <= 0)
            {
                vortexSpawnTimer = 2500f;
                int Swidth = ScreenManager.GetInstance().Width;
                int Sheight = ScreenManager.GetInstance().Height + 200;
                VortexEnemy en = new VortexEnemy(CurrentStage.Players, 4, 4000f, 10000f)
                {
                    Velocity = new Vector2(0, -200f),
                    Position = new Vector2(200, Sheight),
                    Texture = vortexEnemyTexture,
                    BulletTexture = Blank,
                    Size = new Point(vortexEnemyTexture.Width * 3, vortexEnemyTexture.Height * 3),
                };
                VortexEnemy en2 = new VortexEnemy(CurrentStage.Players, 4, 4000f, 10000f)
                {
                    Velocity = new Vector2(0, -200f),
                    Position = new Vector2(Swidth - 200, Sheight),
                    Texture = vortexEnemyTexture,
                    BulletTexture = Blank,
                    Size = new Point(vortexEnemyTexture.Width * 3, vortexEnemyTexture.Height * 3),
                };
                CurrentStage.AddEnemy(en);
                CurrentStage.AddEnemy(en2);
            }

            if ((float)Health / MAX_HEALTH < 0.5f && sinSpawner <= 0)
            {
                sinSpawner = 1750f;
                CurrentStage.SpawnSinEnemy(new Vector2(ScreenManager.GetInstance().Width + 100, 300), false);
            }

            if ((float)Health / MAX_HEALTH <= 0.3f && sinSpawner2 <= 0)
            {
                sinSpawner2 = 1750f;
                CurrentStage.SpawnInvisSinEnemy(new Vector2(-100, 300), true);
            }

            if (opacity < 1)
            {
                opacity += (float)gameTime.ElapsedGameTime.TotalSeconds / 3;
            }


            //Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Velocity = Vector2.Zero;
            // no velocity, position is fixed
            int width = ScreenManager.GetInstance().Width;
            int height = ScreenManager.GetInstance().Height;
            Position = new Vector2((float)(Math.Cos(theta) + 1) / 2 * width * 0.75f + 0.125f * width,
                (float)(Math.Sin(theta) + 1) / 2 * height / 4 + 200);
            Rotation += dt / 3000 * MathHelper.TwoPi;
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
            spriteBatch.Draw(Texture, drawRect, null, Color.White * opacity, Rotation, Origin, SpriteEffects.None, 0f);

            // draws the health bar
            float prog = (float)(Health) / MAX_HEALTH;
            int width = (int)(ScreenManager.GetInstance().Width * 3 / 4 * prog);
            Rectangle healthRect = new Rectangle(ScreenManager.GetInstance().Width / 2, 32, width, 32);
            spriteBatch.Draw(Blank, healthRect, null, Color.Red, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }
    }
}
