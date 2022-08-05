using System;
using System.Collections.Generic;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    [SerializeField] private int hitCounter;

    [SerializeField] private float comboTimeWindow = 5;
    [SerializeField] private float currentComboTime;
    
    private Dictionary<int, string> comboMessages;
    
    private bool comboInProgress;
    public static event Action<int> OnCombo;
    public static event Action<string> OnComboMessage; 
    public static event Action OnComboEnded;

    private void Awake()
    {
        comboMessages = new Dictionary<int, string>
        {
            {10, "NICE"}, {19, "GOOD"}, {28, "GREAT!"}, {36, "STRONG!!"}, {44, "AWESOME!"}, {51, "SUPER!!"},
            {58, "HARDCORE!!"}, {64, "WHAT THE...?!"}, {70, "UNREAL..."}, {75, "EXTERMINATOR!"}, {80, "KILLER!!"}, 
            {84, "ENFORCER!!"}, {88, "BULLET HELL!!!"}, {91, "DEVIL!!"}, {94, "ACTUAL HELL!"}, {96, "REAL SATAN!"},
            {98, "ARRRRRGGGGHHHH!!!!"},{100, "DOOM BALL!!!!"}
        };
        
        Dash.OnDashHit += RegisterHit;
        PlayerProjectile.OnPlayerProjectileHit += RegisterHit;

        ResetHitCounter();
    }

    private void Update()
    {
        if (comboInProgress)
        {
            if (currentComboTime > 0)
            {
                currentComboTime -= Time.deltaTime;
            }
            else
            {
                comboInProgress = false;
                OnComboEnded?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        Dash.OnDashHit -= RegisterHit;
        PlayerProjectile.OnPlayerProjectileHit -= RegisterHit;
    }
    
    private void RegisterHit()
    {
        if (currentComboTime <= 0) ResetHitCounter();

        hitCounter++;
        
        if (hitCounter <= 1) return;
        OnCombo?.Invoke(hitCounter);
            
        if(comboMessages.ContainsKey(hitCounter))
        {
            string message = comboMessages[hitCounter];
            OnComboMessage?.Invoke(message);
        }
            
        comboInProgress = true;
    }
    
    private void ResetHitCounter()
    {
        comboInProgress = false;
        hitCounter = 0;
        currentComboTime = comboTimeWindow;
    }
}