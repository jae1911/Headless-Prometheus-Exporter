# Headless Prometheus Exporter

A [Prometheus](https://prometheus.io) exporter for the [Resonite](https://resonite.com) headless software.

Check out the [basic setup video tutorial](https://v.j4.lc/w/aPZVNi8JY4qGy2ZW6ozWpH). Also see the [minimal Grafana setup](https://g.j4.lc/general-stuff/configuration/grafana-minimal-setup).

## Upcoming changes

You can see upcoming changes on the [compare page](https://g.j4.lc/general-stuff/resonite/headless-prometheus-exporter/-/compare/1.0.4...beep), selecting the latest tag and the main branch being `beep`.

## Installation

1. Download and install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader)
1. Download the [latest version of this mod](https://g.j4.lc/general-stuff/resonite/headless-prometheus-exporter/-/releases/permalink/latest)
1. Move the mod dll into the `rml_mods` folder
1. Set your Prometheus scrape config to `localhost:9000` or `headless_container:9000`
1. Enjoy

## Data

Generic data:
- `totalPlayers` - Total players in all the sessions
- `totalWorlds` - Total worlds opened
- `engineFps` - FPS of the server
- `completedGatherJobs` - Completed Gather jobs
- `startedGatherJobs` - Started Gather jobs
- `failedGatherJobs` - Failed Gather jobs
- `engineUpdateTime` - Engine update time

Per world:
- `world_users{label="<sessionId>"}` - Users in current world
- `world_maxusers{label="<sessionId>"}` - Maximum users allowed in world
- `world_network{label="<sessionId>",type="<type>"}` - Network stats (see below in sample)

Those names will not change in the future to ensure backwards compatibility of metrics.

### Data sample

Data may look like:
```
# RESONITE HEADLESS PROMETHEUS EXPORTER
totalPlayers 2
totalWorlds 1
engineFps 120.06451
completedGatherJobs 0
startedGatherJobs 0
failedGatherJobs 0
engineUpdateTime 0.0012886
# WORLD STATS 
world_users{label="S-U-DN0:TESTS"} 2
world_maxusers{label="S-U-DN0:TESTS"} 32
world_network{label="S-U-DN0:TESTS",type="totalCorrections"} 109
world_network{label="S-U-DN0:TESTS",type="totalProcessedMessages"} 5880
world_network{label="S-U-DN0:TESTS",type="totalReceivedConfirmations"} 0
world_network{label="S-U-DN0:TESTS",type="totalReceivedControls"} 3
world_network{label="S-U-DN0:TESTS",type="totalReceivedDeltas"} 1674
world_network{label="S-U-DN0:TESTS",type="totalReceivedFulls"} 0
world_network{label="S-U-DN0:TESTS",type="totalReceivedStreams"} 4213
world_network{label="S-U-DN0:TESTS",type="totalSentConfirmations"} 1674
world_network{label="S-U-DN0:TESTS",type="totalSentDeltas"} 7372
world_network{label="S-U-DN0:TESTS",type="totalSentControls"} 3
world_network{label="S-U-DN0:TESTS",type="totalSentFulls"} 2
world_network{label="S-U-DN0:TESTS",type="totalSentStreams"} 134
world_network{label="S-U-DN0:TESTS",type="lastGeneratedDeltaChanges"} 1
world_network{label="S-U-DN0:TESTS",type="messagesToProcessCount"} 0
world_network{label="S-U-DN0:TESTS",type="messagesToTransmitCount"} 0
world_network{label="S-U-DN0:TESTS",type="averagePlayerLatency"} 0
```

### Configuration

To configure the mod, you will need to create a file named `HeadlessPrometheusExporter.json` in the `rml_config` folder containing the following content:
```
{
    "version": "1.0.0",
    "values": {
        "WebServerPort": 8080
    }
}
```

Current configuration keys are:
- `WebServerPort` - On which port the Prometheus data web server will run; default is `9000`
- `FullNetworkStats` - Export all the network stats; default is `true`, set to `false` to use less storage on the Prometheus side

---

This repository is also [mirrored on GitHub](https://github.com/jae1911/Headless-Prometheus-Exporter).