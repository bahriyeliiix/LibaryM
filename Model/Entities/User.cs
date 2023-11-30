using Model.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
    }
}
