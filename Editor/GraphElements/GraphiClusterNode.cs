using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace GraphiCluster.GraphElements
{
    public class GraphiClusterNode : Node
    {
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.StopPropagation();
        }
    }
}