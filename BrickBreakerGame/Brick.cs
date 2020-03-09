using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BrickBreakerGame
{
    public class Brick : GameObject
    {
        public Color Color = Color.White;

        public Brick(Game game, string color) : base(game)
        {
            textureName = "textures/brick";

            switch (color.ToLower())
            {
                case "r": Color = Color.Red;
                    break;
                case "g": Color = Color.Green;
                    break;
                case "b": Color = Color.Blue;
                    break;
                case "y": Color = Color.Yellow;
                    break;
                case "p": Color = Color.Purple;
                    break;
                default: Color = Color.White;
                    break;
            }
        }
    }
}
