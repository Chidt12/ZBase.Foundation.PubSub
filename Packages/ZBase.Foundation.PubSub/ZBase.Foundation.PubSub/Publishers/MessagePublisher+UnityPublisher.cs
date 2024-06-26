#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    partial class MessagePublisher
    {
        public readonly partial struct UnityPublisher<TScope> : IMessagePublisher<TScope>
            where TScope : UnityEngine.Object
        {
            private readonly Publisher<UnityObjectRef<TScope>> _publisher;

            public UnityObjectRef<TScope> Scope => _publisher.Scope;

            public bool IsValid => _publisher.IsValid;

            internal UnityPublisher([NotNull] MessagePublisher publisher, [NotNull] TScope scope)
            {
                _publisher = new(publisher, scope);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public CachedPublisher<TMessage> Cache<TMessage>(ILogger logger = null)
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return default;
                }
#endif

                return _publisher.Cache<TMessage>(logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Publish<TMessage>(
                  CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _publisher.Publish<TMessage>(cancelToken, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Publish<TMessage>(
                  TMessage message
                , CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _publisher.Publish(message, cancelToken, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public UniTask PublishAsync<TMessage>(
                  CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                return _publisher.PublishAsync<TMessage>(cancelToken, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public UniTask PublishAsync<TMessage>(
                  TMessage message
                , CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                return _publisher.PublishAsync(message, cancelToken, logger);
            }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            private bool Validate(ILogger logger)
            {
                if (IsValid == true)
                {
                    return true;
                }

                (logger ?? DefaultLogger.Default).LogError(
                    $"{GetType().Name} must be retrieved via `{nameof(MessagePublisher)}.{nameof(UnityScope)}` API"
                );

                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            partial void RetainUsings();
#endif
        }
    }
}
