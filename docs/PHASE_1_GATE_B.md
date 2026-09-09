# Phase 1 Gate B — Android local realm runtime

Status: started after Gate A smoke installation.

## Scope

Gate B proves that the required OpenMU server services can run inside the Android application in airplane mode. It does not import MU assets, implement character persistence, enable bots or claim gameplay compatibility.

## First implementation slice

1. Add a separate `PocketMU.RealmHost` library targeting `net10.0` with no web host, PostgreSQL, Docker or Dapr dependency.
2. Define an explicit lifecycle contract: `StartAsync`, readiness, `StopAsync`, and failure cleanup.
3. Keep the host bound to `127.0.0.1` and reserve the proposed discovery/game ports `44405` and `55901`.
4. Use a temporary in-memory service only for the host/protocol spike; it cannot pass Milestone 0.
5. Add an Android diagnostic screen that reports start, ready, stop and bind-failure states.
6. Test two consecutive start/stop cycles and an occupied-port failure on the Odin.

## Exit evidence

- Android APK starts the realm offline and reports readiness.
- Loopback listeners are reachable only from the device process.
- A second start does not leave duplicate listeners or stale receive loops.
- A deliberate stop releases both ports.
- PostgreSQL, web hosting, Docker, Dapr and internet access are absent from the APK.

## Known blocker

The repository currently contains only the Gate A smoke app. OpenMU source and the selected client are not yet vendored or referenced. The next code change must pin the reviewed OpenMU revision and add only the libraries needed for the reduced host; it must not copy proprietary MU data.

