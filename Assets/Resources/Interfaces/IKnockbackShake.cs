using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Resources.Interfaces
{
    public interface IKnockbackShake
    {
        public bool EnableKnockbackShake { get; set; }
        public float KnockbackHeight { get; set; }           // wie stark wird der Gegner zur�ckgeschleudert 
        public float KnockbackWidth { get; set; }           // wie stark wird der Gegner zur�ckgeschleudert 
        public float KnockbackTime { get; set; }            // Wie lange wird der Gegner zur�ckgeschleudert, bis er wieder zum Stehen kommt
        public float StunTime { get; set; }                 // Wie lange bleibt der Gegner anschlie�end noch stehen nach dem Zur�ckschleudern

    }
}
