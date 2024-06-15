# Headless Prometheus Exporter

A [Prometheus](https://prometheus.io) exporter for the [Resonite](https://resonite.com) headless software.

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

Per world:
- `world_users{label="S-<sessionId>"}` - Users in current world
- `world_maxusers{label="<sessionId>"}` - Maximum users allowed in world

Those names will not change in the future to ensure backwards compatibility of metrics.

### Data sample

Data may look like:
```
# RESONITE HEADLESS PROMETHEUS EXPORTER
totalPlayers 1
totalWorlds 1
world_users{label="S-32ad2e00-453a-4a9c-802c-df3d7c3e57d7"} 1
world_maxusers{label="S-32ad2e00-453a-4a9c-802c-df3d7c3e57d7"} 16
engineFps 60.05342
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

---

This repository is also [mirrored on GitHub](https://github.com/jae1911/Headless-Prometheus-Exporter).