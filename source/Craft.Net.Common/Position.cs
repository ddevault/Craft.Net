using System;

namespace Craft.Net.Common
{
	public class Position {

		private int x;
		private int y;
		private int z;

		public Position(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public int getX() {
			return this.x;
		}

		public int getY() {
			return this.y;
		}

		public int getZ() {
			return this.z;
		}
		private static int POSITION_X_SIZE = 26;
		private static int POSITION_Z_SIZE = POSITION_X_SIZE;
		private static int POSITION_Y_SIZE = 64 - POSITION_X_SIZE - POSITION_Z_SIZE;
		private static int POSITION_Y_SHIFT = POSITION_Z_SIZE;
		private static int POSITION_X_SHIFT = POSITION_Y_SHIFT + POSITION_Y_SIZE;
		private static long POSITION_X_MASK = (1L << POSITION_X_SIZE) - 1;
		private static long POSITION_Y_MASK = (1L << POSITION_Y_SIZE) - 1;
		private static long POSITION_Z_MASK = (1L << POSITION_Z_SIZE) - 1;
		public static Position readPosition(MinecraftStream stream) {
			long val = stream.ReadInt64();
			int tempx = (int) (val << 64 - POSITION_X_SHIFT - POSITION_X_SIZE >> 64 - POSITION_X_SIZE);
			int tempy = (int) (val << 64 - POSITION_Y_SHIFT - POSITION_Y_SIZE >> 64 - POSITION_Y_SIZE);
			int tempz = (int) (val << 64 - POSITION_Z_SIZE >> 64 - POSITION_Z_SIZE);
			return new Position(tempx, tempy, tempz);
		}

		public static long writePosition (Position pos)
		{
			Console.WriteLine(((pos.getX() & 0x3FFFFFF) << 38 | (pos.getY() & 0xFFF) << 26 | (pos.getZ() & 0x3FFFFFF)).ToString());
			return ((pos.getX() & 0x3FFFFFF) << 38 | (pos.getY() & 0xFFF) << 26 | (pos.getZ() & 0x3FFFFFF));
          //  return ((pos.getX() & POSITION_X_MASK) << POSITION_X_SHIFT | (pos.getY() & POSITION_Y_MASK) << POSITION_Y_SHIFT | (pos.getZ() & POSITION_Z_MASK));
		}

	}
}

