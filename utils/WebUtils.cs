using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ResoniteModLoader.ResoniteMod;

namespace HeadlessPrometheusExporter.utils;

public class WebUtils
{
    private readonly TcpListener _listener;
    private Task _webTask;
    private readonly ManualResetEvent _terminate = new(false);
    
    public WebUtils(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        Msg($"Starting Prometheus on {port}");
    }

    public void Start()
    {
        _listener.Start();
        
        _webTask = new Task(StartListener);
        _webTask.Start();
    }

    public void Stop()
    {
        _terminate.Set();
        
        _listener.Stop();
    }

    private void StartListener()
    {
        while(!_terminate.WaitOne(0))
        {
            TcpClient client = _listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            
            string result = PromUtils.GeneratePromString();
            
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
