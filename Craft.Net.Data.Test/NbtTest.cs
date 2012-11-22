using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Metadata;
using Craft.Net.Data.NbtSerialization;
using NUnit.Framework;

namespace Craft.Net.Data.Test
{
   [TestFixture]
   public class NbtTest
   {
      [Test]
      public void TestSignTile()
      {
         SignTileEntity entity = new SignTileEntity();
         entity.Text1 = "Test1";
         entity.Text2 = "Test2";
         entity.Text3 = "Test3";
         entity.Text4 = "Test4";

         var serializer = new NbtSerializer(typeof(SignTileEntity));

         var obj = serializer.Serialize(entity);

         var result = (SignTileEntity)serializer.Deserialize(obj);

         Assert.AreEqual(entity.Id, result.Id);
         Assert.AreEqual(entity.Text1, result.Text1);
         Assert.AreEqual(entity.Text2, result.Text2);
         Assert.AreEqual(entity.Text3, result.Text3);
         Assert.AreEqual(entity.Text4, result.Text4);
      }
   }
}