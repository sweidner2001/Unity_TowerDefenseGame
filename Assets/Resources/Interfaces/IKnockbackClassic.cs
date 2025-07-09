using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Resources.Interfaces
{
    public interface IKnockbackClassic
    {
        public bool EnableKnockbackClassic { get; set; }
        public float KnockbackForce { get; set; }           // wie stark wird der Gegner zurückgeschleudert 
        public float KnockbackTime { get; set; }            // Wie lange wird der Gegner zurückgeschleudert, bis er wieder zum Stehen kommt
        public float StunTime { get; set; }                 // Wie lange bleibt der Gegner anschließend noch stehen nach dem Zurückschleudern

    }
}
