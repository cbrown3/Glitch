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
    //A class which contains game variables that are used throughout the program
    static class GameVariables
    {
        //static game attributes
        public static float WINDOW_WIDTH;
        public static float WINDOW_HEIGHT;
        public static int NUMBER_OF_ROOMS;
        public static int NUMBER_OF_ENEMIES;
        public static int DENSITY_OF_TRAPS;
        public static int INITIAL_HEALTH;
        public static int MIN_ENEMY_SPEED;
        public static int MAX_ENEMY_SPEED;
        public static int ENEMIES_REMAINING;
        public static Room ROOT_ROOM;
        public static Room CURRENT_ROOM;
        public static List<Enemy> ENEMIES;
        public static List<Trap> TRAPS;
        public static List<Vector2> ENEMYPOS;
        public static Vector2 ENEMY_DIMENSIONS;
        public static Vector2 PLAYER_DIMENSIONS;
        public static Vector2 TRAP_DIMENSIONS;
        public static Vector2 BULLET_DIMENSIONS;

        //constant game attributes
        public const double PLAYER_SCALE = 0.2;
        public const double ENEMY_SCALE = 0.1;
        public const double TRAP_SCALE = 0.7;
        public const double BULLET_SCALE = 0.03;
    }
}
