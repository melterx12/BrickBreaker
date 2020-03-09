using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BrickBreakerGame
{
    public class Paddle : GameObject
    {
        float speed = 1f;
        public bool ballReleased = false;

        public Paddle(Game game) : base(game)
        {
            textureName = "textures/paddle";
        }

        public override void Update(float deltaTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= speed * deltaTime;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += speed * deltaTime;
            }

            //Clamp paddle position to the screen
            position.X = MathHelper.Clamp(position.X, (texture.Width / 2) + 5, game.virtualWidth - (texture.Width / 2) - 5);

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !ballReleased)
            {
                ballReleased = true;
            }

            base.Update(deltaTime);
        }
    }
}
