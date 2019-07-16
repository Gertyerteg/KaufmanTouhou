using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KaufmanTouhou.Sprites
{
    public class DiagonalEnemy : Enemy
    {
        private float ttl;
        public DiagonalEnemy(Player[] players, float ttl) : base(players)
        {
            this.ttl = ttl;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ttl -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ttl <= 0)
                IsActive = false;
        }
    }
}
