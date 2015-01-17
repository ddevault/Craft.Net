using System;

namespace Craft.Net.Common
{
	public class Position {

		private long x;
		private long y;
		private long z;

		public Position(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public long getX() {
			return this.x;
		}

		public long getY() {
			return this.y;
		}

		public long getZ() {
			return this.z;
		}

		public static Position readPosition(MinecraftStream stream) {
			long value = stream.ReadInt64();
			long tempx = (value >> 38);
			long tempy = (value >> 26) & 0XFFF;
			long tempz = value << 38 >> 38;
			return new Position((int)tempx, (int)tempy, (int)tempz);
		}

		public static long writePosition (Position pos)
		{
            Console.WriteLine(string.Format("X:{0} Y:{1} Z:{2}",pos.x,pos.y,pos.z));
            return ((pos.x & 0x3FFFFFF) << 38) | ((pos.y & 0xFFF) << 26) | (pos.z & 0x3FFFFFF);
		}

	}
}

