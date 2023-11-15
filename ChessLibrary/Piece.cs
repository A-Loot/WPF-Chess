using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChessLibrary
{
    public class Piece
    {
        private Type _type;
        private Color _color;
        private Point _position;

        public Piece(Type type, Color color, Point position)
        {
            _type = type;
            _color = color;
            if (position.X is < 0 or > 7)
                throw new ArgumentException("position.X must be in range 0-7");
            if (position.Y is < 0 or > 7)
                throw new ArgumentException("position.Y must be in range 0-7");
            _position = position;
        }

        public void Move(Point newPosition)
        {
            if (newPosition.X is < 0 or > 7)
                throw new ArgumentException("newPosition.X must be in range 0-7");
            if (newPosition.Y is < 0 or > 7)
                throw new ArgumentException("newPosition.Y must be in range 0-7");
            _position = newPosition;
        }

        public void Display(Canvas can, int size)
        {
            int x = (int)_position.X * size;
            int y = (size * 7) - (int)_position.Y * size;
            Image img = new()
            {
                Height = size,
                Width = size,
                Source = new BitmapImage(ImagePath)
            };
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
            can.Children.Add(img);
        }

        public Type Type => _type;
        public Color Color => _color;
        public Point Position => _position;
        public Uri ImagePath => new($"assets\\{_color}{_type}.png", UriKind.Relative);
    }
}
