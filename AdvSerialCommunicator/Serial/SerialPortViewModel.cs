using AdvSerialCommunicator.Messaging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvSerialCommunicator.Serial
{
    public class SerialPortViewModel : BaseViewModel
    {
        // will be used to bind to the currently connected port
        private string _connectedPort;
        public string ConnectedPort
        {
            get => _connectedPort;
            set => RaisePropertyChanged(ref _connectedPort, value);
        }

        public SerialPort Port { get; set; }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => RaisePropertyChanged(ref _isConnected, value);
        }

        public void CloseAll()
        {
            Disconnect();
            Receiver.StopThreadLoop();
        }

        // Because a button is used to connect/disconnect, well....
        public Command AutoConnectDisconnectCommand { get; }
        // NOt really that useful, but can be used to clear the serialport's receive/send buffers, but they probably wont fill up unless you send a giant message and noone responds... sort of
        public Command ClearBuffersCommand { get; }

        public PortSettingsViewModel Settings { get; set; }

        public MessagesViewModel Messages { get; set; }

        public MessageReceiver Receiver { get; set; }

        public MessageSender Sender { get; set; }

        public SerialPortViewModel()
        {
            Port = new SerialPort();
            Port.ReadTimeout = 1000;
            Port.WriteTimeout = 1000;
            Settings = new PortSettingsViewModel();

            AutoConnectDisconnectCommand = new Command(AutoConnectDisconnect);
            ClearBuffersCommand = new Command(ClearBuffers);
        }

        public void AutoConnectDisconnect()
        {
            IsConnected = Port.IsOpen;
            if (IsConnected)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        public void Connect()
        {
            IsConnected = Port.IsOpen;
            if (IsConnected)
            {
                Messages.AddMessage("Port is already open!");
                return;
            }
            // COM1 is a system com port and can't be used
            if (Settings.SelectedCOMPort == "COM1")
            {
                Messages.AddMessage("Cannot use COM1!");
                return;
            }

            if (string.IsNullOrEmpty(Settings.SelectedCOMPort))
            {
                Messages.AddMessage("Error with the COM port");
            }
            Port.PortName = Settings.SelectedCOMPort;

            try
            {
                Port.Open();
            }
            catch(Exception e)
            {
                // stacktrace is cooler ;)
                Messages.AddMessage("Error opening port: " + e.Message);
                return;
            }
            ConnectedPort = Settings.SelectedCOMPort;
            Messages.AddMessage($"Connected to {ConnectedPort}!");

            IsConnected = Port.IsOpen;
            Receiver.CanReceive = true;
        }

        public void Disconnect()
        {
            IsConnected = Port.IsOpen;
            if (!IsConnected)
            {
                Messages.AddMessage("Port is already closed!");
                return;
            }

            try
            {
                Port.Close();
            }
            catch (Exception e)
            {
                // stacktrace is cooler ;)
                Messages.AddMessage("Error closing port: " + e.Message);
                return;
            }

            Messages.AddMessage($"Disconnected from {ConnectedPort}!");
            ConnectedPort = "(None)";
            // Stops it using resources... sort of
            IsConnected = Port.IsOpen;
            Receiver.CanReceive = false;
        }

        private void ClearBuffers()
        {
            if (!Port.IsOpen)
            {
                Messages.AddMessage("You need to be connected to clear the buffers");
                return;
            }

            Port.DiscardInBuffer();
            Port.DiscardOutBuffer();
        }
    }
}
