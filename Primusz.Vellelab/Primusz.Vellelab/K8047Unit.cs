using System.Runtime.InteropServices;

namespace Primusz.Vellelab
{
    static class K8047Unit
    {
        #region Native Library

        private const string NativeLibrary = "K8047D.dll";

        #endregion

        #region Calling Convention

        /// <summary>
        /// Specifies the calling convention.
        /// </summary>
        /// <remarks>
        /// Specifies <see cref="CallingConvention.Cdecl" /> for Windows and Linux.
        /// </remarks>
        private const CallingConvention Convention = CallingConvention.Cdecl;

        #endregion

        #region Native Functions

        [DllImport(NativeLibrary, CallingConvention = Convention)]
        public static extern void StartDevice();

        [DllImport(NativeLibrary, CallingConvention = Convention)]
        public static extern void StopDevice();

        [DllImport(NativeLibrary, CallingConvention = Convention, EntryPoint = "LEDon")]
        public static extern void LedOn();

        [DllImport(NativeLibrary, CallingConvention = Convention, EntryPoint = "LEDoff")]
        public static extern void LedOff();

        [DllImport(NativeLibrary, CallingConvention = Convention)]
        public static extern void ReadData([MarshalAs(UnmanagedType.LPArray)] int[] volts);

        [DllImport(NativeLibrary, CallingConvention = Convention)]
        public static extern void SetGain(int channel, int gain);

        #endregion
    }
}
