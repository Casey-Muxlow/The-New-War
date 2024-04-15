using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] AudioSource aud;
    [SerializeField] Collider damageCol;

    [Header("----- Enemy Stat -----")]
    [SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int speed;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] float animSpeedTrans;
    [SerializeField] int targetFaceSpeed;

    [Header("----- Weapon -----")]
    [SerializeField] GameObject EnemyBullet;
    [SerializeField] int bulletDamage;
    [SerializeField] int bulletDestroyTime;
    [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [SerializeField] float shootSpeed;
    public Transform enemyshootPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
