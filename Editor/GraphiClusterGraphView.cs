using System.Linq;
using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

using ClusterVR.CreatorKit.Trigger;
using ClusterVR.CreatorKit.Gimmick;

using GraphiCluster.ClusterCreaterKit;

namespace GraphiCluster
{
    public class GraphiClusterGraphView : GraphView
    {
        private static readonly Vector2 GroupMargin = new Vector2(200.0f, 50.0f);
        private static readonly Vector2 NodeMargin = new Vector2(50.0f, 25.0f);

        private IEnumerable<ComponentNode> componentNodes = Enumerable.Empty<ComponentNode>();

        private IEnumerable<GameObjectGroup> gameObjectGroups = Enumerable.Empty<GameObjectGroup>();

        private const int RerenderCountInitial = 0;
        private const int RerenderCountFinal = 4;
        private int rerenderCount = int.MaxValue;

        private string lastScenePath = null;

        private IDictionary<int, Vector2> lastGroupPositions = null;

        public GraphiClusterGraphView() : base()
        {
            style.flexGrow = 1;
            style.flexShrink = 1;

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            Insert(0, new GridBackground());

            this.AddManipulator(new SelectionDragger());
        }

		public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			evt.StopPropagation();
		}

		public void OnGUI()
        {
            if (rerenderCount > RerenderCountFinal)
            {
                return;
            }

            if (rerenderCount == RerenderCountInitial)
            {
                foreach (var g in gameObjectGroups)
                {
                    float x = 0;
                    UpdateGraphelementsPosition(g.ComponentNodes.Where(n => !n.InputPorts.Any()), NodeMargin, ref x);
                    UpdateGraphelementsPosition(g.ComponentNodes.Where(n => n.InputPorts.Any() && n.OutputPorts.Any()), NodeMargin, ref x);
                    UpdateGraphelementsPosition(g.ComponentNodes.Where(n => !n.OutputPorts.Any()), NodeMargin, ref x);
                }
            }
            else if (rerenderCount == RerenderCountFinal) // if RerenderCountFinal is less than 4, it goes wrong because the timing would be too fast
            {
                if (lastGroupPositions == null)
                {
                    float x = 0;
                    UpdateGraphelementsPosition(gameObjectGroups.Where(g => !g.ComponentNodes.SelectMany(n => n.InputPorts).Any()), GroupMargin, ref x);
                    UpdateGraphelementsPosition(gameObjectGroups.Where(g => g.ComponentNodes.SelectMany(n => n.InputPorts).Any() && g.ComponentNodes.SelectMany(n => n.OutputPorts).Any()), GroupMargin, ref x);
                    UpdateGraphelementsPosition(gameObjectGroups.Where(g => !g.ComponentNodes.SelectMany(n => n.OutputPorts).Any()), GroupMargin, ref x);
                }
                else
                {
                    foreach (var g in gameObjectGroups)
                    {
                        if (lastGroupPositions.TryGetValue(g.InstanceId, out var pos))
                        {
                            var rect = g.GetPosition();

                            rect.x = pos.x;
                            rect.y = pos.y;

                            g.SetPosition(rect);
                        }
                    }
                }
            }

            rerenderCount++;
        }

        public void Regenerate()
        {
            rerenderCount = RerenderCountInitial;

            var currentScenePath = SceneManager.GetActiveScene().path;

            if (currentScenePath == lastScenePath)
            {
                lastGroupPositions = gameObjectGroups.ToDictionary(x => x.InstanceId, x => x.GetPosition().position);
            }
            else
            {
                lastGroupPositions = null;
            }

            RemoveAllElements();

            componentNodes = GetAllComponentNodes();
            gameObjectGroups = componentNodes
                .GroupBy(x => x.InstanceId)
                .Select(x => new GameObjectGroup(x.ToArray()))
                .ToArray();

            foreach (var outPort in componentNodes.SelectMany(x => x.OutputPorts).Where(x => !string.IsNullOrEmpty(x.Key)))
            {
                    foreach (var i in FindInPortsRelatedWithOutPort(outPort))
                {
                    outPort.ConnectToInputPort(i);
                }
            }

            foreach (var o in gameObjectGroups)
            {
                AddElement(o);

                foreach (var c in o.ComponentNodes)
                {
                    AddElement(c);

                    foreach (var e in c.AllPorts.SelectMany(x => x.Edges))
                    {
                        AddElement(e);
                    }
                }
            }

            lastScenePath = currentScenePath;
        }

        private void UpdateGraphelementsPosition(IEnumerable<GraphElement> elements, Vector2 margin, ref float x)
        {
            float widthMax = 0;

            float y = 0;

            foreach (var e in elements)
            {
                var rect = e.GetPosition();

                rect.x = x;
                rect.y = y;

                if (rect.width > widthMax)
                {
                    widthMax = rect.width;
                }
                y += rect.height + margin.y;

                e.SetPosition(rect);
            }

            x += widthMax + margin.x;
        }

        private void RemoveAllElements()
        {
            foreach (var o in gameObjectGroups)
            {
                foreach (var c in o.ComponentNodes)
                {
                    foreach (var e in c.AllPorts.SelectMany(x => x.Edges))
                    {
                        RemoveElement(e);
                    }
                    RemoveElement(c);
                }
                RemoveElement(o);
            }
        }

        private ComponentNode[] GetAllComponentNodes()
        {
            var objects = FindObjectsRelatedWithCluster().ToArray();

            foreach (var o in objects)
            {
                var childIds = o.Origin.GetComponentsInChildren<Transform>()
                    .Select(x => x.GetInstanceID())
                    .Where(x => x != o.InstanceId)
                    .ToArray();

                o.ComponentChildren = objects
                    .Where(x => childIds.Contains(x.InstanceId))
                    .ToArray();
            }

            foreach (var o in objects)
            {
                foreach (var c in o.ComponentChildren)
                {
                    if (c.ComponentParent == null || c.ComponentParent.ComponentChildren.Any(x => x.InstanceId == o.InstanceId))
                    {
                        c.ComponentParent = o;
                    }
                }
            }

            return objects;
        }

        private IEnumerable<ComponentNode> FindObjectsRelatedWithCluster()
        {
            var scenePath = SceneManager.GetActiveScene().path;

            foreach (var type in ComponentTypeHelper.AllTypes)
            {
                foreach (var obj in Resources.FindObjectsOfTypeAll(type)
                    .Select(x => (MonoBehaviour)x)
                    .Where(x => x.gameObject.scene.path == scenePath)
                    .Select(x => new ComponentNode(x, type))
                )
                {
                    yield return obj;
                }
            }
        }

        private IEnumerable<MessagePort> FindInPortsRelatedWithOutPort(MessagePort outPort)
        {
            var allInPorts = componentNodes.SelectMany(x => x.InputPorts);

            switch (outPort.TriggerTarget)
            {
                case TriggerTarget.Global:
                    return allInPorts
                        .Where(x => x.GimmickTarget == GimmickTarget.Global && x.Key == outPort.Key && x.portType == outPort.portType);
                case TriggerTarget.SpecifiedItem:
                    return componentNodes
                        .Where(x => outPort.SpecifiedTargetItem != null && x.InstanceId == outPort.SpecifiedTargetItem.gameObject.transform.GetInstanceID())
                        .SelectMany(x => x.InputPorts)
                        .Where(x => x.Key == outPort.Key && x.portType == outPort.portType);
                case TriggerTarget.CollidedItemOrPlayer:
                    return allInPorts
                        .Where(x => (x.GimmickTarget == GimmickTarget.Player || x.GimmickTarget == GimmickTarget.Item) && x.Key == outPort.Key && x.portType == outPort.portType);
                case TriggerTarget.Player:
                    return allInPorts
                        .Where(x => x.GimmickTarget == GimmickTarget.Player && x.Key == outPort.Key && x.portType == outPort.portType);
                case TriggerTarget.Item:
                    return allInPorts
                        .Where(x => x.GimmickTarget == GimmickTarget.Item && x.Key == outPort.Key && x.portType == outPort.portType);
                default:
                    return Enumerable.Empty<MessagePort>();
            }
        }
    }
}