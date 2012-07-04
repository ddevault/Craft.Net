Craft.Net
========

Craft.Net is a library for .Net that implements the Minecraft 12w26a server protocol.
It also contains functionality for manipulating Anvil worlds, simulating block interactions,
and simulating entity interactions.

Craft.Net runs well on Microsoft.Net or [Mono](https://github.com/mono/mono).

Features
-------

Craft.Net is incomplete. However, it aims to eventually reproduce the functionality of the
vanilla Minecraft server. In addition, Craft.Net should use significantly less CPU and
memory on the host machine when operating a server. For more information on progress, see
the latest [Milestone](https://github.com/SirCmpwn/Craft.Net/issues/milestones).

If there are any additional features you would like, or any problems you encounter while
using Craft.Net, please do not hesitate to
[create an issue](https://github.com/SirCmpwn/Craft.Net/issues).

Some cool technical things about Craft.Net:

* Asynchronous I/O: *Minimizes CPU load and increases speed*
* Threaded Chunk Management *Offloads chunk compression and sending to short-lived threads*

Usage
----

Craft.Net is a library, and will not work as a standalone server. However, the basic usage
is simple. To get started, the following code will create a Minecraft 12w26a server:

    MinecraftServer server = new MinecraftServer(new IPEndPoint(IPAddress.Any, 25565));
    server.AddWorld(new World(new FlatlandGenerator()));
    server.Start();
    // ...
    server.Stop();

The MinecraftServer class is the core server class, and runs networking, chunk management,
block updates, and more. Each server must have at least one world to start, and an
InvalidOperationException will be thrown if an attempt is made to start the server without.

Additionally, each world must have a terrain generator, which implements IWorldGenerator.
FlatlandGenerator is a clone of the vanilla Minecraft flatland generator.

You should also consider adding an ILogProvider to the server with
MinecraftServer.AddLogProvider. Included in Craft.Net is ConsoleLogWriter, and FileLogWriter,
which output logs to the command line or a file, respectively. Craft.Net logs messages with
a given LogImportance. High importance is for things like server starting and new clients
logging in. Medium importance is for things like chat and player deaths. Low importance is a
log of all communication on the server, and all packets are logged with a low importance.
Packet logging is only enabled in DEBUG builds.

Contributing
----------

If you wish to contribute your own code to Craft.Net, please create a fork. You are
encouraged to follow the code standards currently in use, and pull requests that do not will
be rejected. You are also encouraged to make small, focused pull requests, rather than large,
sweeping changes. For such changes, it would be better to create an issue instead.

Dependencies
-----------

Craft.Net depends on the following tools and assemblies, which are included in the repository:

* [IKVM](http://www.ikvm.net/) is used to work with Java-like encryption.
* [BouncyCastle](http://www.bouncycastle.org/) is also used for encryption.
* [SharpZipLib](http://www.icsharpcode.net/opensource/sharpziplib/) is used for data compression.

[Minecraft](http://minecraft.net) is not officially affiliated with Craft.Net.
