namespace LesserTerraria
{
    /// <summary>
    /// Provides the entry point and main game loop for the application.
    /// </summary>
    /// <remarks>The Program class is responsible for initializing the game window, setting up the main loop,
    /// and managing the overall game lifecycle. It configures window parameters, handles per-frame updates and drawing,
    /// and ensures proper cleanup when the application exits. This class is typically not instantiated directly;
    /// instead, execution begins with the Main method.</remarks>
    internal class Program
    {
        static readonly List<Texture2D> TextureCash = [];
        static readonly Dictionary<string, Texture2D> TextureDictionary = [];
        static Camera2D camera;
        static Vector2 cameraTargetSmooth;

#pragma warning disable CS8618 
        static Player player;
        static Map map;
#pragma warning restore CS8618 

        static void Main()
        {
            SetupWindow();
            Load();
            Initialize();
            GameLoop();
            Exit();
        }

        static void SetupWindow()
        {
            InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, GAME_TITLE);
            SetTargetFPS(TARGET_FPS);
            SetExitKey(EXIT_KEY);

            DisableCursor();

            //InitWindow(FULLSCREEN_WIDTH, FULLSCREEN_HEIGHT, GAME_TITLE);
            //SetWindowState(ConfigFlags.FullscreenMode);
        }

        static void Load()
        {
            _ = LoadAndCache("Player", TEXTURES_PATH + "player.png");
        }

        static Texture2D LoadAndCache(string name, string path)
        {
            Texture2D tex = LoadTexture(path);
            TextureCash.Add(tex);
            TextureDictionary[name] = tex;
            return tex;
        }

        static void Initialize()
        {
            map = new Map(MAP_WIDTH, MAP_HEIGHT);

            Vector2 playerStartPos = new(MAP_WIDTH / 2 * TILE_SIZE, 0);

            for (int y = MAP_HEIGHT - 1; y > 0; y--)
            {
                if (map.GetTile(MAP_WIDTH / 2, y) == 0)
                {
                    playerStartPos.Y = y * TILE_SIZE;
                    break;
                }
            }
            playerStartPos.Y -= TILE_SIZE * 2;

            player = new Player(
                TextureDictionary["Player"],
                playerStartPos
            );

            camera = new Camera2D
            {
                Target = player.Position,
                Offset = CAMERA_OFFSET,
                Rotation = CAMERA_ROTATION,
                Zoom = CAMERA_ZOOM
            };

            cameraTargetSmooth = player.Position;
        }

        static void GameLoop()
        {
            while (!WindowShouldClose())
            {
                Update();
                Draw();
            }
        }

        static void Update()
        {
            player.Update(GetFrameTime(), map);

            cameraTargetSmooth = Vector2.Lerp(
                cameraTargetSmooth,
                player.Position,
                CAMERA_SMOOTHNESS
            );

            // don't let the camera go outside the map boundaries or display black borders
            float halfScreenWidth = (SCREEN_WIDTH - TILE_SIZE) * 0.5f / camera.Zoom;
            float halfScreenHeight = (SCREEN_HEIGHT * 0.5f + TILE_SIZE) / camera.Zoom;
            float mapPixelWidth = MAP_WIDTH * TILE_SIZE;
            float mapPixelHeight = MAP_HEIGHT * TILE_SIZE + TILE_SIZE;

            float minX = halfScreenWidth;
            float maxX = mapPixelWidth - halfScreenWidth - TILE_SIZE;
            float minY = halfScreenHeight - TILE_SIZE * 2;
            float maxY = mapPixelHeight - halfScreenHeight - TILE_SIZE;

            cameraTargetSmooth.X = Math.Clamp(cameraTargetSmooth.X, minX, Math.Max(minX, maxX));
            cameraTargetSmooth.Y = Math.Clamp(cameraTargetSmooth.Y, minY, Math.Max(minY, maxY));

            camera.Target = cameraTargetSmooth;
        }

        static void Draw()
        {
            #region Starting Drawing
            BeginDrawing();
            ClearBackground(White);
            #endregion

            #region Camera Following Drawing
            BeginMode2D(camera);

            #region Background Drawing
            foreach (Rectangle border in BORDERS)
            {
                DrawRectangleRec(border, Black);
            }

            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    if (y % 2 == 0)
                    {
                        if (x % 2 == 0)
                        {
                            DrawRectangleRec(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE), ColorFromHSV(198f, 0.42f, 0.90f));
                        }
                        else
                        {
                            DrawRectangleRec(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE), ColorFromHSV(120f, 0.94f, 0.52f));
                        }
                    }
                    else
                    {
                        if (x % 2 == 0)
                        {
                            DrawRectangleRec(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE), ColorFromHSV(120f, 0.94f, 0.52f));
                        }
                        else
                        {
                            DrawRectangleRec(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE), ColorFromHSV(198f, 0.42f, 0.90f));
                        }
                    }
                }
            }
            #endregion

            #region Drawing Logic
            map.Draw();
            player.Draw();
            #endregion

            EndMode2D();
            #endregion

            #region UI Fixed Drawing
            player.DrawPosition();
            #endregion

            #region Ending Drawing
            EndDrawing();
            #endregion
        }

        static void Unload()
        {
            foreach (Texture2D texture in TextureCash)
            {
                UnloadTexture(texture);
            }

            TextureCash.Clear();
            TextureDictionary.Clear();
        }

        static void Exit()
        {
            Unload();

            EnableCursor();

            CloseWindow();
        }
    }
}
