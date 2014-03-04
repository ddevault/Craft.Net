# Craft.Net

Craft.Net is a collection of several Minecraft-related .NET libraries. You can pick and choose different
libraries from Craft.Net to accomplish different tasks. The following libraries are included:

* *Craft.Net.AI*: Artificial intelligence for driving mob behavior (planned)
* **Craft.Net.Anvil**: Support for editing Minecraft Anvil worlds
  * Works with in-memory worlds or on-disk worlds
  * Can save worlds from Craft.Net.Client to disk
* **Craft.Net.Client**: A Minecraft multiplayer client library
  * Multiplayer client for connecting to servers
  * Utilities like LastLogin decryption, vanilla server list parsing/modification, etc
* **Craft.Net.Common**: Minecraft data types and common utilities
  * Defines values for biomes, difficulty, game modes, etc
  * Includes structs for working with item stacks, metadata dictionaries, etc
  * Cryptography support for interop with Minecraft's Java-based crypto
* **Craft.Net.Entities**: Structure and behavior of Minecraft entities
  * Includes players, items, blocks
* **Craft.Net.Logic**: Handles block and item interaction logic
  * Includes classes for all Minecraft blocks and items
  * Defines logic such as wheat growth, torch placement, etc
* **Craft.Net.Networking**: Generic Minecraft networking support
  * Includes all packet definitions
  * Includes AES/CFB stream crypto utilities
* **Craft.Net.Physics**: Simple physics engine that aims to recreate Minecraft phsyics
  * Support for AABB entities interacting with each other and Anvil worlds
* **Craft.Net.Server**: A Minecraft multiplayer server library
  * Includes entity management, physics
  * Many block interactions (such as growth, sand logic, etc) implemented
  * Survival mode, creative mode, adventure mode support
  * PvP combat
* **Craft.Net.TerrainGeneration**: Various terrain generators
  * Vanilla flatland support
  * Custom perlin-based terrain generator

More libraries are planned for the future. All libraries in Craft.Net support Windows, Linux, and Mac. The
long-term goal is to recreate the entire Minecraft ecosystem in C#, and to offer interopability with vanilla
Minecraft.

**A note on submodules**: Craft.Net includes external dependencies in the form of git submodules. When cloning
Craft.Net, please use `git clone --recursive git://github.com/SirCmpwn/Craft.Net.git`. If you've already cloned
it, you can fetch submodules manually with `git submodule update --init`.

The latest supported Minecraft version is **1.7.5**.

## Classic Support

There is work-in-progress support for Minecraft classic. The following libraries are available:

* **Craft.Net.Classic.Common**: Common utilities and classes for working with Minecraft Classic
* **Craft.Net.Classic.Networking**: Minecraft Classic networking support
  * Full support for Minecraft Classic networking protocol

Libraries like Craft.Net.Classic.Server, Craft.Net.Classic.Client, Craft.Net.Classic.Level, and more are also
planned.

## Community and Related Projects

There's an active Craft.Net development, discussion, and support community on IRC. Join us to chat at
[#craft.net on irc.freenode.net](http://webchat.freenode.net/?channels=craft.net&uio=d4). This is the best place
to ask questions. You can also let us know if you are doing anything cool with Craft.Net, like these related
projects:

* [PartyCraft](https://github.com/SirCmpwn/PartyCraft): Custom Minecraft server with plugin support
* [SMProxy](https://github.com/SirCmpwn/SMProxy): Proxy for Minecraft network protocol debugging and reverse engineering

## Snapshot Support

Craft.Net often supports Minecraft pre-release snapshots. If you want to try it out, look at the snapshot
branch. Run the following command to get snapshot support: `git pull origin snapshot && git checkout snapshot`

This branch has bleeding-edge support for the upcoming version of Minecraft.

## HTTPS Certificates on Mono

Mono does not trust any certificates whatsoever after install, and you need to tell it which ones to trust.
This means that Craft.Net.Client will fail to log into minecraft.net when connecting to online-mode servers.
The easiest way to fix this is to just trust all the Mozilla root certificates by running
`mozroots --import --sync`.

## Usage

Craft.Net is a very large, expansive project. A basic overview will be given here, but you are encouraged to
[visit the wiki](https://github.com/SirCmpwn/Craft.Net/wiki) to learn more about specific sub-projects.

### Generic Networking

You can use Craft.Net.Networking to work with the Minecraft network protocol. Craft.Net.Networking depends on
Craft.Net.Data to describe Minecraft data types. To use it, create a MinecraftStream around a NetworkStream,
and then you can use the `PacketReader` class to read packets from the stream. Craft.Net.Networking also
includes an AES/CFB stream that you can wrap your NetworkStream in to encrypt it.

### Servers

Here's an example of using Craft.Net.Server to run a Minecraft server:

```csharp
var server = new MinecraftServer(new IPEndPoint(IPAddress.Any, 25565));
var generator = new FlatlandGenerator();
// Creates a level in the "world" directory, using the flatland generator
// You may omit "world" to create a level in memory.
minecraftServer.AddLevel(new Level(generator, "world"));
minecraftServer.Start();
```

### Clients

Here's an example of connecting to a Minecraft server with Craft.Net.Client:

```csharp
var session = new Session("PlayerName");
// Uncomment this code to use the user's saved lastlogin instead
//var lastLogin = LastLogin.GetLastLogin();
//var session = Session.DoLogin(lastLogin.Username, lastLogin.Password);
var client = new MinecraftClient(session);
// Connect to the server at 127.0.0.1:25565
client.Connect(new IPEndPoint(IPAddress.Loopback, 25565));
// Alternative:
//client.Connect("server.address.here:25565");
```

Session.DoLogin can be used to authenticate with minecraft.net for online-mode servers. This code also
includes a (commented out) example of decrypting the user's LastLogin file.

### World Editing

You can use Craft.Net.World do manipulate Minecraft save data. You can also add a reference to Craft.Net.Logic
to get some nice extension methods that let you work with blocks by name
(ex. `world.SetBlock(Coordinates3D.Zero, new BedrockBlock())`). Here's an example of loading a world:

```csharp
var level = Level.LoadFrom("world");
level.DefaultWorld.SetBlockId(Coordinates3D.Zero, 22); // Set to lapis block
level.Save();
```

If Craft.Net supports the terrain generator the level.dat file specifies, it will automatically generate
chunks when attempting to work with blocks in an ungenerated area. You can also explicitly set the terrain
generator from the constructor. You may add support for your own generators by implementing IWorldGenerator.

## Building from source

On Windows, add "C:\Windows\Microsoft.NET\Framework\v4.0.30319" to your path. Then, run this from the root
of the repository:

    msbuild

On Linux and Mac, install Mono 2.10 or better, and then use this command:

    xbuild

You can also build Craft.Net with Visual Studio 2010 or newer, and any version of MonoDevelop or SharpDevelop.

## Contributing

If you wish to contribute your own code to Craft.Net, please create a fork. You are encouraged to follow
the code standards currently in use, and pull requests that do not will be rejected. You are also
encouraged to make small, focused pull requests, rather than large, sweeping changes. For such changes,
it would be better to create an issue instead. You can
[browse other pull requests](https://github.com/SirCmpwn/Craft.Net/pulls?direction=desc&page=1&sort=created&state=closed)
and see which of them have been accepted for more guidance.

## Getting help

You can get help by [making an issue](https://github.com/SirCmpwn/Craft.Net/issues) on GitHub, or joining
\#craft.net on irc.freenode.net. If you are already knowledgable about using Craft.Net, consider contributing
to the [wiki](https://github.com/SirCmpwn/Craft.Net/wiki) for the sake of others.

## Dependencies

We try to keep these to a minimum, and refactor them away when possible. The current list is:

* [DotNetZip](http://dotnetzip.codeplex.com/) for compression/decompression with zlib
* [fNbt](https://github.com/fragmer/fNbt) for NBT data manipulation
* [BouncyCastle](http://www.bouncycastle.org/csharp/) for encryption on Mono.

## Licensing

Craft.Net uses the permissive [MIT license](http://www.opensource.org/licenses/mit-license.php/).

In a nutshell:

* You are not restricted on usage of Craft.Net; commercial, private, etc, all fine.
* The developers are not liable for what you do with it.
* Craft.Net is provided "as is" with no warranty.

[Minecraft](http://minecraft.net) is not officially affiliated with Craft.Net.
