using ChessLibrary;
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
		public MainWindow()
		{
			InitializeComponent();
			chessBoard.SetDefaultPosition();
			chessBoard.Display(canvasChessBoard, 64);
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
				chessBoard.Display(canvasChessBoard, 64);
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

		}
	}
}
