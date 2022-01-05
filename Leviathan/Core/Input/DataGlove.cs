using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace Leviathan.Util
{
    public class DataGlove : IDataGloveListener
    {
        private Mutex quat_mutex;
        private Quaternion quat;
        private Quaternion quat2;
        private DataGloveSerialWorker worker;
        private SerialPort serialPort;
        public Quaternion Received_Quat
        {
            get
            {
                try
                {
                    quat_mutex.WaitOne();
                    return quat;
                } finally
                {
                    quat_mutex.ReleaseMutex();
                }
            }
            set
            {
                try
                {
                    quat_mutex.WaitOne();
                    quat = value;
                }
                finally
                {
                    quat_mutex.ReleaseMutex();
                }
            }
        }

        public Quaternion Received_Quat2
        {
            get
            {
                try
                {
                    quat_mutex.WaitOne();
                    return quat2;
                }
                finally
                {
                    quat_mutex.ReleaseMutex();
                }
            }
            set
            {
                try
                {
                    quat_mutex.WaitOne();
                    quat2 = value;
                }
                finally
                {
                    quat_mutex.ReleaseMutex();
                }
            }
        }
        public DataGlove(string comport, int baud)
        {
            quat = Quaternion.Identity;
            quat_mutex = new Mutex();
            serialPort = new SerialPort(comport, baud);
            serialPort.Open();
            worker = new DataGloveSerialWorker(serialPort, this);
            worker.Start();
        }

        public void OnQuaternionReceived(Quaternion quat, int id)
        {
            if(id == 8)
            {
                this.quat = quat;
            } else
            {
                this.quat2 = quat;
            }
        }
    }

    public class DataGloveSerialWorker
    {
        private Thread worker;
        private bool shutdownrequested;

        private SerialPort serial;
        private IDataGloveListener listener;

        public DataGloveSerialWorker(SerialPort _serialport, IDataGloveListener lst)
        {
            this.serial = _serialport;
            this.listener = lst;
        }

        public void Start()
        {
            if(worker == null)
            {
                worker = new Thread(new ThreadStart(ThreadFunc));
            } else
            {
                if(worker.IsAlive)
                {
                    throw new Exception("Could not start DataGloveSerialWorker");
                }
            }
            shutdownrequested = false;
            worker.Start();
        }

        public void RequestShutdown()
        {
            shutdownrequested = true;
            worker.Join();
        }

        private void ThreadFunc()
        {
            string line = string.Empty;
            if(!serial.IsOpen)
            {
                throw new Exception("Serial Port was not open");
            }

            while (serial.IsOpen && !shutdownrequested)
            {
                if(serial.BytesToRead > 0)
                {
                    line = serial.ReadLine();
                    Console.WriteLine(line);
                    String[] tokens = line.Split('\t');
                    if (tokens.Length == 5)
                    {
                        int id = int.Parse(tokens[0]);
                        float w = float.Parse(tokens[1]);
                        float x = float.Parse(tokens[2]);
                        float y = float.Parse(tokens[3]);
                        float z = float.Parse(tokens[4]);
                        Quaternion quat = new Quaternion(x, z, y, w);
                        listener.OnQuaternionReceived(quat, id);
                        
                    }
                }
            }
        }

    }

    public interface IDataGloveListener
    {
        void OnQuaternionReceived(Quaternion quat, int id);
    }

    public struct FingerOrient
    {
        Quaternion Distal;
        Quaternion Intermediate;
        Quaternion Proximal;

        public static readonly FingerOrient Identity = new FingerOrient()
        {
            Distal = Quaternion.Identity,
            Intermediate = Quaternion.Identity,
            Proximal = Quaternion.Identity
        };
    }

    public struct HandOrient
    {
        public FingerOrient Thumb_Finger;
        public FingerOrient Index_Finger;
        public FingerOrient Middle_Finger;
        public FingerOrient Ring_Finger;
        public FingerOrient Pinky_Finger;

        public static readonly HandOrient Identity = new HandOrient()
        {
            Thumb_Finger = FingerOrient.Identity,
            Index_Finger = FingerOrient.Identity,
            Middle_Finger = FingerOrient.Identity,
            Ring_Finger = FingerOrient.Identity,
            Pinky_Finger = FingerOrient.Identity
        };

    }

    
}

