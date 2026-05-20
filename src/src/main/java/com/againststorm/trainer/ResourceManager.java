package com.againststorm.trainer;

import java.util.HashMap;
import java.util.Map;
import java.util.Optional;
import java.util.logging.Logger;

/**
 * Manages resource addresses and values for Against the Storm game.
 * Stores known memory offsets for key resources like food, wood, and tools.
 */
public class ResourceManager {

    private static final Logger LOG = Logger.getLogger(ResourceManager.class.getName());
    private final MemoryReader reader;
    private final Map<String, Long> resourceAddresses;

    /**
     * Constructs a ResourceManager with a MemoryReader and predefined offsets.
     *
     * @param reader the MemoryReader instance for reading game memory
     */
    public ResourceManager(MemoryReader reader) {
        this.reader = reader;
        this.resourceAddresses = new HashMap<>();
        // Base offsets (example values, would be found via pointer scanning)
        resourceAddresses.put("food", 0x7F1234567890L);
        resourceAddresses.put("wood", 0x7F1234567898L);
        resourceAddresses.put("tools", 0x7F12345678A0L);
        resourceAddresses.put("amber", 0x7F12345678A8L);
    }

    /**
     * Retrieves the current value of a resource by name.
     *
     * @param resourceName the name of the resource (e.g., "food", "wood")
     * @return Optional containing the resource value as int, empty if not found or error
     */
    public Optional<Integer> getResource(String resourceName) {
        Long address = resourceAddresses.get(resourceName.toLowerCase());
        if (address == null) {
            LOG.warning("Unknown resource: " + resourceName);
            return Optional.empty();
        }
        return reader.readInt(address);
    }

    /**
     * Gets all currently tracked resources and their values.
     *
     * @return a map of resource names to their current integer values
     */
    public Map<String, Integer> getAllResources() {
        Map<String, Integer> result = new HashMap<>();
        for (Map.Entry<String, Long> entry : resourceAddresses.entrySet()) {
            Optional<Integer> value = reader.readInt(entry.getValue());
            value.ifPresent(v -> result.put(entry.getKey(), v));
        }
        return result;
    }

    /**
     * Updates the memory address for a specific resource (e.g., after re-scanning).
     *
     * @param resourceName the resource name
     * @param newAddress   the new memory address
     */
    public void updateAddress(String resourceName, long newAddress) {
        resourceAddresses.put(resourceName.toLowerCase(), newAddress);
        LOG.info("Updated address for " + resourceName + " to 0x" + Long.toHexString(newAddress));
    }
}
