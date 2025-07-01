
namespace DesignPatterns.Singleton
{
    /// <summary>
    /// Generic non-MonoBehaviour singleton base for classes with parameterless constructors.
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        static T _instance;

        /// <summary>
        /// Globally accessible instance of type T.
        /// </summary>
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }

        // Protected to prevent external instantiation.
        protected Singleton() { }
    }
}
