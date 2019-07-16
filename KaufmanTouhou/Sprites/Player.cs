using KaufmanTouhou.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static KaufmanTouhou.Sprites.Bullet;

namespace KaufmanTouhou.Sprites
{
    /// <summary>
    /// Corresponding player colors to each player number.
    /// </summary>
    public static class PlayerColors
    {
        public static Color PlayerOne = new Color(0, 0, 255);
        public static Color PlayerTwo = new Color(255, 0, 0);
        public static Color PlayerThree = new Color(0, 255, 0);
        public static Color PlayerFour = new Color(255, 255, 0);
    }

    /// <summary>
    /// A player within the game world.
    /// </summary>
    public class Player : Sprite
    {
        private PlayerIndex index;
        private List<Bullet> bullets
        {
            get { return CurrentStage.Bullets; }
        }
        public SoundEffect rocketLaunch, rocketImpact, shootSound;
        private List<SoundEffect> hurtSounds;
        public Texture2D BulletTexture, RocketTexture, pointerTexture, blank, Explosion;
        private float bulletTimer, shieldTimer, rocketTimer, shieldRechargeTimer, invulnTimer;
        private bool shieldActive
        {
            get { return shieldTimer > 0f; }
        }
        public Color PointerColor
        {
            get;
            private set;
        }
        private List<Rocket> rockets;
        private Random rand;
        public const float INVULNERABILITY_TIMESTAMP = 800f;

        /// <summary>
        /// Creates a new instance of the <c>Player</c>.
        /// </summary>
        /// <param name="index"></param>
        public Player(PlayerIndex index, Texture2D pointerTexture, Texture2D blank, List<SoundEffect> hurtEffects)
        {
            rand = new Random();
            hurtSounds = hurtEffects;
            this.blank = blank;
            this.index = index;
            Health = 0;
            invulnTimer = 0f;
            this.pointerTexture = pointerTexture;

            switch (index)
            {
                case PlayerIndex.One:
                    PointerColor = PlayerColors.PlayerOne;
                    break;
                case PlayerIndex.Two:
                    PointerColor = PlayerColors.PlayerTwo;
                    break;
                case PlayerIndex.Three:
                    PointerColor = PlayerColors.PlayerThree;
                    break;
                case PlayerIndex.Four:
                    PointerColor = PlayerColors.PlayerFour;
                    break;
            }
        }
        public void SetRockets(List<Rocket> rockets)
        {
            this.rockets = rockets;
        }

        /// <summary>
        /// The speed of which the player traverses through space.
        /// </summary>
        public const float SPEED = 400;

        /// <summary>
        /// Updates the <c>Player</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            invulnTimer -= dt;
            rocketTimer -= dt;

            InputManager im = InputManager.Instance;
            Vector2 velocity = GamePad.GetState(index).ThumbSticks.Left;
            velocity.Y *= -1;
            if (ScreenManager.DEV_MODE)
            {
                if (im.KeyDown(Keys.W))
                {
                    velocity.Y = -1;
                }
                else if (im.KeyDown(Keys.S))
                {
                    velocity.Y = 1;
                }

                if (im.KeyDown(Keys.A))
                {
                    velocity.X = -1;
                }
                else if (im.KeyDown(Keys.D))
                {
                    velocity.X = 1;
                }
            }
            if (ScreenManager.DEV_MODE && velocity.Length() != 0)
                velocity.Normalize();
            bulletTimer += dt;
            shieldTimer -= dt;
            shieldRechargeTimer -= dt;
            Velocity = velocity;
            Velocity *= SPEED;

            if (invulnTimer <= 0)
            {
                CheckBulletCollision();
            }
            else
            {
                float vMag = Math.Max(0, (invulnTimer - INVULNERABILITY_TIMESTAMP / 2) / INVULNERABILITY_TIMESTAMP);
                GamePad.SetVibration(index, vMag, vMag);
            }

            if (shieldTimer < 0f && shieldRechargeTimer <= 0 && im.IsButtonDown(Buttons.A, (int)index) 
                || (ScreenManager.DEV_MODE && im.KeyPressed(Keys.Q)))
            {
                shieldTimer = 3000f;
                shieldRechargeTimer = shieldTimer * 2f;
            }

            // Shoots bullets
            if (bulletTimer > 450f && (im.IsButtonDown(Buttons.RightTrigger, (int)index) 
                || (ScreenManager.DEV_MODE && im.KeyDown(Keys.Space))))
            {
                //int shootFx = rand.Next(0, 3);

                shootSound.Play(1f, 0f, 0f);
                bulletTimer = 0;
                SinBullet b = new SinBullet(EntitySide.PLAYER, 7000f, 800, true, true);
                SinBullet b2 = new SinBullet(EntitySide.PLAYER, 7000f, 800, true, false);
                LinearBullet b3 = new LinearBullet(EntitySide.PLAYER, 7000f, new Vector2(0, -800));
                Vector2 offsetVel = Velocity / 4f;
                b.Texture = BulletTexture;
                b.Size = new Point(8, 7);
                b.Position = Position;
                b.InitVelocity = offsetVel;
                b.Color = Color.SkyBlue;
                b2.Texture = BulletTexture;
                b2.Size = new Point(8, 7);
                b2.InitVelocity = offsetVel;
                b2.Position = Position;
                b2.Color = Color.SkyBlue;
                b3.Texture = BulletTexture;
                b3.InitVelocity = offsetVel;
                b3.Size = new Point(8, 8);
                b3.Position = Position;
                b3.Color = Color.SkyBlue;

                bullets.Add(b);
                bullets.Add(b2);
                bullets.Add(b3);
            };
            // Fire rocket
            if (im.IsButtonDown(Buttons.LeftTrigger, (int)index) && rocketTimer < 0)
            {
                rocketTimer = 900f;
                List<Sprite> sprites = new List<Sprite>();
                sprites.AddRange(GameScreen.CurrentStage.Enemies);
                Rocket r = new Rocket(sprites, blank, rocketLaunch, rocketImpact)
                {
                    Texture = RocketTexture,
                    Explosion = Explosion,
                    Color = Color.White,
                    Position = Position,
                    Size = new Point(RocketTexture.Width, RocketTexture.Height),
                };
                CurrentStage.AddRocket(r);
            }
            // inverts the y-axis to be NOT INVERTED

            if (Velocity.Length() > 0)
                Rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            UpdatePosition(gameTime);
        }

        private const float SAFE_MULT = 0.18f;

        /// <summary>
        /// Checks the list of bullets for bullet collision.
        /// </summary>
        public void CheckBulletCollision()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                if (!b.Side.Equals(EntitySide.PLAYER))
                {
                    float mag = (float)Math.Sqrt(Math.Pow(b.Size.X, 2) + Math.Pow(b.Size.Y, 2));
                    float playerMag = (float)Math.Sqrt(Math.Pow(Texture.Width, 2) + Math.Pow(Texture.Height, 2)) * SCALE;

                    if (Vector2.Distance(b.Position, Position) < (mag + playerMag) * SAFE_MULT)
                    {
                        if (!shieldActive)
                        {
                            int sel = rand.Next(0, hurtSounds.Count);
                            hurtSounds[sel].Play(0.5f, 0f, 0f);
                            Health--;
                            invulnTimer = INVULNERABILITY_TIMESTAMP;
                        }

                        Console.WriteLine("Player hit by enemy bullet, HP: " + Health);
                        bullets.RemoveAt(i--);

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the position and physics of the <c>Player</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdatePosition(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ClampPosition(ScreenManager.GetInstance().Width, ScreenManager.GetInstance().Height);
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Clamps the position of the player to be within the game world (given bounds).
        /// </summary>
        public void ClampPosition(int width, int height)
        {
            float x = Math.Min(width, Math.Max(0, Position.X));
            float y = Math.Min(height, Math.Max(0, Position.Y));
            Position = new Vector2(x, y);
        }

        /// <summary>
        /// Draws the player to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            float opacity = 1.4f - invulnTimer / INVULNERABILITY_TIMESTAMP;
            Rectangle drawRect = new Rectangle((int)Position.X,
                (int)Position.Y, Texture.Width * SCALE, Texture.Height * SCALE);
            spriteBatch.Draw(Texture, drawRect, 
                null, Color * opacity, 0f, Origin, SpriteEffects.None, 0f);

            spriteBatch.Draw(pointerTexture, new Rectangle(drawRect.X, drawRect.Y + 70,
                pointerTexture.Width * 5, pointerTexture.Height * 5), null, PointerColor,
                0f, new Vector2(pointerTexture.Width / 2f, pointerTexture.Height / 2f), 
                SpriteEffects.None, 0f);

            if (shieldActive)
                spriteBatch.Draw(blank, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width * 3 / 2, drawRect.Height * 3 / 2),
                    null,Color.Blue * 0.3f, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }
    }
}
