using System;

namespace AgainstTheStormTrainer.Core
{
    /// <summary>
    /// Provides a speed hack by modifying the game's time scale.
    /// Toggles between normal speed and 2x speed.
    /// </summary>
    public class SpeedHack : IDisposable
    {
        private readonly GameMemoryManager _memoryManager;
        private readonly IntPtr _timeScaleAddress;

        /// <summary>
        /// Gets the current speed multiplier (1.0f for normal, 2.0f for fast).
        /// </summary>
        public float CurrentMultiplier { get; private set; }

        /// <summary>
        /// Initializes the speed hack with the time scale memory address.
        /// </summary>
        /// <param name="memoryManager">Active game memory manager.</param>
        public SpeedHack(GameMemoryManager memoryManager)
        {
            _memoryManager = memoryManager;
            // Example address for time scale (needs updating per patch)
            _timeScaleAddress = new IntPtr(0x00B2E4A0);
            CurrentMultiplier = 1.0f;
        }

        /// <summary>
        /// Toggles the game speed between normal and 2x.
        /// </summary>
        public void ToggleSpeed()
        {
            if (CurrentMultiplier == 1.0f)
            {
                if (_memoryManager.WriteFloat(_timeScaleAddress, 2.0f))
                    CurrentMultiplier = 2.0f;
            }
            else
            {
                if (_memoryManager.WriteFloat(_timeScaleAddress, 1.0f))
                    CurrentMultiplier = 1.0f;
            }
        }

        /// <summary>
        /// Disposes the speed hack (no unmanaged resources).
        /// </summary>
        public void Dispose()
        {
            // Optionally reset speed on exit
            if (_memoryManager.IsProcessOpen)
                _memoryManager.WriteFloat(_timeScaleAddress, 1.0f);
        }
    }
}
