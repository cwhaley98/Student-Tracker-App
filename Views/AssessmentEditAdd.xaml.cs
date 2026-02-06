using C971_Mobile_App_PA.Schemas;
using Plugin.LocalNotification;

namespace C971_Mobile_App_PA.Views;

public partial class AssessmentEditAdd : ContentPage
{
    private Assessment _assessment;
    private int _courseId;

    public AssessmentEditAdd(Assessment assessment, int courseId)
    {
        InitializeComponent();
        _courseId = courseId;

        if (assessment != null)
        {
            _assessment = assessment;
            Title = "Edit Assessment";
        }
        else
        {
            _assessment = new Assessment
            {
                CourseId = _courseId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(7)
            };
            Title = "Add Assessment";
        }

        BindingContext = _assessment;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_assessment.AssessmentTitle))
        {
            await DisplayAlert("Error", "Please enter a title.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(_assessment.AssessmentType))
        {
            await DisplayAlert("Error", "Please select an Assessment Type.", "OK");
            return;
        }

        bool isNew = _assessment.AssessmentId == 0;

        await App.Database.SaveAssessmentAsync(_assessment);
        await ScheduleNotifications(_assessment);

        CourseAssessments.ToastMessage = isNew ? "Assessment added!" : "Assessment updated!";
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // --- NOTIFICATION LOGIC (Same as Course, adapted for Assessment) ---
    private async Task ScheduleNotifications(Assessment assessment)
    {
        // Use AssessmentId for IDs to avoid collision with Course IDs
        // We add 100,000 to the ID to keep it safe from Course IDs (assuming < 100k courses)
        int startId = assessment.AssessmentId + 100000;
        int endId = assessment.AssessmentId + 200000;

        if (!assessment.notificationsEnabled)
        {
            LocalNotificationCenter.Current.Cancel(startId);
            LocalNotificationCenter.Current.Cancel(endId);
            return;
        }

        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        // 1. Start Notification (Immediate)
        var startRequest = new NotificationRequest
        {
            NotificationId = startId,
            Title = "Assessment Starting",
            Description = $"{assessment.AssessmentTitle} starts on {assessment.StartDate:MM/dd/yyyy}",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(3) // Immediate
            }
        };
        await LocalNotificationCenter.Current.Show(startRequest);

        // 2. End Notification (Immediate)
        var endRequest = new NotificationRequest
        {
            NotificationId = endId,
            Title = "Assessment Due",
            Description = $"{assessment.AssessmentTitle} is due on {assessment.EndDate:MM/dd/yyyy}",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(5) // Immediate
            }
        };
        await LocalNotificationCenter.Current.Show(endRequest);
    }
}