using System;

namespace ScreenCaptureTool.DataAccess
{
    public class SingletonBase<T> where T : class
    {
        private static T _instance;
        private static object _lockObject = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                            _instance = (T)Activator.CreateInstance(typeof(T));
                    }
                }

                return _instance;
            }
        }
    }
}
