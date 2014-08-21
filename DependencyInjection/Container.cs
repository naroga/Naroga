using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using Naroga.DependencyInjection.Service;

namespace Naroga.DependencyInjection
{

    /// <summary>
    /// 
    /// </summary>
    public class Container : UnityEngine.MonoBehaviour
    {

        public List<Service.MonoBehaviour> monoBehaviourServices;
        public List<Service.Service> services;

        protected Dictionary<string, object> activeServices = new Dictionary<string,object>();

        public object get(string name)
        {
            
            //If the service is already instantiated/initiated, just return it.
            if (activeServices.ContainsKey(name))
            {
                return activeServices[name];
            }

            //If the service isn't instantiated, we must instantiate it.
            //First, check on the monoBehaviourServices list to see if a component is defined as a service.
            if (monoBehaviourServices.Exists(delegate(Service.MonoBehaviour localService) { return localService.name == name; }))
            {
                
                //Gets the monobehaviour instance defined in the inspector.
                Service.MonoBehaviour mbService = monoBehaviourServices.Find(delegate(Service.MonoBehaviour localService) { return localService.name == name; });
                Component component = mbService.gameObject.GetComponent(mbService.className);

                //Sets all the arguments.
                foreach (Service.MonoBehaviour.Argument argument in mbService.arguments)
                {
                    MethodInfo method = component.GetType().GetMethod(getSetterName(argument.property));
                    method.Invoke(component, new object[] { this.resolveArgument(argument.value) });
                }                

                activeServices[name] = component;

            }
            //If no component is defined as a service, check to see if there is a class defined to be instantiated as a service.
            else
            {

                if (services.Exists(delegate(Service.Service localService) { return localService.name == name; }))
                {

                    Service.Service serviceInfo = services.Find(delegate(Service.Service localService) { return localService.name == name; });
                    Type type = Type.GetType(serviceInfo.className);
                    List<object> arguments = new List<object>();

                    foreach (string argument in serviceInfo.arguments)
                    {
                        arguments.Add(this.resolveArgument(argument));
                    }

                    object service = Activator.CreateInstance(type, arguments.ToArray());
                    activeServices[name] = service;

                }
                else
                {
                    throw new ArgumentException("There is no service with name '" + name + "'.");
                }

            }

            return activeServices[name];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected string getSetterName(string propertyName)
        {
            return "set" + char.ToUpper(propertyName[0]) + propertyName.Substring(1);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        protected object resolveArgument(string argument) {
            
            switch (argument[0]) {
                case '@' :
                    return this.get(argument.Substring(1));
                case '%':
                    string[] path = argument.Substring(1).Split(":".ToCharArray());
                    UnityEngine.MonoBehaviour go = (UnityEngine.MonoBehaviour)GameObject.FindGameObjectWithTag(path[0]).GetComponent(path[1]);
                    return go;
                default:
                    return argument;
            }

        }

    }

}