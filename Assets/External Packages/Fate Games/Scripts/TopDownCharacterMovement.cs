using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Swerve))]
    public class TopDownCharacterMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 4;
        [SerializeField] private float rotationSpeed = 4;
        [SerializeField] private Animator animator;
        private Rigidbody rb;
        private Swerve swerve;
        private Transform _transform;
        private Vector3 previousPosition;
        private Transform mainCameraTransform;

        private float velocity { get => Vector3.Distance(_transform.position, previousPosition) / Time.deltaTime; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            swerve = GetComponent<Swerve>();
            mainCameraTransform = Camera.main.transform;
            _transform = transform;
            SetPreviousPosition();
            InitializeRigidbody();
        }

        private void InitializeRigidbody()
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.constraints = RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;
        }

        private void SetAnimatorVelocity()
        {
            if (animator)
                animator.SetFloat("Velocity", velocity);
        }

        private void SetPreviousPosition()
        {
            previousPosition = _transform.position;
        }

        private void SetVelocityToZero()
        {
            rb.velocity = Vector3.zero;
        }

        public void Move()
        {
            Vector3 direction = new Vector3(swerve.Difference.x, 0, swerve.Difference.y);
            direction = Quaternion.Euler(0, mainCameraTransform.rotation.eulerAngles.y, 0) * direction;
            float sqrMagnitude = direction.sqrMagnitude;
            if (sqrMagnitude > 0.1f)
            {
                rb.MoveRotation(Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed));
                rb.MovePosition(_transform.position + direction * Time.fixedDeltaTime * swerve.Rate * speed);
            }
        }

        private void FixedUpdate()
        {
            Move();
            SetAnimatorVelocity();
            SetPreviousPosition();
            SetVelocityToZero();
        }
    }


}
