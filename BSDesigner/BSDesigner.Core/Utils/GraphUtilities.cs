using BSDesigner.Core.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSDesigner.Core.Utils
{
    public static class GraphUtilities
    {
        /// <summary>
        /// Return if the target element is connected with the source element based on the nesting function
        /// </summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="source">The source element</param>
        /// <param name="target">The target element</param>
        /// <param name="nestingFunction">The function used to obtain the directly nested elements of a given element</param>
        /// <returns>True if source and target are nested</returns>
        /// <exception cref="ArgumentNullException">Thrown if source or targets are null</exception>
        public static bool IsNestedWith<T>(this T source, T target, Func<T, IEnumerable<T>> nestingFunction)
        {
            if(source == null || target == null)
            {
                throw new ArgumentNullException("Source and target elements must be not null");
            }

            var unvisitedNodes = new HashSet<T>();
            var visitedNodes = new HashSet<T>();

            unvisitedNodes.Add(source);
            while (unvisitedNodes.Count > 0)
            {
                var n = unvisitedNodes.First();
                unvisitedNodes.Remove(n);
                visitedNodes.Add(n);
                foreach (var parent in nestingFunction(n))
                {
                    if (target.Equals(parent))
                        return true;
                    if (!visitedNodes.Contains(parent))
                        unvisitedNodes.Add(parent);
                }
            }
            return false;
        }

        /// <summary>
        /// Return the nested unique elements of the given source element based on the nesting function.
        /// </summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="element">The source element</param>
        /// <param name="nestingFunction">The function used to obtain the directly nested elements of a given element</param>
        /// <returns>A collection of nested elements</returns>
        /// <exception cref="ArgumentNullException">Thrown if element is null</exception>
        public static HashSet<T> GetNestedElements<T>(this T element, Func<T, IEnumerable<T>> nestingFunction, bool includeSelf = false)
        {
            if (element == null)
            {
                throw new ArgumentNullException("Source and target elements must be not null");
            }

            var unvisitedNodes = new HashSet<T>();
            var visitedNodes = new HashSet<T>();
            unvisitedNodes.Add(element);

            while (unvisitedNodes.Count > 0)
            {
                var n = unvisitedNodes.First();
                unvisitedNodes.Remove(n);
                visitedNodes.Add(n);
                foreach (var parent in nestingFunction(n))
                {
                    if (!visitedNodes.Contains(parent))
                        unvisitedNodes.Add(parent);
                }
            }

            if(!includeSelf)
            {
                visitedNodes.Remove(element);
            }

            return visitedNodes;
        }
    }
}
