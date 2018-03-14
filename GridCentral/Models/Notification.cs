using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCentral.Models
{
    public class Notification
    {
        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public NotificationType Type
        {
            get;
            set;
        }
    }

    public enum NotificationType
    {
        Confirmation, Notification, Success, Error, Warning
    }
}
