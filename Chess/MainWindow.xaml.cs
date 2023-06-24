using ChessLibrary;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Board chessBoard = new Board();
		Piece selectedPiece = null;
		List<Point> legalMoves;
		ChessLibrary.Color currentColor = ChessLibrary.Color.White;
		public MainWindow()
		{
			InitializeComponent();
			chessBoard.SetDefaultPosition();
			chessBoard.Display(CanvasChessBoard, 64);
		}

		private void MenuItemImport_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "chess save files (*.chesssave)|*.chesssave|txt files (*.txt)|*.txt|All files (*.*)|*.*";
			bool? result = ofd.ShowDialog();
			if (result == true)
			{
				string content = System.IO.File.ReadAllText(ofd.FileName);
				try
				{
					currentColor = chessBoard.Import(content);
				}
				catch
				{
					MessageBox.Show("The content of the selected file is not in the required format", "Export Game", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				chessBoard.Display(CanvasChessBoard, 64);
			}
		}

		private void MenuItemExport_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "chess save files (*.chesssave)|*.chesssave|All files (*.*)|*.*";
			bool? result = sfd.ShowDialog();
			if (result == true)
			{
				string content = chessBoard.Export(currentColor);
				System.IO.File.WriteAllText(sfd.FileName, content);
			}
		}

		private void MenuItemNewGame_Click(object sender, RoutedEventArgs e)
		{
			currentColor = ChessLibrary.Color.White;
			chessBoard.SetDefaultPosition();
			chessBoard.Display(CanvasChessBoard, 64);
		}

		private void CanvasChessBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			chessBoard.Display(CanvasChessBoard, 64);
			Point p = e.GetPosition(CanvasChessBoard);
			Point pos = new Point((int)p.X / 64, 7 - (int)p.Y / 64);
			selectedPiece = chessBoard.GetPiece(pos);
			if (selectedPiece?.Color != currentColor)
			{
				return;
			}
			try
			{
				legalMoves = chessBoard.GetLegalMoves(selectedPiece);
				foreach (Point move in legalMoves)
				{
					Rectangle square = new Rectangle
					{
						Width = 64,
						Height = 64,
						Fill = (Brush)new BrushConverter().ConvertFrom("#4F00FF00")
					};
					square.MouseLeftButtonDown += RectangleHighlighted_MouseLeftButtonDown;
					Canvas.SetLeft(square, move.X * 64);
					Canvas.SetTop(square, (7 - move.Y) * 64);
					CanvasChessBoard.Children.Add(square);
				}
			}
			catch { }
		}

		private void RectangleHighlighted_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Point p = e.GetPosition(CanvasChessBoard);
			Point pos = new Point((int)p.X / 64, 7 - (int)p.Y / 64);

			// the following is needed because click evens still count one pixel to the right of or below the actual element
			// i don't know why it works that way, but i don't think i'd be able to change it unless i made my own custom rectangle class
			if (!legalMoves.Contains(pos))
			{
				return;
			}

			Piece capturedPiece = chessBoard.GetPiece(pos);
			bool success = chessBoard.Remove(capturedPiece);

			selectedPiece.Move(pos);
			selectedPiece = null;
			chessBoard.Display(CanvasChessBoard, 64);
			currentColor = currentColor == ChessLibrary.Color.White ? ChessLibrary.Color.Black : ChessLibrary.Color.White;

			if (success && capturedPiece?.Type == ChessLibrary.Type.King)
			{
				MessageBoxResult result = MessageBox.Show($"{currentColor} has won the game!\nDo you want to play again?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{
					currentColor = ChessLibrary.Color.White;
					chessBoard.SetDefaultPosition();
					chessBoard.Display(CanvasChessBoard, 64);
				}
				else
				{
					Close();
				}
			}
		}
	}
}
