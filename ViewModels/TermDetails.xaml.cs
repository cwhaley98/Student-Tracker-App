using C971_Mobile_App_PA.Schemas;
using C971_Mobile_App_PA.Views;

namespace C971_Mobile_App_PA.ViewModels;

public partial class TermDetails : ContentPage
{
    private Term _selectedTerm;

    // 1. Shared Mailbox for Toast Messages
    public static string ToastMessage = "";

    public TermDetails(Term term)
    {
        InitializeComponent();
        _selectedTerm = term;

        // Populate static data
        TermTitleLabel.Text = _selectedTerm.TermTitle;
        TermDateLabel.Text = $"{_selectedTerm.TermStart:MM/dd/yyyy} - {_selectedTerm.TermEnd:MM/dd/yyyy}";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCourses();

        // 2. Check for success messages from CourseEditAdd page
        if (!string.IsNullOrEmpty(ToastMessage))
        {
            ShowSuccess(ToastMessage);
            ToastMessage = "";
        }
    }

    private async void OnCourseSelected(object sender, EventArgs e)
    {
        try
        {
            var label = (Label)sender;
            var tapGesture = (TapGestureRecognizer)label.GestureRecognizers[0];
            var course = (Course)tapGesture.CommandParameter;

            // Navigate to the Details page, passing the selected term
            await Navigation.PushAsync(new CourseDetails(course));
        }
        catch (Exception ex)
        {
            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Navigation failed: " + ex.Message, "OK");
        }
    }

    private async Task LoadCourses()
    {
        // Fetch courses for THIS term only
        var courses = await App.Database.GetCoursesAsync(_selectedTerm.TermId);
        CoursesCollection.ItemsSource = courses;
        CoursesHeaderLabel.Text = $"Courses ({courses.Count})";
    }

    private async void ShowSuccess(string message)
    {
        SuccessLabel.Text = message;
        SuccessBanner.IsVisible = true;
        await Task.Delay(3000);
        SuccessBanner.IsVisible = false;
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        // Pass 'null' for course, but 'TermId' is required to link it
        await Navigation.PushAsync(new CourseEditAdd(null, _selectedTerm.TermId));
    }

    private async void OnEditCourseClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var course = (Course)button.CommandParameter;
        // Pass the existing course and the termId
        await Navigation.PushAsync(new CourseEditAdd(course, _selectedTerm.TermId));
    }

    private async void OnDeleteCourseClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var course = (Course)button.CommandParameter;

        bool confirm = await Application.Current.Windows[0].Page.DisplayAlert("Confirm", $"Delete '{course.CourseTitle}'?", "Yes", "No");
        if (confirm)
        {
            await App.Database.DeleteCourseAsync(course);
            ShowSuccess("Course deleted successfully!");
            await LoadCourses();
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}