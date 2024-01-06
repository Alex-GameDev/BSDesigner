using System;
using System.Collections.Generic;
using System.Text;

namespace BSDesigner.Core
{
    public interface IBlackboard
    {
        IEnumerable<BlackboardField> GetAllFields();
        BlackboardField<T> CreateField<T>(string id);
        BlackboardField<T> CreateField<T>(string id, T value);
        BlackboardField CreateField(string id, object value, Type? fieldType = null);
        BlackboardField CreateField(string id, Type fieldType);
        BlackboardField<T> GetFieldById<T>(string id);
        BlackboardField GetFieldById(string id);
        IEnumerable<string> GetFieldIds();
        void RemoveFieldById(string id);
        void Clear();

        void CopyTo(IBlackboard other);
    }
}
