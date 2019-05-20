using System;
using System.Collections.Generic;
using System.Text;

namespace Osma.Mobile.App.Events
{
    public enum ApplicationEventType
    {
        ConnectionsUpdated
    }

    public class ApplicationEvent
    {
        public ApplicationEventType Type { get; set; }
    }
}
