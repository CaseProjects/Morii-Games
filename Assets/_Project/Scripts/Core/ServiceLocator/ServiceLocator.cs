using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceLocator
{
    private static readonly IDictionary<Type, ServiceDescriptor> _serviceDescriptors =
        new Dictionary<Type, ServiceDescriptor>();


    public static void Register(object implementation)
    {
        var serviceDescriptor = new ServiceDescriptor(implementation);
        _serviceDescriptors.Add(serviceDescriptor.ServiceType, serviceDescriptor);
    }

    private static object GetInternal(Type serviceType)
    {
        _serviceDescriptors.TryGetValue(serviceType, out var serviceDescriptor);

        if (serviceDescriptor == null)
            throw new Exception($"Service of type {serviceType.Name} is not registered");

        return serviceDescriptor.Implementation;
    }

    public static T Get<T>() => (T)GetInternal(typeof(T));

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Clear()
    {
        SceneManager.sceneLoaded += (scene, _) => ClearData();
        ClearData();
    }

    private static void ClearData() => _serviceDescriptors.Clear();
}