using Student_Tracker_App.Services;

namespace Student_Tracker_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var navPage = new NavigationPage(new MainPage());

            // Force the top bar text to be Black for the entire app
            navPage.BarTextColor = Colors.Black;
            navPage.BarBackgroundColor = Colors.White; // Optional: Ensure bar is white

            return new Window(navPage);
        }

       public static DatabaseServices Database { get; private set; } = new DatabaseServices(Constants.DatabasePath);
    }
}