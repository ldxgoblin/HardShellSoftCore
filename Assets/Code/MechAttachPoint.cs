using System;
using System.Collections;
using UnityEngine;

public class MechAttachPoint : MonoBehaviour
{
    private InputHandler mechInputHandler;
    private InputHandler playerInputHandler;

    private CircleCollider2D circleCollider2D;
    private float cooldown = 1f;
    
    private GameObject currentRider = null;
    
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
            
            currentRider.transform.parent = transform;
            currentRider.SetActive(false);
            
            EnterMech(playerInputHandler);
        }
    }

    private void EnterMech(InputHandler origin)
    {
        mechInputHandler.SwapInputSource(origin);
        mechIsOccupied = true;
    }

    private void ExitMech()
    {
        playerInputHandler.SwapInputSource(mechInputHandler);
        ReleaseRider();
        mechIsOccupied = false;
    }

    private void ReleaseRider()
    {
        currentRider.transform.parent = null;
        currentRider.SetActive(true);
        
        StartCoroutine(CoolDown());
        
        currentRider = null;  
    }

    private IEnumerator CoolDown()
    {
        circleCollider2D.enabled = false;
        yield return new WaitForSeconds(cooldown);
        circleCollider2D.enabled = true;
    }
}