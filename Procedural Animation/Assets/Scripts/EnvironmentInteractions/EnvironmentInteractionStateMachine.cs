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

    private EnvironmentInteractionContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
    }

    private void Awake()
    {
        ValidateConstraints();

        _context = new EnvironmentInteractionContext(_leftIkConstraint, _rightIkConstraint,
            _leftMultiRotationConstraint, _rightMultiRotationConstraint,
            _rigidbody, _rootCollider, transform.root);

        ConstructEnvironmentDetectionCollider();
        InitializeStates();
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

    private void InitializeStates()
    {
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));
    
        CurrentState = States[EEnvironmentInteractionState.Reset];
    
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        float wingspan = _rootCollider.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_rootCollider.center.x,
            _rootCollider.center.y + (.25f * wingspan),
            _rootCollider.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;

    }

}
