using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] private State nextState;
    public override State RunCurrentState()
    {
        return nextState;
    }
}
