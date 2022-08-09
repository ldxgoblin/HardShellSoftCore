using UnityEngine;

public class Mech : Player
{
    [SerializeField] private AudioClip mechActivationClip;
    [SerializeField] private AudioClip mechDeactivationClip;
    [SerializeField] private GameObject blurb;

    private Move move;
    public bool MechIsActive { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        move = GetComponent<Move>();
        
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

        move.enabled = true;
        
        audioSource.PlayOneShot(mechActivationClip);
    }

    private void SetMechInActive()
    {
        Debug.Log("Mech deactivated!");
        MechIsActive = false;
        blurb.gameObject.SetActive(true);

        rigidbody2D.mass = 10000;
        move.enabled = false;

        audioSource.PlayOneShot(mechDeactivationClip);
    }
}