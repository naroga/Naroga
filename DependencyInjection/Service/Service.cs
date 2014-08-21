using UnityEngine;
using System.Collections;

namespace Naroga.DependencyInjection.Service
{

    [System.Serializable]
    public class Service
    {
        public string name;
        public string className;
        public string[] arguments;
    }

}