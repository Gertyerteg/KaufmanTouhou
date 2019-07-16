using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou.Screens
{
    /// <summary>
    /// A level within the game that is indepedently updated and drawn.
    /// Think of it as a screen within a screen!
    /// </summary>
    public class Stage
    {
        /// <summary>
        /// The contentmanager of the stage.
        /// </summary>
        public ContentManager Content;

        /// <summary>
        /// The players within the game.
        /// </summary>
        public Player[] Players;

        public List<Enemy> Enemies;
        public List<Bullet> Bullets;
        public List<Rocket> Rockets;

        /// <summary>
        /// Creates a new instance of the <c>Stage</c>.
        /// </summary>
        /// <param name="content"></param>
        public Stage(ContentManager content, Player[] players)
        {
            Players = players;
            Content = new ContentManager(content.ServiceProvider, "Content");
            Bullets = new List<Bullet>();
            Enemies = new List<Enemy>();
            Rockets = new List<Rocket>();
        }

        public void AddRocket(Rocket r)
        {
            Rockets.Add(r);
        }

        public void AddEnemy(Enemy e)
        {
            Enemies.Add(e);
        }

        public void AddBullet(Bullet b)
        {
            Bullets.Add(b);
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Player GetPlayer(int index)
        {
            return Players[index];
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Player GetPlayer(PlayerIndex index)
        {
            return GetPlayer((int)index);
        }

        /// <summary>
        /// Indicates whether the stage is finished.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStageFinished()
        {
            return false;
        }

        /// <summary>
        /// Initializes the stage.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Unloads the contentmanager.
        /// </summary>
        public virtual void Unload()
        {
            Content.Unload();
        }

        /// <summary>
        /// The amount of players currently active in the game.
        /// </summary>
        /// <returns></returns>
        public int GetPlayerCount()
        {
            int pl = 0;
            foreach (Player p in Players)
            {
                if (p != null)
                    pl++;
            }

            return pl;
        }

        /// <summary>
        /// Updates the instance of the <c>Stage</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i]?.Update(gameTime);
            }

            for (int i = 0; i < Bullets.Count; i++)
            {
                if (!Bullets[i].IsActive)
                {
                    Bullets.RemoveAt(i--);
                }
                else
                    Bullets[i].Update(gameTime);
            }

            for (int i = 0; i < Rockets.Count; i++)
            {
                if (!Rockets[i].IsActive)
                {
                    Rockets.RemoveAt(i--);
                }
                else
                    Rockets[i].Update(gameTime);
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].IsActive)
                {
                    Enemies[i].Update(gameTime);
                }
                else
                    Enemies.RemoveAt(i--);
            }

        }

        /// <summary>
        /// Pauses the stage.
        /// </summary>
        public virtual void Pause()
        {

        }

        /// <summary>
        /// Draws the <c>Stage</c> to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in Bullets)
                b.Draw(spriteBatch);

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i]?.Draw(spriteBatch);
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw(spriteBatch);
            }

            for (int i = 0; i < Rockets.Count; i++)
            {
                Rockets[i].Draw(spriteBatch);
            }
        }
    }
}
