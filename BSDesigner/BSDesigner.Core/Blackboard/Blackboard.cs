using System;
using System.Collections.Generic;

namespace BSDesigner.Core
{
    /// <summary>
    /// Data class that allow write and read variables of any type identified by a name.
    /// </summary>
    [Serializable]
    public class Blackboard : IBlackboard
    {
        Dictionary<string, BlackboardField> fields = new Dictionary<string, BlackboardField>();

        /// <summary>
        /// Get all the fields from the blackboard.
        /// </summary>
        /// <returns>The fields stored in the blackboard.</returns>
        public IEnumerable<BlackboardField> GetAllFields() => fields.Values;


        /// <summary>
        /// Default constructor
        /// </summary>
        public Blackboard()
        {
        }

        /// <summary>
        /// Create a new blackboard from a field collection.
        /// </summary>
        /// <param name="fields">The fields</param>
        public Blackboard(IEnumerable<BlackboardField> fields)
        {
            foreach (var blackboardField in fields)
            {
                this.fields[blackboardField.Name] = blackboardField;
            }
        }

        /// <summary>
        /// Create a new field of type <typeparamref name="T"/> named <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of the variable stored in the field.</typeparam>
        /// <param name="id">The field identifier</param>
        /// <returns>The field created.</returns>
        public BlackboardField<T> CreateField<T>(string id)
        {
            var field = new BlackboardField<T>(id);
            fields[id] = field;
            return field;
        }

        /// <summary>
        /// Create a new field of type <typeparamref name="T"/> named <paramref name="id"/> 
        /// that stores <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the variable stored in the field.</typeparam>
        /// <param name="id">The field identifier</param>
        /// <param name="value">The initial value of the field.</param>
        /// <returns>The field created.</returns>
        public BlackboardField<T> CreateField<T>(string id, T value)
        {
            var field = CreateField<T>(id);
            field.Value = value;
            return field;
        }

        /// <summary>
        /// Create a new field of the type defined in <paramref name="fieldType"/> or the type of <paramref name="value"/>
        /// if the first is null.
        /// </summary>
        /// <param name="id">The field identifier</param>
        /// <param name="value">The initial value of the field.</param>
        /// <param name="fieldType">The type of the field</param>
        /// <returns>The field created.</returns>
        /// <exception cref="Exception"></exception>
        public BlackboardField CreateField(string id, object value, Type? fieldType = null)
        {
           var type = fieldType ?? value.GetType();

            if (type == null) throw new Exception();

            var field = CreateField(id, type);

            field.BaseValue = value;
            return field;
        }

        /// <summary>
        /// Create a new field of the type defined in <paramref name="fieldType"/>.
        /// </summary>
        /// <param name="id">The field identifier.</param>
        /// <param name="fieldType">The type of the field</param>
        /// <returns>The created </returns>
        public BlackboardField CreateField(string id, Type fieldType)
        {
            var completeType = typeof(BlackboardField<>).MakeGenericType(fieldType);
            var field = (BlackboardField)Activator.CreateInstance(completeType);
            fields.Add(id, field);
            return field;
        }

        /// <summary>
        /// Get the field that holds a variable of type <typeparamref name="T"/> named <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="id">The identifier of the requested field.</param>
        /// <returns>The field found.</returns>
        public BlackboardField<T> GetFieldById<T>(string id)
        {
            var field = GetFieldById(id);
            return (BlackboardField<T>) field;
        }

        /// <summary>
        /// Get the field named <paramref name="id"/>
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="id">The identifier of the requested field.</param>
        /// <returns>The field found.</returns>
        public BlackboardField GetFieldById(string id)
        {
            var field = fields[id];
            return field;
        }

        /// <summary>
        /// Get all the fields
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetFieldIds()
        {
            return fields.Keys;
        }

        public void RemoveFieldById(string id)
        {
            var field = GetFieldById(id);
            field.Unbind();
            fields.Remove(id);
        }

        /// <summary>
        /// Remove all the fields in the blackboard.
        /// </summary>
        public void Clear()
        {
            foreach (var field in fields.Values)
            {
                field.Unbind();
            }

            fields.Clear();
        }

        /// <summary>
        /// Move all the fields from this blackboard to other (Used in deserialization).
        /// </summary>
        /// <param name="other">The other blackboard</param>
        public void CopyTo(IBlackboard other)
        {
            foreach (var kvp in fields)
            {
                fields.Add(kvp.Key, kvp.Value);
            }
        }
    }
}