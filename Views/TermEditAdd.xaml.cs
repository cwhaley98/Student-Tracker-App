using Student_Tracker_App.Schemas;

namespace Student_Tracker_App.Views;

public partial class TermEditAdd : ContentPage
{
    //Local term variable to hold the term being edited or added
    private Term _term;

    // Constructor accepts a Term object (can be null for adding new term)
    public TermEditAdd(Term termToEdit)
    {
        InitializeComponent();

        if (termToEdit != null)
        {
            // EDIT MODE: Use the existing term passed from the list
            _term = termToEdit;
            Title = "Edit Term";
        }
        else
        {
            // ADD MODE: Create a new term instance
            _term = new Term();
            _term.TermStart = DateTime.Today;
            _term.TermEnd = DateTime.Today.AddMonths(6); // Default 6 month term
            Title = "Add Term";
        }

        // Bind the term data to the UI
        BindingContext = _term;

    }

    // Logic for the Calendar Icon Clicks
    private void OnStartDateCalendarClicked(object sender, EventArgs e)
    {
        StartDatePicker.Focus();
    }

    private void OnEndDateCalendarClicked(object sender, EventArgs e)
    {
        EndDatePicker.Focus();
    }

    // Save button click handler
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Basic Validation
        if (string.IsNullOrWhiteSpace(_term.TermTitle))
        {
            await DisplayAlert("Validation Error", "Please enter a Term Title.", "OK");
            return;
        }

        if (_term.TermEnd < _term.TermStart)
        {
            await DisplayAlert("Validation Error", "End Date cannot be before Start Date.", "OK");
            return;
        }

        // CAPTURE STATUS BEFORE SAVING
        // Check if ID is 0 now, because after the save line runs, it will no longer be 0 afterwards
        bool isNew = _term.TermId == 0;

        // Confirmation Alert Logic
        bool confirm = false;

        // Check if we are adding a new term (ID is 0) or editing an existing one
        if (isNew)
        {
            // ADD Mode Confirmation
            confirm = await DisplayAlert("Add Term", "Do you want to add this term?", "Yes", "No");
        }
        else
        {
         // EDIT Mode Confirmation
         confirm = await DisplayAlert("Save Changes", "Do you want to save changes to this term?", "Yes", "No");
        }

        // Process the Result of the Confirmation
        if (confirm)
        {
            // If "Yes", save to database and navigate back
            await App.Database.SaveTermAsync(_term);
            
            // Set the message for the List page directly
            if (isNew)
            {
                TermsList.ToastMessage = "Term added!";
            }
            else
            {
                TermsList.ToastMessage = "Term updated!";
            }

            await Navigation.PopAsync();
        }
        else
        {
            //If "No", do nothing and return to the listing without saving
            await Navigation.PopAsync();
        }
    }

    // Cancel button click handler
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // Just go back without saving
        await Navigation.PopAsync();
    }

    // Logic for DateSelected
    //Used for immediate validation
    private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        // Check if dates are invalid (End before Start)
        if (_term.TermEnd < _term.TermStart)
        {
            await DisplayAlert("Validation Error", "End Date cannot be before Start Date.", "OK");
        }
    }
}