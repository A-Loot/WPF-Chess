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
		private Position _position;

		public Piece(Type type, Color color, Position position)
		{
			_type = type;
			_color = color;
			_position = position;
		}

		public void Display(Canvas can, int size)
		{
			int x = (int)_position.Column * size;
			int y = (size * 7) - (int)_position.Row * size;
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
		public Position Position
		{
			get { return _position; }
		}
		public Uri ImagePath
		{
			get { return new Uri($"assets\\{_color}{_type}.png", UriKind.Relative); }
		}
	}
}
