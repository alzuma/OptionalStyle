using System;
using System.Threading.Tasks;
using OptionalStyle.exceptions;

namespace OptionalStyle
{
    public class Optional
    {
        public static Optional<T> Of<T>(T value) => Optional<T>.Of(value);
        public static Optional<T> OfNullable<T>(T value) => Optional<T>.OfNullable(value);
        public static Optional<T> Empty<T>() => Optional<T>.Empty();
    }

    public class Optional<T> : Optional
    {
        private static readonly Optional<T> LocalEmpty = new Optional<T>();
        private readonly T _value;

        private Optional()
        {
            _value = default;
        }

        private Optional(T value)
        {
            _value = value;
        }

        public static Optional<T> Of(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            return value == null ? Empty() : Of(value);
        }

        public static Optional<T> Empty()
        {
            return LocalEmpty;
        }


        public T Get()
        {
            if (!IsPresent())
            {
                throw new OptionalValueNotSetException();
            }

            return _value;
        }

        public Optional<TResult> Map<TResult>(Func<T, TResult> mapFunc)
        {
            return !IsPresent() ? Optional<TResult>.Empty() : Optional<TResult>.Of(mapFunc(_value));
        }

        public async Task<Optional<TResult>> Map<TResult>(Func<T, Task<TResult>> mapFunc)
        {
            return !IsPresent() ? Optional<TResult>.Empty() : Optional<TResult>.Of(await mapFunc(_value));
        }

        public bool IsPresent()
        {
            return _value != null;
        }

        public void IfPresent(Action<T> actionToPerform)
        {
            if (IsPresent())
            {
                actionToPerform(_value);
            }
        }

        public async Task IfPresent(Func<T, Task> actionToPerform)
        {
            if (IsPresent())
            {
                await actionToPerform(_value);
            }
        }

        public T OrElse(T other)
        {
            return IsPresent() ? _value : other;
        }
        
        public T OrElseGet(Func<T> function)
        {
            return IsPresent() ? _value : function();
        }

        public T OrElseGet<TResult>(Func<TResult> function) where TResult : T
        {
            return IsPresent() ? _value : function();
        }

        public async Task<T> OrElseGet(Func<Task<T>> function)
        {
            return IsPresent() ? _value : await function();
        }

        public T OrElseThrow(Func<Exception> e)
        {
            if (!IsPresent())
            {
                throw e();
            }

            return _value;
        }

        public T OrElseThrow<TException>() where TException : Exception, new()
        {
            if (!IsPresent())
            {
                throw new TException();
            }

            return _value;
        }
    }
}