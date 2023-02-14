using Battleships.Core.Boards;
using Battleships.Core.Games;
using Battleships.Core.Placings;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Battleships.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            ComposeTypes();
            RegisterAppStart<GameViewModel>();
        }

        private void ComposeTypes()
        {
            Mvx.IoCProvider.RegisterType<IPlacingStrategy, RandomPlacingStrategy>();
            Mvx.IoCProvider.RegisterType<IRandomNumberGenerator, RandomNumberGenerator>();
            Mvx.IoCProvider.RegisterType<IBoardFactory, BoardFactory>();
            Mvx.IoCProvider.RegisterType<Game>();
        }
    }
}
