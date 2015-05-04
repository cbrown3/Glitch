﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Glitch
{
    //Player Class, used for current player
    class Player : Entity
    {
        //attributes
        private int laserCharge;
        protected bool hasKey;
        private Bullet playerBullet;
        private Rectangle rect;


        //properties
        public int LaserCharge
        {
            get { return laserCharge; }
            private set { laserCharge = value < 0 ? 0 : value; }
        }

        public bool HasKey
        {
            get { return hasKey; }
            set { hasKey = value; }
        }

        public Bullet Bullet
        {
            get { return playerBullet; }
            set { playerBullet = value; }
        }
        //constructor
        public Player(Vector2 pos, Rectangle cd, int dir, int hth, Bullet b)
            : base(pos, cd, dir)
        {
            laserCharge = 100;
            playerBullet = b;
            hasKey = false;
            rect = cd;
        }

        //player fires the laser
        public void Fire()
        {
            if (!playerBullet.IsActive)
            {
                playerBullet.Direction = direction;
                playerBullet.Position = position;
                playerBullet.IsActive = true;
            }
        }

        //Moves the player based on direction
        public override void Move()
        {
            switch (direction)
            {
                case 0: //up
                    position.Y -= 5f; break;
                case 1: //right
                    position.X += 5f; break;
                case 2: //down
                    position.Y += 5f; break;
                case 3: //left
                    position.X -= 5f; break;
            }
            // Check the edges
            if (position.X < 75)
            {
                if (position.Y >= 215 && position.Y <= 270 && GameVariables.CURRENT_ROOM.Left != null)
                {
                    position.X = 920;
                    GameVariables.CURRENT_ROOM = GameVariables.CURRENT_ROOM.Left;
                }
                else
                {
                    position.X = 75;
                }
            }

            if (position.X > 920)
            {
                if (position.Y >= 215 && position.Y <= 270 && GameVariables.CURRENT_ROOM.Right != null)
                {
                    position.X = 75;
                    GameVariables.CURRENT_ROOM = GameVariables.CURRENT_ROOM.Right;
                }
                else
                {
                    position.X = 920;
                }
            }

            if (position.Y < 20)
            {
                if (position.X >= 465 && position.X <= 535 && GameVariables.CURRENT_ROOM.Up != null)
                {
                    position.Y = 475;
                    GameVariables.CURRENT_ROOM = GameVariables.CURRENT_ROOM.Up;
                }
                else
                {
                    position.Y = 20;
                }
            }

            if (position.Y > 475)
            {
                if (position.X >= 465 && position.X <= 535 && GameVariables.CURRENT_ROOM.Down != null)
                {
                    position.Y = 20;
                    GameVariables.CURRENT_ROOM = GameVariables.CURRENT_ROOM.Down;
                }
                else
                {
                    position.Y = 475;
                }
            }
        }

        //draws the player
        public override void Draw(Texture2D sprite, SpriteBatch sb)
        {
            sb.Begin();
                    //sb.Draw(sprite, position, null, Color.White, 0, position, 0.01f, SpriteEffects.None, 0);
            sb.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, (int)(sprite.Width * 0.25), (int)(sprite.Height * 0.25)), Color.White);
                    Console.WriteLine("P: " + position.X + " " + position.Y);
            sb.End();
        }
    }
}
