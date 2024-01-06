using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{

    public class EditorUtilities : MonoBehaviour
    {
        /// <summary>
        /// Print a message in the console if the setting is enable.
        /// </summary>
        /// <param name="message"></param>
        public static void LOG(string message)
        {
            if (VisualToolSettings.instance.DebugUI)
            {
                Debug.Log(message);
            }
        }
    }
}
