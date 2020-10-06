using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using ClusterVR.CreatorKit.Gimmick.Implements;
using ClusterVR.CreatorKit.Operation;

using TriggerParam = ClusterVR.CreatorKit.Trigger.Implements.TriggerParam;

using GraphiCluster.ClusterCreaterKit;
using GraphiCluster.GraphElements;

namespace GraphiCluster
{
    public class ComponentNode : GraphiClusterNode
    {
        #region Private Static Fields

        private static readonly MessagePort[] MessagePortsEmpty = new MessagePort[0];

        #endregion

        #region Public Properies

        public MessagePort[] InputPorts { get; set; } = MessagePortsEmpty;

        public MessagePort[] OutputPorts { get; set; } = MessagePortsEmpty;

        public IEnumerable<MessagePort> AllPorts { get => InputPorts.Concat(OutputPorts); }

        public MonoBehaviour Origin { get; }

        public Type Type { get; }

        public ComponentType ComponentType { get; }

        public int InstanceId { get; }

        public Trigger[][] TriggerSets { get; }

        public GimmickKey Gimmick { get; }

        public Logic Logic { get; }

        public LotteryChoice[] LotteryChoices { get; }

        public ComponentNode ComponentParent { get; set; }

        public ComponentNode[] ComponentChildren { get; set; }

        #endregion

        #region Constructors

        public ComponentNode(MonoBehaviour monoBehaviour, Type type)
        {
            Origin = monoBehaviour;

            Type = type;

            InstanceId = Origin.transform.GetInstanceID();

            ComponentType = ComponentTypeHelper.FromType(type);

            var fieldInfos = Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            TriggerSets = FindTriggerSets(fieldInfos);

            Gimmick = FindGimmick(fieldInfos);

            Logic = FindLogic(fieldInfos);

            LotteryChoices = FindLotteryChoices(fieldInfos);

            InputPorts = MakeInputPorts();

            OutputPorts = MakeoutputPorts();

            var inPortSetFirst = InputPorts.FirstOrDefault();
            if (inPortSetFirst != null)
            {
                this.inputContainer.Add(inPortSetFirst.MakeLabel());

                foreach (var x in InputPorts)
                {
                    this.inputContainer.Add(x);
                }
            }

            foreach (var x in OutputPorts)
            {
                this.outputContainer.Add(x.MakeLabel());
                this.outputContainer.Add(x);
            }

            this.title = Type.Name;
        }

        #endregion

        #region Private Methods

        private MessagePort[] MakeInputPorts()
        {
            if (ComponentType == ComponentType.Trigger)
            {
                return MessagePortsEmpty;
            }

            return ComponentTypeHelper.TypeOfGimmickToParameterTypes(Type)
                .Select(parameterType => new MessagePort(Gimmick.Key, parameterType, Gimmick.Target))
                .ToArray();
        }

        private MessagePort[] MakeoutputPorts()
        {
            if (ComponentType == ComponentType.Gimmick)
            {
                return MessagePortsEmpty;
            }

            if (TriggerSets != null)
            {
                return TriggerSets.SelectMany(x => x.Select(x2 => new MessagePort(x2))).ToArray();
            }

            if (Logic != null)
            {
                return Logic.Statements.Select(x => new MessagePort(x)).ToArray();
            }

            if (LotteryChoices != null)
            {
                return LotteryChoices.SelectMany(x => x.Triggers.Select(x2 => new MessagePort(new Trigger(x2)))).ToArray();
            }

            return MessagePortsEmpty;
        }

        private GimmickKey FindGimmick(FieldInfo[] fieldInfos)
        {
            var key = GetOriginField<GimmickKey>(fieldInfos);
            if (key != null)
            {
                return key;
            }

            var globalKey = GetOriginField<GlobalGimmickKey>(fieldInfos);
            if (globalKey != null)
            {
                return globalKey.Key;
            }

            var playerKey = GetOriginField<PlayerGimmickKey>(fieldInfos);
            if (playerKey != null)
            {
                return playerKey.Key;
            }

            return null;
        }

        private Trigger[][] FindTriggerSets(FieldInfo[] fieldInfos)
        {
            var res = fieldInfos
                .Where(x => x.FieldType == typeof(TriggerParam[]))
                .Select(f =>
                {
                    var triggerParams = (TriggerParam[])f.GetValue(this.Origin);

                    return triggerParams.Select(x => new Trigger(x, f.Name)).ToArray();

                })
                .ToArray();

            return res.Any() ? res : null;
        }

        private Logic FindLogic(FieldInfo[] fieldInfos) => GetOriginField<Logic>(fieldInfos);

        private LotteryChoice[] FindLotteryChoices(FieldInfo[] fieldInfos)
        {
            if (!ComponentType.Lottery.ToTypes().Contains(this.Type))
            {
                return null;
            }

            var assembly = Assembly.GetAssembly(this.Type);
            var choiceType = assembly.GetType($"{this.Type.Namespace}.{this.Type.Name}+Choice");
            if (choiceType == null)
            {
                return null;
            }

            var choiceFieldInfo = fieldInfos.FirstOrDefault(x => x.FieldType.IsArray && x.FieldType.GetElementType() == choiceType);
            var choices = choiceFieldInfo?.GetValue(this.Origin) as IEnumerable;
            if (choices == null)
            {
                return null;
            }

            var choiceFieldInfos = choiceType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var triggerParamsFieldInfo = choiceFieldInfos.FirstOrDefault(x => x.FieldType == typeof(TriggerParam[]));
            var weightFieldFinfo = choiceFieldInfos.FirstOrDefault(x => x.Name == "weight");

            var res = choices
                .Cast<object>()
                .Select(x => new LotteryChoice
                {
                    Triggers = (TriggerParam[])triggerParamsFieldInfo?.GetValue(x),
                    Weight = (float)weightFieldFinfo?.GetValue(x),
                })
                .ToArray();

            return res.Any() ? res : null;
        }

        private T GetOriginField<T>(IEnumerable<FieldInfo> fieldInfos) where T : class
            => fieldInfos.FirstOrDefault(x => x.FieldType == typeof(T))?.GetValue(this.Origin) as T;

        #endregion
    }
}