using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float brzina = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool hoda;
    private Vector3 zadnjiVektorZaInterakciju ;
    private BaseCounter selectedCounter;
    private KitchenObj kitchenObj; 


    //setujemo instance
    private void Awake()
    {
        if (Instance != null){
            //ako nije null to znaci da je vec setovano na nesto, a to je greska, ne mozemo imati vise instanci one su singleton
            Debug.LogError("Doslo je do greske");
        }
        
            
        
        Instance = this;
    }

    private void Start()
    {
        //dodajem event listenera pomocu += za event iz GameInput klase
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);


        }

    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {

       if(selectedCounter != null)
        {
            selectedCounter.Interact(this);

           
        }


    }

    private void Update()
    {
        FunkcijaZaKretanje();
        FunkcijaZaInterakcije();


    }
    public bool DaLiHoda()
    {
        return hoda;
    }
    private void FunkcijaZaInterakcije()
    {
        

        float duzinaZaInterakciju = 2f;
        Vector2 pocetniVektor = gameInput.GetMovementVectorNormalized();
        Vector3 smerKretanja = new Vector3(pocetniVektor.x, 0f, pocetniVektor.y);

        //ovo radim jer kad prestanem da drzim W i idem u objekat onda prestane da ga detektuje a ja ocu i tad kad sam okrenut da ga ispisuje pa pamtim stari ssmerKretanja

        if (smerKretanja != Vector3.zero)
        {
            zadnjiVektorZaInterakciju = smerKretanja;
        }
        
        
        if (Physics.Raycast(transform.position, zadnjiVektorZaInterakciju, out RaycastHit raycastHit, duzinaZaInterakciju,countersLayerMask)){
            // ova fja vraca boolean (true ako udarimo nesto) ali vraca podatke o sudaru u varijablu raycastHit jer je to out tip
          //  Debug.Log(raycastHit.transform);
            //raycastHit.transform vraca objekat koji je udaren

            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) // ovde je bio clearcounter, al onda nece prepoznati ostale countere, pa smo napravili base counter kog nasledjuju svi counteri
            {
                //to je clearcounter ako ovo u ifu vrati true
             //   clearCounter.Interact();

                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);

                }
                else
                {
                    //ako je locirano nesto ali pak to nesto nije clearcounter onda takodje treba da deselektujemo selectedcounter
                    //  selectedCounter = null;
                    SetSelectedCounter(null);

                }

            }
            else
            {
                //ako raycast nije locirao nista onda ispred nas nema nista tako da cemo postaviti selektovani kredenac da je null
                //  selectedCounter = null;
                //ispaljujemo event
                SetSelectedCounter(null);

            }
        }

    }

    private void FunkcijaZaKretanje()
    {

        Vector2 pocetniVektor = gameInput.GetMovementVectorNormalized();
        Vector3 smerKretanja = new Vector3(pocetniVektor.x, 0f, pocetniVektor.y);

        float duzinaKretanja = brzina * Time.deltaTime;
        float velicinaIgraca = .7f;
        float visinaIgraca = 1.5f;
        // bool daLiMozePomeranje = !Physics.Raycast(transform.position, smerKretanja, velicinaIgraca);
        bool daLiMozePomeranje = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * visinaIgraca, velicinaIgraca, smerKretanja, duzinaKretanja);
        // bolji je capsule cast jer sprecava sudaranja i po ivicama naseg igraca, a raycast baca laser iz centra naseg igraca vizuelno i moze da se desi sudar

        // ovo pravimo da ukoliko drzimo dijagonalno kretanje a on udari u zid da se automatski krecemo levo ili desno
        if (!daLiMozePomeranje)
        {
            // ne mozemo da se krecemo po smeruKretanja

            //probamo po X osi kretanje
            Vector3 smerX = new Vector3(smerKretanja.x, 0, 0).normalized; // normalizujemo ga jer ce ovakvo kretanje biti sporije od kretanja kada idemo samo levo ili desno
            daLiMozePomeranje = smerKretanja.x !=0 &&!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * visinaIgraca, velicinaIgraca, smerX, duzinaKretanja);
            //smerKretanja.x!=0 provera da se proveri da li uopste pokusavamo da se pomerimo
            if (daLiMozePomeranje)
            {
                //mozemo da se krecemo po x osi
                smerKretanja = smerX;
            }
            else
            {
                //ne mozemo da se pomeramo po X osi, probajmo po Z
                Vector3 smerZ = new Vector3(0, 0, smerKretanja.z).normalized;
                daLiMozePomeranje =smerKretanja.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * visinaIgraca, velicinaIgraca, smerZ, duzinaKretanja);
                if (daLiMozePomeranje)
                {
                    //mozemo da se krecemo po z osi
                    smerKretanja = smerZ;

                }
                // ako ovo ne moze onda ne mozemo ni u jednom pravcu da se krecemo
            }



        }


        if (daLiMozePomeranje)
        {
            transform.position += smerKretanja * brzina * Time.deltaTime;

        }

        hoda = smerKretanja != Vector3.zero;

        float brzinaRotiranja = 10f;
        transform.forward = Vector3.Slerp(transform.forward, (-1) * smerKretanja, Time.deltaTime * brzinaRotiranja);
        // -1 jer je isao suprotno
        //Debug.Log(smerKretanja);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        //ispaljujemo event
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter



        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObj(KitchenObj kitchenObj)
    {
        this.kitchenObj = kitchenObj;
    }

    public KitchenObj GetKitchenObj()
    {
        return kitchenObj;
    }

    public void ClearKitchenObj()
    {
        kitchenObj = null;
    }

    public bool HasKitchenObj()
    {
        return (kitchenObj != null);
    }
}
