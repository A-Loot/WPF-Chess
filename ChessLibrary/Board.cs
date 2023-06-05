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
					_list.Add(new Piece(Type.Pawn, color, new Point(x, 1 + i * 5)));
				}

				_list.Add(new Piece(Type.Rook, color, new Point(0, i * 7)));
				_list.Add(new Piece(Type.Knight, color, new Point(1, i * 7)));
				_list.Add(new Piece(Type.Bishop, color, new Point(2, i * 7)));
				_list.Add(new Piece(Type.Queen, color, new Point(3, i * 7)));
				_list.Add(new Piece(Type.King, color, new Point(4, i * 7)));
				_list.Add(new Piece(Type.Bishop, color, new Point(5, i * 7)));
				_list.Add(new Piece(Type.Knight, color, new Point(6, i * 7)));
				_list.Add(new Piece(Type.Rook, color, new Point(7, i * 7)));
			}
		}

		public string Export()
		{
			string text = string.Empty;
			for (int i = 0; i <= 7; i++)
			{
				for (int j = 0; j<= 7; j++)
				{
					Piece piece = GetPiece(new Point(j, i));
					if (piece == null)
					{
						text += "--";
						continue;
					}

					if (piece.Color == Color.White)
					{
						text += 'w';
					}
					else
					{
						text += 'b';
					}

					if (piece.Type == Type.King)
					{
						text += 'k';
					}
					else if (piece.Type == Type.Queen)
					{
						text += 'q';
					}
					else if (piece.Type == Type.Rook)
					{
						text += 'r';
					}
					else if (piece.Type == Type.Bishop)
					{
						text += 'b';
					}
					else if (piece.Type == Type.Knight)
					{
						text += 'n';
					}
					else
					{
						text += 'p';
					}
				}
			}
			return text;
		}

		public void Import(string text)
		{
			_list.Clear();
			text = text.ToLower();
			if (text.Length != 128)
			{
				throw new Exception("text is not in the required format");
			}

			for (int i = 0; i < 128; i += 2)
			{
				if (text[i] == '-')
				{
					continue;
				}

				Color color;
				if (text[i] == 'w')
				{
					color = Color.White;
				}
				else if (text[i] == 'b')
				{
					color = Color.Black;
				}
				else
				{
					throw new Exception("text is not in the required format");
				}

				Type type;
				if (text[i + 1] == 'k')
				{
					type = Type.King;
				}
				else if (text[i + 1] == 'q')
				{
					type = Type.Queen;
				}
				else if (text[i + 1] == 'r')
				{
					type = Type.Rook;
				}
				else if (text[i + 1] == 'b')
				{
					type = Type.Bishop;
				}
				else if (text[i + 1] == 'n')
				{
					type = Type.Knight;
				}
				else if (text[i + 1] == 'p')
				{
					type = Type.Pawn;
				}
				else
				{
					throw new Exception("text is not in the required format");
				}

				int y = (i / 2) / 8;
				int x = (i / 2) - y * 8;
				Piece piece = new Piece(type, color, new Point(x, y));
				_list.Add(piece);
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

		private bool IsValidMove(int x, int y)
		{
			bool isValidMove = x >= 0 && x <= 7 && y >= 0 && y <= 7;
			return isValidMove;
		}

		private bool IsValidMove(int x, int y, bool pieceAllowed)
		{
			bool pieceExists = GetPiece(new Point(x, y)) != null;
			bool isValidMove = x >= 0 && x <= 7 && y >= 0 && y <= 7 && pieceExists == pieceAllowed;
			return isValidMove;
		}

		// currently ignored moves/events (some of them might be implemented in seperate functions):
		// - check: if someone gets checked or checkmated, nothing happens
		// - castling: the king cannot castle
		// - en passant: pawns cannot capture en passant
		// - promotion: pawns cannot promote to another piece when they reach the end of the board
		// - capture: the list that is returned has no indication if a move is a capture or not
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
				if (IsValidMove(posx + 1, posy + direction, true))
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
				int posx = (int)position.X;
				int posy = (int)position.Y;
				int[] deltax = { -2, -2, -1, -1, 1, 1, 2, 2 };
				int[] deltay = { -1, 1, -2, 2, -2, 2, -1, 1 };
				Point p;

				for (int i = 0; i <= 7; i++)
				{
					if (IsValidMove(posx + deltax[i], posy + deltay[i]))
					{
						p = new Point(posx + deltax[i], posy + deltay[i]);
						moves.Add(p);
					}
				}
			}

			else if (piece.Type == Type.Bishop)
			{
				int posx = (int)position.X;
				int posy = (int)position.Y;
				int[] deltax = { -1, -1, 1, 1 };
				int[] deltay = { -1, 1, -1, 1 };
				Point p;

				for (int i = 0; i < 4; i++)
				{
					int newX = posx + deltax[i];
					int newY = posy + deltay[i];

					while (IsValidMove(newX, newY))
					{
						p = new Point(newX, newY);
						moves.Add(p);

						newX += deltax[i];
						newY += deltay[i];
					}
				}
			}

			else if (piece.Type == Type.Rook)
			{
				int posx = (int)position.X;
				int posy = (int)position.Y;
				int[] deltax = { -1, 0, 0, 1 };
				int[] deltay = { 0, -1, 1, 0 };
				Point p;

				for (int i = 0; i < 4; i++)
				{
					int newX = posx + deltax[i];
					int newY = posy + deltay[i];

					while (IsValidMove(newX, newY))
					{
						p = new Point(newX, newY);
						moves.Add(p);

						newX += deltax[i];
						newY += deltay[i];
					}
				}
			}

			else if (piece.Type == Type.Queen)
			{
				int posx = (int)position.X;
				int posy = (int)position.Y;
				int[] deltax = { -1, -1, -1, 0, 0, 1, 1, 1 };
				int[] deltay = { -1, 0, 1, -1, 1, -1, 0, 1 };
				Point p;

				for (int i = 0; i < 8; i++)
				{
					int newX = posx + deltax[i];
					int newY = posy + deltay[i];

					while (IsValidMove(newX, newY))
					{
						p = new Point(newX, newY);
						moves.Add(p);

						newX += deltax[i];
						newY += deltay[i];
					}
				}
			}

            else if (piece.Type == Type.King)
			{
				int posx = (int)position.X;
				int posy = (int)position.Y;
				int[] deltax = { -1, -1, -1, 0, 0, 1, 1, 1 };
				int[] deltay = { -1, 0, 1, -1, 1, -1, 0, 1 };
				Point p;

				for (int i = 0; i <= 7; i++)
				{
					if (IsValidMove(posx + deltax[i], posy + deltay[i]))
					{
						p = new Point(posx + deltax[i], posy + deltay[i]);
						moves.Add(p);
					}
				}
			}

			return moves;
		}
	}
}
