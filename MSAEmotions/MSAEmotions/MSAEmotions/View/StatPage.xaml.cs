using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MSAEmotions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatPage : ContentPage
    {
        public StatPage()
        {
            InitializeComponent();
        }
        async void RefreshStats(object sender, EventArgs e)
        {
            loading.IsRunning = true;
            List<EmotionTable> emotionInformation = await EasyTablesBackend.EasyTablesBackendInstance.GetEmotion();
            EmotionCount.Text = Convert.ToString(emotionInformation.Count);
            CommonMood.Text = Convert.ToString(emotionInformation.GroupBy(s => s.Emotion).OrderByDescending(s => s.Count()).First().Key);
            loading.IsRunning = false;
        }
    }
}