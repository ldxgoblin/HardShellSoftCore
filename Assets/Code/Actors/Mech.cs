using UnityEngine;

public class Mech : Player
{
    private bool mechIsActive = false;
    public bool MechIsActive { get; }

    [SerializeField] private AudioClip mechActivationClip;
    [SerializeField] private AudioClip mechDeactivationClip;

    private AudioSource audioSource;
    
    protected override void Awake()
    {
        base.Awake();
        
        MechAttachPoint.OnMechActivation += SetMechActive;
        MechAttachPoint.OnMechDeactivation += SetMechInActive;
        
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
    }

    protected void OnDestroy()
    {
        MechAttachPoint.OnMechActivation -= SetMechActive;
        MechAttachPoint.OnMechDeactivation -= SetMechInActive;
    }

    private void SetMechActive()
    {
        Debug.Log("Mech activated!");
        mechIsActive = true;
        
        audioSource.PlayOneShot(mechActivationClip);
    }
    
    private void SetMechInActive()
    {
        Debug.Log("Mech deactivated!");
        mechIsActive = false;
        
        audioSource.PlayOneShot(mechDeactivationClip);
    }
}