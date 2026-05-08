using System;

namespace Primusz.Vellelab
{
    public class Channel
    {
        #region Members

        private GainLevel gain;

        #endregion

        #region Properties

        /// <summary>
        /// Individual channel number.
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// The input amplifier gain value.
        /// </summary>
        public GainLevel Gain
        {
            get { return gain; }
            set
            {
                gain = value;
                SetGain(Number, gain);
            }
        }

        /// <summary>
        /// Full scale input voltage.
        /// </summary>
        public int Voltage
        {
            get
            {
                int voltage;

                switch (Gain)
                {
                    case GainLevel.One:
                        voltage = 30;
                        break;
                    case GainLevel.Two:
                        voltage = 15;
                        break;
                    case GainLevel.Five:
                        voltage = 6;
                        break;
                    case GainLevel.Ten:
                        voltage = 3;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return voltage;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Individual channel
        /// </summary>
        /// <param name="number">Between 1 and 4 which corresponds to the input channel number whose gain data is to be changed.</param>
        public Channel(int number)
            : this(number, GainLevel.One)
        { }

        /// <summary>
        /// Individual channel
        /// </summary>
        /// <param name="number">Between 1 and 4 which corresponds to the input channel number whose gain data is to be changed.</param>
        /// <param name="gain">The input amplifier gain value. Valid values are only: 1, 2, 5 and 10.</param>
        public Channel(int number, GainLevel gain)
        {
            if (!(number > 0 && number <= 4))
                throw new ArgumentException("Invalid channel number: " + number, "number");

            Number = number;
            Gain = gain;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Selects one of the input amplifier hardware pre-selected gain settings.
        /// The corresponding full scale (A/D output 255) voltages are following:
        /// Gain: 1=30V, 2=15V, 5=6V, 10=3V
        /// </summary>
        /// <param name="channel">Between 1 and 4 which corresponds to the input channel number whose gain data is to be changed.</param>
        /// <param name="gain">The input amplifier gain value. Valid values are only: 1, 2, 5 and 10.</param>
        public static void SetGain(int channel, GainLevel gain)
        {
            // Check Bounds
            if (!(channel > 0 && channel <= 4))
                throw new ArgumentException("Invalid channel number: " + channel, "channel");

            K8047Unit.SetGain(channel, (int) gain);
        }

        #endregion
    }
}
