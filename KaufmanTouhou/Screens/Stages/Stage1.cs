using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaufmanTouhou.Sprites;
using Microsoft.Xna.Framework.Content;

namespace KaufmanTouhou.Screens.Stages
{
    /// <summary>
    /// The first level of the game.
    /// </summary>
    public class Stage1 : Stage
    {
        /// <summary>
        /// Creates a new instance of Stage1.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="players"></param>
        public Stage1(ContentManager content, Player[] players) : base(content, players)
        {
        }
    }
}
