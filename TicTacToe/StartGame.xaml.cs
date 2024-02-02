


namespace TicTacToe;

public partial class StartGame : ContentPage
{
    public String DifficultyLevel { get; set; } 

    public String Dimension { get; set; }

    public StartGame()
    {
        BindingContext = this;
        InitializeComponent();
        

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        int enteredDimension;

        bool isValid = int.TryParse(Dimension, out enteredDimension);
        bool validDimension = enteredDimension >= 2 && enteredDimension <= 10 && isValid;
        if (validDimension)
            await Navigation.PushAsync(new Home( DifficultyLevel, enteredDimension, new char[enteredDimension, enteredDimension]));

        else
            await DisplayAlert("Invalid Number", "Please enter a valid number for dimension", "OK");
    }

    private void RadioButtonEasy_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        DifficultyLevel = "Easy";
    }

    private void RadioButtonHard_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        DifficultyLevel = "Hard";
    }
}