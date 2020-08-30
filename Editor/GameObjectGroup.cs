using System.Linq;
using UnityEngine;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace GraphiCluster
{
    public class GameObjectGroup : Group
    {
        private static readonly StyleSheet IconButtonStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/dev.r01.graphicluster/Editor/Resources/GameObjectButton.uss");

		public ComponentNode[] ComponentNodes { get; }

        public Transform Transform { get; }

        public int InstanceId { get; }

        public GameObjectGroup(ComponentNode[] componentNodes)
        {
            ComponentNodes = componentNodes;

            Transform = componentNodes.FirstOrDefault()?.Origin?.transform;

            AddElements(componentNodes);

            title = Transform?.name;

            InstanceId = Transform?.GetInstanceID() ?? 0;

            headerContainer.style.flexDirection = FlexDirection.Row;

            var button = new Button(SelectGameObject);
            button.styleSheets.Add(IconButtonStyleSheet);

            headerContainer.Add(button);
            headerContainer.Sort((x, y) => Object.ReferenceEquals(x, button) ? -1 : 1);
        }

        private void SelectGameObject()
        {
            if (Transform != null)
            {
                Selection.activeObject = Transform;
            }
        }

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            if (Transform != null)
            {
                Transform.name = newName;
            }
        }
    }
}