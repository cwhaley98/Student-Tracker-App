using C971_Mobile_App_PA.Schemas;

namespace C971_Mobile_App_PA.ViewModels;

public partial class CourseDetails : ContentPage
{
    private Course _course;

    public CourseDetails(Course course)
    {
        InitializeComponent();
        _course = course;
        BindingContext = _course;

        // Handle the "Yes/No" logic for notifications manually
        string notifText = _course.notificationsEnabled == "true" ? "Yes" : "No";
        NotificationLabel.Text = $"Notifications Enabled?  {notifText}";
    }

    // 1. SHARE FUNCTIONALITY
    private async void OnShareNotesClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_course.CourseNotes))
        {
            await DisplayAlert("Info", "No notes to share.", "OK");
            return;
        }

        // This triggers the native Android Share Sheet
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = _course.CourseNotes,
            Title = $"Notes for {_course.CourseTitle}"
        });
    }

    // 2. Navigation Logic
    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnAssessmentClicked(object sender, EventArgs e)
    {
        // Pass the Course ID and Title to the new page
        await Navigation.PushAsync(new Views.CourseAssessments(_course.CourseId, _course.CourseTitle));
    }
}