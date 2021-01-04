using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvSerialCommunicator.Messaging
{
    public class MessagesViewModel : BaseViewModel
    {
        private int _messagesCount;
        private string _messagesText;
        private string _toBeSentText;

        public int MessagesCount
        {
            get => _messagesCount;
            set => RaisePropertyChanged(ref _messagesCount, value);
        }

        public string MessagesText
        {
            get => _messagesText;
            set => RaisePropertyChanged(ref _messagesText, value);
        }

        public string ToBeSentText
        {
            get => _toBeSentText;
            set => RaisePropertyChanged(ref _toBeSentText, value);
        }

        public Command ClearMessagesCommand { get; }
        public Command SendMessageCommand { get; }

        public MessageSender Sender { get; set; }

        public MessagesViewModel()
        {
            MessagesCount = 0;
            MessagesText = "";
            ToBeSentText = "";

            ClearMessagesCommand = new Command(ClearMessages);
            SendMessageCommand = new Command(SendMessage);
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(ToBeSentText))
            {
                try
                {
                    Sender.SendMessage(ToBeSentText);
                    AddSentMessage(ToBeSentText);
                    // Clear text after sending. this is a personal preference ;)
                    ToBeSentText = "";
                }
                catch(TimeoutException timeout)
                {
                    AddMessage("Timeout Exception. Couldn't send message");
                }
                catch (Exception e)
                {
                    AddMessage("Error: " + e.Message);
                }
            }
        }

        private void ClearMessages()
        {
            MessagesText = "";
            MessagesCount = 0;
        }

        public void AddSentMessage(string message)
        {
            // (Date) | TX> hello there
            AddMessage($"{DateTime.Now} | TX> {message}");
        }

        public void AddReceivedMessage(string message)
        {
            // (Date) | RX> hello there
            AddMessage($"{DateTime.Now} | RX> {message}");
        }

        public void AddMessage(string message)
        {
            WriteLine(message);
            MessagesCount++;
        }

        public void WriteLine(string text)
        {
            MessagesText += text + '\n';
        }
    }
}
