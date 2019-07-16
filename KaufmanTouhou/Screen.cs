using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaufmanTouhou
{
    /// <summary>
    /// An instance for the <c>ScreenManager</c> that holds a specific role.
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// Updates the logic of the <c>Screen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Called when the <c>Screen</c> is about to be switched or the game is closing.
        /// </summary>
        public virtual void Unload()
        {

        }

        /// <summary>
        /// Initializes the <c>Screen</c>.
        /// </summary>
        /// <param name="Content"></param>
        public virtual void LoadContent(ContentManager Content)
        {

        }

        /// <summary>
        /// Draws the contents of the <c>Screen</c> to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphics"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {

        }
    }

    public enum ScreenState
    {
        GAME,
        MENU,
        READY,
        AFTER,
    }
}
