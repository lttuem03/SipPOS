using Google.Api.Gax.Grpc.Rest;
using Google.Cloud.TextToSpeech.V1;
using SipPOS.Services.Entity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace SipPOS.Services.Entity.Implementations
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private TextToSpeechClient client = new TextToSpeechClientBuilder
        {
            GrpcAdapter = RestGrpcAdapter.Default,
            CredentialsPath = Package.Current.InstalledLocation.Path + "/Assets/Credentials/sippos-446716-3130d3000da3.json"
        }.Build();

        private VoiceSelectionParams voiceSelection = new VoiceSelectionParams
        {
            LanguageCode = "vi-VN",
            SsmlGender = SsmlVoiceGender.Female,
            Name = "vi-VN-Wavenet-A"
        };

        private AudioConfig audioConfig = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Mp3
        };

        private string tempraoryPath = Package.Current.InstalledLocation.Path + "/Assets/Credentials/payment.mp3";

        private MediaPlayer mediaPlayer = new MediaPlayer();

        public bool Speak(string text)
        {
            try
            {
                var input = new SynthesisInput
                {
                    Text = text
                };

                var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

                using (var output = File.Create(tempraoryPath))
                {
                    response.AudioContent.WriteTo(output);
                    mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(tempraoryPath));
                    mediaPlayer.Play();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SpeakPaymentSuccess(decimal amount)
        {
            return Speak($"Thanh toán thành công {amount} đồng. Cảm ơn quý khách.");
        }

        public bool SpeakPaymentSuccessViaCash(decimal amount)
        {
            return Speak($"Thanh toán thành công {amount} đồng bằng tiền mặt. Cảm ơn quý khách.");
        }

        public bool SpeakPaymentSuccessViaQRPay(decimal amount)
        {
            return Speak($"Thanh toán thành công {amount} đồng bằng mã QR. Cảm ơn quý khách.");
        }
    }
}
