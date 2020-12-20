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
                        e.Graphics.DrawImage(Game.Map[x, y].Picture, new Point(x * size + Game.Map[x, y].OffsetX, y * size + Game.Map[x, y].OffsetY));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S:
                case Keys.Down:
                    Game.player.Move(Direction.Down);
                    break;
                case Keys.W:
                case Keys.Up:
                    Game.player.Move(Direction.Up);
                    break;
                case Keys.A:
                case Keys.Left:
                    Game.player.Move(Direction.Left);
                    break;
                case Keys.D:
                case Keys.Right:
                    Game.player.Move(Direction.Right);
                    break;
            }
        }
    }
}