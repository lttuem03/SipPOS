using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Services.Entity.Interfaces
{
    public interface ITextToSpeechService
    {
        bool Speak(string text);

        bool SpeakPaymentSuccess(decimal amount);

        bool SpeakPaymentSuccessViaQRPay(decimal amount);

        bool SpeakPaymentSuccessViaCash(decimal amount);
    }
}
