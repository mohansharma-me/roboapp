using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Threading;

namespace USBModem
{
    public enum CREG
    {
        NOTREGISTERED_NOTSEARCHING,
        REGISTERED_HOMENETWORK,
        NOTREGISTERED_SEARCHING,
        REGISTRATION_DENIED,
        UNKNOWN,
        REGISTERED_ROAMING,
        NONE
    }

    public enum CSIGNAL
    {
        Low,
        Ok,
        Good,
        Excellent,
        Full,
        No,
        Nothing
    }


    public enum MessageSent
    {
        CANT_SEND,
        SENT,
        ERROR
    }

    public enum MessageType
    {
        ALL,
        REC_UNREAD,
        REC_READED,
        STO_SENT,
        STO_UNSENT
    }

    public enum Connection
    {
        Connected,
        Disconnected
    }

    public class Port
    {
        /* Exception lists */
        public Exception PortNotOpenException = new Exception("Can't open port.");
        private bool notification = true;
        private SerialPort port = null;
        private bool isUsed = false;
        public bool disableAutoThreadStart = false;
        private static long staticlock = 1;

        public delegate void ErrorHandler(Port port,String errTitle,String errMsg,Exception exception);
        public delegate void MessageHandler(Port port,List<Message> receivedMessages);
        public delegate void ConnectionHandler(Port port,Connection connEvent);
        public delegate void NetworkHandler(Port port,CSIGNAL signal,CREG registration);

        public event ErrorHandler HandleError;
        public event MessageHandler MessageReceived;
        public event ConnectionHandler USBConnection;
        public event NetworkHandler SignalChanged;

        private Thread thrMessages, thrConnection, thrSignal;

        public Port()
        {
            port = new SerialPort();
            disableAutoThreadStart = false;
            ConnectionState = Connection.Disconnected;
            TypeOfMessage = MessageType.REC_UNREAD;
            RemoveReadedMessages = true;
            port.Encoding = Encoding.GetEncoding("iso-8859-1");
            setCommonFields();
            getSignal = true;
            HandleError += new ErrorHandler(Port_HandleError);
            MessageReceived += new MessageHandler(Port_MessageReceived);
            SignalChanged += new NetworkHandler(Port_SignalChanged);
            USBConnection += new ConnectionHandler(Port_USBConnection);
        }

        void Port_USBConnection(Port port, Connection connectionEvent)
        {
        }
        void Port_SignalChanged(Port port, CSIGNAL signal,CREG registration)
        {
        }
        void Port_MessageReceived(Port port,List<Message> receivedMessages)
        {
            //donothing
        }
        void Port_HandleError(Port port, string errTitle, string errMsg, Exception exception)
        {
            //do nothing
        }

        public Port(String portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            port = new SerialPort();
            disableAutoThreadStart = false;
            ConnectionState = Connection.Disconnected;
            TypeOfMessage = MessageType.REC_UNREAD;
            RemoveReadedMessages = true;
            setCommonFields();
            port.PortName = portName;
            port.BaudRate = baudRate;
            port.DataBits = dataBits;
            port.Parity = parity;
            port.StopBits = stopBits;
        }

        public void startThreads()
        {
            try
            {
                thrSignal = new Thread(new ThreadStart(_thrSignal));
                thrConnection = new Thread(new ThreadStart(_thrConnection));
                thrConnection.Start();
                thrSignal.Start();
            }
            catch (Exception) { }
            //thrMessages.Start();
        }

        public void stopThreads()
        {
            try
            {
                if(thrConnection!=null && thrConnection.IsAlive)
                    thrConnection.Abort();
                //if (thrMessages != null && thrConnection.IsAlive)
                    //thrMessages.Abort();
                if (thrSignal != null && thrConnection.IsAlive)
                    thrSignal.Abort();
                ConnectionState = Connection.Disconnected;
            }
            catch (Exception) { }
        }

        public long AccessControl()
        {
            Console.WriteLine("NewKey Call: "+staticlock);
            while (isUsed)
                Thread.Sleep(500);
            isUsed = true;
            long k = ++staticlock;
            Console.WriteLine("NewKey Picked:"+k);
            return k;
        }
        public void FreeControl(long key) {
            Console.WriteLine("OldKey Call:" + key);
            if (staticlock == key) { 
                isUsed = false; 
                Console.WriteLine("Released:" + key); 
            } 
        }

        private void _thrConnection()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (port.CDHolding)
                        {
                            ConnectionState = Connection.Connected;
                            USBConnection(this, Connection.Connected);
                        }
                        else
                        {
                            ConnectionState = Connection.Disconnected;
                            USBConnection(this, Connection.Disconnected);
                        }
                    }
                    catch (Exception) { }
                    Thread.Sleep(2000);
                }
            }
            catch (ThreadAbortException) { }
            try
            {
                Thread.ResetAbort();
            }
            catch (Exception) { }
        }
        private void _thrSignal()
        {
            long key = -1;
            try
            {
                while (true)
                {
                    if (ConnectionState == Connection.Disconnected)
                        break;
                    try
                    {
                        if (isOpen())
                        {
                            key = AccessControl();
                                port.WriteLine("at+csq");//write("at+csq");
                                Thread.Sleep(300);
                                String data = port.ReadExisting();//read();
                            FreeControl(key);
                            int _cr = data.LastIndexOf("+CSQ: ");
                            data = data.Substring(_cr + 6);
                            String[] arr = data.Split(new char[] { ',' });
                            if (arr.Length == 2)
                            {
                                try
                                {
                                    CREG reg = _creg();
                                    int val = Int32.Parse(arr[0].Trim());
                                    if (val >= 2 && val <= 9)
                                    {
                                        SignalChanged(this, CSIGNAL.Low,reg);
                                    }
                                    else if (val >= 10 && val <= 14)
                                    {
                                        SignalChanged(this, CSIGNAL.Ok,reg);
                                    }
                                    else if (val >= 15 && val <= 19)
                                    {
                                        SignalChanged(this, CSIGNAL.Good,reg);
                                    }
                                    else if (val >= 20 && val <= 30)
                                    {
                                        SignalChanged(this, CSIGNAL.Full,reg);
                                    }
                                    else if (val == 31)
                                    {
                                        SignalChanged(this, CSIGNAL.Excellent,reg);
                                    }
                                    else if (val == 99)
                                    {
                                        SignalChanged(this, CSIGNAL.No,reg);
                                    }
                                    else
                                    {
                                        SignalChanged(this, CSIGNAL.Nothing,reg);
                                    }
                                }
                                catch (Exception)
                                {
                                    SignalChanged(this, CSIGNAL.Nothing,CREG.UNKNOWN);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { FreeControl(key); }

                    String input = "";
                    try
                    {
                        String mttt = TypeOfMessage.ToString().Replace("_", " ");
                        key = AccessControl();
                            port.WriteLine("at+cmgl=\"" + mttt + "\"");//write("at+cmgl=\"" + mttt + "\"");
                            Thread.Sleep(1000);
                            input = port.ReadExisting();//input = read();
                        FreeControl(key);
                        List<Message> msgs = new List<Message>();
                        try
                        {
                            Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)");//\r\n
                            Match m = r.Match(input);
                            while (m.Success)
                            {
                                Message msg = new Message();
                                msg.Index = m.Groups[1].Value;
                                msg.Status = m.Groups[2].Value;
                                msg.Sender = m.Groups[3].Value;
                                msg.Alphabet = m.Groups[4].Value;
                                msg.Time = m.Groups[5].Value;
                                msg.MessageText = m.Groups[6].Value;
                                msgs.Add(msg);

                                m = m.NextMatch();
                            }
                        }
                        catch (Exception ex)
                        { FreeControl(key); }



                        if (input.ToLower().Contains("ok"))
                        {
                            input = "ok";
                            MessageReceived(this, msgs);
                        }
                        if (input.ToLower().Contains("error"))
                        {
                            input = "false";
                        }
                        if (RemoveReadedMessages)
                        {
                            port.WriteLine("at+cmgd=1,3");//write("AT+CMGD=1,3");
                        }

                    }
                    catch (Exception ex)
                    {
                        if (notification)
                            showError("Reading Messages", "Error in raading messages.", ex);
                    }

                    try
                    {
                        port.BaseStream.Flush();
                    } catch(Exception) {}

                    Thread.Sleep(5000);
                }
            }
            catch (ThreadAbortException)
            {}
            try
            {
                Thread.ResetAbort();
            }
            catch (Exception) { }
        }

        public volatile MessageType TypeOfMessage;
        public volatile bool RemoveReadedMessages;
        public volatile Connection ConnectionState;

        private void setCommonFields()
        {
            port.PortName = "EMPTY";
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.RtsEnable = true;
            port.DtrEnable = true;
            port.Handshake = Handshake.RequestToSend;
            port.ReceivedBytesThreshold = 1;
            port.NewLine = Environment.NewLine;
            port.ReadTimeout = port.WriteTimeout = 4000;
        }


        /// <summary>
        /// Set port name if port is invalid then portname will be EMPTY.
        /// </summary>
        /// <param name="portname"></param>
        public void setPort(String portname)
        {
            if (portname.Length > 0)
            {
                port.PortName = portname;
            }
            else
            {
                port.PortName = "EMPTY";
            }
        }

        private void showError(String title,String msg,Exception ex)
        {
            HandleError(this, title, msg, ex);
        }

        /// <summary>
        /// Check whether port is opened or not.
        /// </summary>
        /// <returns></returns>
        public bool isOpen()
        {
            return port.IsOpen; 
        }

        /// <summary>
        /// Open port connection for communication.
        /// </summary>
        public void open()
        {
            if (!port.PortName.Equals("EMPTY"))
            {
                try
                {
                    port.Open();
                    if (!disableAutoThreadStart)
                        startThreads();
                }
                catch (Exception ex)
                {
                    showError("Error","Error in opening modem:" + port.PortName,ex);
                }
            }
            else
            {
                showError("Port number is invalid.", "Please specify the port number.",new Exception("Port number is invalid."));
            }
        }

        /// <summary>
        /// Flush the basestream of port connection.
        /// </summary>
        public void flush() { try { port.BaseStream.Flush(); } catch (Exception ex) { } }

        /// <summary>
        /// Read from port
        /// </summary>
        /// <returns></returns>
        public String read()
        {
            flush();
            if (port.IsOpen)
            {
                String msg = "", line = "";
                do
                {
                    try
                    {
                        line = port.ReadLine();
                    }
                    catch (TimeoutException tex) { line = ""; }
                    catch (Exception ex)
                    {
                        showError("Reading modem", "Error in reading from modem.", ex);
                        line = "";
                    }
                    if (line.Length > 0)
                    {
                        msg += Environment.NewLine + line;
                    }
                } while (line.Length != 0);
                try
                {
                    port.DiscardInBuffer();
                }
                catch (Exception) { }
                return msg;
            }
            else
            {
                showError("Modem closed", "Please open the port for communication first!",new Exception("Modem closed."));
                return null;
            }
        }

        /// <summary>
        /// Write to port.
        /// </summary>
        /// <param name="txt"></param>
        public void write(String txt)
        {
            flush();            
            if (port.IsOpen)
            {
                try
                {
                    port.WriteLine(txt);
                }
                catch (Exception ex)
                {
                    showError("Write modem", "Error in writing to modem", ex);
                }
            }
            else
            {
                showError("Modem Closed", "Please open the port for communication first!",new Exception("Modem closed."));
            }
            try
            {
                //port.DiscardInBuffer();
                port.DiscardOutBuffer();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Get direct access to serial port
        /// </summary>
        /// <returns></returns>
        public SerialPort getDirect()
        {
            return port;
        }

        /// <summary>
        /// Get all available ports attached to the system.
        /// </summary>
        /// <returns></returns>
        public static String[] getAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Close port connection.
        /// </summary>
        public void close()
        {
            try
            {
                port.Close();
                stopThreads();
                ConnectionState = Connection.Disconnected;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Set to trigger error handler or not.
        /// </summary>
        public bool Notification
        {
            set
            {
                notification = value;
            }
            get
            {
                return notification;
            }
        }

        public bool getSignal { get; set; }
        
        /// <summary>
        /// This method is for fetching network status of sim card.
        /// Returns the status through enum: CREG
        /// </summary>
        public CREG _creg()
        {
            long key = -1;
            CREG retCREG = CREG.NONE;
            try
            {
                if (isOpen())
                {
                    key = AccessControl();
                        port.WriteLine("at+creg?");
                        Thread.Sleep(500);
                        String data = port.ReadExisting();
                    FreeControl(key);
                    int _cr = data.LastIndexOf("+CREG: ");
                    data = data.Substring(_cr + 7, 3);
                    String[] arr = data.Split(new char[] { ',' });
                    if (arr.Length == 2)
                    {
                        try
                        {
                            retCREG = (CREG)Int32.Parse(arr[1]);
                        }
                        catch (Exception ex) { retCREG = CREG.UNKNOWN; }
                    }
                }
            }
            catch (Exception ex)
            {
                FreeControl(key);
                retCREG = CREG.UNKNOWN;
                if (notification)
                    showError("Network Error","Unable to get response from network.",ex);
            }
            return retCREG;
        }

        /// <summary>
        /// Set the modem into text mode.
        /// </summary>
        public bool _cmgf()
        {
            long key=-1;
            bool ret = false;
            try
            {
                if (isOpen())
                {
                    key = AccessControl();
                        port.WriteLine("at+cmgf=1");
                        Thread.Sleep(500);
                        String data = port.ReadExisting();
                    FreeControl(key);
                    if (data.ToUpper().Contains("OK"))
                    {
                        ret=true;
                    }
                }
            }
            catch (Exception ex)
            {
                FreeControl(key);
                if (notification)
                    showError("Message format", "Unable to set message format.",ex);
            }
            return ret;
        }

        /// <summary>
        /// Verify that modem is present or not.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="text"></param>
        /// <returns>True/False</returns>
        public bool _at()
        {
            bool flag = false;
            if (isOpen())
            {
                write("AT");
                Thread.Sleep(500);
                if (read().Contains("OK"))
                {
                    return true;
                }
            }
            return flag;
        }


        /// <summary>
        /// This will send the text message to given to given cell number.
        /// Returns:
        ///     ERROR1, ERROR2,
        ///     OK,
        ///     FALSE,
        ///     TEXTMODE, AT
        /// </summary>
        /// <param name="number"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public MessageSent sendMessage(String number, String text)
        {
            long key = -1;
            String input="";
            if (isOpen())
            {
                try
                {
                    ConnectionState = Connection.Connected;
                    flush();
                    String command = "AT+CMGS=\"" + number + "\"";
                    key = AccessControl();
                        port.WriteLine(command);//write(command);
                        Thread.Sleep(300);
                        command = text;
                        port.WriteLine(command);//write(command);
                        Thread.Sleep(300);
                        write(Convert.ToString((char)(26)));
                        Thread.Sleep(300);
                        input = port.ReadExisting();
                    FreeControl(key);
                }
                catch (Exception ex)
                {
                    FreeControl(key);
                    return MessageSent.ERROR;
                }

                if (input.ToUpper().Contains("OK"))
                {
                    return MessageSent.SENT;
                }
                else if (input.ToUpper().Contains("ERROR"))
                {
                    showError("MSg Error",input,new Exception());
                    return MessageSent.ERROR;
                }
            }
            return MessageSent.CANT_SEND;
        }

    }

    public class Message
    {
        public string Index { get; set; }
        public string Sender { get; set; }
        public string Alphabet { get; set; }
        public string MessageText { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
    }
}
