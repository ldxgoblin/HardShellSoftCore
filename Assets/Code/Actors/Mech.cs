using System;
using UnityEngine;
using UnityEngine.UI;

public class Mech : Player
{
    [SerializeField] private AudioClip mechActivationClip;
    [SerializeField] private AudioClip mechDeactivationClip;
    [SerializeField] private GameObject blurb;

    [SerializeField] private GameObject chargeMeter;
    private Transform chargeMeterTransform;
    [SerializeField] private Vector3 chargeMeterOffset;

    private Move move;
    
    [SerializeField] private Sprite mechOccupiedSprite;
    private Sprite mechUnoccupiedSprite;
    
    public bool MechIsActive { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        mechUnoccupiedSprite = spriteRenderer.sprite;
        
        move = GetComponent<Move>();

        chargeMeterTransform = chargeMeter.transform;
        
        MechAttachPoint.OnMechActivation += SetMechActive;
        MechAttachPoint.OnMechDeactivation += SetMechInActive;

        audioSource = GetComponent<AudioSource>();
        
        MechIsActive = false;
        blurb.gameObject.SetActive(true);

        rigidbody2D.mass = 10000;
        move.enabled = false;
    }

    protected void OnDestroy()
    {
        MechAttachPoint.OnMechActivation -= SetMechActive;
        MechAttachPoint.OnMechDeactivation -= SetMechInActive;
    }

    private void Update()
    {
        chargeMeterTransform.position = transform.position + chargeMeterOffset;
    }

    private void SetMechActive()
    {
        MechIsActive = true;
        blurb.gameObject.SetActive(false);

        chargeMeter.SetActive(true);
        
        spriteRenderer.sprite = mechOccupiedSprite;
        
        rigidbody2D.mass = 5;

        move.enabled = true;
        
        audioSource.PlayOneShot(mechActivationClip);
    }

    private void SetMechInActive()
    {
        MechIsActive = false;
        blurb.gameObject.SetActive(true);
        
        chargeMeter.SetActive(false);

        spriteRenderer.sprite = mechUnoccupiedSprite;
        
        rigidbody2D.mass = 10000;
        move.enabled = false;

        audioSource.PlayOneShot(mechDeactivationClip);
    }
}