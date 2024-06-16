using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static ResoniteModLoader.ResoniteMod;

namespace HeadlessPrometheusExporter.utils;

public class WebUtils
{
    private readonly TcpListener _listener;
    private Thread _webThread;
    private readonly PromUtils _pu;
    private CancellationTokenSource _cancellationTokenSource;

    public WebUtils(int port, bool fullNetworkStats)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        Msg($"Starting Prometheus on ${port}");
        _pu = new PromUtils(fullNetworkStats);
    }

    public void Start()
    {
        _listener.Start();
        _webThread = new Thread(StartListener);
        _cancellationTokenSource = new CancellationTokenSource();
        _webThread.Start();
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
        _webThread.Abort();
        _listener.Stop();
    }

    private void StartListener()
    {
        while(!_cancellationTokenSource.IsCancellationRequested)
        {
            TcpClient client = _listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            
            string result = _pu.GeneratePromString();
            
            string reply = "HTTP/1.0 200 OK" 
                           + Environment.NewLine 
                           + "Content-Length: " 
                           + result.Length 
                           + Environment.NewLine 
                           + "Content-Type: text/plain" 
                           + Environment.NewLine 
                           + Environment.NewLine
                           + result
                           + Environment.NewLine + Environment.NewLine;

            byte[] replyBytes = Encoding.UTF8.GetBytes(reply);
            
            stream.Write(replyBytes, 0, replyBytes.Length);
        }
    }
}
