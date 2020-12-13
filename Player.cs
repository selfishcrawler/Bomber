using System;
using System.Drawing;
using System.Timers;

namespace BomberCore
{
    public class Player : GameObject
    {
        public Bitmap Picture
        {
            get => Sprite[Animation, currentDirection == Direction.None ? 0 : (int)currentDirection];
            private set => Picture = value;
        }

        public int OffsetX      { get; private set; }
        public int OffsetY      { get; private set; }
        public Point Position   { get => position;  }

        Bitmap[,] Sprite;
        Direction currentDirection;
        int Animation;
        bool MoveAllowed;
        Timer animationTimer;
        Point position;

        public Player()
        {
            Sprite = new Bitmap[3, 4];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 4; j++)
                {
                    Sprite[i, j] = new Bitmap(65, 65);
                    using (Graphics g = Graphics.FromImage(Sprite[i, j]))
                        g.DrawImage(Properties.Resources.BomberMan, 0, 0, new Rectangle(65 * j, 65 * i, 65, 65), GraphicsUnit.Pixel);
                }
            OffsetX = OffsetY = 0;
            currentDirection = Direction.None;
            Animation = 0;
            MoveAllowed = true;
            animationTimer = new Timer(100);
            animationTimer.Elapsed += AnimationTimer_Elapsed;
            animationTimer.AutoReset = true;
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void TellCoords(int x, int y)
        {
            position = new Point(x, y);
        }

        private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Animation == 2)
                Animation = 0;
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
            if (OffsetX == 0 && OffsetY == 0)
            {
                animationTimer.Stop();
                Animation = -1;
                MoveAllowed = true;
                currentDirection = Direction.None;
            }
            Animation++;
            Form1.formPointer.Invalidate();
        }

        public void Move(Direction direction)
        {
            if (!MoveAllowed)
                return;
            if (!Game.CanMove(this, direction))
                return;
            MoveAllowed = false;
            Game.Map[position.X, position.Y] = null;
            switch (direction)
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
            }
            Game.Map[position.X, position.Y] = this;
            currentDirection = direction;

            animationTimer.Start();
        }
    }
}