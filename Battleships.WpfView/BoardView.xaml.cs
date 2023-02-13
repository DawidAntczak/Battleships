using Battleships.Core.Boards;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Battleships.WpfView
{
    [MvxViewFor(typeof(BoardViewModel))]
    public partial class BoardView : MvxWpfView
    {
        public BoardView()
        {
            InitializeComponent();
        }

        private void BoardMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(sender as Canvas);

            var position = new Position((int)point.Y, (int)point.X);

            var boardViewModel = (BoardViewModel)ViewModel;
            if (boardViewModel.ShotCommand?.CanExecute(position) == true)
            {
                boardViewModel.ShotCommand?.Execute(position);
            }
        }
    }
}
