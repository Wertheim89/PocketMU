# Phase 1 Gate A — build, rights and data foundation

Status: implementation started; APK build and Odin installation remain unverified.

This gate adds an independent, asset-free .NET 10 Android smoke app. It does not include OpenMU, either candidate game client, proprietary MU data, terrain, textures, sounds or downloadable game content.

Before importing client code, PocketMU needs a redistribution grant for the chosen client. The requested `bhrama-br/muonline-android` tree had no named licence/copying file in the audited revision. The comparison client `bernatvadell/muonline` describes educational/non-commercial terms rather than a clear redistribution licence. Public visibility is not permission to redistribute.

OpenMU is MIT-licensed, but that does not grant rights to Webzen/MU Online assets or another project's code and media. The future importer must accept user-selected, legally obtained files on the device and must not download or bundle MU data.

## Exit checklist

- [ ] Client reuse/distribution grant recorded.
- [ ] Lawful device-local data/import profile documented.
- [ ] GitHub Actions completes `build-android-smoke`.
- [ ] APK is attached to the workflow run.
- [ ] APK installs on Odin 2 Portal without Android Studio.
- [ ] APK launches in airplane mode and displays the Gate A message.
- [ ] APK checksum, commit SHA, Android version and test date recorded.

Do not start server integration until the rights and smoke-install checks are complete.

