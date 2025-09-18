# Accelerometer App

An Android application that demonstrates advanced accelerometer functionality with sound feedback for different types of device movement.

## Features

### 1. Accelerometer Detection
- Detects if the device has an accelerometer sensor
- Displays the availability status in the UI
- Shows real-time accelerometer values for X, Y, and Z axes

### 2. Movement Direction Detection
The app detects and responds to movement in different directions:

- **Horizontal Movement (X-axis)**: Left-right movement
- **Vertical Movement (Y-axis)**: Up-down movement  
- **Depth Movement (Z-axis)**: Forward-backward movement (toward/away from user)

### 3. Sound Feedback
Different sounds are played for each type of movement:
- `horizontal_movement.mp3` - for X-axis movement
- `vertical_movement.mp3` - for Y-axis movement
- `depth_movement.mp3` - for Z-axis movement
- `free_fall.mp3` - for free fall detection

### 4. Free Fall Detection
- Detects when the device is in free fall
- Uses total acceleration magnitude below threshold
- Requires multiple consecutive readings to avoid false positives
- Special visual and audio alert when detected

### 5. Visual Feedback
- Real-time accelerometer values display
- Color-coded movement indicators:
  - **Red (X)**: Horizontal movement indicator
  - **Green (Y)**: Vertical movement indicator  
  - **Blue (Z)**: Depth movement indicator
  - **Yellow**: Active movement state
  - **Orange**: Free fall alert

## Technical Implementation

### MainActivity.kt Features:
- Implements `SensorEventListener` for accelerometer data
- Uses `MediaPlayer` for sound playback
- Threshold-based movement detection
- Smooth UI updates with movement state tracking
- Proper resource management for sensors and media players

### Movement Detection Algorithm:
- Compares current accelerometer values with previous readings
- Uses configurable thresholds for movement sensitivity
- Separate detection for each axis (X, Y, Z)
- Debouncing to prevent rapid state changes

### Free Fall Detection:
- Calculates total acceleration magnitude: `sqrt(x² + y² + z²)`
- Compares against free fall threshold (0.5 m/s²)
- Requires 5 consecutive readings below threshold
- Prevents false positives from brief sensor noise

## File Structure

```
AccelerometerApp/
├── app/
│   ├── src/main/
│   │   ├── java/com/example/accelerometerapp/
│   │   │   └── MainActivity.kt
│   │   ├── res/
│   │   │   ├── layout/
│   │   │   │   └── activity_main.xml
│   │   │   ├── values/
│   │   │   │   ├── strings.xml
│   │   │   │   ├── colors.xml
│   │   │   │   └── styles.xml
│   │   │   └── raw/
│   │   │       ├── horizontal_movement.mp3
│   │   │       ├── vertical_movement.mp3
│   │   │       ├── depth_movement.mp3
│   │   │       └── free_fall.mp3
│   │   └── AndroidManifest.xml
│   └── build.gradle
├── build.gradle
└── settings.gradle
```

## Usage

1. Install the app on an Android device
2. Launch the app to see accelerometer status
3. Move the device in different directions to trigger sounds
4. Observe the visual indicators changing colors
5. Drop the device gently to test free fall detection (be careful!)

## Requirements

- Android API level 21 (Android 5.0) or higher
- Device with accelerometer sensor
- Audio playback capability

## Notes

- Sound files are currently placeholders - replace with actual audio files for full functionality
- Movement thresholds can be adjusted in the code for different sensitivity levels
- The app includes proper lifecycle management for sensors and media resources