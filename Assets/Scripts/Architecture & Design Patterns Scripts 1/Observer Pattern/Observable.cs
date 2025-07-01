
using UnityEngine;
using UnityEngine.Events;

namespace DesignPatterns.ObserverPattern
{
    /// <summary>
    /// Generic observable wrapper using UnityEvent to notify listeners on value changes.
    /// </summary>
    [System.Serializable]
    public class Observable<T>
    {
        [SerializeField] private T _value;
        [SerializeField] private UnityEvent<T> _onValueChanged;

        public T value
        {
            get => _value;
            set => Set(value);
        }

        public Observable(T initialValue, UnityAction<T> initialHandler = null)
        {
            _value = initialValue;
            _onValueChanged = new UnityEvent<T>();
            if (initialHandler != null) { _onValueChanged.AddListener(initialHandler); }
        }

        public void Set(T value)
        {
            if (value.Equals(_value) == false)
            {
                _value = value;
                Invoke();
            }
        }

        public void Invoke()
        {
            _onValueChanged.Invoke(_value);
        }

        public void AddListener(UnityAction<T> handler)
        {
            if (handler == null)
            {
                Debug.LogError($"Observable<{_value.GetType().FullName}>: Handler {handler.Method.Name} is null.");
                return;
            }
            if (_onValueChanged == null) { _onValueChanged = new UnityEvent<T>(); }
            _onValueChanged.AddListener(handler);
        }

        public void RemoveListener(UnityAction<T> handler)
        {
            if (handler == null || _onValueChanged == null) return;
            _onValueChanged.RemoveListener(handler);
        }

        public void RemoveAllListeners()
        {
            if (_onValueChanged == null) return;
            _onValueChanged.RemoveAllListeners();
        }
    }
}
