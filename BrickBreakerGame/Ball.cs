using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace BrickBreakerGame
{
    public class Ball : GameObject
    {
        float speed = 1f;
        Vector2 direction = new Vector2(0f, -1f);

        int colImmunityFrames = 0;

        public Ball(Game game) : base(game)
        {
            textureName = "textures/ball";
        }

        public override void Update(float deltaTime)
        {
            if (game.paddle.ballReleased)
            {
                position += direction * speed * deltaTime;
            }
            else
            {
                position = new Vector2(game.paddle.position.X, game.paddle.position.Y - game.paddle.height);
            }

            CheckCollision();

            base.Update(deltaTime);
        }

        private void CheckCollision()
        {
            //If in collision immunity frames, decrement by 1 and return from the function
            if (colImmunityFrames > 0)
            {
                colImmunityFrames--;
                return;
            }

            float radius = texture.Width / 2;
            float paddleZone = game.paddle.width / 5;

            //Handle paddle collision
            if ((position.X > (game.paddle.position.X - radius - game.paddle.width / 2)) &&
                (position.X < (game.paddle.position.X + radius + game.paddle.width / 2)) &&
                (position.Y < game.paddle.position.Y) &&
                (position.Y > (game.paddle.position.Y - radius - game.paddle.height / 2)))
            {
                //Reflect ball at different angles depending on where the ball hit the paddle
                float paddleDistance = game.paddle.width + radius * 2;
                float ballLocation = position.X - (game.paddle.position.X - radius - game.paddle.width / 2);
                float paddlePercent = ballLocation / paddleDistance;

                if (paddlePercent < 0.1f)
                    direction = Vector2.Reflect(direction, new Vector2(-0.35f, -0.981f));
                else if (paddlePercent > 0.1f && paddlePercent < 0.2f)
                    direction = Vector2.Reflect(direction, new Vector2(-0.25f, -0.981f));
                else if (paddlePercent > 0.2f && paddlePercent < 0.4f)
                    direction = Vector2.Reflect(direction, new Vector2(-0.2f, -0.981f));
                else if (paddlePercent > 0.4f && paddlePercent < 0.5f)
                    direction = Vector2.Reflect(direction, new Vector2(-0.1f, -0.981f));
                else if (paddlePercent == 0.5f && paddlePercent < 0.6f)
                    direction = Vector2.Reflect(direction, new Vector2(0f, -0.981f));
                else if (paddlePercent > 0.5f && paddlePercent < 0.6f)
                    direction = Vector2.Reflect(direction, new Vector2(0.1f, -0.981f));
                else if (paddlePercent > 0.6f && paddlePercent < 0.8f)
                    direction = Vector2.Reflect(direction, new Vector2(0.2f, -0.981f));
                else if (paddlePercent > 0.8f && paddlePercent < 0.9f)
                    direction = Vector2.Reflect(direction, new Vector2(0.25f, -0.981f));
                else if (paddlePercent > 0.9f)
                    direction = Vector2.Reflect(direction, new Vector2(0.35f, -0.981f));

                colImmunityFrames = 1; //Set 1 frame of collision detection immunity
            }

            //Handle brick collision
            foreach (Brick b in game.level.brick)
            {
                if ((position.X > (b.position.X - radius - b.width / 2)) &&
                    (position.X < (b.position.X + radius + b.width / 2)) &&
                    (position.Y < b.position.Y + radius + b.height / 2) &&
                    (position.Y > (b.position.Y - radius - b.height / 2)))
                {
                    if ((position.Y < (b.position.Y - b.height / 2)) ||
                        (position.Y > (b.position.Y + b.height / 2)))
                            direction = new Vector2(direction.X, -direction.Y);
                    else
                        direction = new Vector2(-direction.X, direction.Y);

                    game.RemoveBrick(b);

                    break;
                }
            }

            //Handle wall collision
            if ((Math.Abs(position.X) < radius) || (Math.Abs(position.X - game.virtualWidth) < radius)) //Left and right wall
            {
                direction = new Vector2(-direction.X, direction.Y);
                colImmunityFrames = 1;
            }
            else if (Math.Abs(position.Y) < radius) //Top wall collision
            {
                direction = new Vector2(direction.X, -direction.Y);
                colImmunityFrames = 1;
            }
            else if (Math.Abs(position.Y - game.virtualHeight) < radius) //Bottom wall
                game.LoseBall();
        }
    }
}
