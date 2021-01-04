using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvSerialCommunicator.Messaging
{
    /// <summary>
    /// Used for sending messages to the serial port (on the main thread...). Just an easier way to manage receiving/sending tbh
    /// </summary>
    public class MessageSender
    {
        public bool CanSend { get; set; }
        public SerialPort Port { get; set; }
        public MessagesViewModel Messages { get; set; }

        public MessageSender()
        {
            CanSend = true;
        }

        /// <summary>
        /// Sends a message through the serial port. This COULD throw exceptions which can be handled somewhere else
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message, bool shouldSendNewLine = true)
        {
            // Adds a new line if needed
            string newMessage = message + (shouldSendNewLine ? "\n" : "");
            // Gets the bytes of the message using the serial port's encoding
            // Will be using a byte buffer because it's faster than sending strings using 
            // the serial port's build in methods... apparently
            byte[] buffer = Port.Encoding.GetBytes(newMessage);

            for(int i = 0; i < buffer.Length; i++)
            {
                Port.BaseStream.WriteByte(buffer[i]);
            }
        }
    }
}
