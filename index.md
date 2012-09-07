---
title: Craft&#46;Net
layout: base
---

Craft.Net is a collection of [.Net](http://en.wikipedia.org/wiki/.NET_Framework) libraries
for working with Minecraft. It includes a 12w36a server, and tools for manipulating Anvil
world saves, simulating block and entity interactions, and more.

## Getting Started

If you wish to use Craft.Net to run a Minecraft server, it's nice and simple:

    MinecraftServer server = new MinecraftServer(new IPEndPoint(IPAddress.Any, 25565));
    server.AddLevel(new Level("world"));
    server.Start();
    // ...
    server.Stop();

If you want to manipulate an Anvil world, that's also pretty easy. Here's an example of
changing the block at &lt;0, 0, 0&gt; in the "example" world of the user's machine:

    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        ".minecraft", "example");
    Level level = new Level(path);
    level.World.SetBlock(Vector3.Zero, new GoldBlock());
    level.Save();

You can learn more at the [Wiki](https://github.com/SirCmpwn/Craft.Net/wiki).

## Getting Help

You can get help by making an [issue on GitHub](https://github.com/SirCmpwn/Craft.Net/issues),
or joining \#craft.net on irc.freenode.net.  If you are already knowledgable about using
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

[Minecraft](http://minecraft.net/) is not officially affiliated with Craft.Net.