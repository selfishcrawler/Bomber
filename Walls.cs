using System.Drawing;

namespace BomberCore
{
    class Wall : GameObject
    {
        public Bitmap Picture
        {
            get => Destructible ? Properties.Resources.Weakwall : Properties.Resources.Wall;
        }

        public int OffsetX          { get => 0; }
        public int OffsetY          { get => 0; }
        public Point Position       { get; private set; }
        public bool Destructible    { get; init; }

        public Wall(bool Destructible)
        {
            this.Destructible = Destructible;
        }

        public void Destroy()
        {
            if (!Destructible)
                return;
            Game.Map[Position] = null;
        }

        public void TellCoords(int x, int y)
        {
            Position = new Point(x, y);
        }
    }
}