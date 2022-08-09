using UnityEngine;

public class Mech : Player
{
    [SerializeField] private AudioClip mechActivationClip;
    [SerializeField] private AudioClip mechDeactivationClip;
    [SerializeField] private GameObject blurb;

    private Move moveScript;
    public bool MechIsActive { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        moveScript = GetComponent<Move>();
        
        MechAttachPoint.OnMechActivation += SetMechActive;
        MechAttachPoint.OnMechDeactivation += SetMechInActive;

        audioSource = GetComponent<AudioSource>();
        
        SetMechInActive();
    }

    protected void OnDestroy()
    {
        MechAttachPoint.OnMechActivation -= SetMechActive;
        MechAttachPoint.OnMechDeactivation -= SetMechInActive;
    }

    private void SetMechActive()
    {
        Debug.Log("Mech activated!");
        MechIsActive = true;
        blurb.gameObject.SetActive(false);

        rigidbody2D.mass = 5;

        moveScript.enabled = true;
        
        audioSource.PlayOneShot(mechActivationClip);
    }

    private void SetMechInActive()
    {
        Debug.Log("Mech deactivated!");
        MechIsActive = false;
        blurb.gameObject.SetActive(true);

        rigidbody2D.mass = 10000;
        moveScript.enabled = false;

        audioSource.PlayOneShot(mechDeactivationClip);
    }
}