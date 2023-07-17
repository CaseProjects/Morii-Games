using System;
using System.Collections.Generic;

namespace Core.SignalBus
{
    public class SignalBus
    {
        private readonly Dictionary<Type, object> _localDeclarationMap = new Dictionary<Type, object>();

        public void DeclareSignal<TSignal>()
        {
            var declaration = new SignalDeclaration<TSignal>();
            _localDeclarationMap.Add(typeof(TSignal), declaration);
        }

        public void Subscribe<TSignal>(Action<TSignal> callback)
        {
            void WrapperCallback(TSignal args) => callback(args);
            InternalSubscribe<TSignal>(WrapperCallback);
        }


        private void InternalSubscribe<TSignal>(Action<TSignal> callback)
        {
            var signalDeclaration = GetSignalDeclaration<TSignal>();
            signalDeclaration.Add(callback);
        }


        public void Fire<TSignal>() where TSignal : new()
        {
            InternalFire(new TSignal());
        }

        public void Fire<TSignal>(TSignal signal)
        {
            InternalFire(signal);
        }


        private void InternalFire<TSignal>(TSignal signalData)
        {
            var signalDeclaration = GetSignalDeclaration<TSignal>();
            signalDeclaration.Fire(signalData);
        }

        private SignalDeclaration<TSignal> GetSignalDeclaration<TSignal>()
        {
            _localDeclarationMap.TryGetValue(typeof(TSignal), out var declarationObject);
            return (SignalDeclaration<TSignal>)declarationObject ??
                   throw new Exception($"Signal {typeof(TSignal)} is not declared");
        }

        private class SignalDeclaration<TSignal>
        {
            private readonly List<Action<TSignal>> _subscriptions = new List<Action<TSignal>>();

            public void Fire(TSignal signal)
            {
                foreach (var obj in _subscriptions) obj.Invoke(signal);
            }

            public void Add(Action<TSignal> subscription)
            {
                _subscriptions.Add(subscription);
            }
        }
    }
}