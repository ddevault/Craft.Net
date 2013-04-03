using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Craft.Net.Nbt;
using fNbt;

namespace Craft.Net.Test
{
    [TestFixture]
    public class NbtTest
    {
        // Test type for serialization
        // Contains everything the serializer can serialize
        // Populates everything with random values when constructing.
        private class TestType
        {
            public static Random Random;

            public TestType()
            {
                Random = new Random();
                Thing1 = (byte)Random.Next();
                Thing2 = new byte[128];
                Random.NextBytes(Thing2);
                Thing3 = Random.Next();
                Thing4 = new int[128];
                for (int i = 0; i < Thing4.Length; i++)
                    Thing4[i] = Random.Next();
                Thing5 = (short)Random.Next();
                Thing6 = Random.Next();
                Thing7 = Random.NextDouble();
                Thing8 = (float)Random.NextDouble();
                Thing9 = GenerateRandomString();
                Thing10 = new string[16];
                for (int i = 0; i < 16; i++)
                    Thing10[i] = GenerateRandomString();
                Thing11 = new ItemStack(1, 2, 3);
                Custom = new CustomSerializable();
            }

            private string GenerateRandomString()
            {
                const string characters = "abcdefghijklmnopqrstuvwxyz";
                var value = new char[16];
                for (int i = 0; i < value.Length; i++)
                    value[i] = characters[Random.Next(characters.Length)];
                return new string(value);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TestType))
                    return false;
                var test = obj as TestType;
                return test.Thing1 == Thing1 &&
                    test.Thing2.SequenceEqual(Thing2) &&
                    test.Thing3 == Thing3 &&
                    test.Thing4.SequenceEqual(Thing4) &&
                    test.Thing5 == Thing5 &&
                    test.Thing6 == Thing6 &&
                    test.Thing7 == Thing7 &&
                    test.Thing8 == Thing8 &&
                    test.Thing9 == Thing9 &&
                    test.Thing10.SequenceEqual(Thing10) &&
                    test.Thing11 == Thing11;
            }

            public byte Thing1 { get; set; }
            public byte[] Thing2 { get; set; }
            public int Thing3 { get; set; }
            public int[] Thing4 { get; set; }
            public short Thing5 { get; set; }
            public long Thing6 { get; set; }
            public double Thing7 { get; set; }
            public float Thing8 { get; set; }
            public string Thing9 { get; set; }
            public string[] Thing10 { get; set; }
            public ItemStack Thing11 { get; set; }
            public CustomSerializable Custom { get; set; }
        }

        private class CustomSerializable : INbtSerializable
        {
            public int Test { get; set; }

            public CustomSerializable()
            {
                Test = TestType.Random.Next();
            }

            public NbtCompound Serialize()
            {
                return new NbtCompound("", new[] { new NbtString("Example", Test.ToString()) });
            }

            public void Deserialize(NbtCompound value)
            {
                Test = int.Parse(value["Example"].StringValue);
            }
        }

        [Test]
        public void TestSerializer()
        {
            var serializer = new NbtSerializer(typeof(TestType));
            var test = new TestType();
            var serialized = serializer.Serialize(test);
            var deserialized = (TestType)serializer.Deserialize(serialized);
            Assert.AreEqual(test, deserialized);
        }
    }
}
