using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : EnvironmentInteractionState {

    float _elapsedTime = 0.0f;
    float _resetDuration = 2.0f;
    float _lerpDuration = 10.0f;
    float _rotationSpeed = 500f;

    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }
    public override void EnterState()
    {
        _elapsedTime = 0.0f;
        Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        Context.CurrentIntersectingCollider = null;
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset,
            Context.ColliderCenterY,
            _elapsedTime / _lerpDuration);
        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight,
            0, _elapsedTime / _lerpDuration);
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight,
            0, _elapsedTime / _lerpDuration);
        Context.CurrentIkTargetTransform.localPosition = Vector3.Lerp(Context.CurrentIkTargetTransform.localPosition,
            Context.CurrentOriginalTargetPosition,
            _elapsedTime / _lerpDuration);
        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation,
            Context.OriginalTargetRotation,
            _rotationSpeed * Time.deltaTime);
    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isMoving = Context.Rigidbody.velocity != Vector3.zero;
        if (_elapsedTime >= _resetDuration && isMoving)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerStay(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
        ResetIkTargetPositionTracking(other);
    }

}
