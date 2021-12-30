using System;
using System.Linq;
using System.Collections.Generic;

using ClusterVR.CreatorKit;
using ClusterVR.CreatorKit.Trigger.Implements;
using ClusterVR.CreatorKit.Gimmick.Implements;
using ClusterVR.CreatorKit.Operation.Implements;

namespace GraphiCluster.ClusterCreaterKit
{
    public enum ComponentType
    {
        Trigger,
        Gimmick,
        Logic,
        Lottery,
        Timer,
    }

    public static class ComponentTypeHelper
    {
        #region Static Data

        private static readonly IDictionary<Type, ParameterType[]> _TypeOfGimmickToParameterTypes = new Dictionary<Type, ParameterType[]>
        {
            {
                typeof(AddContinuousForceItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Bool,
                    ParameterType.Float,
					ParameterType.Integer
				}
			},
            {
                typeof(AddContinuousTorqueItemGimmick),
                new ParameterType[]
                {
					ParameterType.Bool,
					ParameterType.Float,
					ParameterType.Integer
				}
			},
            {
                typeof(AddInstantForceItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(AddInstantTorqueItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(CreateItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(DestroyItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(GetOffItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
				}
			},
            {
                typeof(JumpCharacterItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
				}
			},
            {
                typeof(PlayAudioSourceGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal,
					ParameterType.Bool
				}
			},
            {
                typeof(PlayTimelineGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(RespawnPlayerGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(SetAngularVelocityCharacterItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Float,
                    ParameterType.Integer
                }
            },
            {
                typeof(SetAngularVelocityItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal,
                    ParameterType.Vector3
                }
            },
            {
                typeof(SetAnimatorValueGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal,
					ParameterType.Bool,
					ParameterType.Float,
					ParameterType.Integer
				}
			},
            {
                typeof(SetFillAmountGimmick),
                new ParameterType[]
                {
                    ParameterType.Float,
                    ParameterType.Integer
                }
            },
            {
                typeof(SetGameObjectActiveGimmick),
                new ParameterType[]
                {
                    ParameterType.Bool
                }
            },
            {
                typeof(SetJumpHeightRatePlayerGimmick),
                new ParameterType[]
                {
                    ParameterType.Float
                }
            },
            {
                typeof(SetMoveSpeedRatePlayerGimmick),
                new ParameterType[]
                {
                    ParameterType.Float
                }
            },
            {
                typeof(SetSliderValueGimmick),
                new ParameterType[]
                {
                    ParameterType.Float,
                    ParameterType.Integer
                }
            },
            {
                typeof(SetTextGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal,
                    ParameterType.Bool,
                    ParameterType.Float,
                    ParameterType.Integer
                }
            },
            {
                typeof(SetVelocityCharacterItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Vector2
                }
            },
            {
                typeof(SetVelocityItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(SetWheelColliderBrakeTorqueItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Bool,
                    ParameterType.Float,
                    ParameterType.Integer,
                }
            },
            {
                typeof(SetWheelColliderMotorTorqueItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Bool,
                    ParameterType.Float,
                    ParameterType.Integer,
                }
            },
            {
                typeof(SetWheelColliderSteerAngleItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Bool,
                    ParameterType.Float,
                    ParameterType.Integer,
                }
            },
            {
                typeof(StopTimelineGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(WarpItemGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
            {
                typeof(WarpPlayerGimmick),
                new ParameterType[]
                {
                    ParameterType.Signal
                }
            },
		};

        private static readonly ParameterType[] TypeOfGimmickToParameterTypesDefault = { ParameterType.Signal };

        private static readonly IDictionary<ComponentType, Type[]> _ComponentTypeToTypes = new Dictionary<ComponentType, Type[]>
        {
            {
                ComponentType.Trigger,
                new Type[]
                {
                    typeof(InitializePlayerTrigger),
                    typeof(InteractItemTrigger),
                    typeof(IsGroundedCharacterItemTrigger),
                    typeof(OnAngularVelocityChangedItemTrigger),
                    typeof(OnCollideItemTrigger),
                    typeof(OnCreateItemTrigger),
                    typeof(OnGetOffItemTrigger),
                    typeof(OnGetOnItemTrigger),
                    typeof(OnGrabItemTrigger),
                    typeof(OnJoinPlayerTrigger),
                    typeof(OnReceiveOwnershipItemTrigger),
                    typeof(OnReleaseItemTrigger),
                    typeof(OnVelocityChangedItemTrigger),
                    typeof(SteerItemTrigger),
                    typeof(UseItemTrigger),
                }
            },
            {
                ComponentType.Gimmick,
                _TypeOfGimmickToParameterTypes.Keys.ToArray()
            },
            {
                ComponentType.Logic,
                new Type[]
                {
                    typeof(GlobalLogic),
                    typeof(ItemLogic),
                    typeof(PlayerLogic),
                }
            },
            {
                ComponentType.Lottery,
                new Type[]
                {
                    typeof(GlobalTriggerLottery),
                    typeof(ItemTriggerLottery),
                    typeof(PlayerTriggerLottery),
                }
            },
            {
                ComponentType.Timer,
                new Type[]
                {
                    typeof(GlobalTimer),
                    typeof(ItemTimer),
                    typeof(PlayerTimer),
                }
            },
        };

        #endregion

        #region Public Static Fields

        public static readonly Type[] AllTypes = _ComponentTypeToTypes.SelectMany(x => x.Value).ToArray();

        #endregion

        #region Public Static Methods

        public static Type[] ToTypes(this ComponentType componentType)
            => _ComponentTypeToTypes.TryGetValue(componentType, out var res) ? res : default;

        public static ComponentType FromType(this Type type)
            => _ComponentTypeToTypes.FirstOrDefault(x => x.Value.Contains(type)).Key;


        public static ParameterType[] TypeOfGimmickToParameterTypes(Type type)
            => _TypeOfGimmickToParameterTypes.TryGetValue(type, out var value) ? value : TypeOfGimmickToParameterTypesDefault;

        #endregion
    }
}
