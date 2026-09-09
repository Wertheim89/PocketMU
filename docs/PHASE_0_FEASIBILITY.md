# Phase 0 feasibility audit

Audit date: 9 September 2026. Scope: source-based technical feasibility, not a port or working APK.

## Decision

**CONDITIONAL GO for Phase 1 experiments; no claim that the current projects combine into a working Android realm.**

Recommend one .NET 10 Android APK containing a reduced OpenMU library host and a MonoGame client, communicating over IPv4 loopback. Use an independently implemented embedded persistence provider: SQLite for durable account/character state and a versioned, device-local configuration graph. Keep simulation alive only during an active play session initially.

The biggest Android hosting risk found in source is OpenMU's runtime Roslyn plug-in compilation and its assumptions about physical assembly files and trusted-platform assemblies. The biggest data task is replacing PostgreSQL-dependent persistence without losing the semantics of player saving. Both require early proofs.

The newer related client, bernatvadell/muonline, is technically the stronger candidate because it already targets .NET 10 and has character creation and an Android workflow. **Adoption is conditional on establishing permission to reuse/distribute it.** It is not a verified permissively licensed replacement. The specifically requested bhrama-br client remains the audited baseline; upgrading it is a fallback if permission is established there and the newer client cannot be used.

## Evidence and limits

Inspected the current default-branch trees and selected implementation/configuration files at:

| Project | Pinned commit |
| --- | --- |
| MUnique/OpenMU | 9693f8f6cf037909c9dfd3a9a9438fffcabd9837 |
| bhrama-br/muonline-android | 963a8c18ddce979f3df19d648cd802fdd740d535 |
| bernatvadell/muonline, additional comparison | 7baa3454b890c1b33189fd2bc89c7f6d7af3058d |

Full trees were returned without truncation. Only selected text source/configuration files were retrieved; proprietary asset files, installers and APKs were not downloaded. [Source audit](SOURCE_AUDIT.md) lists the inspected files.

**Observed** below means visible in source. **Proposed** means a design choice. **Unverified** means no build/device/runtime proof. This environment has a .NET 7 runtime but no .NET SDK; no upstream build, packet exchange, APK installation, SQLite adapter or Odin performance test was executed. Workflow definitions show intent, not a successful run. No Phase 1 implementation was performed.

## Protocol and direct compatibility

### Exactly what the sources select

OpenMU is multi-version, not only Season 6. Its initialization tree includes 0.75, 0.95d and Season 6 profiles. The Season 6 initializer explicitly describes **1.04d, Season 6 Episode 3, English**. It registers:

| Server profile | Internal identity | Version bytes | Serial |
| --- | --- | --- | --- |
| GMO Season 6 Episode 3 | season 6, episode 3, English | 31 30 34 30 34, ASCII 10404 | k1Pk2jcET48mxL3b |
| Related open-source-client profile | season 106, episode 3, English | 32 30 34 30 34, ASCII 20404 | k1Pk2jcET48mxL3b |

Internal season 106 denotes the alternate client protocol profile; it is not gameplay Season 106. Evidence: [DataInitialization.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/VersionSeasonSix/DataInitialization.cs#L53) and [ClientVersionResolver.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameServer/ClientVersionResolver.cs#L22).

The requested client has a Season6 / Version097 / Version075 selector, and its shipped configuration selects **Season6, ClientVersion 1.04d, serial 0123456789ABCDEF**. It references OpenMU Network and Network.Packets **0.9.4**, whereas the inspected current server project sources are .NET 10 and identify newer package versions. A shared library name is not an exact revision match. Evidence: [ClientEnums.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Core/Client/ClientEnums.cs), [appsettings.json](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/appsettings.json#L35) and [Client.Main.csproj](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Client.Main.csproj#L24).

A concrete handshake mismatch: requested-client NetworkManager sends raw ASCII bytes for the string 1.04d: **31 2E 30 34 64**. OpenMU's configured bytes are **31 30 34 30 34**. Its resolver falls back to the default 6.3 English profile for unknown bytes, so this is **not proof that login must fail**. The inspected login handler also does not validate ClientSerial; do not misreport the serial difference as a demonstrated rejection. Normalize the version and configure matching serial/profile explicitly rather than relying on fallback. Evidence: [NetworkManager.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/NetworkManager.cs#L78), [ClientVersionResolver.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameServer/ClientVersionResolver.cs#L67), [LogInHandlerPlugIn.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameServer/MessageHandler/Login/LogInHandlerPlugIn.cs#L57).

### Packet path traced

| Milestone exchange | Source evidence | Assessment |
| --- | --- | --- |
| Connect-server discovery | Client uses raw TCP for discovery and parses server connection information | Existing path; shipped host is a private LAN address, not localhost |
| Login | LoginLongPassword; XOR3 credentials; Xor32 + SimpleModulus outgoing game connection; SimpleModulus incoming | Same family and direction as OpenMU's 6.3 encryption factory; raw version string needs normalization |
| Character list/select | RequestCharacterList and SelectCharacter builders, selection scene, game information handlers | Implemented paths exist; live exchange unverified |
| Character creation | No request builder/service method or creation form found in the inspected requested-client selection/network paths | Required work for the original client; scene code that creates rendering objects is not server-side character creation |
| Enter map | CharacterInformation standard/extended handlers and ClientReadyAfterMapChange | Plausible, but optional/extended packet lengths require fixtures |
| Move | WalkRequest, packed directions, configurable direction map | Candidate direct support; test direction mapping, server validation and acknowledgements |
| Attack/experience | HitRequest builder, hero attack invocation, ObjectHit routing and ExperienceGained handlers | Code exists; a real kill, death/despawn and authoritative XP update are unproved |
| Inventory/save | Inventory handler and server persistence boundary | No client-side save substitute; server must commit durable state |

Evidence: [PacketBuilder.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/PacketHandling/PacketBuilder.cs#L21), [ConnectionManager.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/ConnectionManager.cs#L114), [Season6Episode3NetworkEncryptionFactoryPlugIn.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/PlugIns/Season6Episode3NetworkEncryptionFactoryPlugIn.cs#L21), [SelectCharacterScene.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Scenes/SelectCharacterScene.cs), [GameScene.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Scenes/GameScene.cs#L652), [CharacterDataHandler.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Networking/PacketHandling/Handlers/CharacterDataHandler.cs#L582).

**Answer to direct compatibility:** a major protocol rewrite is not justified by the evidence; a direct, version-aligned connection is a credible hypothesis. It is not certified compatible as shipped. Phase 1 must test encryption/counters, exact packet lengths, login padding/encoding, version selection, character creation, movement, damage, XP, inventory and reconnect. Use generated synthetic packets; do not distribute captured proprietary traffic or assets.

The newer client uses 2.04d with a normalization function producing 20404 and matches OpenMU's alternate profile more deliberately. Pair this with the matching endpoint/profile, not merely the original connect-server port. Evidence: [appsettings.json](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/appsettings.json#L36), [NetworkManager.cs](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/Networking/NetworkManager.cs#L806), [CharacterCreationDialog.cs](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/Client.Main/Controls/UI/SelectCharacter/CharacterCreationDialog.cs).

## OpenMU startup and Android feasibility

### Current runtime and services

Observed all-in-one startup is a **net10.0 Microsoft.NET.Sdk.Web** application using WebApplication.CreateBuilder. It composes login, guild, friend, chat, game and connect-server services, setup/configuration services, logging and optional web administration. It supports an in-memory demo path. Container deployment is one option, not a requirement of the game logic. Evidence: [MUnique.OpenMU.Startup.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Startup/MUnique.OpenMU.Startup.csproj#L5), [Program.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Startup/Program.cs#L243).

Important assumptions to remove or replace:

| Area | Observed behavior | PocketMU implication |
| --- | --- | --- |
| Framework | Core GameLogic, GameServer, Network and PlugIns target net10.0 | Original net9.0-android client cannot simply reference the current core; align on .NET 10 |
| Hosting | Web SDK and admin-panel dependency remain even with UI disabled | Write a small Android composition root; do not embed the desktop Startup executable |
| Configuration | Current-directory appsettings.json, environment overrides, database settings and persisted SystemConfiguration | Supply explicit app-private paths and settings; no console prompts or external provisioning |
| Lifecycle | Console cancel/process-exit loop, IHost and StartAsync/StopAsync services | Explicit async realm lifecycle controlled by the application; Android may kill without final callbacks |
| Filesystem | Relative log paths, physical assembly locations, external plugins path | App-private bounded logs and bundled/static plugins; no working-directory assumptions |
| Persistence | PostgreSQL provider or volatile in-memory demo | New durable embedded adapter required |
| Background work | Game simulation, hosted containers, periodic saves, offline/bot AI | Bounded tasks with cancellation; do not equate .NET hosted services with Android background-service permission |
| Networking | TCP listeners; loopback resolver exists; listener binds IPAddress.Any | Change actual bind address as well as advertised address |
| Native/transitive code | Sockets/pipelines, MonoGame Android runtime, future SQLite native library | Verify Android arm64 assets and API support; dependency compatibility is not proven by C# compilation |

### Most important runtime obstacle

PlugInManager generates proxy implementations via PlugInProxyTypeGenerator. That generator invokes Roslyn at runtime. SyntaxTreeExtensions enumerates Nito DLLs alongside Assembly.Location, consults TRUSTED_PLATFORM_ASSEMBLIES, creates metadata references from files, emits an assembly and calls Assembly.Load(byte[]).

This is a concrete desktop-runtime dependency, not merely generic reflection. Android packaged assemblies need not be available as ordinary sibling DLLs, and its runtime environment is not guaranteed to expose the same trusted-assembly list. Disabling trimming alone does not resolve those assumptions.

Evidence: [PlugInManager.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/PlugIns/PlugInManager.cs#L406), [PlugInProxyTypeGenerator.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/PlugIns/PlugInProxyTypeGenerator.cs#L59), [SyntaxTreeExtensions.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/PlugIns/SyntaxTreeExtensions.cs#L43).

Proposed first spike: initialize actual required plug-in points in a minimal .NET 10 Android arm64 app. If stock generation fails, generate/register these proxies at build time using the existing semantics and preserve plug-in discovery metadata. Do not claim it uses Reflection.Emit: the inspected code uses Roslyn compilation. Other transitive dynamic code remains to be audited.

Use ordinary .NET for Android deployment initially; do not choose NativeAOT as a shortcut. NativeAOT explicitly restricts dynamic loading/code generation and its Android support is experimental. This does not mean all Android .NET deployments have those restrictions. [Microsoft NativeAOT documentation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/).

No exhaustive Android unsupported-API analysis was run. Exact remaining failing APIs, if any, require the compile/runtime probe. Also test expression/dynamic-code use in mapping, configuration and dependencies, timer behavior, time-zone access, crypto, exception handling and repeated start/stop.

### Hosting options

| Strategy | Verdict | Reason |
| --- | --- | --- |
| A: stock all-in-one server inside an Android app | Reject as-is | Web host, desktop startup and PostgreSQL assumptions remain |
| B: full server moved to Android services | Reject as first step | Service packaging does not fix runtime, data or dependency issues |
| C: reduced embedded OpenMU in same app/process | **Recommended** | Reuse game rules and protocol while owning lifecycle and persistence; one runtime and install |
| D: separate local Android process/service plus client | Reserve | Better crash isolation, but IPC/readiness, duplicated runtime memory and service restrictions add work |
| Linux ARM64 server binary, Docker, Termux/proot or PostgreSQL daemon | Reject for product | Does not meet the normal self-contained APK requirement; Linux ARM64 is not evidence of Android ABI compatibility |
| Replace loopback protocol with direct game-state calls | Reject initially | Couples the renderer to server internals and bypasses the compatibility path being tested |

Retain local login bookkeeping and the guild/friend interfaces required by GameServer construction. Optional features may be disabled or receive tested minimal implementations, but removing a service registration is not proof it is unnecessary. No Dapr sidecars, web admin server or remote account service should be required for the first realm.

## Persistence

### Existing abstractions

The replacement seam is **IPersistenceContextProvider**, its IRepositoryProvider, generic IRepository<T> and IContext, and specialized IPlayerContext / IConfigurationContext / friend/guild contexts. IContext includes creation, loading, deletion, save and attachment semantics. IPlayerContext includes authentication, account and character lookup, paging/search, and additional feature-specific queries.

Player.SaveProgressAsync delegates into persistence and disconnect attempts to save. Bots also use these contexts. An adapter must preserve object identity and cross-references to game definitions, not merely serialize a Character object. Evidence: [IPersistenceContextProvider.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/IPersistenceContextProvider.cs), [IContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/IContext.cs), [IPlayerContext.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/IPlayerContext.cs#L47), [Player.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Player.cs#L1200).

### Why a connection-string swap will not work

ConnectionConfigurator implements only DatabaseEngine.Npgsql and throws for other engines. The provider uses EF Core 10.0.2 with Npgsql EF provider 10.0.0, custom migrations, separate role/context settings, generated EF models and cached graph loading. JsonQueryBuilder emits PostgreSQL-specific row_to_json, json_build_object, array aggregation and schema-qualified SQL. This requires a new implementation or substantial provider refactoring, not UseSqlite alone.

Evidence: [ConnectionConfigurator.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/ConnectionConfigurator.cs#L132), [Directory.Packages.props](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Directory.Packages.props#L42), [JsonQueryBuilder.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/Json/JsonQueryBuilder.cs#L26), [PersistenceContextProvider.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/EntityFramework/PersistenceContextProvider.cs).

SQLite has no PostgreSQL-style schemas and differs in migrations and type/query support. Reusing all existing migrations is inappropriate. [EF Core SQLite limitations](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/limitations).

The existing InMemory provider is useful for a temporary startup test, **not durable progression**: its documentation says changes are immediately shared across contexts regardless of SaveChanges. A JSON dump on application exit would inherit identity/concurrency problems and cannot handle abrupt process death. Evidence: [InMemoryPersistenceContextProvider.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/InMemory/InMemoryPersistenceContextProvider.cs#L15).

### Proposed embedded approach

Use a separate PocketMU persistence provider, without referencing Persistence.EntityFramework in the Android host:

- Immutable/versioned game definitions loaded into memory, with stable identifiers. Seed definitions from reviewed code; terrain comes from the lawful device-local import.
- SQLite account and character aggregates, schema version and transaction boundaries. A practical first schema can store indexed account/character metadata plus versioned DTO snapshots for inventory, stats, skills, quests and progression, referencing configuration by stable IDs.
- Explicit DTO conversion/reconstruction; do not serialize arbitrary runtime/EF object graphs. Preserve inventory slots, item properties, money, XP, map/location and authentication hashes.
- One coordinated writer, atomic transactions, foreign-key/uniqueness validation where modeled, bounded checkpoints, and a verified flush on deliberate Stop.
- Implement the authentication and character-creation paths used by the POC. Reject unsupported operations explicitly; do not return success while silently dropping data.
- Later expand to normalized tables only where querying, concurrency or migrations require them.

SQLite API choice remains a spike decision: Microsoft.Data.Sqlite or another Android-supported binding, with its ARM64 native packaging tested. EF Core SQLite is possible but not selected as a wholesale transplant of OpenMU's EF layer.

**Difficulty:** high for a correct POC adapter, very high for parity with all OpenMU persistence features. The interfaces reduce coupling but still expose a large connected model. Do not estimate this as a small configuration task.

Phase 1 must prove account authentication and character creation, kill/XP/item/stat saves, close/reopen and a crash during a save. The last acknowledged save must survive; progress since the last completed checkpoint must have a stated bound. Android onDestroy is not a reliable durability mechanism. [Android activity lifecycle](https://developer.android.com/guide/components/activities/activity-lifecycle).

## Bots

**Yes, actual server-side player bots exist in this OpenMU revision.** BotPlayer inherits OfflinePlayer and is connection-less. BotManager loads each account/character into its own persistence context and starts it without a separate client. BotGenerator creates account/character records, marks Account.IsBot and saves via the normal context.

BotFeaturePlugIn and BotConfiguration control enablement, account counts, characters per account, capacity percentage, online presence rotation and startup profiles. Defaults include Disabled, 10 accounts and a 60% bot-capacity limit; these are server defaults, not a recommendation for Odin. BotManager stops through the normal logout/save path; persistence across restarts depends on a durable provider.

Evidence: [BotPlayer.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotPlayer.cs#L15), [BotManager.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotManager.cs), [BotGenerator.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotGenerator.cs#L147), [BotConfiguration.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/GameLogic/Bots/BotConfiguration.cs#L24).

Dependencies include game logic, offline-player/MU-helper behavior, navigation/pathfinding, plug-in registration, account/configuration models and persistence; there is no requirement for another renderer or a network-connected bot client. BotPlayer also documents attribute-graph race failures and restart recovery. Source existence is not proof of stable large populations or acceptable heat/battery load.

Recommendation: zero bots for Milestone 0, then 1–3 in a later measured experiment. No promised population cap until on-device profiling.

## Android client, assets and controls

Requested client: net9.0-android, minimum API 23, manifest target API 35; MonoGame Android and content builder 3.8.3. MainActivity derives from AndroidGameActivity, creates MuGame, keeps the screen on and starts rendering. It does not provide a local-realm lifecycle. MuGame starts connecting during LoadContent, and its exit handler ultimately calls Environment.Exit(0), which would threaten an in-process realm's final save.

Evidence: [MuAndroid.csproj](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/MuAndroid.csproj), [AndroidManifest.xml](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/AndroidManifest.xml), [MainActivity.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/MainActivity.cs#L86), [MuGame.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/MuGame.cs#L578).

The client is not asset-free at runtime. Constants and loaders expect original MU files; README/paths refer to MU Red 1.20.61 data rather than proving a pure Season 6 asset pack. Its source readers/rendering approach and its network season are separate compatibility questions.

Constants has a Windows Debug data path and a release path based on AppDomain.BaseDirectory. LoadScene automatically downloads a Data archive when it considers assets absent, including a fallback URL. Its presence check is not a complete manifest validation. Remove this behavior before any PocketMU run; replace it with an explicit device-local import and validation screen. No referenced asset URL was followed during this audit.

Evidence: [Constants.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Constants.cs#L21), [LoadScene.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Scenes/LoadScene.cs#L126).

The minimum data profile must be discovered by tracing actual Lorencia, character, monster, UI, font and audio loads. Pin file hashes/format versions locally; reject missing/incompatible files before starting. Verify server collision terrain and client coordinates agree. A Season 20-era rendering pack must not silently redefine Season 6 gameplay.

**Server-side assets also matter:** OpenMU's initialization project embeds Terrain*.att resources and loads them through TerrainUpdateHelper. Do not copy those files or silently embed an upstream initialization binary containing them. Replace that resource path with lawful device-imported terrain or provenance-cleared data. The upstream MIT code licence is not sufficient evidence of asset provenance. The client's MGCB manifest also lists images/fonts/effects; review provenance and replace unverified media with original minimal placeholders before packaging.

Evidence: [MUnique.OpenMU.Persistence.Initialization.csproj](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/MUnique.OpenMU.Persistence.Initialization.csproj#L99), [TerrainUpdateHelper.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/TerrainUpdateHelper.cs#L31), [Content.mgcb](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/MuAndroid/Content/Content.mgcb).

The Android configuration already has a useful app-private path: MuGame.EnsureAndroidConfig copies packaged appsettings.json into FilesDir only when absent. Updating the packaged host setting alone can therefore leave an existing installation pointed at the old LAN server. PocketMU needs an explicit configuration migration/override for localhost. Evidence: [MuGame.EnsureAndroidConfig](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/MuGame.cs#L106).

Use Android app-private storage for saves/configuration and a Storage Access Framework picker for import. Do not rely on legacy external-storage permissions or public Downloads paths. Data copy/extraction must validate destination paths, size limits and available space. Large asset copies need progress and a recoverable partial-import state; no download fallback.

### Controller support

The inspected input loop polls keyboard, mouse and touch; GameScene triggers attack from mouse interaction. No gamepad action mapping was found in the audited central input/gameplay paths. Do not confuse MonoGame's API availability with native Odin controls already being implemented.

MonoGame exposes gamepad input on supported mobile devices, so native controls are credible: implement an action layer for movement, target selection, attack, interact, inventory and menu navigation; handle dead zones, hotplug and soft-keyboard entry. Test on the actual Odin. Minimal touch controls may be used for Milestone 0; advanced controller UI remains deferred. Evidence: [MuGame.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/MuGame.cs#L428), [GameScene.cs](https://github.com/bhrama-br/muonline-android/blob/963a8c18ddce979f3df19d648cd802fdd740d535/Client.Main/Scenes/GameScene.cs#L652), [MonoGame input documentation](https://docs.monogame.net/articles/getting_to_know/whatis/input/).

## Localhost and Android lifecycle

Loopback TCP is the proposed transport. Use literal 127.0.0.1 to avoid DNS/IPv6 ambiguity. A same-device client and service can use local TCP without an external realm; validate this in airplane mode rather than checking internet reachability first.

Default OpenMU initialization creates connect-server ports starting at **44405**, ordered by client season: the two Season 6 definitions therefore produce 44405 and 44406. Game endpoints start at **55901** and increase for each client definition and server; do not assume which profile gets a port without inspecting the initialized configuration. Chat starts at 55980, but separate chat is unnecessary for the first POC.

For PocketMU, create only one selected profile and explicitly assign connect 44405 and game 55901 to it. Return 127.0.0.1 and the selected game port in discovery. Alternatively, if using upstream's full dual-profile initialization, configure the alternate client to its actual alternate discovery/endpoint pair. Never change the version string without the endpoint profile.

Evidence: [DataInitializationBase.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Persistence/Initialization/DataInitializationBase.cs#L241), [ServerInfoRequestHandler.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/ConnectServer/PacketHandler/ServerInfoRequestHandler.cs), [LoopbackIpResolver.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/LoopbackIpResolver.cs).

**Important:** LoopbackIpResolver controls the advertised address; Listener.Start actually binds IPAddress.Any. Patch the listener to accept and bind a loopback address. Merely setting the resolver does not prevent LAN access. Loopback is also not an authorization boundary against other apps on the same device; use a local account and do not expose administration.

Evidence: [Listener.cs](https://github.com/MUnique/OpenMU/blob/9693f8f6cf037909c9dfd3a9a9438fffcabd9837/src/Network/Listener.cs#L64).

The manifest already requests INTERNET, which grants socket capability and does not itself require an internet connection. Keep the appropriate permission, eliminate external endpoints and do not infer offline behavior from the permission label. Android local-network rules are evolving; the official page concerns LAN restrictions and does not establish every loopback behavior for this app. Record the Odin OS version and test the selected target/runtime. [Android local-network guidance](https://developer.android.com/privacy-and-security/local-network-permission).

Proposed startup: validate import → open/migrate store → load definitions and create/choose local account → start realm and loopback listeners → expose readiness → allow client discovery/login. Use a real readiness result, timeouts and bounded retry, not a fixed delay.

Proposed stop: stop accepting new work → disconnect/save player → stop simulation/bots → commit/checkpoint → release sockets/store. On app backgrounding, request a checkpoint promptly and stop/suspend the realm deliberately; tolerate Android terminating the process without finishing that sequence.

For Milestone 0 no persistent Android foreground service is required: play while the app is visible. A later separate service must start from permitted user-visible context and satisfy service-type/notification requirements; it is not a guarantee against process death. [Android foreground-service restrictions](https://developer.android.com/develop/background-work/services/fgs/restrictions-bg-start).

## APK automation

**Viable in principle, not verified for PocketMU.** GitHub Actions can install a .NET SDK/Android workload, Java and Android SDK packages, restore NuGet and local tools, build/package a self-contained test APK and upload it as an artifact. The owner need only download/install it.

The requested client has no .github/workflows build definition in the inspected tree. Its Android csproj restores local dotnet tools. Its MGCB file compiles shaders, a font and images, so bare dotnet build is not the whole dependency story.

The newer related project provides a concrete Windows-runner Android job using .NET 10, Java 21, SDK platform 36, NDK 27.2.12479018, workload/tool restore, publish and APK artifact upload. This is useful source evidence; its latest job success was not established and no artifact was downloaded. Evidence: [build.yml](https://github.com/bernatvadell/muonline/blob/7baa3454b890c1b33189fd2bc89c7f6d7af3058d/.github/workflows/build.yml#L52).

Phase 1 build recipe:

1. Pin SDK/workload, Java, Android SDK and MonoGame versions after a minimal arm64 toolchain probe; do not rely on floating latest.
2. Build only the Android project and required library graph, not every desktop/iOS/web project.
3. Audit/allowlist packaged content; exclude proprietary assets and server embedded terrain. No asset download step.
4. Package android-arm64, APK format, with managed/runtime assemblies included. A test APK must be installable without fast deployment or an attached development machine.
5. Upload only the APK plus a short build/provenance report after checks pass. Fail if no APK exists.
6. Test install, first launch and airplane-mode flow on Odin. GitHub success cannot substitute for Adreno/device testing.

For a minimal asset-free .NET Android project, the SignAndroidPackage target can create/sign an APK; Release/self-contained properties and a test signing key must be deliberate. Later release signing uses a protected keystore and secrets, restricted release jobs and a stable key for updates. Do not publish secrets or use a changing debug key for long-term save-preserving upgrades.

References: [Microsoft Android build targets](https://learn.microsoft.com/en-us/dotnet/android/building-apps/build-targets), [GitHub workflow artifacts](https://docs.github.com/en/actions/tutorials/store-and-share-data).

No APK workflow is added in Phase 0: there is no cleared client/content import or Android host to build. The added workflow only checks this documentation repository's allowed file set.

## Risks and exit conditions

BLOCKER means it prevents the specified next outcome as-is, not that research is impossible.

| Risk | Level | Evidence / required resolution |
| --- | --- | --- |
| Stock desktop Startup is not an Android host | BLOCKER for unchanged integration | Web/console/filesystem composition; replace composition root |
| Plug-in proxy runtime compilation | HIGH; possible Android blocker | Roslyn + physical metadata + assembly loading; prove on device or generate proxies during build |
| No durable embedded provider | BLOCKER for Milestone 0 | PostgreSQL-only provider and volatile demo; implement transactional saves |
| Client reuse/redistribution licence unresolved | BLOCKER for client adoption/distribution | No clear grant found in requested tree; related client states educational/non-commercial terms |
| Proprietary assets / embedded terrain | BLOCKER for asset-containing package | Import-only architecture, provenance audit, no upstream asset binaries |
| .NET 9 client / .NET 10 server, packet library drift | HIGH | Align dependency graph and exact packet fixtures |
| Original client missing character-creation path | HIGH | Implement small path or use permission-cleared newer client |
| Version-byte and endpoint-profile mismatch | MEDIUM | Explicit normalization and one configured profile; don't rely on fallback |
| Asset format/map collision mismatch | HIGH | Lawful data manifest and Lorencia rendering/collision test |
| Android pause/kill during save | HIGH | Transactions, checkpoint acknowledgment and crash/relaunch tests |
| Native dependencies, trimming and shaders | HIGH | ARM64 package inspection, plugin test and device render test |
| CPU, RAM, thermals, battery | HIGH | No measured budget; cap maps/FPS, zero bots initially, measure sustained play |
| Gamepad integration | MEDIUM | API exists, game actions still need mapping and Odin test |
| Loopback wiring | LOW after bind/profile fixes | Deterministic local ports; readiness, occupied-port and airplane-mode tests |
| Bot population and concurrency | HIGH for scale | Server code has recovery for AI faults; defer and measure a few bots |
| Automated test APK delivery | MEDIUM | Toolchain is credible; no PocketMU build has yet passed |

## Comparison and rejected alternatives

| Client/server integration | Assessment |
| --- | --- |
| OpenMU + requested bhrama-br MonoGame Android client | Promising protocol lineage; old .NET/packet dependency, raw version encoding, missing creation path, downloader and lifecycle changes needed; licence unresolved |
| OpenMU + newer bernatvadell MonoGame client | Stronger technical baseline (.NET 10, 3.8.4.1 MonoGame, character creation, version normalization, Android CI); licence and asset review still block automatic adoption |
| OpenMU adapted into client process | Recommended host shape with a clean realm boundary; do not merge server rules into rendering code |
| Separate local Android service/process + game client | Viable reserve architecture if measured lifecycle/isolation needs justify it; not the simplest POC |
| A different verified open-source compatible client | No stronger, clearly licensed Android candidate was established by this bounded comparison; don't label the related public repo as cleared OSS |
| Original proprietary Windows client under emulation | Rejected for initial Android APK: incompatible packaging/UX and asset/legal assumptions |
| PostgreSQL-to-SQLite provider-name swap | Rejected: graph SQL, schemas, migrations and semantics differ |
| In-memory demo plus save-on-exit only | Rejected for progression: no crash-safe durability |

## Exactly what Phase 1 must solve

Phase 1 requires a new instruction from the owner. Its scope is Milestone 0 only:

1. Establish client code reuse permission and a lawful asset/import plan, including server terrain and packaged UI media.
2. Select/pin the .NET 10 client/server/profile and dependency versions. Produce an asset-free, self-contained ARM64 Android toolchain smoke APK.
3. Prove real OpenMU plug-in dispatch on Android; replace runtime compilation with build-time proxies if needed.
4. Compose the smallest local realm without web/Dapr/PostgreSQL services and bind only loopback.
5. Implement and validate local account creation and durable embedded persistence, including abrupt termination.
6. Remove client download/desktop-path/forced-exit assumptions; add import validation and readiness/reconnect handling.
7. Prove the exact login/create/select/move/attack/XP packet sequence; add the creation path if using the original client.
8. Complete the full Odin airplane-mode test: Lorencia, move, kill, save, close, reopen, verify progression.

If the Android plug-in/runtime spike fails without a bounded fix, stop and reassess architecture before adding gameplay. If code permission or compatible lawful data cannot be established, do not distribute an APK based on that client/assets. Stop again after Milestone 0; expansion is a separate decision.
