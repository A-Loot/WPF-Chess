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

		public bool Remove(Piece piece)
		{
			return _list.Remove(piece);
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

		public string Export(Color currentColor)
		{
			string text = string.Empty;
			switch (currentColor)
			{
				case Color.White:
					text += "w;";
					break;
				default:
					text += "b;";
					break;
			}

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

					switch (piece.Color)
					{
						case Color.White:
							text += 'w';
							break;
						default:
							text += 'b';
							break;
					}

					switch (piece.Type)
					{
						case Type.King:
							text += 'k';
							break;
						case Type.Queen:
							text += 'q';
							break;
						case Type.Rook:
							text += 'r';
							break;
						case Type.Bishop:
							text += 'b';
							break;
						case Type.Knight:
							text += 'n';
							break;
						default:
							text += 'p';
							break;
					}
				}
			}
			return text;
		}

		public Color Import(string text)
		{
			List<Piece> newList = new List<Piece>();
			text = text.ToLower();
			Color currentColor;
			if (text.Length != 130)
			{
				throw new Exception("text is not in the required format");
			}

			switch (Convert.ToString(text[0]) + Convert.ToString(text[1]))
			{
				case "w;":
					currentColor = Color.White;
					break;
				case "b;":
					currentColor = Color.Black;
					break;
				default:
					throw new Exception("text is not in the required format");
			}

			text = text.Substring(2);
			for (int i = 0; i < 128; i += 2)
			{
				if (text[i] == '-')
				{
					continue;
				}

				Color color;
				switch (text[i])
				{
					case 'w':
						color = Color.White;
						break;
					case 'b':
						color = Color.Black;
						break;
					default:
						throw new Exception("text is not in the required format");
				}

				Type type;
				switch (text[i + 1])
				{
					case 'k':
						type = Type.King;
						break;
					case 'q':
						type = Type.Queen;
						break;
					case 'r':
						type = Type.Rook;
						break;
					case 'b':
						type = Type.Bishop;
						break;
					case 'n':
						type = Type.Knight;
						break;
					case 'p':
						type = Type.Pawn;
						break;
					default:
						throw new Exception("text is not in the required format");
				}

				int y = (i / 2) / 8;
				int x = (i / 2) - y * 8;
				Piece piece = new Piece(type, color, new Point(x, y));
				newList.Add(piece);
			}

			_list.Clear();
			_list = newList;
			return currentColor;
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

		private bool IsValidMove(int x, int y, Color currentColor)
		{
			Piece piece = GetPiece(new Point(x, y));
			bool pieceExists = piece != null;
			bool isValidPosition = x >= 0 && x <= 7 && y >= 0 && y <= 7;

			if (pieceExists)
			{
				bool isOppositeColor = piece.Color != currentColor;
				return isValidPosition && isOppositeColor;
			}
			return isValidPosition;
		}

		private bool IsValidMove(int x, int y, bool mustBeCapture, Color currentColor)
		{
			Piece piece = GetPiece(new Point(x, y));
			bool pieceExists = piece != null;
			bool isValidPosition = x >= 0 && x <= 7 && y >= 0 && y <= 7;
			bool isValidMove = isValidPosition && pieceExists == mustBeCapture;

			if (pieceExists)
			{
				bool isOppositeColor = piece.Color != currentColor;
				return isValidMove && isOppositeColor;
			}
			return isValidMove;
		}

		// currently ignored moves/events:
		// - check: if someone gets checked or checkmated, nothing happens
		// - castling: the king cannot castle
		// - en passant: pawns cannot capture en passant
		// - promotion: pawns cannot promote to another piece when they reach the end of the board
		public List<Point> GetLegalMoves(Piece piece)
		{
			if (piece == null)
			{
				throw new Exception("There is no Piece at this position");
			}

			List<Point> moves = new List<Point>();
			int posX = (int)piece.Position.X;
			int posY = (int)piece.Position.Y;
			Color col = piece.Color;

			if (piece.Type == Type.Pawn)
			{
				int direction = (col == Color.White) ? 1 : -1;
				int startingRow = (col == Color.White) ? 1 : 6;
				Point p;

				if (IsValidMove(posX, posY + direction, false, col))
				{
					p = new Point(posX, posY + direction);
					moves.Add(p);

					if (posY == startingRow && IsValidMove(posX, posY + 2 * direction, false, col))
					{
						p = new Point(posX, posY + 2 * direction);
						moves.Add(p);
					}
				}

				if (IsValidMove(posX + 1, posY + direction, true, col))
				{
					p = new Point(posX + 1, posY + direction);
					moves.Add(p);
				}

				if (IsValidMove(posX - 1, posY + direction, true, col))
				{
					p = new Point(posX - 1, posY + direction);
					moves.Add(p);
				}
			}

			else if (piece.Type == Type.Knight)
			{
				int[] deltaX = { -2, -2, -1, -1, 1, 1, 2, 2 };
				int[] deltaY = { -1, 1, -2, 2, -2, 2, -1, 1 };
				Point p;

				for (int i = 0; i <= 7; i++)
				{
					int newX = posX + deltaX[i];
					int newY = posY + deltaY[i];

					if (IsValidMove(newX, newY , col))
					{
						p = new Point(newX, newY);
						moves.Add(p);
					}
				}
			}

			else if (piece.Type == Type.Bishop)
			{
				int[] deltaX = { -1, -1, 1, 1 };
				int[] deltaY = { -1, 1, -1, 1 };
				Point p;

				for (int i = 0; i < 4; i++)
				{
					int newX = posX + deltaX[i];
					int newY = posY + deltaY[i];

					while (IsValidMove(newX, newY, col))
					{
						p = new Point(newX, newY);
						moves.Add(p);

						newX += deltaX[i];
						newY += deltaY[i];

						if (GetPiece(p) != null)
						{
							break;
						}
					}
				}
			}

			else if (piece.Type == Type.Rook)
			{
				int[] deltaX = { -1, 0, 0, 1 };
				int[] deltaY = { 0, -1, 1, 0 };
				Point p;

				for (int i = 0; i < 4; i++)
				{
					int newX = posX + deltaX[i];
					int newY = posY + deltaY[i];

					while (IsValidMove(newX, newY, col))
					{
						p = new Point(newX, newY);
						moves.Add(p);

						newX += deltaX[i];
						newY += deltaY[i];

						if (GetPiece(p) != null)
						{
							break;
						}
					}
				}
			}

			else if (piece.Type == Type.Queen)
			{
				int[] deltaX = { -1, -1, -1, 0, 0, 1, 1, 1 };
				int[] deltaY = { -1, 0, 1, -1, 1, -1, 0, 1 };
				Point p;

				for (int i = 0; i < 8; i++)
				{
					int newX = posX + deltaX[i];
					int newY = posY + deltaY[i];

					while (IsValidMove(newX, newY, col))
					{
						p = new Point(newX, newY);
						moves.Add(p);

						newX += deltaX[i];
						newY += deltaY[i];

						if (GetPiece(p) != null)
						{
							break;
						}
					}
				}
			}

			else if (piece.Type == Type.King)
			{
				int[] deltaX = { -1, -1, -1, 0, 0, 1, 1, 1 };
				int[] deltaY = { -1, 0, 1, -1, 1, -1, 0, 1 };
				Point p;

				for (int i = 0; i <= 7; i++)
				{
					int newX = posX + deltaX[i];
					int newY = posY + deltaY[i];

					if (IsValidMove(newX, newY, col))
					{
						p = new Point(newX, newY);
						moves.Add(p);
					}
				}
			}

			return moves;
		}
	}
}
