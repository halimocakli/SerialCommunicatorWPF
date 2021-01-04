using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRFramework.Utilities;

namespace AdvSerialCommunicator.Serial
{
    /// <summary>
    /// This could be used in the future to bind the serial port's settings to the view, such as
    /// the BaudRate, DataBits, parity, etc.
    /// </summary>
    public class PortSettingsViewModel : BaseViewModel
    {
        // Will only bind the COM port because the others aren't really needed, unless you need them :////
        private string _selectedComPort;
        public string SelectedCOMPort
        {
            get => _selectedComPort;
            set => RaisePropertyChanged(ref _selectedComPort, value);
        }

        public ObservableCollection<string> AvaliablePorts { get; set; }

        // one of the most common baud rate
        public int BaudRate = 9600;

        // i think determinds how many bits are in 1 byte. so 8. could also use 5.. if you wanted to
        public int DataBits = 8;

        // I think the serialport only supports none, probably
        public StopBits StopBits = StopBits.None;

        // The serialport doesn't support all of the parity options, i think such as mark/even
        public Parity Parity = Parity.None;

        // not using a handshake
        public Handshake Handshake = Handshake.None;

        public Command RefreshPortsCommand { get; }

        public PortSettingsViewModel()
        {
            AvaliablePorts = new ObservableCollection<string>();

            RefreshPortsCommand = new Command(RefreshPorts);

            RefreshPorts();
        }

        private void RefreshPorts()
        {
            AvaliablePorts.Clear();
            foreach(string port in SerialPort.GetPortNames())
            {
                AvaliablePorts.Add(port);
            }
        }
    }
}
