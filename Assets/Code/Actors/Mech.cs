using UnityEngine;

public class Mech : Player
{
    [SerializeField] private AudioClip mechActivationClip;
    [SerializeField] private AudioClip mechDeactivationClip;
    [SerializeField] private GameObject blurb;
    
    public bool MechIsActive { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        MechAttachPoint.OnMechActivation += SetMechActive;
        MechAttachPoint.OnMechDeactivation += SetMechInActive;

        audioSource = GetComponent<AudioSource>();
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
        
        audioSource.PlayOneShot(mechActivationClip);
    }

    private void SetMechInActive()
    {
        Debug.Log("Mech deactivated!");
        MechIsActive = false;

        blurb.gameObject.SetActive(true);

        audioSource.PlayOneShot(mechDeactivationClip);
    }
}