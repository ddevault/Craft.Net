using System;

namespace Craft.Net.Common
{
	public class VarInt
	{
		public byte[] Number;
		public VarInt(long Value)
		{
			if (Value < 0xfd)
			{
				Number = new byte[1];
				Number[0] = (byte)Value;
			}
			else if (Value <= 0xffff)
			{
				Number = new byte[3];
				Number[0] = 0xfd;
				BitConverter.GetBytes((short)Value).CopyTo(Number, 1);
			}
			else if (Value <= 0xffffffff)
			{
				Number = new byte[5];
				Number[0] = 0xfe;
				BitConverter.GetBytes((int)Value).CopyTo(Number, 1);
			}
			else
			{
				Number = new byte[9];
				Number[0] = 0xff;
				BitConverter.GetBytes(Value).CopyTo(Number, 1);
			}
		}
		public byte[] getBytes()
		{
			return Number;
		}
		public long GetInt()
		{
			switch (Number.Length)
			{
				case 1:
				return Number[0];
				case 3:
				return BitConverter.ToInt16(Number, 1);
				case 5:
				return BitConverter.ToInt32(Number, 1);
				case 9:
				return BitConverter.ToInt64(Number, 1);
				default:
				throw new Exception("Invalid Size");

			}
		}
		public static implicit operator VarInt(long num)
		{
			return new VarInt(num);
		}
		public static implicit operator long(VarInt num)
		{
			return num.GetInt();
		}
		public static explicit operator int(VarInt num)
		{
			return (int) num.GetInt();
		}
		public static explicit operator short(VarInt num)
		{
			return (short) num.GetInt();
		}
		public static explicit operator byte(VarInt num)
		{
			return (byte) num.GetInt();
		}
	}
}

