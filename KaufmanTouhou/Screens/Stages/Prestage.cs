using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KaufmanTouhou.Screens.Stages
{
    public class Prestage : Stage
    {
        private Texture2D[] textures;
        private string[] subtitles;
        private SpriteFont font;
        private float timer;
        public Prestage(ContentManager content, Player[] players) : base(content, players)
        {
            font = content.Load<SpriteFont>("DialogueFont");
            textures = new Texture2D[2];
            subtitles = new string[5];
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = content.Load<Texture2D>("Scene" + i);
            }

            subtitles[0] = "Exiled for being evil, the Lich Queen has been spreading havoc with her robot army.";
            subtitles[1] = "During an invasion of the kingdom, the Lich Queen has kidnapped the Princess";
            subtitles[2] = "The King has sent you, Sir HandsomeMcHandsome, to rescue her.";
            subtitles[3] = "He warns you of the three evil henchmen: the Corrupted Sun, the Asteroidbot, and the Evil Spinner";
            subtitles[4] = "Your pet mates, Corey and Chippy, join you on your quest.";
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //base.Update(gameTime);
            for (int i = 0; i < 4; i++)
            {
                if (InputManager.Instance.IsButtonPressed(Buttons.A, i))
                {
                    SetStage(StageNumber + 1);
                }
            }

            if (StageNumber > 4)
            {
                ScreenManager.GetInstance().ChangeScreen(ScreenState.READY);
                MediaPlayer.Stop();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            float opacity = (float)(Math.Cos(timer / 600f) * 0.3f + 0.3f) + 0.4f;
            if (textures.Length > StageNumber && textures[StageNumber] != null)
                spriteBatch.Draw(textures[StageNumber], new Rectangle(0, 0, 
                    ScreenManager.GetInstance().Width, ScreenManager.GetInstance().Height), Color.White);
            Vector2 orig = font.MeasureString(subtitles[StageNumber]) / 2;
            Vector2 pos = new Vector2(ScreenManager.GetInstance().Width / 2, ScreenManager.GetInstance().Height - orig.Y * 4);
            spriteBatch.DrawString(font, subtitles[StageNumber], pos + new Vector2(4, 4), Color.Black * 0.4f * opacity, 0f, orig, 1, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, subtitles[StageNumber], pos, Color.White * opacity, 0f, orig, 1, SpriteEffects.None, 0f);
        }
    }
}
