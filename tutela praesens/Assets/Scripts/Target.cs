using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Sprite feedBackImage { get { return Resources.Load<Sprite>("Sprites/Target_" + inputType); } }
    [SerializeField]
    private SpriteRenderer m_bgImage;
    [SerializeField]
    private Color m_playerColor = Color.blue;
    [SerializeField]
    private Color m_enemyColor = Color.red;

    [SerializeField]
    private GameManager.States inputType;
    [SerializeField]
    private GameManager.Identifiers identity;

    [SerializeField]
    private float m_speed = 5;
    private Vector2 m_moveDirection = Vector2.zero;
    [SerializeField]
    private Vector2 m_moveDestination;
    [SerializeField]
    private float m_distanceToDestination = 2.5f;

    public bool IsMouseOver { get; private set; }

    public void Init(GameManager.States type, GameManager.Identifiers id, Vector2 direction, Vector2 destination)
    {
        inputType = type;
        identity = id;
        m_moveDirection = direction;
        m_moveDestination = destination;
        switch (identity)
        {
            case GameManager.Identifiers.Player:
                m_bgImage.color = m_playerColor;
                break;
            case GameManager.Identifiers.Enemy:
                m_bgImage.color = m_enemyColor;
                break;
        }
        GetComponent<SpriteRenderer>().sprite = feedBackImage;
        GameManager.instance.RoundNext.AddListener(Clear);
        GameManager.instance.RoundEnd.AddListener(Clear);

        m_speed = m_speed + (GameManager.instance.Level / 2.0f);
    }

    // Update is called once per frame
    void Update ()
    {
        transform.position += (Vector3)m_moveDirection * Time.deltaTime * m_speed;
        if (IsMouseOver)
        {
            if (identity == GameManager.Identifiers.Enemy)
            {
                if (Input.GetButtonDown("Defence") && inputType == GameManager.States.Attack)
                {
                    Destroy(this.gameObject);
                    GameManager.instance.Score += 10;
                }
                else if (Input.GetButtonDown("Attack") && inputType == GameManager.States.Defence)
                {

                }
            }
            else if (identity == GameManager.Identifiers.Player)
            {
                if (Input.GetButtonDown("Attack") && inputType == GameManager.States.Attack)
                {
                    GameManager.instance.EnemyScript.CurrentHealth--;
                    GameManager.instance.Score += 10;
                    Destroy(this.gameObject);
                }
                else if (Input.GetButtonDown("Defence") && inputType == GameManager.States.Defence)
                {

                }
            }
        }
        // Otherwise if you miss you mark suffer the consequences.
        else
        {
            if (Input.GetButtonDown("Defence") || Input.GetButtonDown("Attack"))
            {
                GameManager.instance.Score -= 10;
            }
        }
        if (Vector2.Distance(m_moveDestination, transform.position) <= m_distanceToDestination)
        {
            DestinationReached();
        }
	}
    private void DestinationReached()
    {
        if (identity == GameManager.Identifiers.Enemy && inputType == GameManager.States.Attack)
        {
            GameManager.instance.PlayerScript.CurrentHealth--;
        }
        Destroy(gameObject);
    }

    void Clear()
    {
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        IsMouseOver = true;
    }
    private void OnMouseExit()
    {
        IsMouseOver = false;
    }
}
