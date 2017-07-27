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
    public partial class RecentPage : ContentPage
    {
        public RecentPage()
        {
            InitializeComponent();
        }

        async void SyncHistory(object sender, EventArgs e)
        {
            loading.IsRunning = true;
            List<RecentSpellChecks> emotionInformation = await EasyTablesBackend.EasyTablesBackendInstance.GetEmotion();
            EmotionList.ItemsSource = emotionInformation;
            loading.IsRunning = false;
        }
    }
}