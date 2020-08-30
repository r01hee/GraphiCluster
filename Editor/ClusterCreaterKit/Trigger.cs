using ClusterVR.CreatorKit.Trigger.Implements;

namespace GraphiCluster.ClusterCreaterKit
{
    public class Trigger
    {
        public TriggerParam Param { get; }

        public string Name { get; }

        public Trigger(TriggerParam triggerParam, string name)
        {
            Param = triggerParam;
            Name = name;
        }

        public Trigger(TriggerParam triggerParam) : this(triggerParam, "") { }
    }
}