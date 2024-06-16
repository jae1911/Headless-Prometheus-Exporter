using FrooxEngine;
using HarmonyLib;
using HeadlessPrometheusExporter.utils;
using ResoniteModLoader;

namespace HeadlessPrometheusExporter
{
    public class EntryPoint : ResoniteMod
    {
        public override string Name => "Headless Prometheus Exporter";
        public override string Author => "Jae \"J4\" Lo Presti";
        public override string Version => "1.0.4";
        public override string Link => "https://g.j4.lc/general-stuff/resonite/headless-prometheus-exporter";

        public static ModConfiguration ModConf { get; private set; }

        private WebUtils _webServer;

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> WebServerPort = new("WebServerPort", "Port of the Prometheus metrics web server.", () => 9000);

        [AutoRegisterConfigKey]
        public static readonly ModConfigurationKey<bool> FullNetworkStats = new("FullNetworkStats", "Export all the network stats available.", () => true);

        public override void OnEngineInit()
        {
            if (!ModLoader.IsHeadless)
            {
                Warn("This mod is meant to be run on headlesses, skipping mod initialization.");
                return;
            }
            
            ModConf = GetConfiguration();
            
            Harmony harmony = new("lc.j4.hdpromex");
            harmony.PatchAll();

            _webServer = new WebUtils(ModConf!.GetValue(WebServerPort));

            Engine.Current.OnReady += () => _webServer.Start();
            Engine.Current.OnShutdown += () => _webServer.Stop();
        }
    }
}