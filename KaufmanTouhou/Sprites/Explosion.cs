using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou.Sprites
{
    /// <summary>
    /// An explosion effect placed after a sprite dies.
    /// </summary>
    public class Explosion : Sprite
    {
        private float timer;
        public readonly float EXPLOSION_TIME;

        /// <summary>
        /// Whether the explosion is over or not.
        /// </summary>
        public bool IsActive
        {
            get { return EXPLOSION_TIME > timer; }
        }
        
        /// <summary>
        /// Creates a new instance of the <c>Explosion</c>.
        /// </summary>
        public Explosion(float explosionTime)
        {
            
            EXPLOSION_TIME = explosionTime;
        }
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Draws the <c>Explosion</c>
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Point p = Position.ToPoint();
            int frame = (int)(timer / EXPLOSION_TIME * 5);
            Rectangle sourceRect = new Rectangle(12 * frame, 0, 12, 12);
            Rectangle dRect = new Rectangle(p.X, p.Y, Size.X, Size.Y);
            spriteBatch.Draw(Texture, dRect, sourceRect, Color.White, 0f, 
                new Vector2(Texture.Width / 10f, Texture.Height / 10f), SpriteEffects.None, 0f);
        }
    }
}
