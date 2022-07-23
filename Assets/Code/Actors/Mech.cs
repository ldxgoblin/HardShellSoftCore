using UnityEngine;

public class Mech : Player
{
    private bool mechIsActive = false;
    public bool MechIsActive { get; }

    protected override void Awake()
    {
        base.Awake();
        
        MechAttachPoint.OnMechActivation += SetMechActive;
        MechAttachPoint.OnMechDeactivation += SetMechInActive;
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
    }
    
    private void SetMechInActive()
    {
        Debug.Log("Mech deactivated!");
        mechIsActive = false;
    }
}