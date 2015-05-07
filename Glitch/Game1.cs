﻿#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Glitch
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        //attributes
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState kState;
        Texture2D playerFaceUp;
        Texture2D playerFaceRight;
        Texture2D playerFaceDown;
        Texture2D playerFaceLeft;
        Texture2D playerBullet;
        Texture2D enemyBullet;
        Texture2D enemyFaceLeft;
        Texture2D enemyFaceRight;
        Texture2D enemyFaceUp;
        Texture2D enemyFaceDown;
        Texture2D gameWall;
        Texture2D trap;
        Texture2D line;
        Random rgen;
        SpriteFont menuFont;
        Player p1;
        Bullet b1;
        StartMenu sMenu;
        PauseMenu pMenu;
        GameMenu gMenu;
        WinMenu wMenu;
        InstructionMenu iMenu;
        WorldGeneration worldGen;
        ToolLoader tLoader;

        //constructor
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            sMenu = new StartMenu();
            pMenu = new PauseMenu();
            gMenu = new GameMenu();
            wMenu = new WinMenu();
            iMenu = new InstructionMenu();
            tLoader = new ToolLoader();
            rgen = new Random();
            GameVariables.ENEMIES = new List<Enemy>();
            GameVariables.TRAPS = new List<Trap>();
            GameVariables.ENEMYPOS = new List<Vector2>();
            b1 = new Bullet(new Vector2(20, 20), new Rectangle(), 0);
            p1 = new Player(new Vector2(500, 175), new Rectangle(), 0, 5, b1);
            worldGen = new WorldGeneration(b1);
            wMenu = new WinMenu();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            //gets the initial state of the keyboard
            kState = Keyboard.GetState();

            //sets the window width and height to be used in other classes
            GameVariables.WINDOW_HEIGHT = GraphicsDevice.Viewport.Height;
            GameVariables.WINDOW_WIDTH = GraphicsDevice.Viewport.Width;

            //reads information from the external tool
            tLoader.ReadData();

            //generates the world
            worldGen.GenerateWorld();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //texture to make lines around menu selection
            line = new Texture2D(GraphicsDevice, 1, 1);
            line.SetData<Color>(new Color[] { Color.White });   // fill the texture with white

            //Loading textures to use in game
            menuFont = this.Content.Load<SpriteFont>("mainFont");
            enemyBullet = this.Content.Load<Texture2D>("playerbullet");
            enemyFaceLeft = this.Content.Load<Texture2D>("enemyFaceLeft");
            enemyFaceRight = this.Content.Load<Texture2D>("enemyFaceRight");
            enemyFaceUp = this.Content.Load<Texture2D>("enemyFaceUp");
            enemyFaceDown = this.Content.Load<Texture2D>("enemyFaceDown");
            playerBullet = this.Content.Load<Texture2D>("playerbullet");
            playerFaceDown = this.Content.Load<Texture2D>("player_down");
            playerFaceUp = this.Content.Load<Texture2D>("player_up");
            playerFaceRight = this.Content.Load<Texture2D>("player_right");
            playerFaceLeft = this.Content.Load<Texture2D>("player_left");
            trap = this.Content.Load<Texture2D>("trap");
            gameWall = this.Content.Load<Texture2D>("Labratory");

            //Setting up dimensions in GameVariables
            GameVariables.PLAYER_DIMENSIONS = new Vector2(playerFaceUp.Width, playerFaceUp.Height);
            GameVariables.ENEMY_DIMENSIONS = new Vector2(enemyFaceUp.Width, enemyFaceUp.Height);
            GameVariables.TRAP_DIMENSIONS = new Vector2(trap.Width, trap.Height);

            //Setting up rectangles for Collision Detection

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            //if you press the escape key or you press enter at 
            //the start menu while highlighting "quit game", you exit the program
            if (sMenu.EndGame() == true || kState.IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //if start game is not selected, update the menu
            if (sMenu.StartGame() == false)
            {
                kState = Keyboard.GetState();
                sMenu.UpdateMenu();
            }

            //if start game is selected, run the game logic
            if (sMenu.StartGame() == true && kState.IsKeyDown(Keys.P) == false)
            {
                kState = Keyboard.GetState();
                this.ProcessInput(kState);

                b1.Move();

                foreach (Enemy e in GameVariables.ENEMIES)
                {
                    if (e.IsActive)
                    {
                        e.Move();
                        Console.WriteLine(e.Position.X + " " + e.Position.Y);
                    }
                }

                Stopwatch watch = new Stopwatch();
                Random rgen = new Random();
                int randTime = rgen.Next(0, 26);
                int randDirection = rgen.Next(0, 4);

                watch.Start();

                if (watch.ElapsedMilliseconds == randTime)
                {
                    GameVariables.ENEMIES[rgen.Next(0,GameVariables.ENEMIES.Count)].Direction = randDirection;
                    watch.Reset();
                }


            }
            if (kState.IsKeyDown(Keys.P) == true && sMenu.StartGame() == true)
            {
                KeyboardState kstate = Keyboard.GetState();
                pMenu.UpdateMenu();

                if (pMenu.ContinueGame() == true)
                {
                    kState = Keyboard.GetState();
                    sMenu.StartGame();
                }
                if (pMenu.QuitGame() == true || kstate.IsKeyDown(Keys.Escape) == true)
                {
                    Exit();
                }
            }

            // collision detection 

            //player collides with an enemy or enemy bullet
            foreach (Enemy e in GameVariables.ENEMIES)
            {
                //if the player is colliding with an enemy
                if (p1.CheckCollision(e))
                {
                    p1.Health--;

                    e.IsActive = false;
                }
                if (p1.CheckCollision(e.EnemyBullet))
                {
                    p1.Health--;
                }

                if (e.CheckCollision(p1.Bullet))
                {
                    e.IsActive = false;
                    gMenu.Score++;
                }
            }

            //player collides with a trap
            foreach (Trap t in GameVariables.TRAPS)
            {
                if (p1.CheckCollision(t))
                {
                    p1.Health--;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //clear the screen
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            sMenu.DrawMenu(spriteBatch, menuFont, line);

            //if the start game option is selected, run this code
            if (sMenu.StartGame() == true)
            {
                //draw the background
                spriteBatch.Begin();
                spriteBatch.Draw(gameWall, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Silver);
                spriteBatch.DrawString(menuFont, "" + p1.Health, new Vector2(120, 25), Color.Black);
                spriteBatch.End();

                gMenu.DrawText(spriteBatch, menuFont);



                foreach (Trap t in GameVariables.TRAPS)
                {
                    if (GameVariables.CURRENT_ROOM.PosX == t.Room.Item1 && GameVariables.CURRENT_ROOM.PosY == t.Room.Item2)
                    {
                        t.Draw(trap, spriteBatch);
                    }
                }

                foreach (Enemy e in GameVariables.ENEMIES)
                {
                    if (GameVariables.CURRENT_ROOM.PosX == e.RoomNo.Item1 && GameVariables.CURRENT_ROOM.PosY == e.RoomNo.Item2)
                    {
                        switch (e.Direction)
                        {
                            case 0: //up
                                e.Draw(enemyFaceUp, spriteBatch);
                                break;
                            case 1: //right
                                e.Draw(enemyFaceRight, spriteBatch);
                                break;
                            case 2: //down
                                e.Draw(enemyFaceDown, spriteBatch);
                                break;
                            case 3: //left
                                e.Draw(enemyFaceLeft, spriteBatch);
                                break;

                        }
                        e.IsActive = true;
                    }
                    else
                    {
                        e.IsActive = false;
                    }
                }

                //switch statement for player movement
                switch (p1.Direction)
                {
                    case 0: //up
                        p1.Draw(playerFaceUp, spriteBatch);
                        break;
                    case 1: //right
                        p1.Draw(playerFaceRight, spriteBatch);
                        break;
                    case 2: //down
                        p1.Draw(playerFaceDown, spriteBatch);
                        break;
                    case 3: //left
                        p1.Draw(playerFaceLeft, spriteBatch);
                        break;
                }



                //drawing the bullets
                b1.Draw(playerBullet, spriteBatch);
            }

            if (kState.IsKeyDown(Keys.P) == true && sMenu.StartGame() == true)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                pMenu.DrawMenu(spriteBatch, menuFont, line);
            }

            base.Draw(gameTime);
        }

        //processes the keystrokes of the input
        public void ProcessInput(KeyboardState kstate)
        {
            //processes these keyboard inputs only during the game
            if (sMenu.StartGame() == true)
            {
                GraphicsDevice.Clear(Color.Black);
                if (kstate.IsKeyDown(Keys.W))
                {
                    p1.Direction = 0;
                    p1.Move();
                }
                if (kstate.IsKeyDown(Keys.A))
                {
                    p1.Direction = 3;
                    p1.Move();
                }
                if (kstate.IsKeyDown(Keys.S))
                {
                    p1.Direction = 2;
                    p1.Move();
                }
                if (kstate.IsKeyDown(Keys.D))
                {
                    p1.Direction = 1;
                    p1.Move();
                }
                if (kstate.IsKeyDown(Keys.Space))
                {
                    p1.Fire();
                }
            }
        }

        //method stub to detect collisions (code will be moved here later)
        public void DetectCollisions()
        {

        }
      }
    }
