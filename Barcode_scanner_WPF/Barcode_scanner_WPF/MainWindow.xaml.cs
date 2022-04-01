using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Barcode_scanner_WPF
{
    public partial class MainWindow : Window
    {
        SerialPort MySerial = new SerialPort();
        static string SerialData = null;

        public MainWindow()
        {
            InitializeComponent();
            setupSerial(MySerial);

        }
        static void setupSerial(SerialPort serial)
        {
            string[] comPorts = SerialPort.GetPortNames();
            serial.PortName = "COM6";
            serial.Handshake = Handshake.None;
            serial.BaudRate = 56000;
            serial.DataBits = 8;
            serial.Parity = Parity.None;
            serial.StopBits = StopBits.One;
            serial.ReadTimeout = 75;
            serial.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            serial.Open();
            Console.WriteLine(serial.IsOpen);
        }
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string data = null;
            SerialPort sp = (SerialPort)sender;
            char indata = (char)sp.ReadChar();
            while (indata != 3)
            {
                data += indata;
                indata = (char)sp.ReadChar();
            }
            SerialData = data;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textbx1.Text = "....";
            Button1.Content = "Scanning...";
            SerialData = "NULL";
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate { }));
            MySerial.Write(" ");
            while (SerialData == "NULL")
            {
                Thread.Sleep(10);
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate { }));
            }
            textbx1.Text = SerialData;
            Button1.Content = "Start Scanning";
        }

    }
}
