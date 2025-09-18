package com.example.accelerometerapp

import android.content.Context
import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.media.MediaPlayer
import android.os.Bundle
import android.view.View
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import kotlin.math.sqrt

class MainActivity : AppCompatActivity(), SensorEventListener {

    private lateinit var sensorManager: SensorManager
    private var accelerometer: Sensor? = null
    
    // UI Elements
    private lateinit var accelerometerStatusTextView: TextView
    private lateinit var xAxisTextView: TextView
    private lateinit var yAxisTextView: TextView
    private lateinit var zAxisTextView: TextView
    private lateinit var movementTypeTextView: TextView
    private lateinit var horizontalIndicator: TextView
    private lateinit var verticalIndicator: TextView
    private lateinit var depthIndicator: TextView
    private lateinit var freeFallIndicator: TextView
    
    // MediaPlayer instances for different sounds
    private var horizontalMediaPlayer: MediaPlayer? = null
    private var verticalMediaPlayer: MediaPlayer? = null
    private var depthMediaPlayer: MediaPlayer? = null
    private var freeFallMediaPlayer: MediaPlayer? = null
    
    // Movement detection thresholds
    private val movementThreshold = 2.0f
    private val freeFallThreshold = 0.5f
    
    // Track last values and movement states
    private var lastX = 0f
    private var lastY = 0f
    private var lastZ = 0f
    private var isHorizontalMoving = false
    private var isVerticalMoving = false
    private var isDepthMoving = false
    private var isFreeFalling = false
    
    // Free fall detection variables
    private var freeFallCount = 0
    private val freeFallRequiredCount = 5 // Require 5 consecutive readings below threshold
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        
        initializeUI()
        initializeAccelerometer()
        initializeMediaPlayers()
    }
    
    private fun initializeUI() {
        accelerometerStatusTextView = findViewById(R.id.accelerometerStatusTextView)
        xAxisTextView = findViewById(R.id.xAxisTextView)
        yAxisTextView = findViewById(R.id.yAxisTextView)
        zAxisTextView = findViewById(R.id.zAxisTextView)
        movementTypeTextView = findViewById(R.id.movementTypeTextView)
        horizontalIndicator = findViewById(R.id.horizontalIndicator)
        verticalIndicator = findViewById(R.id.verticalIndicator)
        depthIndicator = findViewById(R.id.depthIndicator)
        freeFallIndicator = findViewById(R.id.freeFallIndicator)
    }
    
    private fun initializeAccelerometer() {
        sensorManager = getSystemService(Context.SENSOR_SERVICE) as SensorManager
        accelerometer = sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER)
        
        if (accelerometer != null) {
            accelerometerStatusTextView.text = getString(R.string.accelerometer_available)
            accelerometerStatusTextView.setTextColor(getColor(R.color.green))
        } else {
            accelerometerStatusTextView.text = getString(R.string.accelerometer_not_available)
            accelerometerStatusTextView.setTextColor(getColor(R.color.red))
        }
    }
    
    private fun initializeMediaPlayers() {
        try {
            // Note: In a real implementation, these would be actual audio files
            // For now, we'll create the MediaPlayer instances but they won't play actual sounds
            // since the files are placeholders
            horizontalMediaPlayer = MediaPlayer.create(this, R.raw.horizontal_movement)
            verticalMediaPlayer = MediaPlayer.create(this, R.raw.vertical_movement)
            depthMediaPlayer = MediaPlayer.create(this, R.raw.depth_movement)
            freeFallMediaPlayer = MediaPlayer.create(this, R.raw.free_fall)
        } catch (e: Exception) {
            // Handle case where sound files are not available
            e.printStackTrace()
        }
    }
    
    override fun onResume() {
        super.onResume()
        accelerometer?.also { sensor ->
            sensorManager.registerListener(this, sensor, SensorManager.SENSOR_DELAY_NORMAL)
        }
    }
    
    override fun onPause() {
        super.onPause()
        sensorManager.unregisterListener(this)
    }
    
    override fun onDestroy() {
        super.onDestroy()
        releaseMediaPlayers()
    }
    
    private fun releaseMediaPlayers() {
        horizontalMediaPlayer?.release()
        verticalMediaPlayer?.release()
        depthMediaPlayer?.release()
        freeFallMediaPlayer?.release()
    }
    
    override fun onSensorChanged(event: SensorEvent?) {
        if (event?.sensor?.type == Sensor.TYPE_ACCELEROMETER) {
            val x = event.values[0]
            val y = event.values[1]
            val z = event.values[2]
            
            // Update UI with current values
            updateAccelerometerValues(x, y, z)
            
            // Detect different types of movement
            detectMovement(x, y, z)
            
            // Detect free fall
            detectFreeFall(x, y, z)
            
            // Update last values
            lastX = x
            lastY = y
            lastZ = z
        }
    }
    
    private fun updateAccelerometerValues(x: Float, y: Float, z: Float) {
        xAxisTextView.text = String.format("X-axis: %.2f", x)
        yAxisTextView.text = String.format("Y-axis: %.2f", y)
        zAxisTextView.text = String.format("Z-axis: %.2f", z)
    }
    
    private fun detectMovement(x: Float, y: Float, z: Float) {
        val deltaX = kotlin.math.abs(x - lastX)
        val deltaY = kotlin.math.abs(y - lastY)
        val deltaZ = kotlin.math.abs(z - lastZ)
        
        // Reset movement states
        val wasHorizontalMoving = isHorizontalMoving
        val wasVerticalMoving = isVerticalMoving
        val wasDepthMoving = isDepthMoving
        
        isHorizontalMoving = deltaX > movementThreshold
        isVerticalMoving = deltaY > movementThreshold
        isDepthMoving = deltaZ > movementThreshold
        
        // Update visual indicators
        updateMovementIndicators()
        
        // Play sounds for new movements
        if (isHorizontalMoving && !wasHorizontalMoving) {
            playSound(horizontalMediaPlayer)
            movementTypeTextView.text = getString(R.string.horizontal_movement)
        }
        
        if (isVerticalMoving && !wasVerticalMoving) {
            playSound(verticalMediaPlayer)
            movementTypeTextView.text = getString(R.string.vertical_movement)
        }
        
        if (isDepthMoving && !wasDepthMoving) {
            playSound(depthMediaPlayer)
            movementTypeTextView.text = getString(R.string.depth_movement)
        }
        
        // Update movement status text
        if (!isHorizontalMoving && !isVerticalMoving && !isDepthMoving) {
            movementTypeTextView.text = getString(R.string.no_movement)
        }
    }
    
    private fun detectFreeFall(x: Float, y: Float, z: Float) {
        // Calculate total acceleration magnitude
        val totalAcceleration = sqrt(x * x + y * y + z * z)
        
        // Check if total acceleration is below free fall threshold
        if (totalAcceleration < freeFallThreshold) {
            freeFallCount++
            if (freeFallCount >= freeFallRequiredCount && !isFreeFalling) {
                isFreeFalling = true
                showFreeFallDetected()
                playSound(freeFallMediaPlayer)
            }
        } else {
            freeFallCount = 0
            if (isFreeFalling) {
                isFreeFalling = false
                hideFreeFallDetected()
            }
        }
    }
    
    private fun updateMovementIndicators() {
        // Update horizontal indicator (X-axis)
        if (isHorizontalMoving) {
            horizontalIndicator.setBackgroundColor(getColor(R.color.yellow))
            horizontalIndicator.setTextColor(getColor(R.color.black))
        } else {
            horizontalIndicator.setBackgroundColor(getColor(R.color.red))
            horizontalIndicator.setTextColor(getColor(R.color.white))
        }
        
        // Update vertical indicator (Y-axis)
        if (isVerticalMoving) {
            verticalIndicator.setBackgroundColor(getColor(R.color.yellow))
            verticalIndicator.setTextColor(getColor(R.color.black))
        } else {
            verticalIndicator.setBackgroundColor(getColor(R.color.green))
            verticalIndicator.setTextColor(getColor(R.color.white))
        }
        
        // Update depth indicator (Z-axis)
        if (isDepthMoving) {
            depthIndicator.setBackgroundColor(getColor(R.color.yellow))
            depthIndicator.setTextColor(getColor(R.color.black))
        } else {
            depthIndicator.setBackgroundColor(getColor(R.color.blue))
            depthIndicator.setTextColor(getColor(R.color.white))
        }
    }
    
    private fun showFreeFallDetected() {
        freeFallIndicator.text = getString(R.string.free_fall_detected)
        freeFallIndicator.visibility = View.VISIBLE
    }
    
    private fun hideFreeFallDetected() {
        freeFallIndicator.visibility = View.GONE
    }
    
    private fun playSound(mediaPlayer: MediaPlayer?) {
        try {
            mediaPlayer?.let { player ->
                if (player.isPlaying) {
                    player.seekTo(0) // Restart if already playing
                } else {
                    player.start()
                }
            }
        } catch (e: Exception) {
            // Handle case where sound files are not available
            e.printStackTrace()
        }
    }
    
    override fun onAccuracyChanged(sensor: Sensor?, accuracy: Int) {
        // Not used in this implementation
    }
}