namespace LesserTerraria
{
    internal class Constants
    {
        // Windows parameters
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 480;
        public const int FULLSCREEN_WIDTH = 1920;
        public const int FULLSCREEN_HEIGHT = 1080;
        public const string GAME_TITLE = "Lesser Terraria";
        public const int TARGET_FPS = 60;
        public const KeyboardKey EXIT_KEY = Escape;

        public const string ASSETS_PATH = "Assets/";
        public const string TEXTURES_PATH = ASSETS_PATH + "Textures/";

        public const string PLAYER_SAVE_PATH = "Player/";
        public const string WORLD_SAVE_PATH = "World/";

        // Game parameters
        public const int TILE_SIZE = 32;
        public const float GRAVITY = 600f;
        public const int MAX_FALL_SPEED = 1000;

        // Map parameters 
        public const int MAP_WIDTH = 150;
        public const int MAP_HEIGHT = 30;

        public static readonly Rectangle[] BORDERS =
        [
            new Rectangle(0, -10, MAP_WIDTH * TILE_SIZE, 10),
            new Rectangle(0, MAP_HEIGHT * TILE_SIZE, MAP_WIDTH * TILE_SIZE, TILE_SIZE),
            new Rectangle(-10, 0, 10, MAP_HEIGHT * TILE_SIZE),
            new Rectangle(MAP_WIDTH * TILE_SIZE, 0, 10, MAP_HEIGHT * TILE_SIZE)
        ];

        // Player parameters
        public const float PLAYER_SPEED = 200f;
        public const float PLAYER_JUMP_VELOCITY = -300f;
        public const float PLAYER_WIDTH = TILE_SIZE * 1;
        public const float PLAYER_HEIGHT = TILE_SIZE * 2;

        // Camera parameters
        public const float CAMERA_SMOOTHNESS = 1f;
        public const float CAMERA_LERP_FACTOR = CAMERA_SMOOTHNESS * (60f / TARGET_FPS);
        public const float CAMERA_ZOOM = 1.0f;
        public const float CAMERA_ROTATION = 0.0f;
        public const float CAMERA_SCALE = 1.0f;
        public static readonly Vector2 CAMERA_OFFSET = new((SCREEN_WIDTH - PLAYER_WIDTH) / 2, (SCREEN_HEIGHT - PLAYER_HEIGHT) / 2);
    }

    internal class Helpers
    {

    }
}
