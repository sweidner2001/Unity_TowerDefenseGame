using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ISoldierBase
    {
        // Membervariablen:
        public Rigidbody2D Rb { get; set; }
        public SoldierState State { get; }


        // Methoden:
        void ChangeState(SoldierState newState);
        void Move(Transform destinationTransform);
        void Die();
    }
}
