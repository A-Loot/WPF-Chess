using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public void Draw()
		{

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
			get { return new Uri($"assets\\{_color}{_type}", UriKind.Relative); }
		}
	}
}
