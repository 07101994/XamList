
using Foundation;
using UIKit;
using MonoTouch.NUnit.UI;

namespace XamList.UnitTests.iOS
{
    [Register(nameof(UnitTestAppDelegate))]
    public partial class UnitTestAppDelegate : UIApplicationDelegate
    {
        UIWindow _window;
        TouchRunner _runner;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            _runner = new TouchRunner(_window);

            // register every tests included in the main application/assembly
            _runner.Add(System.Reflection.Assembly.GetExecutingAssembly());

            _window.RootViewController = new UINavigationController(_runner.GetViewController());

            // make the window visible
            _window.MakeKeyAndVisible();

            return true;
        }
    }
}
