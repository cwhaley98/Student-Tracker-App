namespace C971_Mobile_App_PA
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            // Run this on the UI thread to ensure the visual tree is ready
            Dispatcher.Dispatch(async () =>
            {
                // Double check it starts invisible
                WelcomeLabel.Opacity = 0;

                // Animate to full visibility over 2 seconds
                await WelcomeLabel.FadeTo(1, 5000, Easing.CubicOut);
            });
        }

        private async void OnTermsClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Views.TermsList());
        }
    }
}
