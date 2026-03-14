namespace Student_Tracker_App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            Dispatcher.Dispatch(async () =>
            {
                WelcomeLabel.Opacity = 0;
                // Animate to full visibility over 2 seconds
                await WelcomeLabel.FadeTo(1, 2000, Easing.CubicOut);
            });
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // 1. Validation Check for empty fields
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            // 2. Check if the account actually exists
            bool userExists = await App.Database.CheckUserExistsAsync(username);

            if (!userExists)
            {
                await DisplayAlert("Account Not Found", "This account does not exist. Please click 'Create Account' to register.", "OK");
                PasswordEntry.Text = string.Empty;
                return;
            }

            // 3. If the account exists, validate the password
            bool isValid = await App.Database.ValidateLoginAsync(username, password);

            if (isValid)
            {
                // Clear password and navigate to the main app
                PasswordEntry.Text = string.Empty;
                await Navigation.PushAsync(new Views.TermsList());
            }
            else
            {
                // Account exists, but password was wrong
                await DisplayAlert("Error", "Incorrect password. Please try again.", "Retry");
                PasswordEntry.Text = string.Empty;
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // 1. Validation Check
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter a username and password to create an account.", "OK");
                return;
            }

            // 2. Register
            bool isSuccess = await App.Database.RegisterUserAsync(username, password);

            if (isSuccess)
            {
                await DisplayAlert("Success", "Account created successfully! You may now login.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "That username is already taken. Please try another.", "OK");
            }
        }
    }
}
