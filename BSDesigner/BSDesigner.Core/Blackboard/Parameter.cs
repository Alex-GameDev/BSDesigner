namespace BSDesigner.Core
{
    /// <summary>
    /// Parameter that wraps a variable of type <typeparamref name="T"/>.
    /// </summary>
    public class Parameter<T>
    {
        /// <summary>
        /// The value of the parameter
        /// </summary>
        public T Value { get; set; } = default!;

        /// <summary>
        /// Use this operator to get the value of the parameter without explicitly access to Value property.
        /// </summary>
        /// <param name="param">The parameter.</param>

        public static implicit operator T(Parameter<T> param) => param.Value;

        /// <summary>
        /// Use this operator to create an independent parameter implicitly using the wrapped value.
        /// </summary>
        /// <param name="value">The wrapped value.</param>

        public static implicit operator Parameter<T>(T value) { return new Parameter<T>{ Value = value}; }
    }
}
