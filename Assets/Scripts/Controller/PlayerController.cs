using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //


    private Animator ani;
    private Rigidbody rg;

    [Header("Move Input Info")]
    public float horizontal;
    public float vertical;

    public float speed = 2f;

    [Header("Combo Input Info")]
    public int hitCount = 0;
    public float coolDownTime = 2f;
    private float nextFireTime = 0f;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 1;


    //
    private void Awake()
    {
        ani = GetComponent<Animator>();
        rg = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputCheck();

        if(ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && ani.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            ani.SetBool("hit1", false);
        }
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && ani.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            ani.SetBool("hit2", false);
        }
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && ani.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            ani.SetBool("hit3", false);
            hitCount = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            hitCount = 0;
        }

            if(Input.GetKeyDown(KeyCode.J))
            {
                Combo();
            }


    }

    private void FixedUpdate()
    {


        Move();
    }

    //

    private void InputCheck()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");




    }

    

    private void Move()
    {
        float moveMag = Mathf.Sqrt(horizontal * horizontal + vertical * vertical);
        moveMag = Mathf.Min(1, moveMag);
        Vector3 moveDir = new Vector3(horizontal, 0, vertical);
        Vector3 moveSpeed =  moveDir * speed;
        rg.position += moveSpeed;
        if(moveMag > 0.1f)
        {
            transform.forward = Vector3.Slerp(transform.forward,moveDir,0.1f);
        }

        ani.SetFloat("Speed", moveMag);
    }

    private void Combo()
    {
        lastClickedTime = Time.time;
        hitCount++;

        if(hitCount == 1)
        {
            ani.SetBool("hit1", true);
        }

        hitCount = Mathf.Clamp(hitCount, 0, 3);

        if(ani.GetCurrentAnimatorStateInfo(0).IsName("hit1") && hitCount >= 2 && ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        {
            ani.SetBool("hit1", false);
            ani.SetBool("hit2", true);
        }

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("hit2") && hitCount >= 3 && ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        { 
            ani.SetBool("hit2", false);
            ani.SetBool("hit3", true);
        }



    }

}
