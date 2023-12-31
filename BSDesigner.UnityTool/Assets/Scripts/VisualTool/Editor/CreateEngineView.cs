using System;
using System.Linq;
using BSDesigner.Core;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{

    public class CreateEngineView : EditorWindow
    {
        private EventHandler<Type> m_OnSelectEntryHandler;

        public static void Open(EventHandler<Type> onSelectEntryHandler)
        {
            var window = CreateWindow<CreateEngineView>();
            window.m_OnSelectEntryHandler = onSelectEntryHandler;
            window.ShowModal();
        }


        private void CreateGUI()
        {
            var engineTypes = TypeCache.GetTypesDerivedFrom<BehaviourEngine>().Where(t => !t.IsAbstract);
            foreach (var engineType in engineTypes)
            {
                CreateEntry(engineType);
            }
        }

        private void CreateEntry(Type engineType)
        {
            var element = new VisualElement();
            element.Add(new Button(() => OnSelectEntry(engineType)){ text = engineType.Name});
            rootVisualElement.Add(element);
        }

        private void OnSelectEntry(Type engineType)
        {
            this.Close();
            m_OnSelectEntryHandler.Invoke(this, engineType);
        }
    }
}
