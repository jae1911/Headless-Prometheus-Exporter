using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FrooxEngine;

namespace HeadlessPrometheusExporter.utils;

public abstract class MetricsUtils
{
    private static List<World> GetWorlds()
    {
        return Engine.Current.WorldManager.Worlds.Where(world => world != Userspace.UserspaceWorld && world != null).ToList();
    }
    
    public static (int, int) GenericWorldData()
    {
        List<World> worlds = GetWorlds();
        
        int totalUsers = worlds.Sum(world => world.UserCount);

        return (totalUsers, worlds.Count);
    }

    public static string FullWorldData()
    {
        List<World> worlds = GetWorlds();

        string result = $"# WORLD STATS {Environment.NewLine}";

        foreach (World world in worlds)
        {
            result += $"world_users{{label=\"{world.SessionId}\"}} {world.UserCount}\n";
            result += $"world_maxusers{{label=\"{world.SessionId}\"}} {world.MaxUsers}\n";
            
            if (!EntryPoint.ModConf.GetValue(EntryPoint.FullNetworkStats)) continue;
            
            // Network
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalCorrections\"}} {world.Session.TotalCorrections}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalProcessedMessages\"}} {world.Session.TotalProcessedMessages}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedConfirmations\"}} {world.Session.TotalReceivedConfirmations}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedControls\"}} {world.Session.TotalReceivedControls}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedDeltas\"}} {world.Session.TotalReceivedDeltas}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedFulls\"}} {world.Session.TotalReceivedFulls}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedStreams\"}} {world.Session.TotalReceivedStreams}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentConfirmations\"}} {world.Session.TotalSentConfirmations}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentDeltas\"}} {world.Session.TotalSentDeltas}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentControls\"}} {world.Session.TotalSentControls}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentFulls\"}} {world.Session.TotalSentFulls}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentStreams\"}} {world.Session.TotalSentStreams}\n";

            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"lastGeneratedDeltaChanges\"}} {world.Session.LastGeneratedDeltaChanges}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"messagesToProcessCount\"}} {world.Session.MessagesToProcessCount}\n";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"messagesToTransmitCount\"}} {world.Session.MessagesToTransmitCount}\n";
            
            // Calculate average LNL latency for session
            List<float> playerLatency = new List<float>();
            var allUsers = world.FindUsers(user => user != world.HostUser);
            foreach (var user in allUsers)
            {
                playerLatency.Add(user.Ping);
            }

            float avgLatency = playerLatency.Count > 0 ? playerLatency.Average() : 0;
            
            result += $"world_network{{label=\"{world.SessionId}\",type=\"averagePlayerLatency\"}} {avgLatency}";
        }
        
        return result;
    }

    public static string GetServerFps()
    {
        return Engine.Current.SystemInfo.FPS.ToString(CultureInfo.InvariantCulture);
    }

    public static (int, int, int) GetGatherJobs()
    {
        return (Engine.Current.TotalCompletedGatherJobs, Engine.Current.TotalStartedGatherJobs, Engine.Current.TotalFailedGatherJobs);
    }

    public static string GetEngineUpdateTime()
    {
        return Engine.Current.TotalEngineUpdateTime.ToString(CultureInfo.InvariantCulture);
    }
}