using ClusterVR.CreatorKit.Trigger.Implements;

namespace GraphiCluster.ClusterCreaterKit
{
    public class Trigger
    {
        public ConstantTriggerParam Param { get; }

        public string Name { get; }

        public Trigger(ConstantTriggerParam triggerParam, string name)
        {
            Param = triggerParam;
            Name = name;
        }

        public Trigger(ConstantTriggerParam triggerParam) : this(triggerParam, "") { }
    }
}