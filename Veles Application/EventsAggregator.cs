﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veles_Application
{
    public static class EventsAggregator
    {
        public static void BroadCast(object message)
        {
            if (OnMessageTransmitted != null)
                OnMessageTransmitted(message);
        }

        public static Action<object> OnMessageTransmitted = null!;
    }
}