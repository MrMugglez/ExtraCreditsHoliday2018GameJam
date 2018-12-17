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

    public bool IsMouseOver { get; private set; }

    public void Init(GameManager.States type, GameManager.Identifiers id, Vector2 direction)
    {
        inputType = type;
        identity = id;
        m_moveDirection = direction;
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
                }
                else if (Input.GetButtonDown("Attack") && inputType == GameManager.States.Defence)
                {

                }
            }
            else if (identity == GameManager.Identifiers.Player)
            {
                if (Input.GetButtonDown("Attack") && inputType == GameManager.States.Attack)
                {
                    Destroy(this.gameObject);
                }
                else if (Input.GetButtonDown("Defence") && inputType == GameManager.States.Defence)
                {

                }
            }
        }
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
