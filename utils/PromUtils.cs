using System;

namespace HeadlessPrometheusExporter.utils;

public class PromUtils
{
    private readonly MetricsUtils _mu;

    public PromUtils(bool fullNetworkStats)
    {
        _mu = new MetricsUtils(fullNetworkStats);
    }

    public string GeneratePromString()
    {
        string promString = $"# RESONITE HEADLESS PROMETHEUS EXPORTER{Environment.NewLine}";

        (int totalWorldUsers, int totalWorlds) = _mu.GenericWorldData();

        promString += $"totalPlayers {totalWorldUsers}{Environment.NewLine}";
        promString += $"totalWorlds {totalWorlds}{Environment.NewLine}";

        promString += $"engineFps {_mu.GetServerFps()}{Environment.NewLine}";

        (int completedGatherJobs, int startedGatherJobs, int failedGatherJobs) = _mu.GetGatherJobs();

        promString += $"completedGatherJobs {completedGatherJobs}{Environment.NewLine}";
        promString += $"startedGatherJobs {startedGatherJobs}{Environment.NewLine}";
        promString += $"failedGatherJobs {failedGatherJobs}{Environment.NewLine}";

        promString += $"engineUpdateTime {_mu.GetEngineUpdateTime()}{Environment.NewLine}";
        
        promString += _mu.FullWorldData();
        
        return promString;
    }
}