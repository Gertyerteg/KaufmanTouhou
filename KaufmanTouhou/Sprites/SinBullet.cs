using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KaufmanTouhou.Sprites
{
    /// <summary>
    /// A bullet that follows a sinusoidal path.
    /// </summary>
    public class SinBullet : Bullet
    {
        public readonly float SPEED;
        private float timer;
        private bool isUp;
        private readonly bool reversed;

        /// <summary>
        /// Creates a new instance of the <c>SinBullet</c>.
        /// </summary>
        /// <param name="side"></param>
        /// <param name="ttl"></param>
        /// <param name="speed"></param>
        /// <param name="up"></param>
        /// <param name="reversed"></param>
        public SinBullet(EntitySide side, float ttl, float speed, bool up, bool reversed) : base(side, ttl)
        {
            this.reversed = reversed;
            isUp = up;
            SPEED = speed;
        }
        private const float xMAG = 0.4f;
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
            Rotation = timer / 200f;
            float xDir = (reversed) ? -xMAG : xMAG;
            Vector2 dir = (isUp) ? new Vector2((float)Math.Cos(Rotation) * xDir, -1) : new Vector2((float)Math.Cos(Rotation) * xDir, 1);
            //dir.Normalize();
            Velocity = dir * SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity;
        }

        /// <summary>
        /// Draws the bullet to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = GetDrawRectangle();
            //int border = 1;
            //spriteBatch.Draw(Texture, new Rectangle(rect.X - border, rect.Y - border, rect.Width, rect.Height), null, Color.White, Rotation, Origin, SpriteEffects.None, 0f);
            //spriteBatch.Draw(Texture, new Rectangle(rect.X + border, rect.Y - border, rect.Width, rect.Height), null, Color.White, Rotation, Origin, SpriteEffects.None, 0f);
            //spriteBatch.Draw(Texture, new Rectangle(rect.X - border, rect.Y + border, rect.Width, rect.Height), null, Color.White, Rotation, Origin, SpriteEffects.None, 0f);
            //spriteBatch.Draw(Texture, new Rectangle(rect.X + border, rect.Y + border, rect.Width, rect.Height), null, Color.White, Rotation, Origin, SpriteEffects.None, 0f);
            spriteBatch.Draw(Texture, rect, null, Color, Rotation, Origin, SpriteEffects.None, 0f);
        }
    }
}
