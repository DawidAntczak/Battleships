using Battleships.Core.Boards;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Battleships.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            ComposeServices();

            RegisterAppStart<BoardViewModel>();
        }

        private void ComposeServices()
        {
            CreatableTypes()
                .EndingWith("Strategy")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Generator")
                .AsInterfaces()
                .RegisterAsLazySingleton();


            CreatableTypes()
                .EndingWith("Board")
                .AsTypes()
                .RegisterAsLazySingleton();
        }
    }
}
