using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TerrainAlgorithm
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D box;
        

        int[,] terrain;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
            IsMouseVisible = true;

            terrain = new int[width / 50, height / 50];
            for (int i = 0; i < width / 50; i++)
            {
                for (int j = 0; j < height / 50; j++)
                {
                    terrain[i, j] = 0;
                }
            }
            Terrain();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            box = Content.Load<Texture2D>("box");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = 0; i < GraphicsDevice.Viewport.Width / 50; i++)
            {
                for (int j = 0; j < GraphicsDevice.Viewport.Height / 50; j++)
                {
                    if (terrain[i,j] == -3)
                    {
                        spriteBatch.Draw(box, new Vector2(i * 50, j * 50), Color.Black);
                    }
                    else if (terrain[i, j] == -2)
                    {
                        spriteBatch.Draw(box, new Vector2(i * 50, j * 50), Color.Gray);
                    }
                    else if (terrain[i, j] == -1)
                    {
                        spriteBatch.Draw(box, new Vector2(i * 50, j * 50), Color.LightGray);
                    }
                    else if (terrain[i, j] == 0)
                    {
                        spriteBatch.Draw(box, new Vector2(i * 50, j * 50), Color.White);
                    }
                    else if (terrain[i, j] == 1)
                    {
                        spriteBatch.Draw(box, new Vector2(i * 50, j * 50), Color.LightBlue);
                    }
                    else if (terrain[i, j] == 2)
                    {
                        spriteBatch.Draw(box, new Vector2(i * 50, j * 50), Color.Blue);
                    }
                    else if (terrain[i, j] == 3)
                    {
                        spriteBatch.Draw(box, new Vector2(i * 50, j * 50), Color.DarkBlue);
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void Terrain(int nodes)
        {
            Random rgen = new Random();
            Dictionary<int, int[]> nodeDict = new Dictionary<int, int[]>();
            for (int i = 0; i < nodes; i++)
            {
                int randX = rgen.Next(1, GraphicsDevice.Viewport.Width / 50);
                int randY = rgen.Next(1, GraphicsDevice.Viewport.Height / 50);
                int[] point = { randX, randY };
                nodeDict.Add(i, point);
                terrain[randX, randY] = rgen.Next(-3, 4);
            }

            for (int i = 0; i < GraphicsDevice.Viewport.Height / 50; i++)
            {
                for (int j = 0; j < GraphicsDevice.Viewport.Width / 50; j++)
                {
                    int[] checkArr = { j, i };
                    int leftVal = 0;
                    int rightVal = 0;
                    int topVal = 0;
                    int botVal = 0;
                    int avgCtrl = 1;

                    if (!nodeDict.ContainsValue(checkArr))
                    {
                        try
                        {
                            checkArr[0] = j - 1;
                            if (nodeDict.ContainsValue(checkArr))
                            {
                                leftVal = terrain[checkArr[0], checkArr[1]];
                                avgCtrl++;
                            }
                        }
                        catch (Exception) { };

                        try
                        {
                            checkArr[0] = j + 1;
                            if (nodeDict.ContainsValue(checkArr))
                            {
                                rightVal = terrain[checkArr[0], checkArr[1]];
                                avgCtrl++;
                            }
                        }
                        catch (Exception) { };

                        try
                        {
                            checkArr[0] = j;
                            checkArr[1] = i - 1;
                            if (nodeDict.ContainsValue(checkArr))
                            {
                                topVal = terrain[checkArr[0], checkArr[1]];
                                avgCtrl++;
                            }
                        }
                        catch (Exception) { };

                        try
                        {
                            checkArr[1] = i + 1;
                            if (nodeDict.ContainsValue(checkArr))
                            {
                                botVal = terrain[checkArr[0], checkArr[1]];
                                avgCtrl++;
                            }
                        }
                        catch (Exception) { };
                        

                        int val = (leftVal + rightVal + topVal + botVal) / avgCtrl;
                        terrain[j, i] = val;
                    }
                }
            }
        }

        private void Terrain()
        {
            Random rgen = new Random();
            for (int i = 0; i < GraphicsDevice.Viewport.Width / 50; i++)
            {
                for (int j = 0; j < GraphicsDevice.Viewport.Height / 50; j++)
                {
                    terrain[i, j] = rgen.Next(-3, 4);
                }
            }

            for (int i = 0; i < GraphicsDevice.Viewport.Width / 50; i++)
            {
                for (int j = 0; j < GraphicsDevice.Viewport.Height / 50; j++)
                {
                    int top = 0;
                    int bot = 0;
                    int right = 0;
                    int left = 0;
                    int div = 1;
                    try
                    {
                        //if (terrain[i,j] == 3 || terrain[i,j] == -3)
                        //{
                            int keep = rgen.Next(0, 5);
                            if (keep == 0)
                            {
                                throw new Exception();
                            }
                        //}
                        if (i == 0 && j == 0) //0,0
                        {
                            right = terrain[i + 1, j];
                            bot = terrain[i, j + 1];
                            div = 2;
                        }
                        else if (j == GraphicsDevice.Viewport.Height / 50 && i == GraphicsDevice.Viewport.Width / 50) //M,M
                        {
                            top = terrain[i, j - 1];
                            left = terrain[i - 1, j];
                            div = 2;
                        }
                        else if (j == 0 && i == GraphicsDevice.Viewport.Width / 50) //0,M
                        {
                            left = terrain[i - 1, j];
                            bot = terrain[i, j + 1];
                            div = 2;
                        }
                        else if (j == GraphicsDevice.Viewport.Height / 50 && i == 0) //M,0
                        {
                            top = terrain[i, j - 1];
                            right = terrain[i + 1, j];
                            div = 2;
                        }
                        else if (i == GraphicsDevice.Viewport.Width / 50) //x,M
                        {
                            top = terrain[i, j - 1];
                            bot = terrain[i, j + 1];
                            left = terrain[i - 1, j];
                            div = 3;
                        }
                        else if (j == GraphicsDevice.Viewport.Height / 50) // M,x
                        {
                            left = terrain[i - 1, j];
                            right = terrain[i + 1, j];
                            top = terrain[i, j - 1];
                            div = 3;
                        }
                        else if (i == 0) //x,0
                        {
                            top = terrain[i, j - 1];
                            bot = terrain[i, j + 1];
                            right = terrain[i + 1, j];
                            div = 3;
                        }
                        else if (j == 0) //0,x
                        {
                            right = terrain[i + 1, j];
                            left = terrain[i - 1, j];
                            bot = terrain[i, j + 1];
                            div = 3;
                        }
                        else
                        {
                            right = terrain[i + 1, j];
                            left = terrain[i - 1, j];
                            bot = terrain[i, j + 1];
                            top = terrain[i, j - 1];
                            div = 4;
                        }
                        terrain[i, j] = (top + bot + right + left) / div;
                    }
                    catch (Exception) { };
                }
            }
        }
    }
}
