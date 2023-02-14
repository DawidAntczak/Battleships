using Battleships.Core.Boards;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Battleships.Core.Games
{
    public class GameViewModel : MvxViewModel
    {
        public IMvxCommand NewGameCommand => new MvxCommand(Game.StartNewGame);
        public IMvxCommand ShotCommand => new MvxCommand<Position>(Game.Shoot, Game.CanShoot);

        public Game Game { get; }

        public GameViewModel(Game game)
        {
            Game = game;
        }

        public override Task Initialize()
        {
            // comment out if you want to press the new game button before the board generates for the first time
            // it's usefull if ship amount and size would be configurable
            Game.StartNewGame();
            return base.Initialize();
        }
    }
}
