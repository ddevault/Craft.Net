# Craft.Net

Craft.Net is consists of several libraries for .NET that that accomplish various
Minecraft-related tasks. Included in Craft.Net are:

* Craft.Net.Data: A library to work with various kinds of Minecraft-related data.
* Craft.Net.Server: An implementation of the 1.4.4 Minecraft server protocol.

Craft.Net runs well on Microsoft.Net or [Mono](https://github.com/mono/mono).

## Features

Craft.Net is incomplete. However, it aims to eventually reproduce the functionality of the
vanilla Minecraft server. In addition, Craft.Net should use significantly less CPU and
memory on the host machine when operating a server. For more information on progress, see
the latest [Milestone](https://github.com/SirCmpwn/Craft.Net/issues/milestones).

If there are any additional features you would like, or any problems you encounter while
using Craft.Net, please do not hesitate to
[create an issue](https://github.com/SirCmpwn/Craft.Net/issues).

Some cool technical things about Craft.Net that are already implemented:

* **Asynchronous I/O:** *Minimizes CPU load and increases speed*
* **Threaded Chunk Management:** *Offloads chunk compression and sending to short-lived threads*
* **Anvil Support:** *Includes support for loading, saving, creating, and manipulating Anvil worlds*
* **PvP Combat:** *Survival-mode PvP combat is supported*

## Usage

Craft.Net is a library, and will not work as a standalone server. However, the basic usage
is simple. To get started, the following code will create a Minecraft 1.4.4 server:

    MinecraftServer server = new MinecraftServer(new IPEndPoint(IPAddress.Any, 25565));
    server.AddLevel(new Level("world"));
    server.Start();
    // ...
    server.Stop();

This server will create a "world" directory and place an Anvil world in there. The default world
generator is flatland. If a world already exists in that directory, it will use it instead of
creating a new one.

The MinecraftServer class is the core server class, and runs networking, chunk management,
block updates, and more. Each server must have at least one world to start, and an
InvalidOperationException will be thrown if an attempt is made to start the server without.

You should also consider adding an ILogProvider to the server with LogProvider.RegisterProvider.
Included in Craft.Net is ConsoleLogWriter, and FileLogWriter, which output logs to the command
line or a file, respectively. Craft.Net logs messages with a given LogImportance. High importance
is for things like server starting and new clients logging in. Medium importance is for things
like chat and player deaths. Low importance is a log of all communication on the server, and
all packets are logged with a low importance. Packet logging is only enabled in DEBUG builds.

## Building from Source

There are two different configurations for building Craft.Net. You should use the DEBUG
configuration when building for testing purposes, and RELEASE when building for production
purposes. The latter will create Craft.Net.Server.dll in the root of the solution, which
has all of the dependencies merged into one binary.

On Windows, add "C:\Windows\Microsoft.NET\Framework\v4.0.30319" to your path. Then, use

    msbuild.exe /p:Configuration=[RELEASE|DEBUG]

Update the configuration as required. On Linux and Mac, install Mono, and then use this
command:

    xbuild /property:Configuration=[RELEASE|DEBUG]

## Contributing

If you wish to contribute your own code to Craft.Net, please create a fork. You are
encouraged to follow the code standards currently in use, and pull requests that do not will
be rejected. You are also encouraged to make small, focused pull requests, rather than large,
sweeping changes. For such changes, it would be better to create an issue instead.

## Getting Help

You can get help by making an [issue on GitHub](https://github.com/SirCmpwn/Craft.Net/issues),
or joining #craft.net on irc.freenode.net.  If you are already knowledgable about using
Craft.Net, consider contributing to the [wiki](https://github.com/SirCmpwn/Craft.Net/wiki) for
the sake of others.

## Dependencies

Craft.Net depends on the following tools and assemblies, which are included in the repository:

* [DotNetZip](http://dotnetzip.codeplex.com/) is used for data compression.
* [BouncyCastle](http://www.bouncycastle.org/) is used for encryption.

## Licensing

Craft.Net uses the permissive [MIT license](http://www.opensource.org/licenses/mit-license.php/).

In a nutshell:

* You are not restricted on usage of Craft.Net; commercial, private, etc, all fine.
* The developers are not liable for what you do with it.
* Craft.Net is provided "as is" with no warranty.

[Minecraft](http://minecraft.net) is not officially affiliated with Craft.Net.
