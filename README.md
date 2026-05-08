# Primusz.Vellelab

A robust C# library for interfacing with the K8047 USB A/D converter, enabling precise data acquisition and hardware control for laboratory and industrial applications.

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Framework%204.5+-blue.svg)]()

## Features

- **Multi-channel A/D conversion** - Support for 4 independent input channels
- **Programmable gain amplifier** - Selectable gain settings (1×, 2×, 5×, 10×) for enhanced measurement precision
- **Singleton device management** - Thread-safe device instance with synchronized access
- **Hardware LED control** - Visual device status indication
- **High-precision timing** - 10ms timer counter for synchronized data acquisition
- **Voltage calibration** - Automatic scaling based on selected gain levels

## System Requirements

- **.NET Framework** 4.5 or higher
- **Windows** OS (USB driver support via K8047D.dll)
- **K8047 hardware** connected via USB
- **Administrator privileges** for driver installation

## Installation

### NuGet
```bash
Install-Package Primusz.Vellelab
```

### Manual
1. Clone the repository:
```bash
git clone https://github.com/primusz/Vellelab.git
```

2. Build the solution:
```bash
msbuild Primusz.Vellelab/Primusz.Vellelab.sln
```

3. Reference the compiled DLL in your project

## Quick Start

```csharp
using Primusz.Vellelab;

// Initialize the device
Device device = Device.Instance;
device.Start();

// Read data from all channels
VoltageData data = device.Read();
Console.WriteLine($"Channel 1: {data.Ch1}V");
Console.WriteLine($"Channel 2: {data.Ch2}V");
Console.WriteLine($"Channel 3: {data.Ch3}V");
Console.WriteLine($"Channel 4: {data.Ch4}V");
Console.WriteLine($"Timer: {data.Time}ms");

// Cleanup
device.Stop();
```

## API Documentation

### Device Class

The `Device` class is a singleton that manages the K8047 hardware interface.

#### Properties

- **Instance** - Gets the singleton device instance (thread-safe)
- **Channels** - Read-only collection of Channel objects (indexed 0-3)
- **LedStatus** - Gets/sets the status LED state (true = on, false = off)

#### Methods

```csharp
public void Start()
```
Initializes USB communication and loads the K8047 drivers. Must be called before any data operations.

```csharp
public void Stop()
```
Closes USB communication and unloads drivers. Should be called during application cleanup.

```csharp
public VoltageData Read()
```
Reads current A/D converter values from all 4 channels and returns calibrated voltage data.

```csharp
public void ReadData(int[] buffer)
```
Low-level method to read raw A/D converter values. Buffer must be 8 integers in size.

```csharp
public void LedOn()
```
Turns on the device status LED.

```csharp
public void LedOff()
```
Turns off the device status LED.

### Channel Class

Represents an individual input channel with independent gain configuration.

#### Properties

- **Number** - Channel number (1-4, read-only)
- **Gain** - Input amplifier gain level (1, 2, 5, 10)
- **Voltage** - Full-scale input voltage based on current gain:
  - Gain 1×: 30V
  - Gain 2×: 15V
  - Gain 5×: 6V
  - Gain 10×: 3V

#### Example

```csharp
Channel ch1 = device.Channels[0];
ch1.Gain = GainLevel.Two;  // Set to 2× gain, 15V full-scale
```

### VoltageData Class

Contains the A/D converter readings from a single acquisition cycle.

#### Properties

- **Time** - Timer counter value (in 10ms increments)
- **Ch1, Ch2, Ch3, Ch4** - Voltage values (double) for each channel, calibrated to the selected gain

### GainLevel Enum

Specifies the input amplifier gain configuration:

```csharp
public enum GainLevel
{
    One = 1,    // 1× gain
    Two = 2,    // 2× gain
    Five = 5,   // 5× gain
    Ten = 10    // 10× gain
}
```

## Advanced Usage

### Continuous Data Acquisition

```csharp
Device device = Device.Instance;
device.Start();

while (acquiring)
{
    VoltageData data = device.Read();
    
    // Process voltage data
    ProcessMeasurement(data);
    
    Thread.Sleep(100);  // Respect device timing constraints
}

device.Stop();
```

### Dynamic Gain Adjustment

```csharp
// Switch channels to different gain levels based on signal magnitude
if (Math.Abs(data.Ch1) > 25)
{
    device.Channels[0].Gain = GainLevel.Ten;  // Lower sensitivity
}
else if (Math.Abs(data.Ch1) < 1)
{
    device.Channels[0].Gain = GainLevel.One;  // Higher sensitivity
}
```

## Dependencies

- **K8047D.dll** - Native USB interface library (included in K8047 driver package)

## Building

```bash
# Clone repository
git clone https://github.com/primusz/Vellelab.git
cd Vellelab

# Build in Debug mode
msbuild Primusz.Vellelab/Primusz.Vellelab.sln /p:Configuration=Debug

# Build in Release mode
msbuild Primusz.Vellelab/Primusz.Vellelab.sln /p:Configuration=Release
```

## Thread Safety

The Device singleton implementation is fully thread-safe:
- Double-checked locking pattern for instance creation
- Volatile field for instance reference
- Thread-safe channel access via IList

However, concurrent `Read()` calls should be serialized to prevent race conditions with the underlying driver.

## Troubleshooting

### "K8047D.dll not found"
- Ensure K8047 USB driver is installed
- Verify K8047D.dll is in the system PATH or application directory

### "Invalid channel number"
- Channel numbers must be 1-4
- Device.Channels is 0-indexed (use indices 0-3)

### No data received
- Verify K8047 device is connected via USB
- Call `device.Start()` before `device.Read()`
- Check Windows Device Manager for K8047 device presence

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

For issues, questions, or suggestions, please visit the [GitHub Issues](https://github.com/primusz/Vellelab/issues) page.

## Author

Vellelab Team - Data Acquisition Solutions

---

**Note:** This library requires the K8047 USB A/D converter hardware and corresponding drivers to function properly.
