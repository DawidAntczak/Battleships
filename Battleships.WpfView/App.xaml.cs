using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace Battleships.WpfView
{
    public partial class App : MvxApplication
    {
        public App()
        {
            this.RegisterSetupType<Setup>();
        }
    }
}
