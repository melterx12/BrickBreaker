using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreakerGame
{
    public class Level : GameObject
    {
        int rows;
        int columns;
        public string levelName;
        public List<Brick> brick;

        public Level(Game game, string levelName) : base(game)
        {
            textureName = "textures/brick";
            this.levelName = levelName;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            loadLevel(levelName);
        }

        private void loadLevel(string levelName)
        {
            string path = Environment.CurrentDirectory + @"\levels\" + levelName + ".dat";

            if (File.Exists(path))
            {
                string[] data = File.ReadAllText(path).Split(',');

                rows = 1;
                columns = 0;

                bool numColumnsDetermined = false;

                //Determine number of rows and columns in the level data
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Contains("\r\n"))
                    {
                        rows++;
                        data[i] = data[i].Replace("\r\n", "");
                        numColumnsDetermined = true;
                    }

                    if (!numColumnsDetermined)
                        columns++;
                }

                brick = new List<Brick>();

                //Loop through rows and columns to generate list of brick objects
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < columns; c++)
                    {
                        if (data[(columns * r) + c] != ".")
                        {
                            brick.Add(new Brick(game, data[(columns * r) + c])
                            {
                                width = texture.Width,
                                height = texture.Height,
                                position = new Vector2((width/2) + c * width, (height/2) + r * height)
                            });
                        }
                    }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error: Failed to load " + levelName + "!", "Brick Breaker Game", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                game.Exit();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                foreach (Brick b in brick)
                {
                    Vector2 drawPosition = b.position;
                    drawPosition.X -= texture.Width / 2;
                    drawPosition.Y -= texture.Height / 2;
                    spriteBatch.Draw(texture, drawPosition, b.Color);
                }
            }
        }
    }
}
