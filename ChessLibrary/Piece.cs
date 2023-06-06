using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
			if (position == null)
				throw new ArgumentNullException("position");
			if (position.X < 0 || position.X > 7)
				throw new ArgumentException("position.X must be in range 0-7");
			if (position.Y < 0 || position.Y > 7)
				throw new ArgumentException("position.Y must be in range 0-7");
			_position = position;
		}

		public void Display(Canvas can, int size)
		{
			int x = (int)_position.X * size;
			int y = (size * 7) - (int)_position.Y * size;
			Image img = new Image
			{
				Height = size,
				Width = size,
				Source = new BitmapImage(ImagePath)
			};
			Canvas.SetLeft(img, x);
			Canvas.SetTop(img, y);
			can.Children.Add(img);
		}

		public Type Type
		{
			get { return _type; }
		}
		public Color Color
		{
			get { return _color; }
		}
		public Point Position
		{
			get { return _position; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("position");
				if (value.X < 0 || value.X > 7)
					throw new ArgumentException("position.X must be in range 0-7");
				if (value.Y < 0 || value.Y > 7)
					throw new ArgumentException("position.Y must be in range 0-7");
				_position = value;
			}
		}
		public Uri ImagePath
		{
			get { return new Uri($"assets\\{_color}{_type}.png", UriKind.Relative); }
		}
	}
}
