using System;
using System.Collections.Generic;

namespace Primusz.Vellelab
{
    public sealed class Device
    {
        #region Members

        private static volatile Device instance;
        private static readonly object SyncRoot = new Object();
        private static bool ledStatus;

        #endregion

        #region Properties

        public static Device Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                            instance = new Device();
                    }
                }
                return instance;
            }
        }

        public IList<Channel> Channels { get; private set; }

        public bool LedStatus
        {
            get
            {
                return ledStatus;
            }
            set
            {
                ledStatus = value;

                if (ledStatus)
                {
                    LedOn();
                }
                else
                {
                    LedOff();
                }
            }
        }

        #endregion

        #region Constructors

        private Device()
        {
            Channels = new List<Channel>
            {
                new Channel(1, GainLevel.One),
                new Channel(2, GainLevel.One),
                new Channel(3, GainLevel.One),
                new Channel(4, GainLevel.One)
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the communication routines for the K8047 unit. Loads the drivers needed to communicate via the USB port.
        /// This procedure must be performed in the beginning of the application program.
        /// </summary>
        public void Start()
        {
            K8047Unit.StartDevice();
            LedOn();
        }

        /// <summary>
        /// Unloads the communication routines for K8047 unit and unloads the drivers needed to communicate via the USB port.
        /// This is the last action of the application program before termination.
        /// </summary>
        public void Stop()
        {
            K8047Unit.StopDevice();
            LedOff();
        }

        /// <summary>
        /// Reads the timer counter status and the A/D data from the K8047 to a buffer in the application program.
        /// The timer counter is incremented every 10 ms.
        /// The new data from the A/D converter channels 1...4 is updated every time the timer counter is incremented.
        /// </summary>
        /// <param name="buffer">A pointer to the data array of 8 integers where the data will be read.</param>
        public void ReadData(int[] buffer)
        {
            K8047Unit.ReadData(buffer);
        }

        /// <summary>
        /// Read data from the A/D converter channels.
        /// </summary>
        /// <returns>Returns channel voltages.</returns>
        public VoltageData Read()
        {
            int[] buffer = new int[8];

            ReadData(buffer);

            VoltageData data = new VoltageData
            {
                Time = buffer[0] + (buffer[1] << 8),
                Ch1 = Math.Round((buffer[2] / 255d) * Channels[0].Voltage, 2),
                Ch2 = Math.Round((buffer[3] / 255d) * Channels[1].Voltage, 2),
                Ch3 = Math.Round((buffer[4] / 255d) * Channels[2].Voltage, 2),
                Ch4 = Math.Round((buffer[5] / 255d) * Channels[3].Voltage, 2)
            };

            return data;
        }

        /// <summary>
        /// Turns the Record LED on.
        /// </summary>
        public void LedOn()
        {
            K8047Unit.LedOn();
            ledStatus = true;
        }

        /// <summary>
        /// Turns the Record LED off.
        /// </summary>
        public void LedOff()
        {
            K8047Unit.LedOff();
            ledStatus = false;
        }

        #endregion
    }
}