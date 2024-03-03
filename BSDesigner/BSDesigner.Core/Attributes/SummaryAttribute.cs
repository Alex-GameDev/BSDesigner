using System;

namespace BSDesigner.Core.Attributes
{
    /// <summary>
    /// Specify a summary description of an element
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SummaryAttribute : Attribute
    {
        /// <summary>
        /// Text content
        /// </summary>
        public string Text { get; set; }

        public SummaryAttribute(string text)
        {
            Text = text;
        }
    }
}