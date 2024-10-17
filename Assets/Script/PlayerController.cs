using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _moveSpeed = 500f;
    [SerializeField]private float _jumpSpeed = 7f;
    public LayerMask groundLayer,Enemy;
    private bool Grounded;
    private Rigidbody2D _rb;
    private Animator _anim;
    private string currentAnim = "";
    private bool isJumping = false;
    public TMP_Text Score;
    private float countScore = 0;
    private bool isGrounded = false;
    public GameObject  Panel,Button,Panel2;
    public Button button;
    // private float startTime;
    private float delay = 2f;
    private bool isDead = false;
    
    void Start()
    {
         
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        // var horizontal = Input.GetAxisRaw("Horizontal"); // -1 đi lùi, 0 đứng im, 1 tiến về phía trước => return -1,0,1
        // if(Mathf.Abs(horizontal)> 0.1f){
        //     ChangeAnim("IsRunning");
        //     _rb.velocity = new Vector2(horizontal * _moveSpeed * Time.deltaTime  , _rb.velocity.y);
        // }else{
        //     ChangeAnim("IsIdle");
        //     _rb.velocity = Vector2.zero;
        // }

    }
    private void Move()
    {   
        if(isDead){
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
         Grounded = checkGrounded();
        //  Debug.Log(Grounded);
            if (Grounded && !isGrounded){
                isGrounded = true;
                isJumping = false;
                // Debug.Log("Landed");
            }else if (!Grounded && isGrounded){
                isGrounded = false;
                // Debug.Log("Left ground");
            }

            if (isGrounded && Input.GetKeyDown(KeyCode.Space)){
                isJumping = true;
                Grounded = false;
                ChangeAnim("IsJump");
                _rb.AddForce(_jumpSpeed * Vector2.up, ForceMode2D.Impulse);
                // Debug.Log("Jump initiated");
            }
            // Kiểm tra nếu có di chuyển
            if (Mathf.Abs(horizontal) > 0.1f){
                // Flip nhân vật
                if (horizontal > 0){
                    ChangeAnim("IsRunning"); // Animation cho đi tiến
                    _rb.velocity = new Vector2(horizontal * _moveSpeed * Time.deltaTime, _rb.velocity.y);
                    Flip(true); // Lật sang phải
                }else if (horizontal < 0){
                    ChangeAnim("IsRunning"); // Animation cho đi lùi
                    _rb.velocity = new Vector2(horizontal * _moveSpeed * Time.deltaTime, _rb.velocity.y);
                    Flip(false); // Lật sang trái
                }
            }
            else if (isGrounded && !isJumping){
                ChangeAnim("IsIdle");
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        
       
    }

    private void Flip(bool facingRight)
    {
        Vector3 localScale = transform.localScale;
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            //localScale.x = Mathf.Abs(localScale.x); // Lật sang phải
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
            //localScale.x = -Mathf.Abs(localScale.x); // Lật sang trái 
        }
        transform.localScale = localScale; // Áp dụng thay đổi
    }
    private bool checkGrounded(){
       
        Debug.DrawLine(transform.position, transform.position + Vector3.down*0.5f, Color.red);
        RaycastHit2D hit  = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
        return hit.collider != null;
       
    }
    private bool checkHitEnemy(){
        Debug.DrawLine(transform.position, transform.position + Vector3.down*0.7f, Color.red);
        RaycastHit2D hit  = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, Enemy);
        return hit.collider != null;
    }
    private void ChangeAnim(string animName){
        // Debug.Log(animName);
        if(currentAnim != animName){
            _anim.ResetTrigger(animName);
            currentAnim = animName;
            
            _anim.SetTrigger(currentAnim);

        }
    }
      private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Coin")){
            countScore +=1;
            Score.SetText("" + countScore);
            var name = other.attachedRigidbody.name;
            Destroy(GameObject.Find(name));
        }
        if(other.gameObject.CompareTag("Done")){
            Panel2.SetActive(true);
            Time.timeScale=0;
        }
       
      
     }
     private void OnCollisionEnter2D(Collision2D collision2D){
            if(collision2D.gameObject.CompareTag("End")){
                    ChangeAnim("IsDied");
                    isDead = true;
                   StartCoroutine(StopAfterDelay());
               
        }
        if(collision2D.gameObject.CompareTag("Arrow")){
               ChangeAnim("IsDied");
                isDead = true;
                Destroy(collision2D.gameObject); 
                StartCoroutine(StopAfterDelay());
        }
        if(collision2D.gameObject.CompareTag("Enemy")){
            if(checkHitEnemy()){
                Destroy(collision2D.gameObject);
            }
        }
     }
     private IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(delay); // Đợi đúng thời gian quy định
        Time.timeScale = 0; // Dừng trò chơi
        Panel.SetActive(true); // Hiển thị panel
    
    }
       
}
