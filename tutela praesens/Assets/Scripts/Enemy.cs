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
        Vector2 direction = RandomNormalizedVector();
        Vector2 offset = direction * m_offsetDistanceMultiplier;
        var attack = Instantiate(TargetPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        attack.GetComponent<Target>().Init(GameManager.States.Attack, alignment, -direction, GameManager.instance.transform.position);
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead)
        {
            GameManager.instance.Score += 1000 * GameManager.instance.Level;
            GameManager.instance.NextFight();
            GameManager.instance.Level++;
            Destroy(gameObject);
        }
    }
    public void CanAttack(bool attack)
    {
        canAttack = true;
    }
}
