using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    public interface IItem : IEntity
    {
        public String Name { get; }
        public String ToString();
        public bool IsTwoHanded {get;}

        public void ApplyOnPickUp(Player player);
        public void ApplyOnDePickUp(Player player);
        public void ApplyOnHanded(Player player);
        public void ApplyOnDeHanded(Player player);
    }

}
