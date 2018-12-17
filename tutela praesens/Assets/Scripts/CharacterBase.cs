using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public GameObject TargetPrefab;

    protected GameManager.Identifiers alignment;

    [SerializeField]
    protected int m_maxHealth = 3;
    public int MaxHealth { get { return m_maxHealth; } }
    protected int m_currentHealth;
    public int CurrentHealth { get { return m_currentHealth; } set { m_currentHealth = value; } }
    public bool IsDead { get { return m_currentHealth <= 0; } }
    protected bool canAttack = false;

    [SerializeField]
    protected float m_offsetDistanceMultiplier = 5;

    [SerializeField, Range(0, 100)]
    protected float m_counterChance;
    protected float m_normalizedCounterChance { get { return m_counterChance / 100.0f; } }

    [SerializeField]
    protected float m_attackTimerMin = 2;
    [SerializeField]
    protected float m_attackTimerMax = 4;
    protected float m_attackTimer
    { get
        {
            return Random.Range(m_attackTimerMin - GameManager.instance.Level / 2.0f,
                                m_attackTimerMax - GameManager.instance.Level / 2.0f);
        }
    }
    protected float m_timePassed;

    //depreciated
    [System.Obsolete("No longer supported")]
    protected Vector2 m_facingDirection = Vector2.right;

    protected Vector2 RandomNormalizedVector()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        float length = true ? Mathf.Abs(x) + Mathf.Abs(y) : 1;
        Vector2 temp = new Vector2(x / length, y / length);

        return temp;
    }

    public abstract void Attack();

    protected void Begin()
    {
        canAttack = true;
        Debug.Log("Come at me!");
    }

    public void ResetCharacter()
    {
        m_timePassed = 0;
    }

    protected void End()
    {
        canAttack = false;
    }

    protected virtual void Start()
    {
        GameManager.instance.RoundStart.AddListener(Begin);
        GameManager.instance.RoundEnd.AddListener(End);
        m_currentHealth = m_maxHealth;
    }

    protected virtual void Update()
    {
        if (canAttack)
        {
            m_timePassed += Time.deltaTime;
            if (m_timePassed >= m_attackTimer)
            {
                //Debug.Log("Hrrgh");
                Attack();
                m_timePassed = 0;
            }
        }
    }
}
