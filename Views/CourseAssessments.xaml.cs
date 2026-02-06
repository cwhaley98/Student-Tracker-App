using C971_Mobile_App_PA.Schemas;
using C971_Mobile_App_PA.ViewModels; // For AssessmentDetails

namespace C971_Mobile_App_PA.Views;

public partial class CourseAssessments : ContentPage
{
    private int _courseId;
    public static string ToastMessage = ""; // Mailbox for success messages

    public CourseAssessments(int courseId, string courseTitle)
    {
        InitializeComponent();
        _courseId = courseId;
        CourseTitleLabel.Text = $"{courseTitle} Assessments";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAssessments();

        // Check for toast messages from Edit/Add page
        if (!string.IsNullOrEmpty(ToastMessage))
        {
            await DisplayAlert("Success", ToastMessage, "OK"); // Simple alert for now, or reuse your Banner logic
            ToastMessage = "";
        }
    }

    private async Task LoadAssessments()
    {
        var assessments = await App.Database.GetAssessmentsAsync(_courseId);
        AssessmentsCollection.ItemsSource = assessments;
    }

    private async void OnAssessmentSelected(object sender, EventArgs e)
    {
        var label = (Label)sender;
        var tap = (TapGestureRecognizer)label.GestureRecognizers[0];
        var assessment = (Assessment)tap.CommandParameter;

        await Navigation.PushAsync(new AssessmentDetails(assessment));
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        // Pass null for new assessment, but pass the CourseId
        await Navigation.PushAsync(new AssessmentEditAdd(null, _courseId));
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        var assessment = (Assessment)btn.CommandParameter;
        await Navigation.PushAsync(new AssessmentEditAdd(assessment, _courseId));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        var assessment = (Assessment)btn.CommandParameter;

        bool confirm = await Application.Current.Windows[0].Page.DisplayAlert("Confirm", "Delete this assessment?", "Yes", "No");
        if (confirm)
        {
            await App.Database.DeleteAssessmentAsync(assessment);
            await LoadAssessments();
        }
    }

    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}