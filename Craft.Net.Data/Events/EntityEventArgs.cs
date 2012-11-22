using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Events
{
   public class EntityEventArgs : EventArgs
   {
      public Entity Entity { get; set; }

      public EntityEventArgs(Entity entity)
      {
         Entity = entity;
      }
   }
}