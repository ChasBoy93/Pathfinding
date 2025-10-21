using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.Rendering.LookDev;
using Unity.Android.Gradle.Manifest;
using UnityEngine.Events;

namespace Controller
{
    [RequireComponent(typeof (CharacterController))]
    public class FPController : MonoBehaviour
    {
        [Header("Movment Parameters")]
        public float maxSpeed => sprintInput ? sprintSpeed : walkSpeed;
        public float acceleration = 20f;

        [SerializeField] float walkSpeed = 3.5f;
        [SerializeField] float sprintSpeed = 8f;

        [SerializeField] float jumpHeight = 2f;
        int timesJumped = 0;

        [SerializeField] bool CanBoubleJump = true;

        public bool Sprinting
        {
            get
            {
                return sprintInput && currentSpeed > 0.1f;
            }
        }

        [Header("Looking Parameters")]
        public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
        public float pitchLimit = 85f;
        [SerializeField] float currentPitch = 0f;

        public float CurrentPitch
        {
            get => currentPitch;

            set
            {
                currentPitch = Mathf.Clamp(value, -pitchLimit, pitchLimit);
            }
        }

        [Header("Camera Parameters")]
        [SerializeField] float cameraNormalFOV = 60f;
        [SerializeField] float cameraSprintFOV = 70f;
        [SerializeField] float cameraFOVSmothing = 1f;

        float TargetCameraFOV
        {
            get
            {
                return Sprinting ? cameraSprintFOV : cameraNormalFOV;
            }
        }

        [Header("Physics Parameters")]
        [SerializeField] float GravitySacle = 3f;

        public float vertialVelocity = 0f;
        public Vector3 currentVelocity { get; private set; }
        public float currentSpeed { get; private set; }

        bool wasGrounded = false;
        public bool IsGrounded => characterController.isGrounded;

        [Header("Input")]
        public Vector2 moveInput;
        public Vector2 lookInput;
        public bool sprintInput;

        [Header("Components")]
        public CinemachineCamera fpCamera;
        public CharacterController characterController;

        [Header("Events")]
        public UnityEvent landed;

        #region Unity Methods

        void OnValidate()
        {
            if(characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
        }

        void Update()
        {
            MoveUpdate();  
            LookUpdate();
            CameraUpdate();

            if(!wasGrounded && IsGrounded)
            {
                timesJumped = 0;
                landed?.Invoke();
            }
            wasGrounded = IsGrounded;
        }
        #endregion

        #region Controller Methods

        public void TryJump()
        {
            if(IsGrounded == false)
            {
                if(CanBoubleJump == false || timesJumped >= 2)
                {
                    return;
                }

            }

            vertialVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * GravitySacle);

            timesJumped++;
        }

        void MoveUpdate()
        {
            Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
            motion.y = 0f;
            motion.Normalize();

            if(motion.sqrMagnitude >= 0.01f)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, motion * maxSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, acceleration * Time.deltaTime);
            }

            if(IsGrounded && vertialVelocity <= 0.01f)
            {
                vertialVelocity = -3f;
            }
            else
            {
                vertialVelocity += Physics.gravity.y * GravitySacle * Time.deltaTime;
            }


            Vector3 fullVelocity = new Vector3(currentVelocity.x, vertialVelocity, currentVelocity.z);

            CollisionFlags flags =  characterController.Move(fullVelocity * Time.deltaTime);

            if((flags & CollisionFlags.Above) != 0 && vertialVelocity > 0.01f)
            {
                vertialVelocity = 0f;
            }

            currentSpeed = currentVelocity.magnitude;
        }

        void LookUpdate()
        {
            Vector2 input = new Vector2(lookInput.x * lookSensitivity.x, lookInput.y * lookSensitivity.y);
            // Looking up and down
            currentPitch -= input.y;

            fpCamera.transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);

            // Looking left and right
            transform.Rotate(Vector3.up * input.x);
        }

        void CameraUpdate()
        {
            float targetFOV = cameraNormalFOV;

            if(Sprinting)
            {
                float speedRatio = currentSpeed / sprintSpeed;

                targetFOV = Mathf.Lerp(cameraNormalFOV, cameraSprintFOV, speedRatio);
            }

            fpCamera.Lens.FieldOfView = Mathf.Lerp(fpCamera.Lens.FieldOfView, targetFOV, cameraFOVSmothing * Time.deltaTime);
        }

        #endregion
    }
}
