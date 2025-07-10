using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Resources.Interfaces
{
    public interface IHealth
    {
        public int MaxHealth { get; set; }
        public int CoinsOnDeath { get; set; }
    }
}
