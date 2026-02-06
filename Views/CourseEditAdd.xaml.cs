using C971_Mobile_App_PA.Schemas;
using C971_Mobile_App_PA.ViewModels;
using Plugin.LocalNotification;

namespace C971_Mobile_App_PA.Views;

public partial class CourseEditAdd : ContentPage
{
    private Course _course;
    private int _termId;

    // Constructor accepts a Course (null for new) and the TermId to link it to
    public CourseEditAdd(Course course, int termId)
    {
        InitializeComponent();
        _termId = termId;

        if (course != null)
        {
            // Edit Mode
            _course = course;
            Title = "Edit Course";
        }
        else
        {
            // Add Mode
            _course = new Course();
            _course.TermId = _termId; // LINK TO TERM
            _course.StartDate = DateTime.Today;
            _course.EndDate = DateTime.Today.AddMonths(1);
            Title = "Add Course";
        }

        BindingContext = _course;

        // Handle the string-to-boolean conversion for the switch
        NotificationSwitch.IsToggled = _course.notificationsEnabled == "true";
    }

    private async Task ScheduleNotifications(Course course)
    {
        // 1. Check if notifications are enabled
        if (course.notificationsEnabled != "true")
        {
            LocalNotificationCenter.Current.Cancel(course.CourseId);
            LocalNotificationCenter.Current.Cancel(course.CourseId + 10000);
            LocalNotificationCenter.Current.Cancel(course.CourseId + 20000);
            return;
        }

        // 2. Request Permission
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        // ---------------------------------------------------------
        // NOTIFICATION 1: Course Starts (Immediate Confirmation)
        // ---------------------------------------------------------
        var startNotification = new NotificationRequest
        {
            NotificationId = course.CourseId,
            Title = "Course Starting",
            Description = $"{course.CourseTitle} starts on {course.StartDate:MM/dd/yyyy}",
            Schedule = new NotificationRequestSchedule
            {
                // Trigger immediately (3 seconds delay) regardless of the actual date
                NotifyTime = DateTime.Now.AddSeconds(3)
            }
        };
        await LocalNotificationCenter.Current.Show(startNotification);

        // ---------------------------------------------------------
        // NOTIFICATION 2: Course Ends (Immediate Confirmation)
        // ---------------------------------------------------------
        var endNotification = new NotificationRequest
        {
            NotificationId = course.CourseId + 10000,
            Title = "Course Ending",
            Description = $"{course.CourseTitle} ends on {course.EndDate:MM/dd/yyyy}",
            Schedule = new NotificationRequestSchedule
            {
                // Trigger immediately (5 seconds delay) to prevent overlap
                NotifyTime = DateTime.Now.AddSeconds(5)
            }
        };
        await LocalNotificationCenter.Current.Show(endNotification);

        // ---------------------------------------------------------
        // NOTIFICATION 3: Reminder (1 Day Before End)
        // ---------------------------------------------------------
        var reminderDate = course.EndDate.Date.AddDays(-1);

        // Only schedule if the reminder date is in the future
        if (reminderDate >= DateTime.Today)
        {
            var reminderNotification = new NotificationRequest
            {
                NotificationId = course.CourseId + 20000,
                Title = "Course Ending Soon",
                Description = $"{course.CourseTitle} ends tomorrow! Wrap up your work.",
                Schedule = new NotificationRequestSchedule
                {
                    // Stays scheduled for Noon on the day before
                    NotifyTime = reminderDate.AddHours(12)
                }
            };
            await LocalNotificationCenter.Current.Show(reminderNotification);
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Validation
        if (string.IsNullOrWhiteSpace(_course.CourseTitle))
        {
            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Enter a course title", "OK");
            return;
        }
        // Validate Course Status
        if (string.IsNullOrWhiteSpace(_course.CourseStatus))
        {
            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Please select a Course Status.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(_course.InstructorName))
        {
            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Enter an instructor name", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(_course.InstructorEmail))
        {
            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Enter an instructor email", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(_course.InstructorPhone))
        {
            await Application.Current.Windows[0].Page.DisplayAlert("Error", "Enter an instructor phone", "OK");
            return;
        }

        // 2. Capture "New" status before saving
        bool isNew = _course.CourseId == 0;

        // 3. Confirm Save
        string action = isNew ? "add this course" : "save changes";
        bool confirm = await Application.Current.Windows[0].Page.DisplayAlert("Save", $"Do you want to {action}?", "Yes", "No");

        if (confirm)
        {
            // Convert Switch back to string
            _course.notificationsEnabled = NotificationSwitch.IsToggled ? "true" : "false";

            await App.Database.SaveCourseAsync(_course);

            // Schedule the alerts
            await ScheduleNotifications(_course);

            // Set the Toast Message for the Details Page
            TermDetails.ToastMessage = isNew ? "Course added successfully!" : "Course updated successfully!";

            await Navigation.PopAsync();
        }
        // If "No", do nothing and return to Term Details page without saving
        else
        {
            await Navigation.PopAsync();
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}