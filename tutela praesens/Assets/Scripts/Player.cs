using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    protected override void Start()
    {
        base.Start();
        alignment = GameManager.Identifiers.Player;
    }
    public override void Attack()
    {
        //Debug.Log("Hyahhhh!");
        Vector2 direction = RandomNormalizedVector();
        Vector2 offset = direction * m_offsetDistanceMultiplier;
        var attack = Instantiate(TargetPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        attack.GetComponent<Target>().Init(GameManager.States.Attack, alignment, direction);
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead && !GameManager.instance.GameOver)
        {

            GameManager.instance.GameOver = true;
        }
    }
}
