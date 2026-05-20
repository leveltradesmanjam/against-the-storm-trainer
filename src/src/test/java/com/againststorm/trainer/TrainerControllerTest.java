package com.againststorm.trainer;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Unit tests for TrainerController, using a mock MemoryReader that simulates memory.
 */
class TrainerControllerTest {

    private TrainerController controller;
    private MockMemoryReader mockReader;

    @BeforeEach
    void setUp() {
        mockReader = new MockMemoryReader();
        ResourceManager resourceManager = new ResourceManager(mockReader);
        controller = new TrainerController(resourceManager);
    }

    @Test
    void testActivateSetsActive() {
        assertFalse(controller.isActive());
        controller.activate();
        assertTrue(controller.isActive());
    }

    @Test
    void testDeactivateClearsActive() {
        controller.activate();
        assertTrue(controller.isActive());
        controller.deactivate();
        assertFalse(controller.isActive());
    }

    @Test
    void testGetResourceReturnsMockValue() {
        // Mock returns 100 for food
        Optional<Integer> food = controller.getResource("food");
        assertTrue(food.isPresent());
        assertEquals(100, food.get());
    }

    @Test
    void testGetResourceUnknownReturnsEmpty() {
        Optional<Integer> unknown = controller.getResource("stone");
        assertFalse(unknown.isPresent());
    }

    @Test
    void testSetResourceReturnsFalse() {
        assertFalse(controller.setResource("food", 999));
    }

    /**
     * A simple mock that returns fixed values for known addresses.
     */
    private static class MockMemoryReader extends MemoryReader {

        public MockMemoryReader() {
            super(0); // pid not used in mock
        }

        @Override
        public Optional<Integer> readInt(long address) {
            // Simulate known resource addresses
            if (address == 0x7F1234567890L) {
                return Optional.of(100); // food
            } else if (address == 0x7F1234567898L) {
                return Optional.of(50);  // wood
            } else if (address == 0x7F12345678A0L) {
                return Optional.of(30);  // tools
            } else if (address == 0x7F12345678A8L) {
                return Optional.of(20);  // amber
            }
            return Optional.empty();
        }

        @Override
        public Optional<Double> readDouble(long address) {
            return Optional.empty();
        }
    }
}
