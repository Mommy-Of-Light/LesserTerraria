namespace LesserTerraria
{
    internal class Player(Texture2D texture, Vector2 position)
    {
        #region Fields
        private Rectangle _hitbox = new(position.X, position.Y, PLAYER_WIDTH, PLAYER_HEIGHT);
        private Texture2D _texture = texture;
        private Vector2 _position = position;
        private Vector2 _velocity = Vector2.Zero;
        private bool isOnGround = false;
        #endregion

        #region Properties
        public Rectangle Hitbox
        {
            get => _hitbox;
            set => _hitbox = value;
        }

        public Texture2D Texture
        {
            get => _texture;
            set => _texture = value;
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }
        #endregion

        #region Update Method
        public void Update(float dt, Map map)
        {
            if (IsKeyDown(D))
                _velocity.X = PLAYER_SPEED;
            else if (IsKeyDown(A))
                _velocity.X = -PLAYER_SPEED;
            else
                _velocity.X = 0;

            if (IsKeyDown(Space) && isOnGround)
            {
                _velocity.Y = PLAYER_JUMP_VELOCITY;
                isOnGround = false;
            }

            if (!isOnGround)
                _velocity.Y += GRAVITY * dt;
            else
                _velocity.Y = 0;

            if (_velocity.Y > MAX_FALL_SPEED)
                _velocity.Y = MAX_FALL_SPEED;

            _position.X += _velocity.X * dt;

            if (_velocity.X == 0)
                _position.X = (float)Math.Round(_position.X);

            _hitbox.X = (int)_position.X;

            foreach (Rectangle border in BORDERS)
            {
                if (CheckCollisionRecs(_hitbox, border))
                {
                    if (_velocity.X > 0)
                        _position.X = border.X - _hitbox.Width;
                    else if (_velocity.X < 0)
                        _position.X = border.X + border.Width;

                    _position.X = (float)Math.Floor(_position.X);
                    _velocity.X = 0;
                    _hitbox.X = (int)_position.X;
                }
            }
            for (int x = 0; x < map.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < map.Tiles.GetLength(1); y++)
                {
                    int tile = map.Tiles[x, y];
                    if (tile != 0)
                    {
                        Rectangle tileRect = map.TileRectangles[x, y];
                        if (CheckCollisionRecs(_hitbox, tileRect))
                        {
                            if (_velocity.X > 0)
                                _position.X = tileRect.X - _hitbox.Width;
                            else if (_velocity.X < 0)
                                _position.X = tileRect.X + tileRect.Width;
                            _position.X = (float)Math.Floor(_position.X);
                            _velocity.X = 0;
                            _hitbox.X = (int)_position.X;
                        }
                    }
                }
            }

            _position.Y += _velocity.Y * dt;

            _hitbox.Y = (int)_position.Y;

            isOnGround = false;
            foreach (Rectangle border in BORDERS)
            {
                if (CheckCollisionRecs(_hitbox, border))
                {
                    if (_velocity.Y > 0)
                    {
                        _position.Y = border.Y - _hitbox.Height;
                        isOnGround = true;
                    }
                    else if (_velocity.Y < 0)
                    {
                        _position.Y = border.Y + border.Height;
                    }
                    _position.Y = (float)Math.Round(_position.Y);
                    _velocity.Y = 0;
                    _hitbox.Y = (int)_position.Y;
                }
            }
            for (int x = 0; x < map.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < map.Tiles.GetLength(1); y++)
                {
                    int tile = map.Tiles[x, y];
                    if (tile != 0)
                    {
                        Rectangle tileRect = map.TileRectangles[x, y];
                        if (CheckCollisionRecs(_hitbox, tileRect))
                        {
                            if (_velocity.Y > 0)
                            {
                                _position.Y = tileRect.Y - _hitbox.Height;
                                isOnGround = true;
                            }
                            else if (_velocity.Y < 0)
                            {
                                _position.Y = tileRect.Y + tileRect.Height;
                            }
                            else
                            {
                                isOnGround = true;
                            }
                            _position.Y = (float)Math.Floor(_position.Y);
                            _velocity.Y = 0;
                            _hitbox.Y = (int)_position.Y;
                        }
                    }
                }
            }

            if (isOnGround)
                _position.Y = (float)Math.Floor(_position.Y);

            if (_position.Y - PLAYER_HEIGHT >= MAP_HEIGHT * TILE_SIZE)
            {
                _position = new Vector2(MAP_WIDTH * TILE_SIZE / 2, 0);
                _velocity = Vector2.Zero;
                for (int y = MAP_HEIGHT - 1; y > 0; y--)
                {
                    if (map.GetTile(MAP_WIDTH / 2, y) == 0)
                    {
                        _position.Y = y * TILE_SIZE;
                        break;
                    }
                }
                _position.Y -= TILE_SIZE;

            }

            _hitbox.X = (int)_position.X;
            _hitbox.Y = (int)_position.Y;
        }
        #endregion

        #region Draw Method
        public void Draw()
        {
            DrawTextureRec(_texture, new Rectangle(0, 0, (int)PLAYER_WIDTH, (int)PLAYER_HEIGHT), _position, White);
            DrawDebug();
        }

        public void DrawDebug()
        {
            DrawRectangleLines((int)_hitbox.X, (int)_hitbox.Y, (int)_hitbox.Width, (int)_hitbox.Height, Red);
        }

        public void DrawPosition()
        {
            int tileX = (int)(_position.X / TILE_SIZE);
            int tileY = (int)(_position.Y / TILE_SIZE);

            int centerX = MAP_WIDTH / 2;
            int posX = tileX - centerX;

            string posXText = posX switch
            {
                > 0 => $"{posX}' EAST",
                < 0 => $"{-posX}' WEST",
                _ => "Center"
            };

            DrawText($"{tileY} tiles", 10, 40, 20, Black);

            DrawText(posXText, 10, 10, 20, Black);
        }
        #endregion
    }
}
