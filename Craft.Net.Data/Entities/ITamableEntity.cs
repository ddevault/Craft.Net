using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Entities
{
    public interface ITamableEntity
    {
        string Owner { get; set; }
        bool Sitting { get; set; }
    }
}
