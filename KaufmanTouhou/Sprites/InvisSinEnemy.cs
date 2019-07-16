using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KaufmanTouhou.Sprites
{
    public class InvisSinEnemy : SinPathEnemy
    {
        private float timer, opacity;
        
        public InvisSinEnemy(Player[] players, bool goingRight) : base(players, goingRight)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Size.X,
                Size.Y), null, Color * opacity, Rotation, Origin, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            opacity = (float)(-Math.Cos(timer / 1000f) * 0.1f) + 0.07f;
            
        }
    }
}
