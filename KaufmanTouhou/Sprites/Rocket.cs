using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou.Sprites
{
    public class Rocket : Sprite
    {
        private List<Sprite> sprites;
        private Sprite target;

        /// <summary>
        /// The texture of the explosion
        /// </summary>
        public Texture2D Explosion
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            private set;
        }

        private float explosionTimer;
        private float noTargetTimer;
        public bool IsExploded
        {
            get;
            private set;
        }
        private Texture2D blank;
        private readonly float RADIUS;
        private SoundEffect launch, impact;

        /// <summary>
        /// Creates a new instance of the <c>Rocket</c>.
        /// </summary>
        public Rocket(List<Sprite> s, Texture2D blank, SoundEffect launch, SoundEffect impact)
        {
            this.launch = launch;
            this.impact = impact;

            RADIUS = 40f;
            this.blank = blank;
            sprites = s;
            IsActive = true;
            Rotation = MathHelper.PiOver2;

            launch.Play();
        }

        private const float SAFE_MULT = 1f;
        /// <summary>
        /// Updates the logic of the Rocket.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            Rotation = getAngleToNearestTarget();
            if (explosionTimer > 400f)
                IsActive = false;
            if (IsExploded)
            {
                explosionTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
                Velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 900;
            if (target != null)
            {
                noTargetTimer = 0;
                float targetDist = Vector2.Distance(target.Position, Position);
                // temporary constant
                if (!IsExploded && targetDist < RADIUS)
                {
                    IsExploded = true;
                    Console.WriteLine("Rocket exploded!");
                    impact.Play();
                    // inflict damage to surrounding sprites
                    foreach (Sprite s in sprites)
                    {
                        float sDist = Vector2.Distance(s.Position, Position);
                        float mag = (float)Math.Sqrt(Math.Pow(Size.X, 2) + Math.Pow(Size.Y, 2));
                        float playerMag = RADIUS;
                        if (sDist < (mag + playerMag) * SAFE_MULT)
                        {
                            s.Health -= 1;
                            return;
                        }
                    }
                }
            }
            else
            {
                noTargetTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (noTargetTimer > 1900f)
                    IsActive = false;
            }
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity = Vector2.Zero;
        }

        private float getAngleToNearestTarget()
        {
            target = GetNearestTarget();
            if (target == null)
                return -MathHelper.PiOver2;
            Vector2 proj = target.Position - Position;

            return (float)Math.Atan2(proj.Y, proj.X);
        }

        private Sprite GetNearestTarget()
        {
            Sprite nearest = null;
            float nearestDist = 0;
            foreach(Sprite s in sprites)
            {
                if (nearest == null)
                {
                    nearest = s;
                    nearestDist = Vector2.Distance(nearest.Position, Position);
                }
                else
                {
                    float dist = Vector2.Distance(s.Position, Position);

                    if (dist < nearestDist)
                    {
                        nearest = s;
                        nearestDist = dist;
                    }
                }
            }

            return nearest;
        }

        /// <summary>
        /// Draws the rocket to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Point p = Position.ToPoint();
            Rectangle drawRect = new Rectangle(p.X, p.Y, Size.X, Size.Y);
            if (IsExploded && IsActive)
            {
                //float prog = explosionTimer / 800f;
                //spriteBatch.Draw(blank, new Rectangle(p.X, p.Y, (int)(2 * RADIUS), (int)(2 * RADIUS)),
                //    null, Color.White * 0.8f, -prog * MathHelper.Pi / 15f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                //spriteBatch.Draw(blank, new Rectangle(p.X, p.Y, (int)(2 * RADIUS), (int)(2 * RADIUS)),
                //    null, Color.White * 0.8f, MathHelper.PiOver4 + prog * MathHelper.Pi / 10f,
                //    new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
                int frame = (int)(explosionTimer / 400f * 5);
                Rectangle sourceRect = new Rectangle(12 * frame, 0, 12, 12);
                Rectangle dRect = new Rectangle(p.X, p.Y, (int)(3 * RADIUS), (int)(3 * RADIUS));
                spriteBatch.Draw(Explosion, dRect, sourceRect, Color.White, 0f, new Vector2(Explosion.Width / 10f, Explosion.Height / 10f), SpriteEffects.None, 0f);
            }
            else if (!IsExploded)
            {
                spriteBatch.Draw(Texture, drawRect, null, Color, Rotation, Origin, SpriteEffects.None, 0f);
            }
        }
    }
}
