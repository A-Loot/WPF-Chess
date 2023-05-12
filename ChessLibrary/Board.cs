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

		public Piece GetPiece(Point position)
		{
			foreach (Piece piece in _list)
			{
				if (piece.Position == position)
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

		public void Remove(Point position)
		{
			_list.Remove(GetPiece(position));
		}

		public void Clear()
		{
			_list.Clear();
		}

		public void SetDefaultPosition()
		{
			_list.Clear();
			for (int i = 0; i <= 1; i++)
			{
				Color color = Color.White + i;

				for (int x = 0; x <= 7; x++)
				{
					_list.Add(new Piece(Type.Pawn, color, new Point(x, 1)));
				}

				_list.Add(new Piece(Type.Rook, color, new Point(0, 0)));
				_list.Add(new Piece(Type.Knight, color, new Point(1, 0)));
				_list.Add(new Piece(Type.Bishop, color, new Point(2, 0)));
				_list.Add(new Piece(Type.Queen, color, new Point(3, 0)));
				_list.Add(new Piece(Type.King, color, new Point(4, 0)));
				_list.Add(new Piece(Type.Bishop, color, new Point(5, 0)));
				_list.Add(new Piece(Type.Knight, color, new Point(6, 0)));
				_list.Add(new Piece(Type.Rook, color, new Point(7, 0)));
			}
		}

		public void Display(Canvas can, int squareSize)
		{
			can.Children.Clear();
			Brush light = (Brush)new BrushConverter().ConvertFrom("#FFCE9E");
			Brush dark = (Brush)new BrushConverter().ConvertFrom("#D18B47");
			for (int i = 0; i <= 7; i++)
			{
				for (int j = 0; j <= 7; j++)
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

		private bool IsValidMove(int x, int y, bool pieceAllowed)
		{
			bool pieceExists = GetPiece(new Point(x, y)) != null;
			bool isValidMove = x >= 0 && x <= 7 && y >= 0 && y <= 7 && pieceExists == pieceAllowed;
			return isValidMove;
		}

		public List<Point> GetLegalMoves(Point position)
		{
			Piece piece = GetPiece(position);
			List<Point> moves = new List<Point>();

			if (piece.Type == Type.Pawn)
			{
				int direction = (piece.Color == Color.White) ? 1 : -1;
				int startingRow = (piece.Color == Color.White) ? 1 : 6;
				int posx = (int)position.X;
				int posy = (int)position.Y;
				Point p;

				if (IsValidMove(posx, posy + direction, false))
				{
					p = new Point(posx, posy + direction);
					moves.Add(p);
				}
				if (position.Y == startingRow && IsValidMove(posx, posy + 2 * direction, false))
				{
					p = new Point(posx, posy + 2 * direction);
					moves.Add(p);
				}
				if(IsValidMove(posx + 1, posy + direction, true))
				{
					p = new Point(posx + 1, posy);
					moves.Add(p);
				}
				if (IsValidMove(posx - 1, posy + direction, true))
				{
					p = new Point(posx - 1, posy);
					moves.Add(p);
				}
			}

			else if (piece.Type == Type.Knight)
			{

			}

			else if (piece.Type == Type.Bishop)
			{

			}

			else if (piece.Type == Type.Rook)
			{

			}

			else if (piece.Type == Type.Queen)
			{

			}

			else if (piece.Type == Type.King)
			{

			}

			return moves;
		}
	}
}
