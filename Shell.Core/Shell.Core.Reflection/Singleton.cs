using System;


namespace Shell.Core.Reflection
{
    public class Singleton<T> where T : class
    {
        public static T Instance { get => getInstance(); }
        private static T instance { get; set; }
        private static T getInstance()
        {
            if (!typeof(T).IsClass || typeof(T).IsAbstract) return null;
            if (instance == null) instance = Activator.CreateInstance<T>();
            return instance;
        }
    }
}