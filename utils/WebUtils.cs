﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static ResoniteModLoader.ResoniteMod;

namespace HeadlessPrometheusExporter.utils;

public class WebUtils()
{
    private readonly TcpListener _listener;
    private Thread _webThread;
    private readonly PromUtils _pu;

    public WebUtils(int port) : this()
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _pu = new PromUtils();
    }

    public void Start()
    {
        _webThread = new Thread(StartListener);
        _webThread.Start();
    }

    public void Stop()
    {
        // Not the most elegant solution
        // It throws, but it works
        _listener.Stop();
        _webThread.Interrupt();
    }

    private void StartListener()
    {
        _listener.Start();
        
        do
        {
            TcpClient client = _listener.AcceptTcpClient();
            byte[] buffer = new byte[10240];
            NetworkStream stream = client.GetStream();
            int length = stream.Read(buffer, 0, buffer.Length);
            string incomingMessage = Encoding.UTF8.GetString(buffer, 0, length);

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
        while (true);
    }
}