using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FrooxEngine;

namespace HeadlessPrometheusExporter.utils;

public class MetricsUtils(bool fullNetworkStats)
{
    private List<World> GetWorlds()
    {
        return Engine.Current.WorldManager.Worlds.Where(world => world != Userspace.UserspaceWorld && world != null).ToList();
    }
    
    public (int, int) GenericWorldData()
    {
        List<World> worlds = GetWorlds();
        
        int totalUsers = 0;

        foreach (World world in worlds)
        {
            totalUsers += world.UserCount;
        }
        
        return (totalUsers, worlds.Count);
    }

    public string FullWorldData()
    {
        List<World> worlds = GetWorlds();

        string result = $"# WORLD STATS {Environment.NewLine}";

        foreach (World world in worlds)
        {
            result += $"world_users{{label=\"{world.SessionId}\"}} {world.UserCount}{Environment.NewLine}";
            result += $"world_maxusers{{label=\"{world.SessionId}\"}} {world.MaxUsers}{Environment.NewLine}";

            if (!fullNetworkStats) continue;
            
            // Network
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalCorrections\"}} {world.Session.TotalCorrections}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalProcessedMessages\"}} {world.Session.TotalProcessedMessages}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedConfirmations\"}} {world.Session.TotalReceivedConfirmations}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedControls\"}} {world.Session.TotalReceivedControls}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedDeltas\"}} {world.Session.TotalReceivedDeltas}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedFulls\"}} {world.Session.TotalReceivedFulls}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalReceivedStreams\"}} {world.Session.TotalReceivedStreams}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentConfirmations\"}} {world.Session.TotalSentConfirmations}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentDeltas\"}} {world.Session.TotalSentDeltas}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentControls\"}} {world.Session.TotalSentControls}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentFulls\"}} {world.Session.TotalSentFulls}{Environment.NewLine}";
            result +=
                $"world_network{{label=\"{world.SessionId}\",type=\"totalSentStreams\"}} {world.Session.TotalSentStreams}{Environment.NewLine}";
        }
        
        return result;
    }

    public string GetServerFps()
    {
        return Engine.Current.SystemInfo.FPS.ToString(CultureInfo.InvariantCulture);
    }

    public (int, int, int) GetGatherJobs()
    {
        return (Engine.Current.TotalCompletedGatherJobs, Engine.Current.TotalStartedGatherJobs, Engine.Current.TotalFailedGatherJobs);
    }

    public string GetEngineUpdateTime()
    {
        return Engine.Current.TotalEngineUpdateTime.ToString(CultureInfo.InvariantCulture);
    }
}