using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Diagnostics;

namespace MSAEmotions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmotionsPage : ContentPage
    {
        EmotionServiceClient emotionClient;
        MediaFile photo;

        public EmotionsPage()
        {
            InitializeComponent();
            emotionClient = new EmotionServiceClient("bf64f07d4b994504a092daced559e522");
        }

        async void OnPhotoClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "emotion.jpg",
                    PhotoSize = PhotoSize.Small
                });

                if (photo != null)
                {
                    image.Source = ImageSource.FromStream(photo.GetStream);
                }
                else
                {
                    return;
                }
            }
            else
            {
                await DisplayAlert("No Camera", "Camera unavailable.", "OK");
            }

            ((Button)sender).IsEnabled = false;
            activityIndicator.IsRunning = true;

            // Recognize emotion
            try
            {
                if (photo != null)
                {
                    using (var photoStream = photo.GetStream())
                    {
                        Emotion[] emotionResult = await emotionClient.RecognizeAsync(photoStream);
                        if (emotionResult.Any())
                        {
                            // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                            emotionResultLabel.Text = emotionResult.FirstOrDefault().Scores.ToRankedList().FirstOrDefault().Key;
                        }
                        photo.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (emotionResultLabel.Text != null)
            {
                await postEmotionAsync();
            }

            activityIndicator.IsRunning = false;
            ((Button)sender).IsEnabled = true;
        }

        async Task postEmotionAsync()
        {
            EmotionTable model = new EmotionTable()
            {
                Emotion = emotionResultLabel.Text
            };
            await EasyTablesBackend.EasyTablesBackendInstance.PostEmotion(model);
        }
    }
}
    