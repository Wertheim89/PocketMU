# Phase 0 source audit record

Date: 9 September 2026.

## Scope and acquisition

Repository trees were retrieved from GitHub's recursive tree API and selected text files were read at fixed revisions through the GitHub connection. All three trees reported truncated=false. The source list below records fetched files; long files were inspected selectively around the relevant implementations, not exhaustively reviewed line by line.

Direct shallow source-clone attempts did not succeed in this workspace. The connector-based source inspection was the successful fallback. No proprietary game assets, terrain binaries, media files, installers or APKs were downloaded. Reading references to asset filenames/URLs is not downloading those assets.

| Repository | Default branch inspected | Commit | Commit date (UTC) |
| --- | --- | --- | --- |
| MUnique/OpenMU | master | 9693f8f6cf037909c9dfd3a9a9438fffcabd9837 | 2026-09-09 05:03:24 |
| bhrama-br/muonline-android | main | 963a8c18ddce979f3df19d648cd802fdd740d535 | 2025-05-31 18:39:11 |
| bernatvadell/muonline | main | 7baa3454b890c1b33189fd2bc89c7f6d7af3058d | 2026-02-10T21:00:15Z |

The original PocketMU repository was empty before this audit.

## Checks actually performed

The following source assertions were evaluated against the retrieved text/tree snapshots. These establish source facts only; none is an Android build or gameplay test.

- PASS: Requested client targets net9.0-android.
- PASS: Current OpenMU core targets net10.0.
- PASS: Original client sends raw version ASCII.
- PASS: Configured 1.04d differs from server 10404 bytes.
- PASS: Server login does not reference ClientSerial.
- PASS: Listener binds all interfaces as supplied.
- PASS: PostgreSQL-only branch exists.
- PASS: Runtime compiler requires desktop-style metadata.
- PASS: Server bots persist through ordinary context.
- PASS: No creation builder in original PacketBuilder.
- PASS: No GamePad polling in original MuGame input host.
- PASS: Requested client's full tree has no workflow.
- PASS: Requested client's tree has no named license/copying file.
- PASS: Alternative's Android project targets .NET 10.
- PASS: OpenMU initialization embeds terrain.

For the version comparison, 1.04d encodes to 31 2E 30 34 64; the server definition is 31 30 34 30 34. The server's fallback resolver prevents concluding that this alone rejects login.

Repository validation is separate: the documentation allowlist and credential-pattern check are executed locally against the staged deliverable file set. A subsequent GitHub workflow run, if available, is supplementary and must not be represented as an APK build.

## Not performed / unresolved

- No .NET SDK is installed in the task environment; only .NET 7 runtime information was available.
- No upstream restore/build, .NET Android API-compatibility analysis or transitive native-binary inventory.
- No device-side plug-in generation or build-time proxy prototype.
- No actual client/server packet exchange, fixture replay or character creation.
- No SQLite adapter, migration or crash-recovery test.
- No Odin install, controller test, FPS/memory/thermal measurement or offline-network trace.
- No confirmed successful upstream Android CI run or downloaded build artifact.
- No client code licensing clearance and no verification of proprietary data provenance.
- No exhaustive review of every client input call site or every OpenMU API.
- No Phase 1 work.

The feasibility report labels conclusions that depend on these items as proposed or unverified.

## Inspected source files

Links are pinned to the audited revision. Server code licence: MIT at the listed LICENSE file. No named licence/copying file was found in the requested client's tree; the related client's README includes educational/non-commercial language. These observations do not establish redistribution rights for either client or any asset.

### MUnique/OpenMU

- [LICENSE](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/LICENSE)
- [src/ConnectServer/ClientListener.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/ConnectServer/ClientListener.cs)
- [src/ConnectServer/PacketHandler/ServerInfoRequestHandler.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/ConnectServer/PacketHandler/ServerInfoRequestHandler.cs)
- [src/Directory.Build.props](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Directory.Build.props)
- [src/Directory.Packages.props](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Directory.Packages.props)
- [src/GameLogic/Bots/BotConfiguration.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotConfiguration.cs)
- [src/GameLogic/Bots/BotFeaturePlugIn.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotFeaturePlugIn.cs)
- [src/GameLogic/Bots/BotGenerator.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotGenerator.cs)
- [src/GameLogic/Bots/BotManager.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotManager.cs)
- [src/GameLogic/Bots/BotPlayer.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotPlayer.cs)
- [src/GameLogic/MUnique.OpenMU.GameLogic.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/MUnique.OpenMU.GameLogic.csproj)
- [src/GameLogic/Offline/OfflinePlayer.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Offline/OfflinePlayer.cs)
- [src/GameLogic/Player.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Player.cs)
- [src/GameLogic/PlayerActions/LoginAction.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/PlayerActions/LoginAction.cs)
- [src/GameLogic/PlugIns/PeriodicSaveProgressPlugIn.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/PlugIns/PeriodicSaveProgressPlugIn.cs)
- [src/GameServer/ClientVersionResolver.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameServer/ClientVersionResolver.cs)
- [src/GameServer/GameServer.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameServer/GameServer.cs)
- [src/GameServer/MUnique.OpenMU.GameServer.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameServer/MUnique.OpenMU.GameServer.csproj)
- [src/GameServer/MessageHandler/Login/LogInHandlerPlugIn.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameServer/MessageHandler/Login/LogInHandlerPlugIn.cs)
- [src/Network/Listener.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/Listener.cs)
- [src/Network/LoopbackIpResolver.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/LoopbackIpResolver.cs)
- [src/Network/MUnique.OpenMU.Network.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/MUnique.OpenMU.Network.csproj)
- [src/Network/PlugIns/OpenSourceClientNetworkEncryptionFactoryPlugIn.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/PlugIns/OpenSourceClientNetworkEncryptionFactoryPlugIn.cs)
- [src/Network/PlugIns/Season6Episode3NetworkEncryptionFactoryPlugIn.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/PlugIns/Season6Episode3NetworkEncryptionFactoryPlugIn.cs)
- [src/Persistence/EntityFramework/ConfigFileDatabaseConnectionStringProvider.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/ConfigFileDatabaseConnectionStringProvider.cs)
- [src/Persistence/EntityFramework/ConfigurationChangeListener.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/ConfigurationChangeListener.cs)
- [src/Persistence/EntityFramework/ConnectionConfigurator.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/ConnectionConfigurator.cs)
- [src/Persistence/EntityFramework/EntityDataContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/EntityDataContext.cs)
- [src/Persistence/EntityFramework/Extensions/ModelBuilder/CharacterExtensions.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/Extensions/ModelBuilder/CharacterExtensions.cs)
- [src/Persistence/EntityFramework/Json/JsonQueryBuilder.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/Json/JsonQueryBuilder.cs)
- [src/Persistence/EntityFramework/MUnique.OpenMU.Persistence.EntityFramework.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/MUnique.OpenMU.Persistence.EntityFramework.csproj)
- [src/Persistence/EntityFramework/PersistenceContextProvider.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/PersistenceContextProvider.cs)
- [src/Persistence/EntityFramework/PlayerContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/PlayerContext.cs)
- [src/Persistence/IContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/IContext.cs)
- [src/Persistence/IPersistenceContextProvider.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/IPersistenceContextProvider.cs)
- [src/Persistence/IPlayerContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/IPlayerContext.cs)
- [src/Persistence/IRepository.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/IRepository.cs)
- [src/Persistence/InMemory/InMemoryContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/InMemory/InMemoryContext.cs)
- [src/Persistence/InMemory/InMemoryPersistenceContextProvider.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/InMemory/InMemoryPersistenceContextProvider.cs)
- [src/Persistence/InMemory/PlayerInMemoryContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/InMemory/PlayerInMemoryContext.cs)
- [src/Persistence/Initialization/BaseMapInitializer.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/BaseMapInitializer.cs)
- [src/Persistence/Initialization/DataInitializationBase.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/DataInitializationBase.cs)
- [src/Persistence/Initialization/MUnique.OpenMU.Persistence.Initialization.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/MUnique.OpenMU.Persistence.Initialization.csproj)
- [src/Persistence/Initialization/TerrainUpdateHelper.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/TerrainUpdateHelper.cs)
- [src/Persistence/Initialization/VersionSeasonSix/DataInitialization.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/VersionSeasonSix/DataInitialization.cs)
- [src/Persistence/Initialization/VersionSeasonSix/GameConfigurationInitializer.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/VersionSeasonSix/GameConfigurationInitializer.cs)
- [src/PlugIns/MUnique.OpenMU.PlugIns.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/PlugIns/MUnique.OpenMU.PlugIns.csproj)
- [src/PlugIns/PlugInManager.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/PlugIns/PlugInManager.cs)
- [src/PlugIns/PlugInProxyTypeGenerator.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/PlugIns/PlugInProxyTypeGenerator.cs)
- [src/PlugIns/SyntaxTreeExtensions.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/PlugIns/SyntaxTreeExtensions.cs)
- [src/Startup/GameServerContainer.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Startup/GameServerContainer.cs)
- [src/Startup/MUnique.OpenMU.Startup.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Startup/MUnique.OpenMU.Startup.csproj)
- [src/Startup/Program.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Startup/Program.cs)
- [src/Startup/appsettings.json](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Startup/appsettings.json)

### bhrama-br/muonline-android

- [Client.Data/Client.Data.csproj](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Data/Client.Data.csproj)
- [Client.Main/Client.Main.csproj](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Client.Main.csproj)
- [Client.Main/Configuration/MuOnlineSettings.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Configuration/MuOnlineSettings.cs)
- [Client.Main/Constants.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Constants.cs)
- [Client.Main/Controls/WalkableWorldControl.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Controls/WalkableWorldControl.cs)
- [Client.Main/Core/Client/ClientEnums.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Core/Client/ClientEnums.cs)
- [Client.Main/MuGame.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/MuGame.cs)
- [Client.Main/Networking/ConnectionManager.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/ConnectionManager.cs)
- [Client.Main/Networking/NetworkManager.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/NetworkManager.cs)
- [Client.Main/Networking/PacketHandling/Handlers/CharacterDataHandler.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/PacketHandling/Handlers/CharacterDataHandler.cs)
- [Client.Main/Networking/PacketHandling/Handlers/ConnectServerHandler.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/PacketHandling/Handlers/ConnectServerHandler.cs)
- [Client.Main/Networking/PacketHandling/Handlers/ScopeHandler.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/PacketHandling/Handlers/ScopeHandler.cs)
- [Client.Main/Networking/PacketHandling/PacketBuilder.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/PacketHandling/PacketBuilder.cs)
- [Client.Main/Networking/PacketHandling/PacketRouter.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/PacketHandling/PacketRouter.cs)
- [Client.Main/Networking/Services/CharacterService.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/Services/CharacterService.cs)
- [Client.Main/Networking/Services/LoginService.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/Services/LoginService.cs)
- [Client.Main/Objects/Player/PlayerObject.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Objects/Player/PlayerObject.cs)
- [Client.Main/Scenes/GameScene.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Scenes/GameScene.cs)
- [Client.Main/Scenes/LoadScene.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Scenes/LoadScene.cs)
- [Client.Main/Scenes/SelectCharacterScene.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Scenes/SelectCharacterScene.cs)
- [Client.Main/appsettings.json](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/appsettings.json)
- [MuAndroid/.config/dotnet-tools.json](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/.config/dotnet-tools.json)
- [MuAndroid/AndroidManifest.xml](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/AndroidManifest.xml)
- [MuAndroid/Content/Arial.spritefont](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/Content/Arial.spritefont)
- [MuAndroid/Content/Content.mgcb](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/Content/Content.mgcb)
- [MuAndroid/MainActivity.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/MainActivity.cs)
- [MuAndroid/MuAndroid.csproj](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/MuAndroid.csproj)
- [README.md](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/README.md)

### bernatvadell/muonline

- [.github/workflows/build.yml](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/.github/workflows/build.yml)
- [Client.Main/Client.Main.csproj](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/Client.Main.csproj)
- [Client.Main/Controls/UI/SelectCharacter/CharacterCreationDialog.cs](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/Controls/UI/SelectCharacter/CharacterCreationDialog.cs)
- [Client.Main/Networking/NetworkManager.cs](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/Networking/NetworkManager.cs)
- [Client.Main/Networking/PacketHandling/PacketBuilder.cs](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/Networking/PacketHandling/PacketBuilder.cs)
- [Client.Main/Networking/Services/LoginService.cs](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/Networking/Services/LoginService.cs)
- [Client.Main/appsettings.json](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/appsettings.json)
- [MuAndroid/MainActivity.cs](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/MuAndroid/MainActivity.cs)
- [MuAndroid/MuAndroid.csproj](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/MuAndroid/MuAndroid.csproj)
- [README.md](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/README.md)

## Official platform references consulted

- [Microsoft .NET for Android build targets](https://learn.microsoft.com/en-us/dotnet/android/building-apps/build-targets): package/sign and SDK dependency targets.
- [Microsoft NativeAOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/): dynamic loading/runtime generation restrictions; not a statement about all Mono-based Android configurations.
- [EF Core SQLite limitations](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/limitations): schema and migration differences.
- [Android activity lifecycle](https://developer.android.com/guide/components/activities/activity-lifecycle): process termination/lifecycle constraints.
- [Android foreground-service restrictions](https://developer.android.com/develop/background-work/services/fgs/restrictions-bg-start): later service architecture limits.
- [Android local-network permissions](https://developer.android.com/privacy-and-security/local-network-permission): changing LAN behavior; device loopback behavior still requires a test.
- [MonoGame input overview](https://docs.monogame.net/articles/getting_to_know/whatis/input/): mobile gamepad API capability.
- [GitHub workflow artifacts](https://docs.github.com/en/actions/tutorials/store-and-share-data): delivering future APKs.

No secondary-source compatibility claim is used as proof of implementation.
