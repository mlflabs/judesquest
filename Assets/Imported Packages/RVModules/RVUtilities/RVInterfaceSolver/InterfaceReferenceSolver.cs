// Created by Ronis Vision. All rights reserved
// 25.09.2016.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Collects all interfaces from given objects list, and their parent calsses and passes it for relevant
/// objects implementing IneedInterface<>
/// </summary>
public class InterfaceReferenceSolver
{
    #region Fields

    private List<object> objects = new List<object>();

    #endregion

    /// <summary>
    /// Default constructor, you need to use AddObjectsWithInterfaces to pass them
    /// </summary>
    public InterfaceReferenceSolver()
    {
    }

    /// <summary>
    /// Creates IRS object, and add objects from array
    /// </summary>
    public InterfaceReferenceSolver(object[] _objects)
    {
        AddObjectsWithIterfaces(_objects);
    }

    #region Public methods

    public void AddObjectsWithIterfaces(object[] _objects)
    {
        foreach (object o in _objects)
        {
            if (o == null)
                Debug.Log("InterfaceReferenceSolver, AddObjectsWithIterfaces: Passed object '" + o + "' is null!");
        }
        objects.AddRange(_objects);
    }

    public void PassInterfaces()
    {
        // First collect all possible interfaces
        List<InterfacesContainer> allInterfacesContainers = GetAllInterfacesContainers(objects);

        foreach (InterfacesContainer container in allInterfacesContainers)
        {
            if (container.implementingObject == null)
                continue;

            PassInterfacesForObject(container, allInterfacesContainers.ToArray());
        }
    }

    #endregion

    #region Not public methods

    private List<InterfacesContainer> GetAllInterfacesContainers(List<object> _objects)
    {
        List<InterfacesContainer> allInterfacesContainers =
            new List<InterfacesContainer>();
        foreach (object obj in _objects)
        {
            if (obj == null)
                continue;

            List<Type> interfaces = obj.GetType().GetInterfaces().ToList();
            List<Type> inheritedTypes = GetAllParentClasses(obj.GetType());

            // get interfaces also from inherited types
            foreach (Type type in inheritedTypes)
                interfaces.AddRange(type.GetInterfaces());

            Type[] iNeedTypes = interfaces.Where(_i => _i.Name == "INeedInterface`1").ToArray();
            Type[] iHaveTypes = interfaces.Where(_i => _i.Name == "IHaveInterface`1").ToArray();
            Type[] iImplementTypes = interfaces.ToArray();

            InterfacesContainer interfacesContainer = new InterfacesContainer
            {
                interfaceINeed = iNeedTypes,
                implementingObject = obj,
                interfacesIImplement = interfaces.ToArray(),
                interfacesIHave = iHaveTypes
            };

            allInterfacesContainers.Add(interfacesContainer);
            // get interfaces from IHaveInterface implementations
            //for (int i = 0; i < interfaces.Count; i++)
            //{
            //    if (interfaces[i].Name == "IHaveInterface`1")
            //    {
            //        MethodInfo mi = interfaces[i].GetMethod("GetInterfaceInstance");
            //        Type interfaceReturnType = mi.ReturnType;
            //        object interfaceReturnedInstance = mi.Invoke(obj, null);
            //        if(interfaceReturnedInstance != null)
            //            interfaces.Add(interfaceReturnType);

            //        InterfacesContainer ic = new InterfacesContainer()
            //        { implementationType = ImplementationType.ihaveInterface,
            //            implementingObject = obj,
            //            interfaces = obj.GetType().GetInterfaces().ToList()
            //        };
            //    }
            //}
        }
        return allInterfacesContainers;
    }

    // _container is IC for whom we will solve interfaces(pass for all his INeedInterface
    private void PassInterfacesForObject(InterfacesContainer _container, InterfacesContainer[] _interfacesContainers)
    {
        // loop through our IC needed interfaces
        foreach (Type neededType in _container.interfaceINeed)
        {
            // loop through all InterfacesContainers
            foreach (InterfacesContainer interfacesContainer in _interfacesContainers)
            {
                // loop through single InterfacesContainer's implementations
                foreach (Type type in interfacesContainer.interfacesIImplement)
                {
                    if (neededType.GetGenericArguments()[0] == type)
                    {
                        MethodInfo mi = neededType.GetMethod("PassInterface");
                        mi.Invoke(_container.implementingObject, new[] {interfacesContainer.implementingObject});
                    }
                }

                foreach (Type type in interfacesContainer.interfacesIHave)
                {
                    if (neededType.GetGenericArguments()[0] == type.GetGenericArguments()[0])
                    {
                        MethodInfo passInterfaceMethodInfomi = neededType.GetMethod("PassInterface");
                        MethodInfo getInterfaceMethodInfomi = type.GetMethod("GetInterfaceInstance");

                        object objectImplementingNeededInterface =
                            getInterfaceMethodInfomi.Invoke(interfacesContainer.implementingObject, null);

                        passInterfaceMethodInfomi.Invoke(_container.implementingObject,
                            new[] {objectImplementingNeededInterface});
                    }
                }
            }
        }
    }

    private List<Type> GetAllParentClasses(Type _type)
    {
        List<Type> types = new List<Type>();

        Type type = _type.BaseType;
        while (type != null)
        {
            types.Add(type);
            type = type.BaseType;
        }

        return types;
    }

    #endregion

    private class InterfacesContainer
    {
        #region Fields

        public object implementingObject;
        public Type[] interfacesIImplement;
        public Type[] interfaceINeed;
        public Type[] interfacesIHave;

        #endregion
    }
}