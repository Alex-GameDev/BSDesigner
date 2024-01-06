using System;

namespace BSDesigner.Core
{
    [Serializable]
    public abstract class BlackboardField
    {
        /// <summary>
        /// The name of the field, used as identifier in the blackboard.
        /// </summary>
        public string Name;

        /// <summary>
        /// The object representation of the field value.
        /// </summary>
        public abstract object? BaseValue { get; set; }

        /// <summary>
        /// The type of the variables that the field can handle.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// Event used to notify parameters when the field is disposed.
        /// </summary>
        public event Action? FieldUnbind;

        protected BlackboardField()
        {
            Name = string.Empty;
        }


        protected BlackboardField(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Method called from the blackboard when the field is removed.
        /// </summary>
        internal void Unbind() => FieldUnbind?.Invoke();
    }

    /// <summary>
    /// A generic type representation of a blackboard field
    /// </summary>
    /// <typeparam name="T">Tye type of the value handled.</typeparam>
    [Serializable]
    public class BlackboardField<T> : BlackboardField
    {
        /// <summary>
        /// The value of the field.
        /// </summary>
        public T Value = default!;

        public override object? BaseValue 
        { 
            get => Value;
            set => Value = (T)value; 
        }

        public override Type Type => typeof(T);

        public BlackboardField() : base()
        {

        }

        public BlackboardField(string name) : base(name)
        {
        }
    }
}