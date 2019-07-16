using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KaufmanTouhou.Sprites
{
    public class Asteroid : LinearBullet
    {
        private float timer;

        /// <summary>
        /// Creates a new instance of the asteroid
        /// </summary>
        /// <param name="side"></param>
        /// <param name="ttl"></param>
        /// <param name="velocity"></param>
        public Asteroid(EntitySide side, float ttl, Vector2 velocity) : base(side, ttl, velocity)
        {
        }

        /// <summary>
        /// Draws the asteroid to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Updates the individual asteroid to check for collision and update timing values.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            TTL -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position += InitVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Rotation += MathHelper.TwoPi / 2000 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            CheckBulletCollision();

            if (TTL < 0 || Health < 0)
                IsActive = false;
        }

        /// <summary>
        /// Checks the bullet list for colliding bullets.
        /// </summary>
        public void CheckBulletCollision()
        {
            List<Bullet> Bullets = CurrentStage.Bullets;
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullet b = Bullets[i];
                if (!b.Side.Equals(EntitySide.ENEMY))
                {
                    float mag = (float)Math.Sqrt(Math.Pow(b.Size.X, 2) + Math.Pow(b.Size.Y, 2));
                    float playerMag = (float)Math.Sqrt(Math.Pow(Size.X / 2, 2) + Math.Pow(Size.Y / 2, 2));

                    if (Vector2.Distance(b.Position, Position) < (mag + playerMag) * Enemy.SAFE_MULT)
                    {
                        Health--;
                        Console.WriteLine("Enemy hit by enemy bullet, HP: " + Health);
                        Bullets.RemoveAt(i--);
                        return;
                    }
                }
            }
        }
    }
}
