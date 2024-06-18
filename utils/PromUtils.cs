using System;

namespace HeadlessPrometheusExporter.utils;

public abstract class PromUtils
{
    public static string GeneratePromString()
    {
        string promString = $"# RESONITE HEADLESS PROMETHEUS EXPORTER\n";

        (int totalWorldUsers, int totalWorlds) = MetricsUtils.GenericWorldData();

        promString += $"totalPlayers {totalWorldUsers}\n";
        promString += $"totalWorlds {totalWorlds}\n";

        promString += $"engineFps {MetricsUtils.GetServerFps()}\n";

        (int completedGatherJobs, int startedGatherJobs, int failedGatherJobs) = MetricsUtils.GetGatherJobs();

        promString += $"completedGatherJobs {completedGatherJobs}\n";
        promString += $"startedGatherJobs {startedGatherJobs}\n";
        promString += $"failedGatherJobs {failedGatherJobs}\n";

        promString += $"engineUpdateTime {MetricsUtils.GetEngineUpdateTime()}\n";
        
        promString += MetricsUtils.FullWorldData();
        
        return promString;
    }
}