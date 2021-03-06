﻿using System.Collections.Generic;
using System.Drawing;
using System.Timers;

namespace BomberCore
{
    public enum BombState
    {
        Preparing, Exploding, Disposed
    }

    public class Bomb
    {
        static Bitmap[] BombSprite, ExplosionSprite;

        static Bomb()
        {
            BombSprite = new Bitmap[3];
            ExplosionSprite = new Bitmap[7];

            for (int i = 0; i < BombSprite.Length; i++)
            {
                BombSprite[i] = new Bitmap(52, 60);
                using (Graphics g = Graphics.FromImage(BombSprite[i]))
                    g.DrawImage(Properties.Resources.Bomb, 0, 0, new Rectangle(60 * i, 0, 60, 52), GraphicsUnit.Pixel);
            }

            for (int i = 0; i < ExplosionSprite.Length; i++)
            {
                ExplosionSprite[i] = new Bitmap(52, 60);
                using (Graphics g = Graphics.FromImage(ExplosionSprite[i]))
                    g.DrawImage(Properties.Resources.Fire, 0, 0, new Rectangle(52 * i, 0, 52, 60), GraphicsUnit.Pixel);
            }
        }

        Timer PrepareTimer, ExplosionTimer;
        int Animation = 0;
        List<Point> explosionMap;

        public Point BombLocation   { get; private set; }
        public BombState State      { get; private set; }
        public Bitmap Picture       { get => State == BombState.Preparing ? BombSprite[Animation] : ExplosionSprite[Animation]; }
        public Point[] ExplosionMap { get => explosionMap.ToArray(); }
        public event System.Action StateChanged;

        public Bomb(Point location)
        {
            BombLocation = location;

            PrepareTimer = new Timer(1000);
            PrepareTimer.Elapsed += PrepareTimer_Elapsed;
            PrepareTimer.AutoReset = true;

            ExplosionTimer = new Timer(50);
            ExplosionTimer.Elapsed += ExplosionTimer_Elapsed;
            ExplosionTimer.AutoReset = true;

            Animation = 0;
            State = BombState.Preparing;
            explosionMap = new List<Point>();
            PrepareTimer.Start();
        }

        private void PrepareTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Animation == 2)
            {
                Animation = 0;
                State = BombState.Exploding;
                PrepareTimer.Stop();


                for (int i = -1; i < 2; i++)
                    for (int j = -1; j < 2; j++)
                        if (BombLocation.X + i >= 0 && BombLocation.X + i < Game.MapWidth && BombLocation.Y + j >= 0 && BombLocation.Y + j < Game.MapHeight)
                        {
                            if (Game.Map[BombLocation.X + i, BombLocation.Y + j] is not null)
                                Game.Map[BombLocation.X + i, BombLocation.Y + j].Destroy();
                            if (Game.Map[BombLocation.X + i, BombLocation.Y + j] is null)
                                explosionMap.Add(new Point(BombLocation.X + i, BombLocation.Y + j));
                        }

                ExplosionTimer.Start();
                return;
            }
            Animation++;
            StateChanged();
        }
        
        private void ExplosionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Animation == 6)
            {
                ExplosionTimer.Stop();
                State = BombState.Disposed;
            }
            Animation++;
            StateChanged();
        }
    }
}