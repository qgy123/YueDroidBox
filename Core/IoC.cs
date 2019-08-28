﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace YueDroidBox.Core
{
    public static class IoC
    {
        public static Func<Type, string, object> GetInstance = (service, key) => { throw new InvalidOperationException("IoC is not initialized"); };

        public static Func<Type, string, IEnumerable<object>> GetAllInstances = (service, key) => { throw new InvalidOperationException("IoC is not initialized"); };

        public static Action<object> BuildUp = instance => { throw new InvalidOperationException("IoC is not initialized"); };

        public static T Get<T>(string key = null)
        {
            return (T)GetInstance(typeof(T), key);
        }

        public static IEnumerable<T> GetAll<T>(string key = null)
        {
            return GetAllInstances(typeof(T), key).Cast<T>();
        }
    }
}