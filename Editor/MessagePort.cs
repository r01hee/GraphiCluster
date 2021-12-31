using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

using ClusterVR.CreatorKit.Trigger;
using ClusterVR.CreatorKit.Gimmick;
using ClusterVR.CreatorKit.Operation;
using ClusterVR.CreatorKit;
using ClusterVR.CreatorKit.Item;

using GraphiCluster.ClusterCreaterKit;
using GraphiCluster.GraphElements;

namespace GraphiCluster
{
    public class MessagePort : GraphiClusterPort
    {
        public string Key { get; }

        public GimmickTarget GimmickTarget { get; }

        public TriggerTarget TriggerTarget { get; }

        public IItem SpecifiedTargetItem { get; }

        public IEnumerable<GraphiClusterEdge> Edges { get => _Edges; }

        public bool IsInput { get; }

        private List<GraphiClusterEdge> _Edges { get; set; } = new List<GraphiClusterEdge>();

        public Label MakeLabel()
        {
            var target = IsInput ? GimmickTarget.ToString() : TriggerTarget.ToString();

            return new Label($"<{Key}> : {target}");
        }

        private MessagePort(string key, Orientation portOrientation, Direction portDirection, Capacity portCapacity, ParameterType parameterType) : base(portOrientation, portDirection, portCapacity, parameterType.ToType())
        {
            Key = key;
            this.portName = parameterType.ToString();
        }

        public MessagePort(string key, ParameterType type, GimmickTarget gimmickTarget)
            : this(key, Orientation.Horizontal, Direction.Input, Capacity.Multi, type)
        {
            GimmickTarget = gimmickTarget;
            IsInput = true;
        }

        public MessagePort(Trigger trigger)
            : this(trigger.Param.Key, Orientation.Horizontal, Direction.Output, Capacity.Multi, trigger.Param.Type)
        {
            TriggerTarget = trigger.Param.Target;
            if (TriggerTarget == TriggerTarget.SpecifiedItem)
            {
                SpecifiedTargetItem = trigger.Param.Convert().SpecifiedTargetItem;
            }
            IsInput = false;
        }

        public MessagePort(Statement statement)
            : this(statement.SingleStatement.TargetState.Key, Orientation.Horizontal, Direction.Output, Capacity.Multi, statement.SingleStatement.TargetState.ParameterType)
        {
            TriggerTarget = statement.SingleStatement.TargetState.Target.ToTriggerTarget();
            IsInput = false;
        }

        public void ConnectToInputPort(Port inputPort)
        {
            _Edges.Add(inputPort.ConnectTo<GraphiClusterEdge>(this));
        }
    }
}
