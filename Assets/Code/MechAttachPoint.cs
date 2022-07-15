using System.Collections;
using UnityEngine;

public class MechAttachPoint : MonoBehaviour
{
    [SerializeField] private float ejectionForce = 10f;
    [SerializeField] [Range(1f, 5f)] private float postEjectionCooldown = 1f;

    private CircleCollider2D circleCollider2D;

    private GameObject currentRider;
    private InputHandler mechInputHandler;

    private bool mechIsOccupied;
    private InputHandler playerInputHandler;
    private Rigidbody2D riderRigidbody2D;

    private void Awake()
    {
        mechInputHandler = transform.parent.gameObject.GetComponent<InputHandler>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (!mechInputHandler.IsInputActive()) return;

        if (mechInputHandler.InputSource.GetExitInput() && mechIsOccupied) ExitMech();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            currentRider = col.gameObject;
            playerInputHandler = currentRider.GetComponent<InputHandler>();
            riderRigidbody2D = currentRider.GetComponent<Rigidbody2D>();

            currentRider.transform.parent = transform;
            transform.localScale = currentRider.transform.localScale;

            // Infinite Dash Fix
            currentRider.GetComponent<Dash>().StopDashInstantly();

            EnterMech(playerInputHandler);
        }
    }

    private void EnterMech(InputHandler origin)
    {
        // Sets the mech parent layer to Player, so enemy Ai can detect it
        transform.parent.gameObject.layer = 6;

        currentRider.SetActive(false);

        mechInputHandler.SwapInputSource(origin);
        mechIsOccupied = true;
    }

    private void ExitMech()
    {
        // Sets the mech parent layer to Default, so enemy Ai can not detect it
        transform.parent.gameObject.layer = 0;

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