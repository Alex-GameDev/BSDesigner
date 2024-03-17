using System;
using System.Reflection;

namespace BSDesigner.Core
{
    /// <summary>
    /// Variable wrapper that allow classes to get values from blackboards.
    /// Parameter class is immutable.
    /// </summary>
    public abstract class Parameter
    {
        /// <summary>
        /// Get the allowed type of the parameter value
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// The current value of the parameter
        /// </summary>
        public abstract object? InternalValue { get; }

        /// <summary>
        /// The field bound to this parameter.
        /// </summary>
        public abstract BlackboardField? BaseBoundField { get; }
    }

    /// <summary>
    /// Parameter that wraps a value type of type <typeparamref name="T"/>.
    /// </summary>
    public class Parameter<T> : Parameter
    {
        #region Properties

        public override object? InternalValue => m_Value;

        public override BlackboardField? BaseBoundField => m_BoundField;

        public override Type Type => typeof(T);

        /// <summary>
        /// The value of the parameter.
        /// If is bound, the value is defined by the bound field.
        /// Otherwise is value.
        /// </summary>
        public T Value => m_BoundField != null ? m_BoundField.Value : m_Value;

        #endregion

        #region Fields

        private readonly T m_Value = default!;

        private readonly BlackboardField<T>? m_BoundField;

        #endregion

        #region Constructors

        public Parameter()
        {
            m_Value = default!;
            m_BoundField = null;
        }

        private Parameter(T value)
        {
            m_Value = value;
            m_BoundField = null;
        }

        private Parameter(BlackboardField<T> value)
        {
            m_Value = default!;
            m_BoundField = value;
        }

        #endregion

        /// <summary>
        /// Use this operator to get the value of the parameter without explicitly access to Value property.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public static implicit operator T(Parameter<T> param) => param.Value;

        /// <summary>
        /// Use this operator to create an independent parameter implicitly using the wrapped value.
        /// </summary>
        /// <param name="value">The wrapped value.</param>
        public static implicit operator Parameter<T>(T value) => new Parameter<T>(value);

        /// <summary>
        /// Use this operator to create a parameter bounded to the specified blackboard field
        /// </summary>
        /// <param name="boundField"></param>
        public static implicit operator Parameter<T>(BlackboardField<T> boundField) => new Parameter<T>(boundField);
    }
}
