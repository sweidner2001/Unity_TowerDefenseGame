using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Resources.Interfaces
{
    internal interface IAttack
    {
        public float PlayerDetectionRange { get; set; } 
        public float AttackCooldown { get; set; }           // Pause zwischen 2 Attacken
        public float MaxAttackRange { get; set; }           // Angriffsabstand zum Gegner
        public LayerMask DetectionLayer { get; set; }       // Wen wollen wir angreifen?
    }
}
