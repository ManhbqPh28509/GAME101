using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyCotroller : MonoBehaviour
{
    public LayerMask Player;
    private bool HitPlayer;
    private bool isShotting = false,isHitPlayer = true;
     private Rigidbody2D _rb;

     private Animator _anim;

    private float idleTime = 1.8f;      // Thời gian chờ để chuyển sang Idle (2 giây)
    private float shotTimer = 0f;     // Bộ đếm thời gian từ lần bắn cuối
    private bool isShooting = true;  // Theo dõi trạng thái bắn

     [SerializeField] private GameObject Arrow;
     [SerializeField]  Transform FirePoint;
     [SerializeField] float AttackSpeed,countDown=0;

    private string currentAnim = "";
    // Start is called before the first frame update
    void Start()
    {
         _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
    {
        shotTimer += Time.deltaTime;
        Attack();

        if (shotTimer >= idleTime)
        {
            ChangeAnim("IsIdle");
            shotTimer = 0f;
        }
    }
       

    }
    private void Attack(){
        HitPlayer = checkhHitPlayer();
        countDown -= Time.deltaTime;
        if(countDown > 0){
            return;
        }
        if(HitPlayer){
            isShotting = true;
            shotTimer = 0f;
            ChangeAnim("IsShotting");
            Instantiate(Arrow,FirePoint.position,transform.rotation);
            countDown = AttackSpeed;
        }
    }
      private void ChangeAnim(string animName){
        // Debug.Log(animName);
        if(currentAnim != animName){
            _anim.ResetTrigger(animName);
            currentAnim = animName;
            
            _anim.SetTrigger(currentAnim);

        }
    }
    private bool checkhHitPlayer(){
        Debug.DrawLine(transform.position, transform.position + Vector3.left*6f, Color.red);
        RaycastHit2D hit  = Physics2D.Raycast(transform.position, Vector2.left, 6f, Player);
        return hit.collider != null;
    }
    
}
