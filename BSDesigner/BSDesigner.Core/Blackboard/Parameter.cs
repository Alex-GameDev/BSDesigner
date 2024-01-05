using System;
namespace BSDesigner.Core
{
    /// <summary>
    /// Variable wrapper that allow classes to get values from blackboards.
    /// </summary>
    public abstract class Parameter
    {
        protected Parameter() { }

        /// <summary>
        /// Get the allowed type of the parameter value
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// The current value of the parameter
        /// </summary>
        public abstract object? ObjectValue { get; set; }

        /// <summary>
        /// The field bound to this parameter.
        /// </summary>
        public abstract BlackboardField? BaseBoundField { get; set; }

        public void Reset()
        {
            BaseBoundField = null;
            ObjectValue = default;
        }
    }

    /// <summary>
    /// Parameter that wraps a value type of type <typeparamref name="T"/>.
    /// </summary>
    public class Parameter<T> : Parameter
    {
        public override object? ObjectValue
        {
            get => Value;
            set => Value = (T)value!;
        }

        public override BlackboardField? BaseBoundField
        {
            get => BoundField;
            set => BoundField = (BlackboardField<T>?)value;
        }
        
        public override Type Type => typeof(T);

        /// <summary>
        /// The value of the parameter
        /// </summary>
        public T Value
        {
            get => _boundField != null ? _boundField.Value : _value;
            set
            {
                if (_boundField != null)
                {
                    _boundField.FieldUnbind -= Reset;
                }
                _boundField = null;
                _value = value;

            }
        }
        private T _value = default!;

        /// <summary>
        /// The blackboard field assigned
        /// </summary>
        public BlackboardField<T>? BoundField
        {
            get => _boundField;
            set
            {
                Value = default!;
                _boundField = value;

                if(_boundField != null)
                {
                    _boundField.FieldUnbind += Reset;
                }
            }
        }

        private BlackboardField<T>? _boundField;

        /// <summary>
        /// Use this operator to get the value of the parameter without explicitly access to Value property.
        /// </summary>
        /// <param name="param">The parameter.</param>

        public static implicit operator T(Parameter<T> param) => param.Value;

        /// <summary>
        /// Use this operator to create an independent parameter implicitly using the wrapped value.
        /// </summary>
        /// <param name="value">The wrapped value.</param>

        public static implicit operator Parameter<T>(T value) => new Parameter<T>{ Value = value};

        /// <summary>
        /// Use this operator to create a parameter bounded to the specified blackboard field
        /// </summary>
        /// <param name="boundField"></param>

        public static implicit operator Parameter<T>(BlackboardField<T> boundField) => new Parameter<T> { BoundField = boundField };
    }
}
