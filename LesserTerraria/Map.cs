namespace LesserTerraria
{
    internal class Map
    {
        private readonly int[,] _tiles;
        private readonly Rectangle[,] _tileRectangles;

        public int[,] Tiles => _tiles;
        public Rectangle[,] TileRectangles => _tileRectangles;

        public int Width => _tiles.GetLength(0);
        public int Height => _tiles.GetLength(1);

        public Map(int width, int height)
        {
            _tiles = new int[width, height];
            _tileRectangles = new Rectangle[width, height];
            Generate(width, height);
        }

        public void Generate(int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y > height - 2)
                        SetTile(x, y, 1);
                    else
                        SetTile(x, y, 0);
                    _tileRectangles[x, y] = new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE);
                }
            }

            // Generate some hills
            //GenerateHill(width / 4, 10, 5);
        }

        private void GenerateHill(int centerX, int width, int height)
        {
            int halfWidth = width / 2;
            for (int y = 0; y < height; y++)
            {
                int rowWidth = (int)(halfWidth * Math.Cos((double)y / height * Math.PI) + halfWidth);
                for (int x = centerX - rowWidth; x <= centerX + rowWidth; x++)
                {
                    if (x >= 0 && x < Width && y >= 0 && y < Height)
                    {
                        SetTile(x, Height - 2 - y, 1);
                    }
                }
            }
        }

        public int GetTile(int x, int y)
        {
            if (x < 0 || x >= _tiles.GetLength(0) || y < 0 || y >= _tiles.GetLength(1))
                return -1;
            return _tiles[x, y];
        }

        public void SetTile(int x, int y, int tileType)
        {
            if (x < 0 || x >= _tiles.GetLength(0) || y < 0 || y >= _tiles.GetLength(1))
                return;
            _tiles[x, y] = tileType;
        }

        public void Draw()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_tiles[x, y] != 0)
                    {
                        DrawRectangleRec(_tileRectangles[x, y], DarkGray);
                    }
                }
            }
        }
    }
}
