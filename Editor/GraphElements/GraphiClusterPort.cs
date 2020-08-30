using System;

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphiCluster.GraphElements
{
    public class GraphiClusterPort : Port
    {
        public GraphiClusterPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
            : base(portOrientation, portDirection, portCapacity, type) { }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            return false;
        }
    }
}
