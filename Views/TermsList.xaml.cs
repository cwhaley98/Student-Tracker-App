using Student_Tracker_App.Schemas;
using Student_Tracker_App.ViewModels;

namespace Student_Tracker_App.Views;

public partial class TermsList : ContentPage
{
    //SIMPLE SHARED MAILBOX
    //Other pages can set this string to trigger the notification
    public static string ToastMessage = "";
	public TermsList()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Fetch data from the SQLite database
            var myTerms = await App.Database.GetTermsAsync();

            // Assign the real data to the CollectionView
            TermsCollection.ItemsSource = myTerms;

            //CHECK FOR MESSAGES
            //If the ToastMessage has text, show it
            if (!string.IsNullOrEmpty(ToastMessage))
            {
                ShowSuccess(ToastMessage);
                ToastMessage = ""; // Clear it so it doesn't show again
            }
        }
        catch (Exception ex)
        {
            // Handle potential database errors
            await DisplayAlert("Error", "Failed to load terms: " + ex.Message, "OK");
        }
    }

    private async void OnTermSelected(object sender, EventArgs e)
    {
        try
        {
            var label = (Label)sender;
            var tapGesture = (TapGestureRecognizer)label.GestureRecognizers[0];
            var term = (Term)tapGesture.CommandParameter;

            // Navigate to the Details page, passing the selected term
            await Navigation.PushAsync(new TermDetails(term));
        }
        catch (Exception ex)
        {
            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Navigation failed: " + ex.Message, "OK");
        }
    }

    // HELPER METHOD TO SHOW AND HIDE THE BANNER
    private async void ShowSuccess(string message)
    {
        SuccessLabel.Text = message;
        SuccessBanner.IsVisible = true;

        // Wait for 3 seconds
        await Task.Delay(3000);

        //Hide it again
        SuccessBanner.IsVisible = false;
    }

    //Handles the Add Term button click event
    private async void OnAddTermClicked(object sender, EventArgs e)
    {
        // Pass 'null' to indicate we are adding a NEW term
        await Navigation.PushAsync(new TermEditAdd(null));
    }

    // Handles the Edit icon click event
    private async void OnEditClicked(object sender, EventArgs e)
    {
        try
        {
            var button = (ImageButton)sender;
            var term = (Term)button.CommandParameter;

            //Navigates to the TermEditAdd page, passing the selected term
            await Navigation.PushAsync(new TermEditAdd(term));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to navigate to edit term: " + ex.Message, "OK");
        }
    }

    //Handles the Delete icon click event
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        try
        {
            var button = (ImageButton)sender;
            var term = (Term)button.CommandParameter;

            //Confirm deletion alert
            bool confirm = await DisplayAlert("Confirm Delete",
                $"Are you sure you want to delete the term '{term.TermTitle}'?", "Yes", "No");

            if (confirm)
            {
                await App.Database.DeleteTermAsync(term);

                // Trigger the green banner directly
                ShowSuccess("Term deleted successfully!");

                // Refresh list logic
                var myTerms = await App.Database.GetTermsAsync();
                TermsCollection.ItemsSource = myTerms;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Delete failed: " + ex.Message, "OK");
        }
    }

    private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        string query = e.NewTextValue;

        //If search bar is cleared, go back to normal view
        if (string.IsNullOrWhiteSpace(query))
        {
            SearchResultsView.IsVisible = false;
            TermsCollection.IsVisible = true; //Ensure this matches the name of existing CollectionView
            return;
        }

        // Hide the normal list and show the search results view
        TermsCollection.IsVisible = false; //Ensure this matches the name of existing CollectionView
        SearchResultsView.IsVisible = true;

        //Fetch the data and bind it to the search results CollectionView
        var results = await App.Database.SearchAcademicItemsAsync(query);
        SearchResultsView.ItemsSource = results;
    }

    private async void OnViewReportsClicked(object sender, EventArgs e)
    {
        //Navigate to the Reports Dashboard page
        await Navigation.PushAsync(new Views.ReportsDashboard());
    }
}