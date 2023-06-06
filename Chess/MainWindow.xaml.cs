﻿using ChessLibrary;
using Microsoft.Win32;
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

namespace Chess
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Board chessBoard = new Board();
		Piece selectedPiece = null;
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
			ofd.Filter = "chess save files (*.chessave)|*.chessave|txt files (*.txt)|*.txt|All files (*.*)|*.*";
			bool? result = ofd.ShowDialog();
			if (result == true)
			{
				string content = System.IO.File.ReadAllText(ofd.FileName);
				try
				{
					chessBoard.Import(content);
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
			sfd.Filter = "chess save files (*.chessave)|*.chessave";
			bool? result = sfd.ShowDialog();
			if (result == true)
			{
				string content = chessBoard.Export();
				System.IO.File.WriteAllText(sfd.FileName, content);
			}
		}

		private void MenuItemNewGame_Click(object sender, RoutedEventArgs e)
		{
			chessBoard.SetDefaultPosition();
			chessBoard.Display(CanvasChessBoard, 64);
		}

		private void CanvasChessBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			chessBoard.Display(CanvasChessBoard, 64);
			Point p = e.GetPosition(CanvasChessBoard);
			Point pos = new Point((int)p.X / 64, 7 - (int)p.Y / 64);
			try
			{
				List<Point> moves = chessBoard.GetLegalMoves(pos);
				selectedPiece = chessBoard.GetPiece(pos);
				if (selectedPiece.Color != currentColor)
				{
					return;
				}
				foreach (Point move in moves)
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
			if (chessBoard.GetPiece(pos) != null)
			{
				chessBoard.Remove(pos);
			}
			selectedPiece.Position = pos;
			selectedPiece = null;
			chessBoard.Display(CanvasChessBoard, 64);
			currentColor = currentColor == ChessLibrary.Color.White ? ChessLibrary.Color.Black : ChessLibrary.Color.White;
		}
	}
}
