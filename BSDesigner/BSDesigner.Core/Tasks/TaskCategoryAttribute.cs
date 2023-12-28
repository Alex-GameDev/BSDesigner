using System;

namespace BSDesigner.Core.Tasks
{
    /// <summary>
    /// Attribute to set the category of a task
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TaskCategoryAttribute : Attribute
    {
        /// <summary>
        /// The category name.
        /// </summary>
        public string Name { get; set; }

        public TaskCategoryAttribute(string name)
        {
            Name = name;
        }
    }
}
