using System.Collections;
using UnityEngine;

public class MechAttachPoint : MonoBehaviour
{
    private InputHandler mechInputHandler;
    private InputHandler playerInputHandler;

    private CircleCollider2D circleCollider2D;
    
    private GameObject currentRider = null;
    private Rigidbody2D riderRigidbody2D;

    [SerializeField] private float ejectionForce = 10f;
    [SerializeField, Range(1f, 5f)] private float postEjectionCooldown = 1f;
    
    private bool mechIsOccupied = false;

    private void Awake()
    {
        mechInputHandler = transform.parent.gameObject.GetComponent<InputHandler>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    
    private void Update()
    {
        if(!mechInputHandler.IsInputActive()) return;

        Debug.Log(mechInputHandler.InputSource);
        
        if (mechInputHandler.InputSource.GetExitInput() && mechIsOccupied)
        {
            ExitMech();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("yooooooo");
        
        if(col.CompareTag("Player"))
        {
            currentRider = col.gameObject;
            playerInputHandler = currentRider.GetComponent<InputHandler>();
            riderRigidbody2D = currentRider.GetComponent<Rigidbody2D>();
            
            currentRider.transform.parent = transform;
            transform.localScale = currentRider.transform.localScale;

            EnterMech(playerInputHandler);
        }
    }

    private void EnterMech(InputHandler origin)
    {
        currentRider.SetActive(false);
        
        mechInputHandler.SwapInputSource(origin);
        mechIsOccupied = true;
    }

    private void ExitMech()
    {
        playerInputHandler.SwapInputSource(mechInputHandler);
        mechIsOccupied = false;
        EjectRider();
    }

    private void EjectRider()
    {
        currentRider.SetActive(true);

        StartCoroutine(CoolDown());
        
        currentRider.transform.parent = null;
        currentRider = null;  
    }

    private IEnumerator CoolDown()
    {
        circleCollider2D.enabled = false;
        yield return new WaitForSeconds(postEjectionCooldown);
        circleCollider2D.enabled = true;
    }
}