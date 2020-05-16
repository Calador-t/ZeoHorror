using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

class PlayerInput : MonoBehaviour, Controls.ICharacterControllerActions
{
	private Controls controls;

	public Vector2 MovementInput;

	public void OnMovement( InputAction.CallbackContext context ) => MovementInput = context.ReadValue<Vector2>();

	private void Awake()
	{
		controls = new Controls();
		controls.CharacterController.SetCallbacks( this );
	}
	private void OnEnable()
	{
		controls.Enable();
	}
	private void OnDisable()
	{
		controls.Disable();
	}
}

