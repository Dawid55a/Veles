using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veles_Application.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string User { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public Message(string User, string Text)
        {
            this.User = User;
            this.Text = Text;
        }
    }
}
