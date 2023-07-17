using System;
using System.Collections.Generic;

namespace MainHandlers
{
    public sealed class ReactiveProperty<T>
    {
        private T _value;
        private readonly List<Action<T>> _subscribers = new();

        public ReactiveProperty()
        {
            _value = default;
        }

        public ReactiveProperty(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                NotifySubscribers();
            }
        }


        public void Subscribe(Action<T> subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void Unsubscribe(Action<T> subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        private void NotifySubscribers()
        {
            foreach (var subscriber in _subscribers) subscriber.Invoke(_value);
        }
    }
}