package com.againststorm.trainer;

import java.util.Map;
import java.util.Optional;
import java.util.logging.Logger;

/**
 * Main controller for the trainer application.
 * Provides high-level operations like freezing resources or printing status.
 */
public class TrainerController {

    private static final Logger LOG = Logger.getLogger(TrainerController.class.getName());
    private final ResourceManager resourceManager;
    private boolean active;

    /**
     * Creates a TrainerController with a given ResourceManager.
     *
     * @param resourceManager the resource manager to use
     */
    public TrainerController(ResourceManager resourceManager) {
        this.resourceManager = resourceManager;
        this.active = false;
    }

    /**
     * Activates the trainer, printing current resource values.
     */
    public void activate() {
        this.active = true;
        LOG.info("Trainer activated.");
        printResources();
    }

    /**
     * Deactivates the trainer.
     */
    public void deactivate() {
        this.active = false;
        LOG.info("Trainer deactivated.");
    }

    /**
     * Checks if the trainer is currently active.
     *
     * @return true if active, false otherwise
     */
    public boolean isActive() {
        return active;
    }

    /**
     * Prints all tracked resources to the console.
     */
    public void printResources() {
        Map<String, Integer> resources = resourceManager.getAllResources();
        if (resources.isEmpty()) {
            System.out.println("No resources could be read. Check game process.");
            return;
        }
        System.out.println("=== Against the Storm Resources ===");
        resources.forEach((name, value) -> System.out.println(name + ": " + value));
        System.out.println("===================================");
    }

    /**
     * Attempts to set a resource to a specific value (write not implemented in MemoryReader for safety).
     * This is a placeholder for demonstration.
     *
     * @param resourceName the name of the resource
     * @param value        the desired value
     * @return true if the operation would be possible (always false in this demo)
     */
    public boolean setResource(String resourceName, int value) {
        LOG.warning("Write operations are not supported in this demonstration.");
        return false;
    }

    /**
     * Gets the current value of a single resource.
     *
     * @param resourceName the resource name
     * @return Optional containing the value, empty if not found
     */
    public Optional<Integer> getResource(String resourceName) {
        return resourceManager.getResource(resourceName);
    }
}
