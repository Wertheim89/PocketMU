using System.Net;
using System.Net.Sockets;

namespace PocketMU.RealmHost;

public sealed record RealmHostOptions(int DiscoveryPort = 44405, int GamePort = 55901);

public sealed class RealmHost : IAsyncDisposable
{
    private readonly RealmHostOptions _options;
    private TcpListener? _discovery;
    private TcpListener? _game;
    private int _started;

    public RealmHost(RealmHostOptions? options = null) => _options = options ?? new RealmHostOptions();

    public bool IsReady => Volatile.Read(ref _started) == 1;
    public RealmHostOptions Options => _options;

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (Interlocked.Exchange(ref _started, 1) == 1)
            throw new InvalidOperationException("The realm is already running.");

        try
        {
            _discovery = CreateListener(_options.DiscoveryPort);
            _game = CreateListener(_options.GamePort);
            return Task.CompletedTask;
        }
        catch
        {
            _discovery?.Stop();
            _game?.Stop();
            _discovery = null;
            _game = null;
            Volatile.Write(ref _started, 0);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _discovery?.Stop();
        _game?.Stop();
        _discovery = null;
        _game = null;
        Volatile.Write(ref _started, 0);
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync() => await StopAsync();

    private static TcpListener CreateListener(int port)
    {
        var listener = new TcpListener(IPAddress.Loopback, port);
        listener.Start();
        return listener;
    }
}
