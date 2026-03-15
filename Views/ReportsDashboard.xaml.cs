namespace Student_Tracker_App.Views;

public partial class ReportsDashboard : ContentPage
{
	public ReportsDashboard()
	{
		InitializeComponent();
	}

	// OnAppearing runs every time the page appears
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadReportDataAsync();
    }

    private async Task LoadReportDataAsync()
    {
        // 1. Fetch the categorized data just once
        var statusReport = await App.Database.GetCourseStatusReportAsync();

        // 2. Bind the list to the screen
        StatusReportView.ItemsSource = statusReport;

        // 3. Calculate the Grand Total instantly from the data we just pulled
        int grandTotal = statusReport.Sum(x => x.Count);

        // 4. Update the Grand Total UI label
        GrandTotalLabel.Text = grandTotal.ToString();
    }
}