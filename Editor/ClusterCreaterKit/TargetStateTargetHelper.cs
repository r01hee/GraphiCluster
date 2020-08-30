using System.Collections.Generic;

using ClusterVR.CreatorKit.Trigger;
using ClusterVR.CreatorKit.Operation;

namespace GraphiCluster.ClusterCreaterKit
{ 
    public static class TargetStateTargetHelper
    {
        private static readonly IDictionary<TargetStateTarget, TriggerTarget> _TargetStateTargetToTriggerTarget = new Dictionary<TargetStateTarget, TriggerTarget>
        {
            { TargetStateTarget.Global, TriggerTarget.Global },
            { TargetStateTarget.Item, TriggerTarget.Item },
            { TargetStateTarget.Player, TriggerTarget.Player },
        };

        public static TriggerTarget ToTriggerTarget(this TargetStateTarget targetStateTarget)
            => _TargetStateTargetToTriggerTarget.TryGetValue(targetStateTarget, out var res) ? res : default;
    }
}
