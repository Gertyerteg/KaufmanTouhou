using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KaufmanTouhou.Sprites
{
    public class LinearBullet : Bullet
    {
        public LinearBullet(EntitySide side, float ttl, Vector2 velocity) : base(side, ttl)
        {
            Velocity = velocity;
            Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
