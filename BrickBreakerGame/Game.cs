using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrickBreakerGame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont spriteFont;

        public int displayWidth, displayHeight, virtualWidth, virtualHeight;

        public Level level;
        public Paddle paddle;
        public Ball ball;

        int numLives;
        string lives;

        int numScore;
        string score;

        public int levelNum = 1;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = true;

#if DEBUG
            graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
#else
            graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#endif
        }

        protected override void Initialize()
        {
            displayWidth = graphics.GraphicsDevice.Viewport.Width;
            displayHeight = graphics.GraphicsDevice.Viewport.Height;

            virtualWidth = 1920;
            virtualHeight = 1080;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Content
            spriteFont = Content.Load<SpriteFont>("fonts/main");

            level = new Level(this, "level1");
            level.LoadContent();

            paddle = new Paddle(this);
            paddle.LoadContent();
            paddle.position = new Vector2((virtualWidth / 2), (virtualHeight - (virtualHeight / 10)));

            ball = new Ball(this);
            ball.LoadContent();
            ball.position = new Vector2(paddle.position.X, paddle.position.Y - 75f);

            numLives = 3;
            lives = "Lives: " + numLives.ToString();

            numScore = 0;
            score = "Score: " + numScore.ToString();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            paddle.Update(deltaTime);
            ball.Update(deltaTime);

            if (level.brick.Count == 0)
            {
                levelNum++;
                LoadLevel(levelNum);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Set up virtual resolution for proper sprite scaling
            var scaleX = (float)displayWidth / virtualWidth;
            var scaleY = (float)displayHeight / virtualHeight;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            spriteBatch.Begin(transformMatrix: matrix);

            level.Draw(spriteBatch);
            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            spriteBatch.DrawString(spriteFont, lives, new Vector2(10, 10), Color.Yellow);
            spriteBatch.DrawString(spriteFont, score, new Vector2(virtualWidth - (score.Length * 12), 10), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void LoseBall()
        {
            numLives--;

            if (numLives > -1)
            {
                lives = "Lives: " + numLives.ToString();

                paddle.position = new Vector2((virtualWidth / 2), (virtualHeight - (virtualHeight / 10)));

                ball = new Ball(this);
                ball.LoadContent();

                ball.position = new Vector2(paddle.position.X, paddle.position.Y - 75f);

                paddle.ballReleased = false;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Game over!", "Brick Breaker Game", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                LoadContent();
            }
        }

        public void RemoveBrick(Brick b)
        {
            if (b.Color == Color.White)
                numScore += 10;
            else if (b.Color == Color.Green)
                numScore += 20;
            else if (b.Color == Color.Blue)
                numScore += 30;
            else if (b.Color == Color.Yellow)
                numScore += 40;
            else if (b.Color == Color.Red)
                numScore += 50;
            else if (b.Color == Color.Purple)
                numScore += 100;

            score = "Score: " + numScore;

            level.brick.Remove(b);
        }

        public void LoadLevel(int levelNum)
        {
            paddle.ballReleased = false;

            ball = new Ball(this);
            ball.LoadContent();
            ball.position = new Vector2(paddle.position.X, paddle.position.Y - 75f);

            level = new Level(this, "level" + levelNum);
            level.LoadContent();            
        }
    }
}
