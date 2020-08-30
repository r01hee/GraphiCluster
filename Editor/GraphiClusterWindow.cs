using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphiCluster
{
    public class GraphiClusterWindow : EditorWindow
    {
        [MenuItem("Cluster/GraphiCluster", priority = 10501)]
        private static void MenuItemGenerator()
        {
            var Window = CreateInstance<GraphiClusterWindow>();
            Window.Show();
            Window.titleContent = new GUIContent("GraphiCluster");
        }

        private GraphiClusterGraphView graphView;

        void OnEnable()
        {
            graphView = new GraphiClusterGraphView()
            {
                style = { flexGrow = 1 },
            };

            rootVisualElement.Add(graphView);

            rootVisualElement.Add(new Button(graphView.Regenerate) { text = "Reload" });
        }

        void OnGUI()
        {
            graphView.OnGUI();
        }
    }
}