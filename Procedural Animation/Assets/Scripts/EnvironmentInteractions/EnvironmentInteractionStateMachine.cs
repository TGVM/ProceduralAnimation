using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Assertions;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset,
    }

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void Awake()
    {
        ValidateConstraints();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left Ik Constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right Ik Constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left Multi-rotation Constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right Multi-rotation Constraint is not assigned.");
        Assert.IsNotNull(_rigidbody, "RigidBody is not assigned.");
        Assert.IsNotNull(_rootCollider, "Root collider is not assigned.");

    }

}
