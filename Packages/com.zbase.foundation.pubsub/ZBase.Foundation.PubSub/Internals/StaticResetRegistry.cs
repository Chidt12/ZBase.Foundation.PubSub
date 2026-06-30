using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZBase.Foundation.PubSub.Internals
{
    /// <summary>
    /// Non-generic registry that runs reset callbacks when entering Play Mode.
    /// Generic types cannot host <c>[RuntimeInitializeOnLoadMethod]</c> directly,
    /// so each closed-generic type registers a callback here from its static ctor.
    /// </summary>
    /// <seealso href="https://docs.unity3d.com/Manual/DomainReloading.html"/>
    internal static class StaticResetRegistry
    {
        private static readonly List<Action> s_callbacks = new();

        public static void Register(Action callback)
        {
            lock (s_callbacks)
            {
                s_callbacks.Add(callback);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetAll()
        {
            lock (s_callbacks)
            {
                foreach (var callback in s_callbacks)
                {
                    callback();
                }
            }
        }
    }
}
