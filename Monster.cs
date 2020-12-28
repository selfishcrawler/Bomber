using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;

namespace BomberCore
{
    class Monster : GameObject
    {
        public Bitmap Picture { get => sprite[(int)currentDirection]; }

        public int OffsetX      { get; private set; }
        public int OffsetY      { get; private set; }
        public Point Position   { get => position;  }

        Bitmap[] sprite;
        Point position;
        Direction currentDirection;
        Timer moveTimer;

        public Monster()
        {
            sprite = new Bitmap[4];
            for (int i = 0; i < 4; i++)
                sprite[i] = Properties.Resources.Monster;
            sprite[0].RotateFlip(RotateFlipType.Rotate90FlipNone);
            sprite[1].RotateFlip(RotateFlipType.RotateNoneFlipX);
            sprite[2].RotateFlip(RotateFlipType.Rotate270FlipNone);

            OffsetX = OffsetY = 0;
            currentDirection = Direction.None;
            moveTimer = new Timer(10);
            moveTimer.AutoReset = true;
            moveTimer.Elapsed += MoveTimer_Elapsed;
            moveTimer.Start();
        }

        private void MoveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (Game.Map)
            {
                if (OffsetX == OffsetY && OffsetY == 0)
                {
                    ChooseDirection();
                    Game.Map[position] = null;
                    switch (currentDirection)
                    {
                        case Direction.Down:
                            OffsetY = -65;
                            position.Y++;
                            break;
                        case Direction.Left:
                            OffsetX = 65;
                            position.X--;
                            break;
                        case Direction.Up:
                            OffsetY = 65;
                            position.Y--;
                            break;
                        case Direction.Right:
                            OffsetX = -65;
                            position.X++;
                            break;
                        case Direction.None:
                            break;
                    }
                    if (Game.Map[position] is Player)
                        Game.Map[position].Destroy();
                    Game.Map[position] = this;
                }
                else
                {
                    switch (currentDirection)
                    {
                        case Direction.Down:
                            OffsetY += 5;
                            break;
                        case Direction.Left:
                            OffsetX -= 5;
                            break;
                        case Direction.Up:
                            OffsetY -= 5;
                            break;
                        case Direction.Right:
                            OffsetX += 5;
                            break;
                    }
                }
                Form1.formPointer.Invalidate();
            }
        }

        void ChooseDirection()
        {
            if (Game.CanMove(this, currentDirection))
                return;
            List<Direction> possibleMoves = new List<Direction>();
            Direction[] dir = System.Enum.GetValues<Direction>();
            for (int i = 0; i < 4; i++)
                if (Game.CanMove(this, dir[i]))
                    possibleMoves.Add(dir[i]);
            if (possibleMoves.Count == 0)
            {
                currentDirection = Direction.None;
                return;
            }
            currentDirection = possibleMoves[new Random().Next(0, possibleMoves.Count)];
        }

        public void Destroy()
        {
            moveTimer.Stop();
            Game.Map[position] = null;
        }

        public void TellCoords(int x, int y)
        {
            position = new Point(x, y);
        }
    }
}