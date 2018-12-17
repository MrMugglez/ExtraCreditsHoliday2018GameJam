using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    protected override void Start()
    {
        base.Start();
        alignment = GameManager.Identifiers.Enemy;
    }

    public override void Attack()
    {
        Debug.Log("I'm gunna getcha!");
    }
}
