using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Resources.Interfaces
{
    public interface IMove
    {
        public abstract float MovingSpeed { get; set; }
    }
}
