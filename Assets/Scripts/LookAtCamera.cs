using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }
    [SerializeField] private Mode mode;
    //ovo se izvrsava posle obicnog update
    private void LateUpdate()
    {
        switch (mode) {
     case Mode.LookAt:
            transform.LookAt(Camera.main.transform);
                //ova funkcija menja ovaj transform da gleda u drugi transofrm
                //sluzi kako bi barovi za secenje gledali u kameru direktno a ne bili suprotnih smerova
                break;
                case Mode.LookAtInverted:
                Vector3 smerOdKamere = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position+smerOdKamere);
                //ovako gledamo skroz suprotno od kamere
                break;

                //ova dva sluze za to da nam ne budu nakrivo barovi
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
                case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;

                break;

    }
    }
}
