using Google.Api.Gax.Grpc.Rest;
using Google.Cloud.TextToSpeech.V1;

using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;

using SipPOS.Services.Entity.Interfaces;

namespace SipPOS.Services.Entity.Implementations;

/// <summary>
/// Service for converting text to speech using Google Cloud Text-to-Speech API.
/// </summary>
public class TextToSpeechService : ITextToSpeechService
{
    private readonly TextToSpeechClient client = new TextToSpeechClientBuilder
    {
        GrpcAdapter = RestGrpcAdapter.Default,
        CredentialsPath = Package.Current.InstalledLocation.Path + "/Assets/Credentials/sippos-446716-3130d3000da3.json"
    }.Build();

    private readonly VoiceSelectionParams voiceSelection = new()
    {
        LanguageCode = "vi-VN",
        SsmlGender = SsmlVoiceGender.Female,
        Name = "vi-VN-Wavenet-A"
    };

    private readonly AudioConfig audioConfig = new()
    {
        AudioEncoding = AudioEncoding.Mp3
    };

    private readonly string tempraoryPath = Package.Current.InstalledLocation.Path + "/Assets/Credentials/payment.mp3";

    private readonly MediaPlayer mediaPlayer = new();

    /// <summary>
    /// Converts the given text to speech and plays it.
    /// </summary>
    /// <param name="text">The text to convert to speech.</param>
    /// <returns>True if the text was successfully converted and played, otherwise false.</returns>
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

    /// <summary>
    /// Converts a payment success message to speech and plays it.
    /// </summary>
    /// <param name="amount">The amount paid.</param>
    /// <returns>True if the message was successfully converted and played, otherwise false.</returns>
    public bool SpeakPaymentSuccess(decimal amount)
    {
        return Speak($"Thanh toán thành công {amount} đồng. Cảm ơn quý khách.");
    }

    /// <summary>
    /// Converts a cash payment success message to speech and plays it.
    /// </summary>
    /// <param name="amount">The amount paid.</param>
    /// <returns>True if the message was successfully converted and played, otherwise false.</returns>
    public bool SpeakPaymentSuccessViaCash(decimal amount)
    {
        return Speak($"Thanh toán thành công {amount} đồng bằng tiền mặt. Cảm ơn quý khách.");
    }

    /// <summary>
    /// Converts a QR payment success message to speech and plays it.
    /// </summary>
    /// <param name="amount">The amount paid.</param>
    /// <returns>True if the message was successfully converted and played, otherwise false.</returns>
    public bool SpeakPaymentSuccessViaQRPay(decimal amount)
    {
        return Speak($"Thanh toán thành công {amount} đồng bằng mã QR. Cảm ơn quý khách.");
    }
}
