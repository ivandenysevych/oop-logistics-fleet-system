using System.Runtime.InteropServices;

namespace Logistics.Domain.Lifecycle;

/// <summary>
/// Demonstrates managed lifecycle over an unmanaged memory buffer.
/// </summary>
public sealed class CargoManifestBuffer : IDisposable
{
    private IntPtr _buffer;
    private bool _disposed;

    public CargoManifestBuffer()
        : this(256)
    {
    }

    public CargoManifestBuffer(int sizeInBytes)
    {
        if (sizeInBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "Buffer size must be greater than zero.");
        }

        SizeInBytes = sizeInBytes;
        _buffer = Marshal.AllocHGlobal(sizeInBytes);

        for (var index = 0; index < sizeInBytes; index++)
        {
            Marshal.WriteByte(_buffer, index, 0);
        }
    }

    public CargoManifestBuffer(CargoManifestBuffer source)
        : this(source?.SizeInBytes ?? throw new ArgumentNullException(nameof(source)))
    {
        for (var index = 0; index < SizeInBytes; index++)
        {
            WriteByte(index, source.ReadByte(index));
        }
    }

    public int SizeInBytes { get; }

    public bool IsDisposed => _disposed;

    ~CargoManifestBuffer()
    {
        Dispose(false);
    }

    public void WriteByte(int index, byte value)
    {
        EnsureNotDisposed();
        ValidateIndex(index);
        Marshal.WriteByte(_buffer, index, value);
    }

    public byte ReadByte(int index)
    {
        EnsureNotDisposed();
        ValidateIndex(index);
        return Marshal.ReadByte(_buffer, index);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (_buffer != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_buffer);
            _buffer = IntPtr.Zero;
        }

        _disposed = true;
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= SizeInBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of buffer bounds.");
        }
    }

    private void EnsureNotDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(CargoManifestBuffer));
        }
    }
}
