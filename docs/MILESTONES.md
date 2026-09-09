# PocketMU milestones

Phase 0 is complete. **Do not start Phase 1 without a new instruction from the owner.**

A development phase describes the work period; Milestone 0 describes the first playable acceptance result. Phase 1's only product target is Milestone 0. The smaller gates below are parts of that milestone, not claims of completed work.

## Milestone 0 — one local, durable gameplay loop

On the actual Odin 2 Portal:

1. Install a self-contained test APK without development tools.
2. Import compatible game data the user is entitled to use; no automatic download.
3. Enable airplane mode with Wi-Fi off.
4. Start the local realm and create a local account.
5. Log in and create a new basic character, initially a Dark Knight to keep the test narrow.
6. Enter Lorencia and move to a reachable basic monster.
7. Kill the monster; verify server-authoritative experience. Pick up a drop if one is produced.
8. Save and note character name/class, XP/level, money, inventory, stats and location.
9. Close the application normally.
10. Relaunch offline and verify that the recorded progression remains.
11. Repeat after a process termination following an acknowledged save.
12. Interrupt a separate save test and verify recovery of the previous complete transaction, with no corrupted or duplicated inventory.

A preset character or an in-memory demo does not pass. A desktop-only test does not pass. A successful build without installing the APK does not pass.

### Phase 1 gate A — rights, data and toolchain

- Establish an applicable client reuse/distribution grant; do not copy code based only on public visibility.
- Pin server/client revisions, .NET 10 Android workload and package graph.
- Define the lawful import profile, including client assets and server collision terrain.
- Remove proprietary content from the planned build inputs; audit UI images/fonts/effects too.
- Build and install an asset-free ARM64 smoke APK through GitHub Actions. Verify standalone install, runtime inclusion and a basic rendered frame or diagnostic screen.

Owner-visible evidence: downloadable test APK and an install/start result. No local SDK setup for the owner.

### Phase 1 gate B — actual Android server runtime

- Compose only the required OpenMU core services.
- Exercise real game-logic plugin points. Test Roslyn generation on Android; use build-time proxies if needed.
- Bind only 127.0.0.1, test raw/encrypted local packets, start/stop twice, and handle port conflicts.
- Prove no desktop web host, PostgreSQL, Docker, Dapr or internet access is required.

Owner-visible evidence: local-realm start/stop works offline. If the core cannot run without an unbounded rewrite, stop and revise the architecture.

### Phase 1 gate C — durable local account and character state

- Implement the required persistence interfaces with SQLite and stable definition references.
- Implement local account creation using compatible password hashing.
- Prove authentication, new-character creation, unique names, save/load, inventory/stat/XP round trips and migrations.
- Test abrupt termination and storage/save errors; do not silently claim success.

Owner-visible evidence: newly created state survives closing/reopening the test application.

### Phase 1 gate D — client protocol and lawful data

- Select one exact protocol profile and explicitly map discovery/game ports.
- Verify version-byte normalization, encryption/counters, login, character list/create/select, map entry, movement, combat/XP and inventory.
- Remove download fallback, Windows paths, immediate network startup and forced process exit.
- Add minimal local data selection/validation, startup readiness and reconnect behavior.
- Test Lorencia rendering and collision terrain alignment.

Owner-visible evidence: character creation and Lorencia entry on Odin with airplane mode on.

### Phase 1 gate E — complete Milestone 0

Run the full acceptance sequence above. Record APK checksum, build/source versions, Odin Android version, test date and observed results. Record startup time, memory and a short sustained-play performance sample without pretending this is final optimization.

Bots stay disabled. Touch may be used. No polished launcher or advanced controller work.

Afterward, stop and report the outcome. Do not begin Milestone 1 automatically.

## Milestone 1 — dependable offline foundation

After approval: improve save recovery/backups, import compatibility diagnostics, repeatable starts, activity recreation, error reporting and regression coverage. Validate installation updates retain save data with a stable signing identity. Expand basic gameplay only after durability and lifecycle are reliable.

Exit: repeatable offline sessions and upgrade/recovery tests pass.

## Milestone 2 — native handheld controls

After approval: map sticks/buttons to movement, targeting, attack, interact and menus. Handle dead zones, hotplug, focus and text entry. Make common gameplay possible without touch where practical.

Exit: complete the Milestone 0 loop using Odin controls, with clearly documented remaining touch-only actions.

## Milestone 3 — small server-side bot population

After approval: introduce 1–3 bots, confirm persistence, cap active maps/AI, measure frame pacing, memory, battery and heat. Increase population only from measured capacity. Verify bots do not corrupt inventory/account state or prevent player saves.

Exit: stable bounded bot sessions on Odin; no unsupported large-population promise.

## Milestone 4 — wider Season 6 gameplay

After approval: expand maps/classes/skills/items and appropriate events progressively, with matched data and protocol fixtures. Maintain offline regression tests and bounded resource use.

Exit: an explicitly documented supported subset; avoid claiming full Season 6 parity without testing.

## Milestone 5 — approachable single-app experience

After approval: refine launcher/import/account/population flows, controller navigation, help and diagnostics. Consider a separate Android service only if measurements show a need.

Exit: a non-developer follows the complete install → import → account → start → play flow without technical setup.

## Milestone 6 — release readiness

After approval: finalize rights/provenance review, protected release signing, repeatable builds, save migration/backup, dependency notices and supported-device documentation. Test upgrade and recovery paths before distributing broadly.

No monetisation, online multiplayer, external account service or patcher is implied by these stages.
