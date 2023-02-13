using Battleships.Core.Boards;
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
            RegisterAppStart<BoardViewModel>();
        }

        private void ComposeTypes()
        {
            Mvx.IoCProvider.RegisterType<IPlacingStrategy, RandomPlacingStrategy>();
            Mvx.IoCProvider.RegisterType<IRandomNumberGenerator, RandomNumberGenerator>();
            Mvx.IoCProvider.RegisterType<Board>();
        }
    }
}
