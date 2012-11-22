using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LibNbt.Tags;

namespace Craft.Net.Data.NbtSerialization
{
   public class NbtSerializer
   {
      public Type Type { get; set; }

      /// <summary>
      /// Decorates the given property or field with the specified
      /// NBT tag name.
      /// </summary>
      public NbtSerializer(Type type)
      {
         Type = type;
      }

      public NbtTag Serialize(object value)
      {
         if (value is byte)
            return new NbtByte("", (byte)value);
         if (value is bool)
            return new NbtByte("", (byte)((bool)value ? 1 : 0));
         else if (value is byte[])
            return new NbtByteArray("", (byte[])value);
         else if (value is double)
            return new NbtDouble("", (double)value);
         else if (value is float)
            return new NbtFloat("", (float)value);
         else if (value is int)
            return new NbtInt("", (int)value);
         else if (value is int[])
            return new NbtIntArray("", (int[])value);
         else if (value is long)
            return new NbtLong("", (long)value);
         else if (value is short)
            return new NbtShort("", (short)value);
         else if (value is string)
            return new NbtString("", (string)value);
         else
         {

            var compound = new NbtCompound();

            if(value == null) return compound;
            var nameAttributes = Attribute.GetCustomAttributes(value.GetType(), typeof(TagNameAttribute));

            if (nameAttributes.Length > 0)
               compound.Name = ((TagNameAttribute)nameAttributes[0]).Name;

            var properties = Type.GetProperties().Where(p =>
               !Attribute.GetCustomAttributes(p, typeof(NbtIgnoreAttribute)).Any());

            foreach (var property in properties)
            {
               if (!property.CanRead)
                  continue;

               NbtTag tag = null;

               string name = property.Name;
               nameAttributes = Attribute.GetCustomAttributes(property, typeof(TagNameAttribute));
               var ignoreOnNullAttribute = Attribute.GetCustomAttribute(property, typeof(IgnoreOnNullAttribute));
               if (nameAttributes.Length != 0)
                  name = ((TagNameAttribute)nameAttributes[0]).Name;

               var innerSerializer = new NbtSerializer(property.PropertyType);
               var propValue = property.GetValue(value, null);

               if(propValue == null)
               {
                  if (ignoreOnNullAttribute != null) continue;
                  if(property.PropertyType.IsValueType)
                  {
                     propValue = Activator.CreateInstance(property.PropertyType);
                  }
                  else if (property.PropertyType == typeof(string))
                     propValue = "";
                  }

                  tag = innerSerializer.Serialize(propValue);
                  tag.Name = name;
                  compound.Tags.Add(tag);
               }

               return compound;
            }
         }

         public object Deserialize(NbtTag value)
         {
            if (value is NbtByte)
               return ((NbtByte)value).Value;
            else if (value is NbtByteArray)
               return ((NbtByteArray)value).Value;
            else if (value is NbtDouble)
               return ((NbtDouble)value).Value;
            else if (value is NbtFloat)
               return ((NbtFloat)value).Value;
            else if (value is NbtInt)
               return ((NbtInt)value).Value;
            else if (value is NbtIntArray)
               return ((NbtIntArray)value).Value;
            else if (value is NbtLong)
               return ((NbtLong)value).Value;
            else if (value is NbtShort)
               return ((NbtShort)value).Value;
            else if (value is NbtString)
               return ((NbtString)value).Value;
            else if(value is NbtCompound)
            {
               var compound = value as NbtCompound;
               var properties = Type.GetProperties().Where(p =>
                  !Attribute.GetCustomAttributes(p, typeof(NbtIgnoreAttribute)).Any());
               var resultObject = Activator.CreateInstance(Type);
               foreach (var property in properties)
               {
                  if (!property.CanWrite)
                     continue;
                  string name = property.Name;
                  var nameAttributes = Attribute.GetCustomAttributes(property, typeof(TagNameAttribute));

                  if (nameAttributes.Length != 0)
                     name = ((TagNameAttribute)nameAttributes[0]).Name;
                  var node = compound.Tags.SingleOrDefault(a => a.Name == name);
                  if (node == null) continue;
                  var data = new NbtSerializer(property.PropertyType).Deserialize(node);

                  if (property.PropertyType == typeof(bool)
                     && data is byte)
                     data = (byte)data == 1;

                  property.SetValue(resultObject, data, null);
               }

               return resultObject;
            }

            throw new NotSupportedException("The node type '" + value.GetType() + "' is not supported.");
         }
      }
}