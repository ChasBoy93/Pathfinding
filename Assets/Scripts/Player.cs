using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Controller
{
    [RequireComponent(typeof(FPController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FPController fpController;

        #region Input Handling

        void OnMove(InputValue value)
        {
            fpController.moveInput = value.Get<Vector2>();
        }

        void OnLook(InputValue value)
        {
            fpController.lookInput = value.Get<Vector2>();
        }

        void OnSprint(InputValue value)
        {
            fpController.sprintInput = value.isPressed;
        }

        void OnJump(InputValue value)
        {
            if(value.isPressed)
            {
                fpController.TryJump();
            }
        }
        #endregion

        #region Unity Methods
        void OnValidate()
        {
            if(fpController == null) fpController = GetComponent<FPController>();
        }

        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion
    }
}
