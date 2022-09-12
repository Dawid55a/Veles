using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veles_Application.ViewModels;

namespace Veles_Application
{
    //Sending evants between views
    public static class EventsAggregator
    {
        public static void SendMessage(object message)
        {
            if (OnMessageTransmitted != null)
                OnMessageTransmitted(message);
        }

        public static void SwitchPage(BaseViewModel message)
        {
            if (OnPageTransmitted != null)
                OnPageTransmitted(message);
        }

        

        public static Action<object> OnMessageTransmitted = null!;
        public static Action<BaseViewModel> OnPageTransmitted = null!;
       

    }
}
