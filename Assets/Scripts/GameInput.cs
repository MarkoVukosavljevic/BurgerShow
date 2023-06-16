using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;

    public event EventHandler OnInteractAlternateAction;

    private void Awake()
    {
        playerInputActions =  new PlayerInputActions();
        playerInputActions.Player.Enable();


        playerInputActions.Player.Interact.performed += Interact_performed;

        //za secenje
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;

    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //ispaljujemo event
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);

    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        
        
            // ? operator proverava da li je ovo levo null ako jeste nece izvrsiti ovo desno i nece doci do NPException, 
            // .Invoke poziva opet  OnInteractAction
            OnInteractAction?.Invoke(this, EventArgs.Empty);
            
                                  
        
    }

    public Vector2 GetMovementVectorNormalized()
    {
        //prvo bitno je ovako bilo pre nego sto smo ubacili InputSystem sa Package Managera, on je alternativa za sve ovo
        //Vector2 pocetniVektor = new Vector2(0, 0);

        Vector2 pocetniVektor = playerInputActions.Player.Move.ReadValue<Vector2>();
        // move je Action, a Player je action map u okviru playerinputactionsa

        //if (Input.GetKey(KeyCode.W))
        //{
        //    pocetniVektor.y = +1;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    pocetniVektor.x = -1;

        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    pocetniVektor.y = -1;

        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    pocetniVektor.x = +1;

        //}
        pocetniVektor = pocetniVektor.normalized;
       // Debug.Log(pocetniVektor);

        return pocetniVektor;
    }
}
