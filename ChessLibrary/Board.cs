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
	public class Board
	{
		private List<Piece> _list;

		public Board()
		{
			_list = new List<Piece>();
		}

		public Piece GetPiece(Position position)
		{
			foreach (Piece piece in _list)
			{
				if ((piece.Position.Column == position.Column) && (piece.Position.Row == position.Row))
				{
					return piece;
				}
			}
			return null;
		}

		public void Add(Piece piece)
		{
			_list.Add(piece);
		}

		public void Remove(Position position)
		{
			_list.Remove(GetPiece(position));
		}

		public void Clear()
		{
			_list.Clear();
		}

		public void Display(Canvas can, int squareSize)
		{
			Brush light = (Brush)new BrushConverter().ConvertFrom("#FFCE9E");
			Brush dark = (Brush)new BrushConverter().ConvertFrom("#D18B47");
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Rectangle square = new Rectangle
					{
						Width = squareSize,
						Height = squareSize,
						Fill = (i + j) % 2 == 0 ? light : dark
					};
					Canvas.SetLeft(square, i * squareSize);
					Canvas.SetTop(square, j * squareSize);
					can.Children.Add(square);
				}
			}

			foreach (Piece piece in _list)
			{
				piece.Display(can, squareSize);
			}
		}
	}
}
