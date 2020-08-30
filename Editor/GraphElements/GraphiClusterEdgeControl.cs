using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphiCluster.GraphElements
{
    public class GraphiClusterEdgeControl : EdgeControl
    {
        public override bool ContainsPoint(Vector2 localPoint)
        {
            return false;
        }

        protected override void UpdateRenderPoints()
        {
            base.UpdateRenderPoints();
        }
    }
}