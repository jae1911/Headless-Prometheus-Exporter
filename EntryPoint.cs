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
        public override string Version => "1.0.1";
        public override string Link => "https://g.j4.lc/general-stuff/resonite/headless-prometheus-exporter";

        private static ModConfiguration _modConf;

        private WebUtils _webServer;

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<int> WebServerPort = new("WebServerPort", "Port of the Prometheus metrics web server.", () => 9000);

        public override void OnEngineInit()
        {
            if (!ModLoader.IsHeadless)
            {
                Warn("This mod is meant to be run on headlesses, skipping mod initialization.");
                return;
            }
            
            _modConf = GetConfiguration();
            
            Harmony harmony = new("lc.j4.hdpromex");
            harmony.PatchAll();

            _webServer = new WebUtils(_modConf!.GetValue(WebServerPort));

            Engine.Current.OnReady += () => _webServer.Start();
            Engine.Current.OnShutdown += () => _webServer.Stop();
        }

        
    }
}