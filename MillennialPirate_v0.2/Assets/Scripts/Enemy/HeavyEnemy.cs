﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState2
{
    ENEMY_ATTACK,
    ENEMY_WALKING,
    ENEMY_DEATH,
    ENEMY_HIT,
    ENEMY_IDLE
}
public class HeavyEnemy : MonoBehaviour
{
    [SerializeField]
    protected float
    hm_health = 5,
    hm_speed = 1.5f;

    [SerializeField]
    private Transform rootTransform;
    private Transform findPlayer = null;

    private Vector3 playerTrans;

    private Animator enemy_Anim;

    private Animation enemy_Walking;
    private Animation enemy_Attacking;
    private Animation enemy_Hit;
    private Animation enemy_Death;
    private Animation enemy_Idle;

    private bool inRange = false;
    private GameObject melee_enemy;


    private void Awake()
    {
        findPlayer = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Start()
    {

        enemy_Anim = GetComponent<Animator>();
        playerTrans = GameObject.Find("Player").transform.position;

        enemy_Attacking = Resources.Load("Animations/EnemyAni/HM_Enemy_Attack") as Animation;
        ChangeDirection();

    }

    EnemyState2 CurrentState = EnemyState2.ENEMY_WALKING;

    void Update()
    {
        Debug.Log(CurrentState);

        if (inRange == false)
        {

            transform.position += (playerTrans - rootTransform.position).normalized * hm_speed * Time.deltaTime;

        }

        switch (CurrentState)
        {
            case EnemyState2.ENEMY_WALKING:

                break;

            case EnemyState2.ENEMY_IDLE:
                StartCoroutine(IdleDelay());
                break;

            case EnemyState2.ENEMY_ATTACK:
                StartCoroutine(AttackDelay());
                break;

            case EnemyState2.ENEMY_HIT:

                break;

            case EnemyState2.ENEMY_DEATH:
                enemy_Anim.SetBool("hasBeenKilled", true);
                break;

        }

        if (hm_health <= 0)
        {
            CurrentState = EnemyState2.ENEMY_DEATH;
            Destroy(gameObject);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inRange = true;
            CurrentState = EnemyState2.ENEMY_ATTACK;
        }

        if (collision.tag == "PlayerAttack")
        {
            CurrentState = EnemyState2.ENEMY_HIT;
        }


        /*if(collision.tag == "LightAttack")
        {
           hm_health -= 1;
        }

        else (collision.tag == "HeavyAttack")
        {
           hm_health -= 3;
        }*/

    }

    IEnumerator AttackDelay()
    {
        enemy_Anim.SetBool("attackTrigger", true);



        yield return new WaitForSeconds(1.20f);

        enemy_Anim.SetBool("attackTrigger", false);

        CurrentState = EnemyState2.ENEMY_IDLE;

    }

    IEnumerator IdleDelay()
    {


        enemy_Anim.SetBool("isIdle", true);

        yield return new WaitForSeconds(1.25f);

        enemy_Anim.SetBool("isIdle", false);

        CurrentState = EnemyState2.ENEMY_ATTACK;

    }

    void EnemyStateChange(EnemyState2 stateChange)
    {
        CurrentState = stateChange;
    }


    private void ChangeDirection()
    {
        if (findPlayer)
        {
            float playerXPos = findPlayer.position.x;
            float transformXPos = transform.position.x;
            float yRot = transform.eulerAngles.y;

            if (transformXPos < playerXPos)
            {
                if (yRot == 0)
                {
                    yRot = 180;
                    transform.eulerAngles = new Vector2(0, yRot);
                }
            }
        }
    }

}
