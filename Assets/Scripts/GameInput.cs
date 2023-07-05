using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Unity.XR.OpenVR;

public class GameInput : MonoBehaviour
{
    //pravimo singleton
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;

    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;


    public enum Binding
    {
        Move_Up,
        Move_Down, 
        Move_Left, 
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        GamePad_Interact,
        GamePad_InteractAlternate,
        GamePad_Pause
    }
    private void Awake()
    {
        Instance = this;
        playerInputActions =  new PlayerInputActions();


        //kad pokrenem novu partiju ocu da mi ostanu sacuvane prethodno izmenjene komande
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            string st = PlayerPrefs.GetString("InputBindings");
            playerInputActions.LoadBindingOverridesFromJson(st);
        }

        playerInputActions.Player.Enable();


        playerInputActions.Player.Interact.performed += Interact_performed;

        //za secenje
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;

        Debug.Log(GetBindingText(Binding.Interact));

       

    }

    private void OnDestroy()
    {
        //OnDestroy funkcija se koristi pri menjanju scena u unityu, ako ovo ne odradimo i ako sa igranja pauziramo odemo na main menu pa na play pa opet pauziramo
        //dobicemo missingreferenceexception jer se stari playerinput actions nije izbrisao i stari listeneri idalje rade i zele da nam pokazu pause ekran od prosle igrice koja je unistena
        //zato moramo da pri unistavanju odbijemo listenere i objekat disposujemo

        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        OnPauseAction?.Invoke(this, EventArgs.Empty);

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

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact:
                //[0] jer je to za tastaturu a posle dodajemo kao index 1 za dzojstik, tostring bi nam ovde ispisao Keyboard e a mi ocemo samo slovo e, pa koristimoo
                //todisplaystring fju
               return playerInputActions.Player.Interact.bindings[0].ToDisplayString(); 
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Move_Up:
                //ovi gore su imali po jednu komandu a za move imamo vise, move je 0, moveup je 1 movedown 2 itd.....
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();

           //za gamepad
            case Binding.GamePad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamePad_InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.GamePad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }


    //sistemski delegat action tipa void
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        //da bi promenili komande moramo da disablujemo action map
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;
        switch (binding) {
            default:
            case Binding.Move_Up:
            inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;

                //za gamepad
            case Binding.GamePad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamePad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;

        }

        //oncomplete je listener, callback je ulazni parametar, jer je 1 ulazni ne trebaju nam zagrade
       inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            Debug.Log(callback.action.bindings[bindingIndex].path); //ovo ce biti prethodna komanda za moveup W
            Debug.Log(callback.action.bindings[bindingIndex].overridePath); //ovo ce biti nova komanda koju cu ja da postavim
            callback.Dispose();
            playerInputActions.Player.Enable();

            //pokrecemo delegat
            onActionRebound();


            //ovaj deo je za cuvanje novih komandi nakon sto se igrica opet pokrene
           string st = playerInputActions.SaveBindingOverridesAsJson();//cuva binding kao string u json formatu
            PlayerPrefs.SetString("InputBindings", st);
            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);

        }).Start();
    }
}
