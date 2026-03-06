using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private static ServiceLocator m_serviceLocator;

    private Dictionary<Type, object> m_services = new();

    public static void ClearServices()
    {
        m_serviceLocator ??= new ServiceLocator();
        m_serviceLocator.m_services.Clear();
    }

    public static void Register<T>(T instance)
    {
        m_serviceLocator.m_services.Add(typeof(T), instance);
    }

    public static T Resolve<T>()
        where T: class
    {
        if (m_serviceLocator == null)
        {
            throw new NullReferenceException("Service Locator is null");
        }

        return m_serviceLocator.m_services[typeof(T)] as T;
    }
}
