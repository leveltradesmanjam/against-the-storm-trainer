using System;
using System.Collections.Generic;

namespace AgainstTheStormTrainer.Core
{
    /// <summary>
    /// Provides cheats for modifying in-game resources (food, wood, etc.).
    /// Uses memory offsets derived from game version analysis.
    /// </summary>
    public class ResourceHack : IDisposable
    {
        private readonly GameMemoryManager _memoryManager;
        private readonly Dictionary<string, IntPtr> _resourceAddresses;

        /// <summary>
        /// Initializes the resource hack with known memory offsets.
        /// NOTE: Offsets are placeholders and will need updating per game patch.
        /// </summary>
        /// <param name="memoryManager">Active game memory manager.</param>
        public ResourceHack(GameMemoryManager memoryManager)
        {
            _memoryManager = memoryManager;
            // Base address for resource array (example offset, not real)
            IntPtr baseAddress = new IntPtr(0x00A3F7C0);
            _resourceAddresses = new Dictionary<string, IntPtr>
            {
                { "food", IntPtr.Add(baseAddress, 0x10) },
                { "wood", IntPtr.Add(baseAddress, 0x14) },
                { "stone", IntPtr.Add(baseAddress, 0x18) },
                { "metal", IntPtr.Add(baseAddress, 0x1C) }
            };
        }

        /// <summary>
        /// Sets all known resources to a specific value.
        /// </summary>
        /// <param name="value">Desired resource amount.</param>
        /// <returns>True if all writes succeeded.</returns>
        public bool SetAllResources(int value)
        {
            bool success = true;
            foreach (var kvp in _resourceAddresses)
            {
                if (!_memoryManager.WriteInt(kvp.Value, value))
                {
                    Console.WriteLine($"Failed to set {kvp.Key}.");
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        /// Gets the current value of a specific resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource (e.g., "food").</param>
        /// <returns>Current value, or -1 if not found.</returns>
        public int GetResourceValue(string resourceName)
        {
            if (_resourceAddresses.TryGetValue(resourceName, out var address))
            {
                return _memoryManager.ReadInt(address);
            }
            return -1;
        }

        /// <summary>
        /// Disposes the resource hack (no unmanaged resources here, but follows pattern).
        /// </summary>
        public void Dispose()
        {
            // No additional cleanup needed; memory manager handles process.
        }
    }
}
