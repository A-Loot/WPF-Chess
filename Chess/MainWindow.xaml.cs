using ChessLibrary;
using Microsoft.Win32;
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
        private Board chessBoard;
        private Piece? selectedPiece;
        private ChessLibrary.Color currentColor;
        public MainWindow()
        {
            InitializeComponent();
            chessBoard = new();
            chessBoard.SetDefaultPosition();
            chessBoard.Display(CanvasChessBoard, 64);
            currentColor = ChessLibrary.Color.White;
        }

        private void MenuItemImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "chess save files (*.chesssave)|*.chesssave|txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
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
            SaveFileDialog sfd = new()
            {
                Filter = "chess save files (*.chesssave)|*.chesssave|All files (*.*)|*.*"
            };
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
            Point pos = new((int)p.X / 64, 7 - (int)p.Y / 64);
            try
            {
                selectedPiece = chessBoard.GetPiece(pos);
            }
            catch { return; }

            if (selectedPiece.Color != currentColor)
            {
                return;
            }

            try
            {
                foreach (Point move in chessBoard.GetLegalMoves(selectedPiece))
                {
                    Rectangle square = new()
                    {
                        Width = 64,
                        Height = 64,
                        Fill = new BrushConverter().ConvertFrom("#4F00FF00") as Brush ?? Brushes.LightGreen
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

            // the following is needed because click evens still count one pixel to the right of or below the actual element
            Rectangle element = sender as Rectangle ?? throw new Exception("something impossible occured");
            if (Canvas.GetLeft(element) + element.Width == p.X || Canvas.GetTop(element) + element.Height == p.Y)
            {
                return;
            }

            Point pos = new((int)p.X / 64, 7 - (int)p.Y / 64);
            bool success = false;
            bool isKing = false;
            try
            {
                Piece capturedPiece = chessBoard.GetPiece(pos);
                isKing = capturedPiece.Type == ChessLibrary.Type.King;
                success = chessBoard.Remove(capturedPiece);
            }
            catch { }

            // temporary solution because i am/was too lazy to implement a proper checkmate
            if (success && isKing)
            {
                MessageBoxResult result = MessageBox.Show($"{currentColor} has won the game!\nDo you want to play again?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    currentColor = ChessLibrary.Color.White;
                    chessBoard.SetDefaultPosition();
                    chessBoard.Display(CanvasChessBoard, 64);
                    return;
                }
                else
                {
                    Close();
                }
            }

            selectedPiece?.Move(pos);
            selectedPiece = null;
            chessBoard.Display(CanvasChessBoard, 64);
            currentColor = currentColor == ChessLibrary.Color.White ? ChessLibrary.Color.Black : ChessLibrary.Color.White;
        }
    }
}
