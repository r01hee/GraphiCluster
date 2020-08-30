using System;
using System.Collections.Generic;

using UnityEngine;

using ClusterVR.CreatorKit;

namespace GraphiCluster.ClusterCreaterKit
{
    public static class ParameterTypeHelper
    {
        private static readonly IDictionary<ParameterType, Type> _ParameterTypeToType = new Dictionary<ParameterType, Type>
        {
            { ParameterType.Signal, typeof(GameObject) },
            { ParameterType.Bool, typeof(bool) },
            { ParameterType.Float, typeof(float) },
            { ParameterType.Integer, typeof(int) },
        };

        public static Type ToType(this ParameterType parameterType)
            => _ParameterTypeToType.TryGetValue(parameterType, out var res) ? res : default;
    }
}