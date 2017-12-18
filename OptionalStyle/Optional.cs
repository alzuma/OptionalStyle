using System;
using System.Collections;
using System.Collections.Generic;
using OptionalStyle.exceptions;

namespace OptionalStyle
{
    public class Optional<T> : IEnumerable<T>
    {
        private readonly T[] _data;

        private Optional(T[] data)
        {
            _data = data;
        }

        public static Optional<T> ValueOf(T value)
        {
            return value == null ? Empty() : new Optional<T>(new[] { value });
        }

        public static Optional<T> Empty()
        {
            return new Optional<T>(new T[0]);
        }

        public T Get()
        {
            if (!IsPresent())
            {
                throw new OptionalValueNotSetException();
            }
            return _data[0];
        }

        public Optional<T> Map(Func<T, Optional<T>> mapFunc)
        {
            return !IsPresent() ? Empty() : mapFunc(_data[0]);
        }

        public bool IsPresent()
        {
            return _data.Length > 0;
        }

        public void IfPresent(Action<T> actionToPerform)
        {
            if (IsPresent())
            {
                actionToPerform(_data[0]);
            }
        }

        public T OrElse(T other)
        {
            return IsPresent() ? _data[0] : other;
        }

        public T OrElseGet(Func<T> function)
        {
            return IsPresent() ? _data[0] : function();
        }

        public T OrElseThrow(Exception e)
        {
            if (!IsPresent())
            {
                throw e;
            }
            return _data[0];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._data.GetEnumerator();
        }
    }
}
