using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KaufmanTouhou.Sprites.Bullet;

namespace KaufmanTouhou.Sprites
{
    public class Enemy : Sprite
    {
        public bool IsActive
        {
            get;
            set;
        }

        public Texture2D BulletTexture
        {
            get;
            set;
        }

        private Player[] players;
        public List<Bullet> Bullets;
        /// <summary>
        /// Creates a new instance of the <c>Enemy</c>.
        /// </summary>
        public Enemy(Player[] players)
        {
            this.players = players;
            IsActive = true;
        }

        public void SetBullets(List<Bullet> bullets)
        {
            this.Bullets = bullets;
        }

        public List<Bullet> getBullets()
        {
            return Bullets;
        }

        public Player GetPlayer(int index)
        {
            return players[index];
        }

        public Player GetNearestPlayer()
        {
            Player p = players[0];
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && p != players[i] && 
                    Vector2.Distance(p.Position, Position) 
                    > Vector2.Distance(Position, players[i].Position))
                {
                    p = players[i];
                }
            }

            return p;

        }

        public float GetAngleBetweenSprite(Sprite sprite)
        {
            return (float)Math.Atan2(sprite.Position.Y - Position.Y, sprite.Position.X - Position.X);
        }

        public const float SAFE_MULT = 0.5f;
        public void CheckBulletCollision()
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullet b = Bullets[i];
                if (!b.Side.Equals(EntitySide.ENEMY))
                {
                    float mag = (float)Math.Sqrt(Math.Pow(b.Size.X, 2) + Math.Pow(b.Size.Y, 2));
                    float playerMag = (float)Math.Sqrt(Math.Pow(Size.X / 2, 2) + Math.Pow(Size.Y / 2, 2));

                    if (Vector2.Distance(b.Position, Position) < (mag + playerMag) * SAFE_MULT)
                    {
                        Health--;
                        Console.WriteLine("Enemy hit by enemy bullet, HP: " + Health);
                        Bullets.RemoveAt(i--);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the logic of the enemy.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            CheckBulletCollision();
            if (Health <= 0)
                IsActive = false;
        }

        /// <summary>
        /// Draws the enemy to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
