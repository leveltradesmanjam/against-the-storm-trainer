package com.againststorm.trainer;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Optional;
import java.util.logging.Logger;

/**
 * Reads process memory using /proc/pid/mem on Linux (or simulated for cross-platform).
 * This is a simplified implementation for demonstration.
 */
public class MemoryReader {

    private static final Logger LOG = Logger.getLogger(MemoryReader.class.getName());
    private final long pid;
    private final Path memPath;

    /**
     * Creates a MemoryReader for a given process ID.
     *
     * @param pid the process ID of the game
     */
    public MemoryReader(long pid) {
        this.pid = pid;
        this.memPath = Paths.get("/proc", String.valueOf(pid), "mem");
    }

    /**
     * Reads a 32-bit integer from the specified memory address.
     *
     * @param address the memory address to read from
     * @return Optional containing the integer if successful, empty otherwise
     */
    public Optional<Integer> readInt(long address) {
        try (var channel = Files.newByteChannel(memPath)) {
            ByteBuffer buffer = ByteBuffer.allocate(4);
            buffer.order(ByteOrder.LITTLE_ENDIAN);
            channel.position(address);
            int bytesRead = channel.read(buffer);
            if (bytesRead < 4) {
                LOG.warning("Failed to read 4 bytes at address " + Long.toHexString(address));
                return Optional.empty();
            }
            buffer.flip();
            return Optional.of(buffer.getInt());
        } catch (IOException e) {
            LOG.warning("Memory read error at " + Long.toHexString(address) + ": " + e.getMessage());
            return Optional.empty();
        }
    }

    /**
     * Reads a 64-bit floating point value from the specified memory address.
     *
     * @param address the memory address to read from
     * @return Optional containing the double if successful, empty otherwise
     */
    public Optional<Double> readDouble(long address) {
        try (var channel = Files.newByteChannel(memPath)) {
            ByteBuffer buffer = ByteBuffer.allocate(8);
            buffer.order(ByteOrder.LITTLE_ENDIAN);
            channel.position(address);
            int bytesRead = channel.read(buffer);
            if (bytesRead < 8) {
                LOG.warning("Failed to read 8 bytes at address " + Long.toHexString(address));
                return Optional.empty();
            }
            buffer.flip();
            return Optional.of(buffer.getDouble());
        } catch (IOException e) {
            LOG.warning("Memory read error at " + Long.toHexString(address) + ": " + e.getMessage());
            return Optional.empty();
        }
    }

    public long getPid() {
        return pid;
    }
}
