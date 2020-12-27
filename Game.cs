using System.Drawing;

namespace BomberCore
{
    public enum Direction
    {
        Down, Left, Up, Right, None
    }

    public static class Game
    {
        public class MapIndexer
        {
            public GameObject this[int x, int y]
            {
                get => map[y, x];
                set
                {
                    map[y, x] = value;
                    Form1.formPointer.Invalidate();
                }
            }

            public GameObject this[Point p]
            {
                get => map[p.Y, p.X];
                set
                {
                    map[p.Y, p.X] = value;
                    Form1.formPointer.Invalidate();
                }
            }
        }

        private static MapIndexer indexer;
        public static MapIndexer Map
        {
            get => indexer ?? (indexer = new MapIndexer());
        }

        static Game()
        {
            Player = new Player();
            map = new GameObject[,]
            {
                { new Monster(), null, null, null, null },
                { null, Player, null, new Wall(true), new Wall(false) },
                { null, null, new Wall(false), null, null },
                { null, null, null, null, null },
                { null, null, null, null, new Monster() },
            };
            Playing = true;
        }

        static GameObject[,] map;

        public static void StopGame()
        {
            Playing = false;
            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++)
                    Map[x, y] = null;
        }

        public static bool CanMove(GameObject gObject, Direction moveDir)
        {
            switch(moveDir)
            {
                case Direction.Down:
                    if (gObject.Position.Y + 1 >= MapHeight || Map[gObject.Position.X, gObject.Position.Y + 1] is Wall)
                        return false;
                    return true;
                case Direction.Left:
                    if (gObject.Position.X - 1 < 0 || Map[gObject.Position.X - 1, gObject.Position.Y] is Wall)
                        return false;
                    return true;
                case Direction.Up:
                    if (gObject.Position.Y - 1 < 0 || Map[gObject.Position.X, gObject.Position.Y - 1] is Wall)
                        return false;
                    return true;
                case Direction.Right:
                    if (gObject.Position.X + 1 >= MapWidth || Map[gObject.Position.X + 1, gObject.Position.Y] is Wall)
                        return false;
                    return true;
            }
            return false;
        }

        public static bool Playing      { get; private set; }
        public static Player Player     { get; private set; }

        public static int MapWidth
        {
            get => map.GetLength(1);
        }
        public static int MapHeight
        {
            get => map.GetLength(0);
        }
    }
}