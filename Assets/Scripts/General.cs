using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal static class General
    {
        public static T GetConfig<T>(string path) where T : ConfigArcher
        {
            path = Path.Join("Config", path);
            return Resources.Load<T>(path);
        }
    }
}
