using System;

namespace HeadlessPrometheusExporter.utils;

public abstract class PromUtils
{
    public static string GeneratePromString()
    {
        string promString = $"# RESONITE HEADLESS PROMETHEUS EXPORTER{Environment.NewLine}";

        (int totalWorldUsers, int totalWorlds) = MetricsUtils.GenericWorldData();

        promString += $"totalPlayers {totalWorldUsers}{Environment.NewLine}";
        promString += $"totalWorlds {totalWorlds}{Environment.NewLine}";

        promString += $"engineFps {MetricsUtils.GetServerFps()}{Environment.NewLine}";

        (int completedGatherJobs, int startedGatherJobs, int failedGatherJobs) = MetricsUtils.GetGatherJobs();

        promString += $"completedGatherJobs {completedGatherJobs}{Environment.NewLine}";
        promString += $"startedGatherJobs {startedGatherJobs}{Environment.NewLine}";
        promString += $"failedGatherJobs {failedGatherJobs}{Environment.NewLine}";

        promString += $"engineUpdateTime {MetricsUtils.GetEngineUpdateTime()}{Environment.NewLine}";
        
        promString += MetricsUtils.FullWorldData();
        
        return promString;
    }
}