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
    public class MiniSun : Enemy
    {
        private float gunTimer;
        private Random rand;
        public MiniSun(Player[] players) : base(players)
        {
            rand = new Random();
        }

        
        public override void Update(GameTime gameTime)
        {
            Player p = GetNearestPlayer();
            gunTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (p != null && Vector2.Distance(p.Position, Position) > 300)
            {
                float a = GetAngleBetweenSprite(p);
                Velocity = new Vector2((float)Math.Cos(a), (float)Math.Sin(a)) * 400f;
            }

            if (p != null && gunTimer > 600)
            {
                gunTimer = 0;
                float a = GetAngleBetweenSprite(p);

                Vector2 vel = new Vector2((float)Math.Cos(a), (float)Math.Sin(a)) * (float)(250f + rand.NextDouble() * 100);

                Bullet b = new LinearBullet(EntitySide.ENEMY, 3000f, vel)
                {
                    Texture = BulletTexture,
                    Size = new Point(16, 16),
                    Position = Position,
                    Color = Color.White,
                };
                Bullets.Add(b);
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity = Vector2.Zero;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Texture, new Rectangle(Position.ToPoint(), Size), null, 
                Color.White, 0f, Origin, SpriteEffects.None, 0f);
        }
    }
}
