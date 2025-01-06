namespace SipPOS.Services.Entity.Interfaces;

/// <summary>
/// Interface for text-to-speech services.
/// </summary>
public interface ITextToSpeechService
{
    /// <summary>
    /// Converts the given text to speech and plays it.
    /// </summary>
    /// <param name="text">The text to convert to speech.</param>
    /// <returns>True if the text was successfully converted and played, otherwise false.</returns>
    bool Speak(string text);

    /// <summary>
    /// Converts a payment success message to speech and plays it.
    /// </summary>
    /// <param name="amount">The amount paid.</param>
    /// <returns>True if the message was successfully converted and played, otherwise false.</returns>
    bool SpeakPaymentSuccess(decimal amount);

    /// <summary>
    /// Converts a QR payment success message to speech and plays it.
    /// </summary>
    /// <param name="amount">The amount paid.</param>
    /// <returns>True if the message was successfully converted and played, otherwise false.</returns>
    bool SpeakPaymentSuccessViaQRPay(decimal amount);

    /// <summary>
    /// Converts a cash payment success message to speech and plays it.
    /// </summary>
    /// <param name="amount">The amount paid.</param>
    /// <returns>True if the message was successfully converted and played, otherwise false.</returns>
    bool SpeakPaymentSuccessViaCash(decimal amount);
}
