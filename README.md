# PocketMU

PocketMU is an experimental project exploring a fully offline MU Online experience on Android handhelds, initially the Odin 2 Portal.

The eventual aim is: install one APK, import legally obtained game data, create a local account, start a local realm, and play without an internet connection or a separate computer.

**Current status: Phase 0 feasibility audit complete. Phase 1 Gate A is in progress. An independent asset-free Android smoke project and GitHub Actions workflow have been added; no playable application or PocketMU realm exists.**

The proposed route is a reduced OpenMU server inside the same .NET for Android application as a MonoGame client, using local loopback networking and an embedded SQLite save store. Server hosting, durable saving, client reuse permission and asset compatibility still need to be proved.

## Read the results

- [Feasibility audit](docs/PHASE_0_FEASIBILITY.md): findings, evidence, risks and recommendation.
- [Proposed architecture](docs/ARCHITECTURE.md): what runs on the device.
- [Milestones](docs/MILESTONES.md): the small first playable test and later stages.
- [Product goal](PROJECT_GOAL.md): the vision and constraints.
- [Source audit](docs/SOURCE_AUDIT.md): exact upstream revisions, inspected files and verification limits.
- [Phase 1 Gate A](docs/PHASE_1_GATE_A.md): rights, data and smoke-APK exit checklist.

## Game data and permissions

**Proprietary MU Online / Webzen game assets are not included.** Do not upload game installers, Data folders, maps, terrain files, models, textures, sounds or asset archives to this repository. Do not add automatic downloads of those assets. A future device-local importer must validate data the user is entitled to use.

An upstream code licence does not establish the rights to every bundled asset. The requested Android client's reuse licence is unresolved; the newer related client describes educational/non-commercial terms. Neither is treated here as cleared for redistribution.

## For the project owner

You do not need Android Studio, .NET, Git, Docker or PostgreSQL installed. There is nothing to install or play yet. Future test APKs should be downloadable from GitHub Actions after the build and device tests pass.

The current Gate A target is an asset-free ARM64 smoke APK. The first gameplay target remains one local account, one character, Lorencia, one monster kill, and a save that survives closing and reopening the app.

## Repository safeguards

The ignore file blocks common asset, credential, build and local-data paths. A Phase 0 repository check permits only the reviewed documentation and safety files. These are safeguards, not a guarantee against secrets pasted into allowed text or a forced Git operation. Do not commit sensitive or proprietary material.

