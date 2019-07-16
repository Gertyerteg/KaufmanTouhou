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
    public class VortexEnemy : Enemy
    {
        private float spinTimer, spinPeriod, bulletTimer, ttl;
        private int spin;

        /// <summary>
        /// Creates a new instance of the <c>VortexEnemy</c>.
        /// </summary>
        /// <param name="players"></param>
        /// <param name="spin"></param>
        /// <param name="period"></param>
        public VortexEnemy(Player[] players, int spin, float period, float ttl)
            : base(players)
        {
            this.spin = spin;
            this.ttl = ttl;
            spinPeriod = period;
        }

        /// <summary>
        /// Updates the logic of the vortex enemy.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            spinTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            ttl -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ttl < 0)
                IsActive = false;
            Rotation  = spinTimer * MathHelper.TwoPi / spinPeriod;

            if (bulletTimer > 500f)
            {
                bulletTimer = 0;
                for (int i = 0; i < spin; i++)
                {
                    float a = Rotation + i * MathHelper.TwoPi / spin;
                    float xD = (float)Math.Cos(a);
                    float yD = (float)Math.Sin(a);
                    Vector2 dir = new Vector2(xD, yD) * 500f;
                    LinearBullet b = new LinearBullet(EntitySide.ENEMY, 6000f, dir)
                    {
                        Position = Position,
                        Size = new Point(10, 6),
                        Texture = BulletTexture,
                        Color = Color.Red,
                    };

                    getBullets().Add(b);
                }
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Draws the vortex enemy to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Rectangle drawRect = new Rectangle(Position.ToPoint(), Size);
            spriteBatch.Draw(Texture, drawRect, null, Color, Rotation, Origin, SpriteEffects.None, 0f);
        }
    }
}
