using System;
namespace Helseboka.Core.Legedialog.Model
{
    public class CreateThreadRequest : SendMessageRequest
    {
        public String title { get; set; }

        public String doctorId { get; set; }

        public String officeId { get; set; }

        public String chatType { get; set; }
    }
}
