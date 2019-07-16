using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou.Sprites
{
    public class LichBoss : Enemy
    {
        private Rectangle sourceRectangle;
        private float timer, timer2;
        private int frameNumber;
        private bool isBlinking, isAppearing;
        private Rectangle drawRectangle;
        private readonly float MAX_HEALTH;
        private Texture2D blank;
        private Rectangle hitbox;
        private Vector2 orig;
        public LichBoss(Player[] players, float health) : base(players)
        {
            hitbox = new Rectangle(0, 0, ScreenManager.GetInstance().Width, ScreenManager.GetInstance().Height / 5);
            MAX_HEALTH = health;
            Health = (int)health;
            Position = new Vector2(ScreenManager.GetInstance().Width / 2f, -800);
            isAppearing = true;
            drawRectangle = new Rectangle((int)Position.X, (int)Position.Y,
                ScreenManager.GetInstance().Width, ScreenManager.GetInstance().Height);
            ContentManager Content = CurrentStage.Content;
            blank = Content.Load<Texture2D>("Blank");
            Texture = Content.Load<Texture2D>("LichKing");
            sourceRectangle = new Rectangle(Texture.Width / 9 * frameNumber, 0, Texture.Width / 9, Texture.Height);
            orig = new Vector2(Texture.Width / 9f / 2f, Texture.Height / 4);
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timer += dt;
            sourceRectangle = new Rectangle(Texture.Width / 9 * frameNumber, 0, Texture.Width / 9, Texture.Height);
            if (timer > 800)
            {
                timer = 0;
                isBlinking = true;
            }

            if (isBlinking)
            {
                if (timer > 200)
                {
                    timer = 0;
                    frameNumber++;
                }

                if (frameNumber > 8)
                    isBlinking = false;
            }
            else
                frameNumber = 0;

            if (isAppearing)
            {
                Position = new Vector2(Position.X, Position.Y + 4);

                isAppearing = Position.Y < 0;
            }
            else
            {
                timer2 += dt;
                Position = new Vector2(0, (float)Math.Cos(timer2 / 700f) * 40 - 40);
            }
            drawRectangle = new Rectangle((int)Position.X, (int)Position.Y, drawRectangle.Width, drawRectangle.Height);

            //base.Update(gameTime);
            CheckCollisionRect();
        }

        public void CheckCollisionRect()
        {
            for (int i = 0; i < CurrentStage.Bullets.Count; i++)
            {
                Bullet b = CurrentStage.Bullets[i];

                if (b.GetDrawRectangle().Intersects(hitbox))
                {
                    CurrentStage.Bullets.RemoveAt(i--);
                    Health--;
                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Texture, drawRectangle, sourceRectangle, Color.White, 0f, orig, SpriteEffects.None, 0f);

            // draws the health bar
            float prog = Health / MAX_HEALTH;
            int width = (int)(ScreenManager.GetInstance().Width * 3 / 4 * prog);
            Rectangle healthRect = new Rectangle(ScreenManager.GetInstance().Width / 2, 32, width, 32);
            spriteBatch.Draw(blank, healthRect, null, Color.Red, 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);

        }
    }
}
