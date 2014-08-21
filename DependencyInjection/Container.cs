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

        public List<Service.MonoBehaviourService> monoBehaviourServices;
        public List<Service.Service> services;

        protected Dictionary<string, object> activeServices = new Dictionary<string,object>();

        protected void Start()
        {

            foreach (Service.MonoBehaviourService mbService in monoBehaviourServices)
            {
                if (!activeServices.ContainsKey(mbService.name))
                {
                    initMonoBehaviourService(mbService.name);
                }
                
            }

        }

        protected void initMonoBehaviourService(string name)
        {

            //Gets the monobehaviour instance defined in the inspector.
            Service.MonoBehaviourService mbService = monoBehaviourServices.Find(delegate(Service.MonoBehaviourService localService) { return localService.name == name; });
            UnityEngine.MonoBehaviour component = (UnityEngine.MonoBehaviour) mbService.gameObject.GetComponent(mbService.className);

            if (component == null)
            {
                foreach (Component cObj in mbService.gameObject.GetComponents(typeof(Component)))
                {
                    Debug.LogError(cObj.GetType().Name);
                }
            }

            //Sets all the arguments.
            foreach (Service.MonoBehaviourService.Argument argument in mbService.arguments)
            {
                MethodInfo method = component.GetType().GetMethod(getSetterName(argument.property));
                method.Invoke(component, new object[] { this.resolveArgument(argument.value) });

            }

            activeServices.Add(name, component);
            
        }

        protected void initService(string name)
        {
            Service.Service serviceInfo = services.Find(delegate(Service.Service localService) { return localService.name == name; });
            Type type = Type.GetType(serviceInfo.className);
            List<object> arguments = new List<object>();

            foreach (string argument in serviceInfo.arguments)
            {
                arguments.Add(this.resolveArgument(argument));
            }

            object service = Activator.CreateInstance(type, arguments.ToArray());

            activeServices.Add(name, service);
        }

        public object get(string name)
        {

            //If the service is already instantiated/initiated, just return it.
            if (activeServices.ContainsKey(name))
            {
                return activeServices[name];
            }

            //If the service isn't instantiated, we must instantiate it.
            //First, check on the monoBehaviourServices list to see if a component is defined as a service.
            if (monoBehaviourServices.Exists(delegate(Service.MonoBehaviourService localService) { return localService.name == name; }))
            {
                initMonoBehaviourService(name);
            }
            //If no component is defined as a service, check to see if there is a class defined to be instantiated as a service.
            else
            {

                if (services.Exists(delegate(Service.Service localService) { return localService.name == name; }))
                {
                    initService(name);
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
        protected object resolveArgument(string argument)
        {

            switch (argument[0])
            {
                case '@':
                    return this.get(argument.Substring(1));
                case '%':
                    //TODO: Add verifications here! If the object is not found, raise an exception. If the object is found, but the component is not, also raise an exception.
                    string[] path = argument.Substring(1).Split(":".ToCharArray());
                    GameObject gObj = GameObject.FindGameObjectWithTag(path[0]);
                    Component go = gObj.GetComponent(path[1]);
                    return go;

                default:
                    return argument;
            }

        }

    }

}