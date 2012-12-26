using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Craft.Net.Data.NbtSerialization;
using fNbt;

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

        public static TileEntity FromNbt(NbtCompound entityTag, out Vector3 position)
        {
            position = Vector3.Zero;
            var id = entityTag.Get<NbtString>("id").Value;
            Type type = (from e in dummyInstances where e.Id == id select e.GetType()).FirstOrDefault();
            if (type == null)
                return null;
            
            var serializer = new NbtSerializer(type);
            
            var entity = (TileEntity)serializer.Deserialize(entityTag);
            
            position = new Vector3(
                entityTag.Get<NbtInt>("x").Value,
                entityTag.Get<NbtInt>("y").Value,
                entityTag.Get<NbtInt>("z").Value);
            return entity;
        }

        public NbtCompound ToNbt(Vector3 position)
        {
            var serializer = new NbtSerializer(GetType());
            
            var entity = (NbtCompound)serializer.Serialize(this);
            
            entity.Add(new NbtInt("x", (int)position.X));
            entity.Add(new NbtInt("y", (int)position.Y));
            entity.Add(new NbtInt("z", (int)position.Z));
           
            return entity;
        }
    }
}
