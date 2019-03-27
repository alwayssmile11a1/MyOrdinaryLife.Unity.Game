using BTAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{

    public float normalSpeed = 5f;
    public float attackSpeed = 5f;
    public Transform[] positionsToMove;

    //Behavior Tree
    private Root m_Ai = BT.Root();
    private int m_NewPositionIndex = -1;
    private Vector2 m_PlayerPosition;
    private PlayerPlatformerController m_Player;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_HitWall;

    private void Awake()
    {
        m_Ai.OpenBranch(
            BT.Sequence().OpenBranch(
                BT.Wait(2f),
                BT.Call(GetNewPosition),
                BT.WaitUntil(MoveToNewPosition),
                BT.Wait(2f),
                BT.Call(RemeberPlayerPosition),
                BT.WaitUntil(AttackPlayer)
            )
        );

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_Player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        m_Ai.Tick();
    }

    private void GetNewPosition()
    {
        m_NewPositionIndex++;
    }

    private bool MoveToNewPosition()
    {
        if (m_NewPositionIndex == positionsToMove.Length) return true;
        transform.position = positionsToMove[m_NewPositionIndex].position;
        return true;
        //if (transform.position == positionsToMove[m_NewPositionIndex].position)
        //{
        //    return true;
        //}
        //else
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, positionsToMove[m_NewPositionIndex].position, normalSpeed * Time.deltaTime);
        //    return false;
        //}

    }

    private void RemeberPlayerPosition()
    {
        m_PlayerPosition = m_Player.transform.position;
    }

    private bool AttackPlayer()
    {
        if (m_Rigidbody2D.position == m_PlayerPosition || m_HitWall)
        {
            m_HitWall = false;
            return true;
        }
        else
        {
            m_Rigidbody2D.MovePosition(Vector3.MoveTowards(m_Rigidbody2D.position, m_PlayerPosition, attackSpeed * Time.deltaTime));
            return false;
        }
    }

    private void BounceBack()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_HitWall = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit Player");
    }

}
