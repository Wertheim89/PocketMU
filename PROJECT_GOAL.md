# PocketMU product goal

## Vision

Create a self-contained, fully offline MU Online experience on Android handhelds, conceptually inspired by Pocket Realm for WoW. Initial hardware: Odin 2 Portal, Snapdragon 8 Gen 2 and Adreno 740. These are the owner's target specifications, not measured performance results.

The intended experience is:

Install PocketMU APK → import legally obtained MU data if needed → create a local account → choose realm population settings → start the local realm → enter the game → play offline.

## Long-term scope

- MU server and client both run on the Android device.
- Season 6-era gameplay, with an explicitly verified wire protocol and data profile.
- Local account creation and durable characters, inventory, attributes and progression.
- Maps, monsters, server-authoritative combat, drops and levelling.
- Optional server-side player bots, bounded by measured device capacity.
- Native controller input and handheld-friendly interaction.
- Eventually one approachable Android application or launcher.
- Automated test APK builds, and later signed releases.

Builds may require internet access in GitHub Actions. Ordinary gameplay must not require internet access, external authentication, a remote realm, Docker, PostgreSQL server or a second device.

## Constraints

The owner is not a developer. Development, dependencies, CI, debugging and technical setup are the implementation agent's responsibility. Owner instructions must describe simple observable actions, such as installing an APK and reporting whether a saved character reappears.

Do not download, embed, commit or redistribute proprietary Webzen/MU game data. Future imports stay on the user's device. Establish code reuse permission, retain applicable notices, and review dependencies separately from asset rights. A public GitHub repository is not itself a licence grant.

Do not start with a full application. Phase 0 is architecture and feasibility only. Phase 1 must be separately requested and stay focused on the Milestone 0 acceptance test.

No polished launcher, large bot population, advanced controller UI, custom graphics, monetisation, online multiplayer, remote account service or patcher before Milestone 0 passes.

## Success

A non-developer installs a test APK on the Odin, imports compatible legally obtained data, creates an account and character, enters Lorencia, moves, kills one basic monster, saves, closes, relaunches and finds the same progression. All gameplay steps work in airplane mode.

This remains a target, not a claim of implemented functionality. See [milestones](docs/MILESTONES.md).
