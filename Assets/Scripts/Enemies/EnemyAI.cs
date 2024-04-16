using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    
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

    bool isShooting;
    bool PlayerInRange;
    bool destinationChosen;
    float angleToPlayer;
    Vector3 playerDir;
    Vector3 startingPos;
    float stoppingDistanceOrig;

    private bool isDead = false;
    private bool TookDmg;
    private bool ChasingPlayer;
    public float notifyRadius = 25f;
    private bool NotifyOthers = false;
    

    private PlayersController playersController;




    void Start()
    {
        playersController = FindAnyObjectByType<PlayersController>();
        agent = GetComponent<NavMeshAgent>();

        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;

        float animationSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animationSpeed, Time.deltaTime * animSpeedTrans));


    }

    void Update()
    {

        if (playersController.IsDead)
        {
            PlayerInRange = false;
            agent.stoppingDistance = 0;
            isShooting = false;
            TookDmg = false;
            ChasingPlayer = false;
            NotifyOthers = false;
        }
        if (agent.isActiveAndEnabled)
        {
            float animationSpeed = agent.velocity.normalized.magnitude;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animationSpeed, Time.deltaTime * animSpeedTrans));

            if (PlayerInRange && !canSeePlayer() && !SearchforPlayer())
            {
                StartCoroutine(roam());
            }
            if ((TookDmg || ChasingPlayer) && !playersController.IsDead)
            {

                StartCoroutine(PlayerLocation());
            }
            else if (!PlayerInRange)
            {
                StartCoroutine(roam());
            }



        }
    }

    // If the enemy took damage and it does not see the player it looks for the player
    bool SearchforPlayer()
    {
        if (TookDmg || NotifyOthers)
        {

            agent.SetDestination(playersController.transform.position);
            return true;
        }

        return false;
    }


    IEnumerator PlayerLocation()
    {
        float SearchTime = 0f;
        while (SearchTime < 8f)
        {
            SearchforPlayer();
            SearchTime += Time.deltaTime;
            yield return null;
        }
        TookDmg = false;
        ChasingPlayer = false;
        NotifyOthers = false;
    }



    IEnumerator AttackLocation()
    {
        float SearchTime = 0f;
        while (SearchTime < 8f)
        {
            agent.SetDestination(playersController.transform.position);
            SearchTime += Time.deltaTime;
            yield return null;
        }
        TookDmg = false;
        ChasingPlayer = false;
        NotifyOthers = false;
    }

    void NotifyNearbyEnemies()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, notifyRadius);

        foreach (var collider in nearbyColliders)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
            {
                EnemyAI nearbyEnemy = collider.GetComponent<EnemyAI>();
                if (nearbyEnemy != null)
                {
                    nearbyEnemy.StartChasing();
                    
                }
            }
        }
    }

    public void StartChasing()
    {
        StopCoroutine(roam());
        ChasingPlayer = true;
        NotifyOthers = true;
        if (!PlayerInRange || PlayerInRange)
        {
            StartCoroutine(AttackLocation());
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.5 && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }

    bool canSeePlayer()
    {
        playerDir = playersController.transform.position - headPos.position;

        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(playersController.transform.position);


                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }


                agent.stoppingDistance = stoppingDistanceOrig;

                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }





    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    

    public void takeDamage(int amount)
    {
        if (isDead)
            return;

        HP -= amount;

        StopAllCoroutines();

        if (HP <= 0)
        {
            isDead = true;
           
           
           
            agent.enabled = false;
            damageCol.enabled = false;
            StartCoroutine(DeactivateWait());
        }

        else
        {
            TookDmg = true;
            ChasingPlayer = true;
            isShooting = false;
           
            // anim.SetTrigger("Damage");
            destinationChosen = false;
            agent.SetDestination(playersController.transform.position);
            NotifyNearbyEnemies();
        }
    }
    IEnumerator DeactivateWait()
    {
        anim.SetBool("Dead", true);
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
            agent.stoppingDistance = 0;
            isShooting = false;
        }
    }
}
