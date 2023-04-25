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
		private Row _row;
		private Column _column;
		private Color _color;
		private string _imagePath;
		public Piece(Type type, Row row, Column column, Color color)
		{
			_type = type;
			_row = row;
			_column = column;
			_color = color;

			_imagePath = $"{_type}{_color}.png";
		}

		public Type Type
		{
			get { return _type; }
		}
		public Row Row
		{
			get { return _row; }
		}
		public Column Column
		{
			get { return _column; }
		}
		public Color Color
		{
			get { return _color; }
		}
	}
}
