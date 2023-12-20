using System;
using System.Collections.Generic;

public class SignalingVariable<T> : IEquatable<SignalingVariable<T>>
{
    private T _value;
    public Action<T, T> OnChangedValue; // newValue, oldValue
    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (_value.Equals(value))
                return;
            var oldValue = _value;
            _value = value;

            OnChangedValue?.Invoke(_value, oldValue);
        }
    }

    public override bool Equals(object obj)
    {
        return obj is SignalingVariable<T> variable &&
               EqualityComparer<T>.Default.Equals(_value, variable._value) &&
               EqualityComparer<Action<T, T>>.Default.Equals(OnChangedValue, variable.OnChangedValue) &&
               EqualityComparer<T>.Default.Equals(Value, variable.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value, OnChangedValue, Value);
    }

    public bool Equals(SignalingVariable<T> other)
    {
        return Equals((object)other);
    }
}