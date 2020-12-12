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
        }

        private static MapIndexer indexer;
        public static MapIndexer Map
        {
            get => indexer ?? (indexer = new MapIndexer());
        }

        static Game()
        {
            player = new Player();
            map = new GameObject[,]
            {
                { null, null, null },
                { null, player, null },
                { null, null, null }
            };
        }

        static GameObject[,] map;

        public static int MapWidth
        {
            get => map.GetLength(1);
        }
        public static int MapHeight
        {
            get => map.GetLength(0);
        }

        public static Player player
        {
            get;
            private set;
        }
    }
}