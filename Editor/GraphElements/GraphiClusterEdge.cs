using UnityEditor.Experimental.GraphView;

namespace GraphiCluster.GraphElements
{
    public class GraphiClusterEdge : Edge
    {
        private const float k_EndPointRadius = 4.0f;
        private const float k_InterceptWidth = 6.0f;

        protected override EdgeControl CreateEdgeControl()
        {
            return new GraphiClusterEdgeControl
             {
                capRadius = k_EndPointRadius,
                interceptWidth = k_InterceptWidth
            };
        }
    }
}