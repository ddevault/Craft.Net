# Craft.Net

Craft.Net is the collective name for several Minecraft-related .NET libraries. You can pick and choose
different libraries to accomplish different tasks. Libraries are included for things such as Minecraft
multiplayer networking and world editing, as well as a multiplayer client and server. The crown jewel
of Craft.Net is the fully functional Minecraft server, with support for everything from survival mode
resource gathering, multiplayer PvP, entity physics, and more.

Craft.Net currently supports Minecraft 1.6 on Windows, Linux and Mac.

**A note on submodules**: Craft.Net includes external dependencies in the form of git submodules.

Use this command to clone the project:

    git clone --recursive git://github.com/SirCmpwn/Craft.Net.git

If you've already cloned it, use this to fetch submodules:

    git submodule update --init

## Snapshot Support

Craft.Net supports most Minecraft pre-releases shortly after they are released. If you want to have
support for snapshots, run the following commands in your git repository:

    git pull origin snapshot
    git checkout snapshot

You'll now be on the snapshot branch, with bleeding-edge support for the latest Minecraft snapshots.

## HTTPS Certificates on Mono

Mono does not trust any certificates whatsoever after install, and you need to tell it which ones to
trust. This means that `Craft.Net.Client` will fail to log into minecraft.net when connecting to
online-mode servers. The easiest way to fix this is to just trust all the Mozilla root certificates
by running `mozroots --import --sync`.

## Features

This is not an exhaustive list.

### Craft.Net

* Full support for the 1.6 (version 72) Minecraft protocol
* Item stack and entity metadata manipulation
* Cryptography support that provides full interoperability with Java clients and servers

### Craft.Net.Data

* Anvil world manipulation
* All 1.6 items and blocks
  * Includes support for most operations, such as planting seeds
* Entity management and simulation
  * Includes physics for entity versus terrain physics simulation
  * Paintings, item frames, players, item stacks, etc
* Custom terrain generation
  * Includes flatland generator, allows custom generators
* Inventory and window management
  * Allows for manipulation of player inventories and windows (crafting tables, etc)
* Math helpers for common Minecraft and 3D operations
  * AABB, ray, vectors and vector rotation, etc

### Craft.Net.Server

* 1.6 multiplayer server
* Provides a layer on top of Craft.Net.Data for multiplayer Minecraft
* Fast networking - low CPU and memory usage
* Modular and extensible
  * Use all or part of it, and customize it to your needs
* In-game weather management

### Craft.Net.Client

*Craft.Net.Client is the newest addition to Craft.Net, and it is unstable and lacking in features.*

* Connect to and play on 1.6 multiplayer servers
* Handles loaded and unloaded chunks from the remote world
  * Can also save server world to disk
* Encrypt/decrypt local lastlogin files (useful for launchers)
* Liase with Minecraft.net for session authentication

In short, Craft.Net is the ideal solution for any Minecraft-related activities on the .NET Framework.

## Usage

Craft.Net is a very large, expansive project. A basic overview will be given here, but you are
encouraged to [visit the wiki](https://github.com/SirCmpwn/Craft.Net/wiki) to learn more.

### General networking

Use this code to read the next Minecraft packet from a given stream, and write it to another:

```csharp
var packet = PacketReader.ReadPacket(stream);
// ...
var output = new MinecraftStream(otherStream);
packet.WriteTo(output);
```

There are also various crypto utilities for encrypting a stream with AES/CFB, or creating Minecraft-
style SHA-1 hex digests, or decoding/encoding ASN.1 x509 certificates.

### Servers

To run a Minecraft server, simply use the following code:

```csharp
var server = new MinecraftServer(new IPEndPoint(IPAddress.Any, 25565));
var generator = new FlatlandGenerator();
// Creates a level in the "world" directory, using the flatland generator
// You may omit "world" to create a level in memory.
minecraftServer.AddLevel(new Level(generator, "world"));
minecraftServer.Start();
```

You need to create a server on a certain endpoint, then provide it a level to spawn players in, and
then start the server.

### Clients

To connect to a Minecraft server, use this code:

```csharp
var session = new Session("PlayerName");
// Uncomment this code to use the user's saved lastlogin
//var lastLogin = LastLogin.GetLastLogin();
//var session = Session.DoLogin(lastLogin.Username, lastLogin.Password);
var client = new MinecraftClient(session);
// Connect to the server at 127.0.0.1:25565
client.Connect(new IPEndPoint(IPAddress.Loopback, 25565));
```

Create a client with a given session (either offline mode with just a username, or online mode with
a username and password via `Session.DoLogin`. Then specify your endpoint and connect.

[Click here](https://gist.github.com/8377075da938b128bef7) if you want to use something like
"c.nerd.nu:25565" and don't know how.

### Data Manipulation

Want to mess with Minecraft data but don't need networking? Use Craft.Net.Data. Here's an example of
loading up a world and changing a block.

```csharp
// Loads the level in the world directory
var level = new Level("world");
level.World.SetBlock(new Vector3(5, 10, 15), new DiamondBlock());
level.Save();
```

And another example for calculating the time to harvest a block with a given tool:

```csharp
var block = new GoldBlock();
short damage; // The damage the item will sustain from mining this block
int milliseconds = block.GetHarvestTime(new DiamondPickaxe(), out damage);
// Use this code if you want to see how long a specific entity will take,
// with regard to things like being underwater:
milliseconds = block.GetHarvestTime(new DiamondPickaxe(), world, playerEntity, out damage);
```

Or maybe you want to spawn a random painting based on the available space in the world (i.e. how
vanilla Minecraft does it):

```csharp
// CreateEntity(world, on which block, in which direction);
var entity = PaintingEntity.CreateEntity(world, new Vector3(1, 2, 3), Vector3.North);
// Creates a painting entity based on the amount of space available in the specified
// location, choosing a random one from the list of available paintings that are the
// required size.
world.OnSpawnEntity(entity);
```

As you can likely tell, Craft.Net.Data does a lot. You might want to just browse around and see if
it does what you're looking for.

## Building from source

There are two different configurations for building Craft.Net. You should use the DEBUG configuration
when building for testing purposes, and RELEASE when building for production purposes.

On Windows, add "C:\Windows\Microsoft.NET\Framework\v4.0.30319" to your path. Then, use

    msbuild.exe /p:Configuration=[RELEASE|DEBUG]

Update the configuration as required. On Linux and Mac, install Mono 2.10 or better, and then use this
command:

    xbuild /property:Configuration=MONO

**NOTE**: It is important that you build the project with the MONO configuration if you intend to use
it on Mono. Craft.Net uses bouncy castle for encryption on Mono, because the Mono CryptoStream
[does not work correctly](https://bugzilla.xamarin.com/show_bug.cgi?id=9247).

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
