using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou
{
    /// <summary>
    /// A line of text within the dialogue.
    /// </summary>
    public struct TextD
    {
        public string Text;
        public float Timer;
        public bool isLeft;

        /// <summary>
        /// Creates a new <c>TextD</c>.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="timer"></param>
        /// <param name="left"></param>
        public TextD(string text, float timer, bool left)
        {
            Text = text;
            Timer = timer;
            isLeft = left;
        }
    }

    public class Dialogue : Sprite
    {
        public bool FinishedPlaying
        {
            get { return !isPlaying; }
        }
        public const float INTERVAL = 700f;
        public Queue<TextD> texts;
        private TextD currentText;
        private float timer, timer2, timer3;
        private bool isPlaying;
        private Texture2D left, right;
        private SpriteFont font;

        /// <summary>
        /// Creates a new instance of the <c>Dialogue</c>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="font"></param>
        public Dialogue(Texture2D left, Texture2D right, SpriteFont font)
        {
            this.font = font;
            timer = timer2 = timer3 = 0f;
            this.left = left;
            this.right = right;
            isPlaying = false;
            currentText = new TextD("", 0f, true);
            texts = new Queue<TextD>();
            isPlaying = true;
        }

        /// <summary>
        /// Enqueues a line of text for the dialogue.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="left"></param>
        /// <param name="timer"></param>
        public void AddText(string text, float timer, bool left)
        {
            TextD td = new TextD(text, timer, left);
            texts.Enqueue(td);
        }

        /// <summary>
        /// Updates the dialogue.
        /// </summary>
        /// <param name="gameTime"></param>
        public bool Update(GameTime gameTime)
        {
            if (timer2 < INTERVAL)
            {
                timer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer3 > INTERVAL)
                {
                    return false;
                }

                if (timer >= currentText.Timer)
                {
                    timer = 0f;

                    if (texts.Count == 0)
                    {
                        isPlaying = false;
                        currentText = new TextD("", 0f, false);
                        timer3 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    else
                    {
                        currentText = texts.Dequeue();
                    }

                }
                    
            }

            return true;
        }

        /// <summary>
        /// Draws the dialogue to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager s = ScreenManager.GetInstance();

            float progress = (isPlaying) ? timer2 / INTERVAL : 1f - timer3 / INTERVAL;
            float factor = 1.5f;
            // draws the dialogues to the screen
            if (left != null)
            {
                float scale = s.Height / (float)left.Height / factor;
                int width = (int)(left.Width * scale);
                spriteBatch.Draw(left, new Rectangle((int)(progress * width) / 2, s.Height / 2, width, (int)(left.Height * scale)), null,
                    Color.White * progress, 0f, new Vector2(left.Width / 2f, left.Height / 2f), SpriteEffects.None, 0f);
            }
            if (right != null)
            {
                float scale = s.Height / (float)right.Height / factor;
                int width = (int)(right.Width * scale);
                spriteBatch.Draw(right, new Rectangle(s.Width - (int)(progress * width) / 2, s.Height / 2, width, (int)(right.Height * scale)), null,
                    Color.White * progress, 0f, new Vector2(right.Width / 2f, right.Height / 2f), SpriteEffects.None, 0f);
            }

            // draws the text to the screen
            if (currentText.isLeft)
            {
                spriteBatch.DrawString(font, currentText.Text, new Vector2(32, s.Height - 200), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, currentText.Text, 
                    new Vector2(s.Width - font.MeasureString(currentText.Text).X - 32, s.Height - 200), Color.White);
            }
        }
    }
}
