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
    public class SinPathEnemy : Enemy
    {
        private Random rand;
        private float pathTimer, gunTimer;
        private readonly bool isGoingRight;

        /// <summary>
        /// Creates a new instance of the enemy.
        /// </summary>
        /// <param name="players"></param>
        public SinPathEnemy(Player[] players, bool goingRight) : base(players)
        {
            rand = new Random();
            Health = 3;
            gunTimer = rand.Next(0, 2000);
            isGoingRight = goingRight;
        }

        /// <summary>
        /// Draws the sinpathenemy to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Size.X,
                Size.Y), null, Color, Rotation, Origin, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Updates the logic of the enemy.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            gunTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            pathTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float x = (isGoingRight) ? 1 : -1;
            Velocity = new Vector2(x * 2, (float)Math.Sin(pathTimer / 500));
            Rotation = GetAngleBetweenSprite(GetNearestPlayer());

            if (gunTimer < 0)
            {
                gunTimer = 500f + rand.Next(0, 1500);

                // shoot
                float angle = Rotation + (float)rand.NextDouble() * 0.4f - 0.2f;
                LinearBullet bullet = new LinearBullet(EntitySide.ENEMY, 10000f, 
                    new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 900f)
                {
                    Texture = BulletTexture,
                    Position = Position,
                    Size = new Point(16, 9),
                    Color = Color.Red,
                };

                getBullets().Add(bullet);

            }

            Position += Velocity * 140f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isGoingRight)
            {
                if (Position.X > ScreenManager.GetInstance().Width + Texture.Width / 2 * SCALE)
                {
                    IsActive = false;
                }
            }
            else
            {
                if (Position.X < -Texture.Width / 2 * SCALE)
                {
                    IsActive = false;
                }
            }
        }

    }
}
