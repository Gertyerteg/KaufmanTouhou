using KaufmanTouhou.Screens;
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
    /// An entity within the game world that has a texture, velocity, and position.
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// The tint at which the texture is drawn at.
        /// </summary>
        public Color Color
        {
            get;
            set;
        }
        /// <summary>
        /// The scale factor of the <c>Sprite</c>.
        /// </summary>
        public const int SCALE = 5;

        /// <summary>
        /// The texture of the <c>Sprite</c>.
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// The vector rate of change of the position.
        /// </summary>
        public Vector2 Velocity
        {
            get;
            set;
        }
        /// <summary>
        /// The current stage of the game.
        /// </summary>
        public Stage CurrentStage
        {
            get { return GameScreen.CurrentStage; }
        }

        /// <summary>
        /// Positional vector for the <c>Sprite</c>.
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }
    
        /// <summary>
        /// The origin point relative to the texture in which the <c>Sprite</c> is drawn and rotated at.
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Texture.Width / 2f, Texture.Height / 2f); }
        }

        public float Rotation
        {
            get;
            set;
        }

        public int Health
        {
            get;
            set;
        }

        /// <summary>
        /// The size of the sprite drawn.
        /// </summary>
        public Point Size
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of the <c>Sprite</c>.
        /// </summary>
        public Sprite()
        {
            Health = 1;
            Color = Color.White;
            Rotation = 0f;
        }
    }
}
