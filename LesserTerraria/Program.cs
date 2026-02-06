namespace LesserTerraria
{
    public class Constants
    {
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 450;

        public const string GAME_TITLE = "Lesser Terraria";

        public const int TARGET_FPS = 60;

        public const KeyboardKey EXIT_KEY = KeyboardKey.Escape;
    }

    public class Program
    {
        static void Main()
        {
            InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, GAME_TITLE);
            SetTargetFPS(TARGET_FPS);
            SetExitKey(EXIT_KEY);

            Initialize();

            while (!WindowShouldClose())
            {
                Update();
                Draw();
            }

            CloseWindow();
        }

        static void Initialize()
        {
            // Initialization logic goes here
        }

        static void Update()
        {
            // Game update logic goes here
        }

        static void Draw()
        {
            #region Starting Drawing
            BeginDrawing();
            ClearBackground(White);
            #endregion

            #region Drawing Logic
            DrawText("Hello raylib-cs!", 190, 200, 20, DarkGray);
            #endregion

            #region Ending Drawing
            EndDrawing();
            #endregion
        }
    }
}
