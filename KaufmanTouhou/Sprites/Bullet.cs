using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou.Sprites
{
    public class Bullet : Sprite
    {
        /// <summary>
        /// The side the bullet resides with. This is to make sure the bullet doesn't collide
        /// with itself or its teammates.
        /// </summary>
        public enum EntitySide
        {
            PLAYER,
            ENEMY,

        }

        /// <summary>
        /// Whether the bullet's time to live has been exceeded.
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        public EntitySide Side
        {
            get;
            set;
        }

        public float TTL;

        public Vector2 InitVelocity;


        /// <summary>
        /// Creates a new instance of the <c>Bullet</c>.
        /// </summary>
        /// <param name="side"></param>
        /// <param name="size"></param>
        /// <param name="ttl"></param>
        public Bullet(EntitySide side, float ttl)
        {
            Side = side;
            TTL = ttl;
            InitVelocity = Vector2.Zero;
            IsActive = true;
        }


        /// <summary>
        /// Updates the bullet's position and logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            TTL -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position += InitVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TTL < 0)
                IsActive = false;
        }

        /// <summary>
        /// Returns the draw rectangle for the bullet's hitbox.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle(Position.ToPoint(), Size);
        }

        /// <summary>
        /// Draws the bullet to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, GetDrawRectangle(), null, Color, Rotation, Origin, SpriteEffects.None, 0f);
        }
    }
}
