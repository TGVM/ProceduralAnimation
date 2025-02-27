using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext : MonoBehaviour
{
    public enum EBodySide
    {
        RIGHT,
        LEFT
    };

    private TwoBoneIKConstraint _leftIkConstraint;
    private TwoBoneIKConstraint _rightIkConstraint;
    private MultiRotationConstraint _leftMultiRotationConstraint;
    private MultiRotationConstraint _rightMultiRotationConstraint;
    private Rigidbody _rigidbody;
    private CapsuleCollider _rootCollider;
    private Transform _rootTransform;
    private Vector3 _leftOriginalTargetPosition;
    private Vector3 _rightOriginalTargetPosition;

    //constructor
    public EnvironmentInteractionContext(TwoBoneIKConstraint leftIkConstraint, TwoBoneIKConstraint rightIkConstraint, MultiRotationConstraint leftMultiRotationConstraint, MultiRotationConstraint rightMultiRotationConstraint, Rigidbody rigidbody, CapsuleCollider rootCollider, Transform rootTransform)
    {
        _leftIkConstraint = leftIkConstraint;
        _rightIkConstraint = rightIkConstraint;
        _leftMultiRotationConstraint = leftMultiRotationConstraint;
        _rightMultiRotationConstraint = rightMultiRotationConstraint;
        _rigidbody = rigidbody;
        _rootCollider = rootCollider;
        _rootTransform = rootTransform;
        _leftOriginalTargetPosition = leftIkConstraint.data.target.transform.localPosition;
        _rightOriginalTargetPosition = rightIkConstraint.data.target.transform.localPosition;
        OriginalTargetRotation = _leftIkConstraint.data.target.rotation;

        CharacterShoulderHeight = leftIkConstraint.data.root.transform.position.y;
        SetCurrentSide(Vector3.positiveInfinity);
    }

    //Read-only properties
    public TwoBoneIKConstraint LeftIkConstraint => _leftIkConstraint;
    public TwoBoneIKConstraint RightIkConstraint => _rightIkConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint => _leftMultiRotationConstraint;
    public MultiRotationConstraint RightMultiRotationConstraint => _rightMultiRotationConstraint;
    public Rigidbody Rigidbody => _rigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
    public Transform RootTransform => _rootTransform;

    public float CharacterShoulderHeight { get; private set; }

    public Collider CurrentIntersectingCollider { get; set; }
    public TwoBoneIKConstraint CurrentIkConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentIkTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float InteractionPointYOffset { get; set; } = 0;
    public float ColliderCenterY { get; set; }
    public Vector3 CurrentOriginalTargetPosition { get; private set; }
    public Quaternion OriginalTargetRotation { get; private set; }
    public float LowestDistance { get; set; } = Mathf.Infinity;

    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3 leftShoulder = _leftIkConstraint.data.root.transform.position;
        Vector3 rightShoulder = _rightIkConstraint.data.root.transform.position;

        bool isLeftCloser = Vector3.Distance(positionToCheck, leftShoulder) <
            Vector3.Distance(positionToCheck, rightShoulder);

        if (isLeftCloser)
        {
            CurrentBodySide = EBodySide.LEFT;
            CurrentIkConstraint = _leftIkConstraint;
            CurrentMultiRotationConstraint = _leftMultiRotationConstraint;
            CurrentOriginalTargetPosition = _leftOriginalTargetPosition;
        }
        else
        {

            CurrentBodySide = EBodySide.RIGHT;
            CurrentIkConstraint = _rightIkConstraint;
            CurrentMultiRotationConstraint = _rightMultiRotationConstraint;
            CurrentOriginalTargetPosition = _rightOriginalTargetPosition;
        }

        CurrentShoulderTransform = CurrentIkConstraint.data.root.transform;
        CurrentIkTargetTransform = CurrentIkConstraint.data.target.transform;

    }
}
