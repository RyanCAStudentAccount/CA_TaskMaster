using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System;

namespace CA_TaskMaster.Views
{
    public partial class MotivationPage : ContentPage
    {
        private readonly List<string> _motivationQuotes = new List<string>
        {
            "Believe you can and you're halfway there. - Theodore Roosevelt",
            "Don't watch the clock; do what it does. Keep going. - Sam Levenson",
            "The secret of getting ahead is getting started. - Mark Twain",
            "The best way to predict the future is to create it. - Peter Drucker",
            "Start where you are. Use what you have. Do what you can. - Arthur Ashe",
            "The future depends on what you do today. - Mahatma Gandhi",
        };

        public MotivationPage()
        {
            InitializeComponent();
            DisplayRandomMotivationQuote();
        }

        private void DisplayRandomMotivationQuote()
        {
            Random random = new Random();
            int index = random.Next(_motivationQuotes.Count);
            MotivationQuoteLabel.Text = _motivationQuotes[index];
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
