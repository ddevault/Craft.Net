using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Craft.Net.Data.NbtSerialization;
using LibNbt.Tags;

namespace Craft.Net.Data
{
    public abstract class TileEntity
    {
        private static Type[] tileEntityTypes;
        private static TileEntity[] dummyInstances;

        static TileEntity()
        {
            // TODO: Tile entity types that aren't loaded at start up (plugins)
            tileEntityTypes = Assembly.GetExecutingAssembly().GetTypes().Where(
                    t => typeof(TileEntity).IsAssignableFrom(t) && !t.IsAbstract).ToArray();
            dummyInstances = tileEntityTypes.Select(entity => (TileEntity)Activator.CreateInstance(entity)).ToArray();
        }

        [TagName("id")]
        public abstract string Id { get; }
        [NbtIgnore]
        public abstract bool SendToClients { get; }

        // TODO: Refactor
        public static TileEntity FromNbt(NbtCompound entityTag, out Vector3 position)
        {
            position = Vector3.Zero;
            var id = entityTag.Get<NbtString>("id").Value;
            Type type = (from e in dummyInstances where e.Id == id select e.GetType()).FirstOrDefault();
            if (type == null)
                return null;
            var entity = (TileEntity)Activator.CreateInstance(type);
            var properties = type.GetProperties().Where(p =>
                !Attribute.GetCustomAttributes(p, typeof(NbtIgnoreAttribute)).Any() && p.CanWrite);
            foreach (var property in properties)
            {
                var name = property.Name;
                var nameAttributes = Attribute.GetCustomAttributes(property, typeof(TagNameAttribute));
                if (nameAttributes.Length != 0)
                    name = ((TagNameAttribute)nameAttributes[0]).Name;
                var tag = entityTag.Get(name);
                if (tag == null)
                    continue;
                if (property.PropertyType == typeof(byte))
                    property.SetValue(entity, ((NbtByte)tag).Value, null);
                else if (property.PropertyType == typeof(byte[]))
                    property.SetValue(entity, ((NbtByteArray)tag).Value, null);
                else if (property.PropertyType == typeof(double))
                    property.SetValue(entity, ((NbtDouble)tag).Value, null);
                else if (property.PropertyType == typeof(float))
                    property.SetValue(entity, ((NbtFloat)tag).Value, null);
                else if (property.PropertyType == typeof(int))
                    property.SetValue(entity, ((NbtInt)tag).Value, null);
                else if (property.PropertyType == typeof(int[]))
                    property.SetValue(entity, ((NbtIntArray)tag).Value, null);
                else if (property.PropertyType == typeof(long))
                    property.SetValue(entity, ((NbtLong)tag).Value, null);
                else if (property.PropertyType == typeof(short))
                    property.SetValue(entity, ((NbtShort)tag).Value, null);
                else if (property.PropertyType == typeof(string))
                    property.SetValue(entity, ((NbtString)tag).Value, null);
                else
                    continue;
            }
            position = new Vector3(
                entityTag.Get<NbtInt>("x").Value,
                entityTag.Get<NbtInt>("y").Value,
                entityTag.Get<NbtInt>("z").Value);
            return entity;
        }

        public NbtCompound ToNbt(Vector3 position)
        {
            var properties = GetType().GetProperties().Where(p =>
                    !Attribute.GetCustomAttributes(p, typeof(NbtIgnoreAttribute)).Any());
            var entity = new NbtCompound();
            entity.Tags.Add(new NbtInt("x", (int)position.X));
            entity.Tags.Add(new NbtInt("y", (int)position.Y));
            entity.Tags.Add(new NbtInt("z", (int)position.Z));
            foreach (var property in properties)
            {
                var value = property.GetValue(this, null);
                if (value == null)
                    continue;
                NbtTag tag = null;
                string name = property.Name;
                var nameAttributes = Attribute.GetCustomAttributes(property, typeof(TagNameAttribute));
                if (nameAttributes.Length != 0)
                    name = ((TagNameAttribute)nameAttributes[0]).Name;
                // TODO: Allow further customization from TileEntity object
                if (value is byte)
                    tag = new NbtByte(name, (byte)value);
                else if (value is byte[])
                    tag = new NbtByteArray(name, (byte[])value);
                else if (value is double)
                    tag = new NbtDouble(name, (double)value);
                else if (value is float)
                    tag = new NbtFloat(name, (float)value);
                else if (value is int)
                    tag = new NbtInt(name, (int)value);
                else if (value is int[])
                    tag = new NbtIntArray(name, (int[])value);
                else if (value is long)
                    tag = new NbtLong(name, (long)value);
                else if (value is short)
                    tag = new NbtShort(name, (short)value);
                else if (value is string)
                    tag = new NbtString(name, (string)value);
                else
                {
                    throw new SerializationException("No NBT tag sutible to handle " + GetType().Name +
                        "." + property.Name);
                }
                entity.Tags.Add(tag);
            }
            return entity;
        }
    }
}
