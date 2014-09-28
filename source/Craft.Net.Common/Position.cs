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
		public static Position readPosition(MinecraftStream stream) {
			long val = stream.ReadInt64();
			int tempx = (int) value >> 38;
			int tempy = (int) value << 26 >> 52;
			int tempz = (int) value << 38 >> 38;
			return new Position(tempx, tempy, tempz);
		}

		public static long writePosition (Position pos)
		{
			return ((pos.getX() & 0x3FFFFFF) << 38 | (pos.getY() & 0xFFF) << 26 | (pos.getZ() & 0x3FFFFFF));
          //  return ((pos.getX() & POSITION_X_MASK) << POSITION_X_SHIFT | (pos.getY() & POSITION_Y_MASK) << POSITION_Y_SHIFT | (pos.getZ() & POSITION_Z_MASK));
		}

	}
}

