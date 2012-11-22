using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
{
   public class MetadataString : MetadataEntry
   {
      public override byte Identifier { get { return 4; } }
      public override string FriendlyName { get { return "string"; } }

      public string Value;

      public MetadataString(byte index) : base(index)
      {
      }

      public MetadataString(byte index, string value) : base(index)
      {
         if (value.Length > 16)
            throw new ArgumentOutOfRangeException("value", "Maximum string length is 16 characters");
         while (value.Length < 16)
            value = value + "\0";
         Value = value;
      }

      public override bool TryReadEntry(byte[] buffer, ref int offset)
      {
         if (buffer.Length - offset < 17)
            return false;
         offset++;
         Value = Encoding.ASCII.GetString(buffer, offset, 16);
         offset += 16;
         return true;
      }

      public override byte[] Encode()
      {
         byte[] data = new byte[17];
         data[0] = GetKey();
         Encoding.ASCII.GetBytes(Value, 0, Value.Length, data, 1);
         return data;
      }
   }
}