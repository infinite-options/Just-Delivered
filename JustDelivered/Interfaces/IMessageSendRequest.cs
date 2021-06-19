using System;
using System.IO;

namespace JustDelivered.Interfaces
{
    public interface IMessageSendRequest
    {
        string SendTextMessage();
        string SendMessage(Stream stream, string[] recipients, string message);
    }
}
