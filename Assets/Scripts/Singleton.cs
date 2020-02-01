using System;
using System.Collections.Generic;

public class Singleton<T> where T : Singleton<T>
{
    private static T s_Instance;

    private static Dictionary<Type, object> s_Registry =
        new Dictionary<Type, object>();

    protected static T Lookup(Type type)
    {
        if (type == null) return null;
        if (s_Registry.ContainsKey(type))
            return s_Registry[type] as T;
        return null;
    }

    public static void Register(T singleton)
    {
        if (singleton == null) return;
        if (s_Registry.ContainsKey(singleton.GetType())) return;
        s_Registry.Add(singleton.GetType(), singleton);
    }

    public static T GetInstance()
    {
        if (s_Instance == null)
            s_Instance = Lookup(typeof(T));
        return s_Instance;
    }
}