using Student_Tracker_App.Schemas;

namespace Student_Tracker_App.ViewModels;

public partial class AssessmentDetails : ContentPage
{
    public AssessmentDetails(Assessment assessment)
    {
        InitializeComponent();
        BindingContext = assessment;

        NotifStatusLabel.Text = assessment.notificationsEnabled ? "Yes" : "No";
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}