using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
   public abstract class SignBlock : Block
   {
      protected SignBlock()
      {
         SignData = new SignTileEntity();
      }

      public override double Hardness
      {
         get { return 1; }
      }

      public override BoundingBox? BoundingBox
      {
         get { return null; }
      }

      public SignTileEntity SignData { get; set; }

      public override TileEntity TileEntity
      {
         get
         {
            return SignData;
         }
         set { SignData = (SignTileEntity)value; }
      }

      public override bool GetDrop(ToolItem tool, out Slot[] drop)
      {
         drop = new[] { new Slot((ushort)new SignItem(), 1) };
         return true;
      }
   }

   public class SignTileEntity : TileEntity
   {
      public override string Id
      {
         get
         {
            return "Sign";
         }
      }

      public override bool SendToClients
      {
         get { return false; }
      }

      public string Text1 { get; set; }
      public string Text2 { get; set; }
      public string Text3 { get; set; }
      public string Text4 { get; set; }
   }
}