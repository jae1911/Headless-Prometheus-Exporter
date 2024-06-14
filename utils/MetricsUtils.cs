using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FrooxEngine;

namespace HeadlessPrometheusExporter.utils;

public class MetricsUtils
{
    private IEnumerable<World> GetWorlds()
    {
        return Engine.Current.WorldManager.Worlds.Where(world => world != Userspace.UserspaceWorld && world != null);
    }
    
    public (int, int) GenericWorldData()
    {
        IEnumerable<World> worlds = GetWorlds();
        
        int totalUsers = 0;

        foreach (World world in worlds)
        {
            totalUsers += world.UserCount;
        }
        
        return (totalUsers, worlds.Count());
    }

    public string FullWorldData()
    {
        IEnumerable<World> worlds = GetWorlds();

        string result = "";

        foreach (World world in worlds)
        {
            result += $"world_users{{label=\"{world.SessionId}\"}} {world.UserCount}{Environment.NewLine}";
            result += $"world_maxusers{{label=\"{world.SessionId}\"}} {world.MaxUsers}{Environment.NewLine}";
        }
        
        return result;
    }

    public string GetServerFps()
    {
        return Engine.Current.SystemInfo.FPS.ToString(CultureInfo.InvariantCulture);
    }
}