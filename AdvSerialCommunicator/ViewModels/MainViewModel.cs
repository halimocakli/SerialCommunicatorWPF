using AdvSerialCommunicator.Messaging;
using AdvSerialCommunicator.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvSerialCommunicator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MessagesViewModel Messages { get; set; }

        public MessageReceiver Receiver { get; set; }

        public MessageSender Sender { get; set; }

        public SerialPortViewModel SerialPort { get; set; }

        public MainViewModel()
        {
            SerialPort = new SerialPortViewModel();
            Receiver = new MessageReceiver();
            Sender = new MessageSender();
            Messages = new MessagesViewModel();

            // hmm
            Receiver.Messages = Messages;
            Messages.Sender = Sender;
            Sender.Messages = Messages;

            SerialPort.Receiver = Receiver;
            SerialPort.Sender = Sender;
            SerialPort.Messages = Messages;

            Receiver.Port = SerialPort.Port;
            Sender.Port = SerialPort.Port;
        }
    }
}
