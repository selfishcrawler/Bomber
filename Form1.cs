using System;
using System.Drawing;
using System.Windows.Forms;

namespace BomberCore
{
    public partial class Form1 : Form
    {
        public static Form1 formPointer;
        private readonly int size;

        public Form1()
        {
            InitializeComponent();
            size = Properties.Resources.Grass.Width;
            
            for (int y = 0; y < Game.MapHeight; y++)
                for (int x = 0; x < Game.MapWidth; x++)
                    Game.Map[x, y]?.TellCoords(x, y);
            formPointer = this;
            ClientSize = new Size(Game.MapWidth * size, Game.MapHeight * size);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int y = 0; y < Game.MapHeight; y++)
                for (int x = 0; x < Game.MapWidth; x++)
                    e.Graphics.DrawImage(Properties.Resources.Grass, new Point(x * size, y * size));

            for (int y = 0; y < Game.MapHeight; y++)
                for (int x = 0; x < Game.MapWidth; x++)
                    if (Game.Map[x, y] is not null)
                    {
                        if (Game.Map[x, y] is Player && Game.Player.Bomb is not null)
                        {
                            Bomb bomb = Game.Player.Bomb;
                            switch (Game.Player.Bomb.State)
                            {
                                case BombState.Preparing:
                                    e.Graphics.DrawImage(bomb.Picture, new Point(bomb.BombLocation.X * size, bomb.BombLocation.Y * size));
                                    break;
                                case BombState.Exploding:
                                    foreach (Point explosion in Game.Player.Bomb.ExplosionMap)
                                        e.Graphics.DrawImage(bomb.Picture, new Point(explosion.X * size, explosion.Y * size));
                                    break;
                            }
                        }
                        e.Graphics.DrawImage(Game.Map[x, y].Picture, new Point(x * size + Game.Map[x, y].OffsetX, y * size + Game.Map[x, y].OffsetY));
                    }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Game.Playing)
                return;
            switch (e.KeyCode)
            {
                case Keys.S:
                case Keys.Down:
                    Game.Player.Move(Direction.Down);
                    break;
                case Keys.W:
                case Keys.Up:
                    Game.Player.Move(Direction.Up);
                    break;
                case Keys.A:
                case Keys.Left:
                    Game.Player.Move(Direction.Left);
                    break;
                case Keys.D:
                case Keys.Right:
                    Game.Player.Move(Direction.Right);
                    break;
                case Keys.Q:
                    Game.Player.PlaceBomb();
                    break;
            }
        }
    }
}