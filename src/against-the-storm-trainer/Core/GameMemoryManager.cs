using System;
using System.Diagnostics;
using System.Linq;
using MemorySharp;

namespace AgainstTheStormTrainer.Core
{
    /// <summary>
    /// Manages access to the game process memory using MemorySharp.
    /// Provides safe read/write operations and handles process attachment.
    /// </summary>
    public class GameMemoryManager : IDisposable
    {
        private readonly string _processName;
        private SharpMemory? _sharpMemory;

        /// <summary>
        /// Gets whether the game process is currently open and attached.
        /// </summary>
        public bool IsProcessOpen => _sharpMemory != null && _sharpMemory.IsRunning;

        /// <summary>
        /// Initializes a new instance and attempts to attach to the game process.
        /// </summary>
        /// <param name="processName">Name of the game process (without .exe).</param>
        public GameMemoryManager(string processName)
        {
            _processName = processName;
            AttachToProcess();
        }

        /// <summary>
        /// Attaches to the first running instance of the specified process.
        /// </summary>
        private void AttachToProcess()
        {
            try
            {
                var processes = Process.GetProcessesByName(_processName);
                if (processes.Length == 0)
                {
                    Console.WriteLine($"Process '{_processName}' not found.");
                    return;
                }

                var targetProcess = processes.First();
                _sharpMemory = new SharpMemory(targetProcess);
                Console.WriteLine($"Attached to process ID: {targetProcess.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to attach to process: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads an integer value from a given memory address.
        /// </summary>
        /// <param name="address">Memory address to read from.</param>
        /// <returns>The integer value, or 0 on failure.</returns>
        public int ReadInt(IntPtr address)
        {
            if (!IsProcessOpen) return 0;
            try
            {
                return _sharpMemory!.Read<int>(address);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Writes an integer value to a given memory address.
        /// </summary>
        /// <param name="address">Memory address to write to.</param>
        /// <param name="value">Value to write.</param>
        /// <returns>True if successful.</returns>
        public bool WriteInt(IntPtr address, int value)
        {
            if (!IsProcessOpen) return false;
            try
            {
                _sharpMemory!.Write(address, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reads a float value from a given memory address.
        /// </summary>
        /// <param name="address">Memory address to read from.</param>
        /// <returns>The float value, or 0f on failure.</returns>
        public float ReadFloat(IntPtr address)
        {
            if (!IsProcessOpen) return 0f;
            try
            {
                return _sharpMemory!.Read<float>(address);
            }
            catch
            {
                return 0f;
            }
        }

        /// <summary>
        /// Writes a float value to a given memory address.
        /// </summary>
        /// <param name="address">Memory address to write to.</param>
        /// <param name="value">Value to write.</param>
        /// <returns>True if successful.</returns>
        public bool WriteFloat(IntPtr address, float value)
        {
            if (!IsProcessOpen) return false;
            try
            {
                _sharpMemory!.Write(address, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Disposes the MemorySharp instance, releasing the process handle.
        /// </summary>
        public void Dispose()
        {
            _sharpMemory?.Dispose();
            _sharpMemory = null;
        }
    }
}
