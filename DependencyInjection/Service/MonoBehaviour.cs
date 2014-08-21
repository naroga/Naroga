using UnityEngine;
using System.Collections;

namespace Naroga.DependencyInjection.Service
{

    [System.Serializable]
    public class MonoBehaviour
    {
        [System.Serializable]
        public struct Argument
        {
            public string property;
            public string value;
        }

        public string name;
        public GameObject gameObject;
        public string className;
        public Argument[] arguments;

    }

}