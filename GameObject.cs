using System.Drawing;

namespace BomberCore
{
    public interface GameObject
    {
        Bitmap Picture  { get; }
        int OffsetX     { get; }
        int OffsetY     { get; }
        Point Position  { get; }

        void Destroy();
        void TellCoords(int x, int y);
    }
}