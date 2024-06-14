using System;

namespace HeadlessPrometheusExporter.utils;

public class PromUtils
{
    private MetricsUtils _mu;

    public PromUtils()
    {
        _mu = new MetricsUtils();
    }
    
    public string GeneratePromString()
    {
        string promString = $"# RESONITE HEADLESS PROMETHEUS EXPORTER{Environment.NewLine}";

        (int, int) genericWorldData = _mu.GenericWorldData();

        promString += $"totalPlayers {genericWorldData.Item1}{Environment.NewLine}";
        promString += $"totalWorlds {genericWorldData.Item2}{Environment.NewLine}";

        promString += _mu.FullWorldData();

        promString += $"engineFps {_mu.GetServerFps()}{Environment.NewLine}";
        
        return promString;
    }
}