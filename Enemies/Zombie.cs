using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public bool tutorialZombie;
    public NavMeshAgent agent;
    public Animator anim;
    public CapsuleCollider capCollider;

    public GameObject target;
    public List<GameObject> humans;
    public ParticleSystem bloodParticle;
    public GameEnd gameEnd;

    [Header("CombatStats")]
    public bool alive;
    public bool isBuried;
    public float health;
    public bool attackAvailable = true;
    public float attackSpeed;
    public float attackRange;
    public float attackDamage;
    public float moveSpeed;

    public bool eating = false;

    public event EventHandler OnDeath;

    private void Awake()
    {
        anim.SetBool("isBuried", isBuried);
        agent.speed = moveSpeed;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        capCollider = GetComponent<CapsuleCollider>();
        StartCoroutine("WaitForSpawnAnim");
        //find nearest civilian or player
        StartCoroutine("FindTarget");
    }
    private void FixedUpdate()
    {
        if (alive && target != null && target.GetComponent<HumanSurvivor>().alive)
        {
            agent.destination = target.transform.position;
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < attackRange && target.GetComponent<HumanSurvivor>().alive)
            {
                if (attackAvailable)
                    StartCoroutine("Attack");
            }
            else if (distance < 1.5 && !target.GetComponent<HumanSurvivor>().alive && target.GetComponent<HumanSurvivor>().isPlayer)
            {
                if (!eating)
                    StartCoroutine("Eat");
            }
        }
    }
    public void TakeRangedDamage(float damage, float knockBack)
    {

        if (alive)
        {
            //knockback
            Vector3 newVector = transform.position - target.transform.position;
            agent.velocity = newVector * knockBack;
            //blood splat and animation
            anim.Play("Damage");
            bloodParticle.transform.LookAt(target.transform.position);
            bloodParticle.Play();

            health -= damage;
            if(health <=0)
            {
                alive = false;
                anim.Play("Die");
                agent.enabled = false;
                capCollider.enabled = false;
                gameEnd.IncreaseKillCount();
                StartCoroutine("DestroySelf");
            }
        }
    }
    public void TakeDamageNoBloodKnockback(float damage)
    {
        if (alive)
        {
            anim.Play("Damage");
            health -= damage;
            if (health <= 0)
            {
                alive = false;
                anim.Play("Die");
                agent.enabled = false;
                capCollider.enabled = false;
                gameEnd.IncreaseKillCount();
                StartCoroutine("DestroySelf");
            }
        }
    }
    public void TakeMeleeDamage(float damage, float knockBack)
    {

        if (alive)
        {
            //knockback
            Vector3 newVector = transform.position - target.transform.position;
            agent.velocity = newVector * knockBack;
            //blood splat and animation
            anim.Play("Damage");
            Vector3 awayDir = (transform.position * 2 - target.transform.position);
            awayDir -= new Vector3(0, awayDir.y, 0);
            bloodParticle.transform.LookAt(awayDir);
            bloodParticle.Play();
            health -= damage;
            if (health <= 0)
            {
                gameEnd.IncreaseKillCount();
                alive = false;
                anim.Play("Die");
                agent.enabled = false;
                capCollider.enabled = false;
                StartCoroutine("DestroySelf");
            }
        }
    }
    public IEnumerator Attack()
    {
        attackAvailable = false;
        target.GetComponent<HumanSurvivor>().TakeDamage(attackDamage);
        anim.Play("Attack");

        yield return new WaitForSeconds(attackSpeed);
        attackAvailable = true;
        yield return null;
    }
    public IEnumerator Eat()
    {
        eating = true;
        Vector3 lookPos = new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z);
        transform.LookAt(lookPos);
        anim.Play("Eat");
        yield return null;
    }
    public IEnumerator WaitForSpawnAnim()
    {
        yield return new WaitForSeconds(2);
        agent.speed = 3.5f;
        yield return null;
    }
    private IEnumerator DestroySelf()
    {
        if (!tutorialZombie)
        {
            yield return new WaitForSeconds(15);
            Destroy(gameObject);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Survivors")
        {
            float distanceToOther = Vector3.Distance(this.transform.position, other.gameObject.transform.position);
            float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
            if(distanceToOther>distanceToTarget)
            {
                target = other.gameObject;
            }
        }
    }
    private GameObject FindNearestHuman()
    {
        GameObject nearestHuman = null;
        float distance = 100;
        foreach(GameObject human in humans)
        {
            if (human.activeSelf == true)
            {
                if (Vector3.Distance(this.transform.position, human.transform.position) < distance)
                {
                    distance = Vector3.Distance(this.transform.position, human.transform.position);
                    nearestHuman = human;
                }
            }
        }

        if (nearestHuman != null)
        {
            return nearestHuman;
        }
        else
            return gameObject;
    }
    private IEnumerator FindTarget()
    {
        while (alive)
        {
            target = FindNearestHuman();
            yield return new WaitForSeconds(1);
            yield return null;
        }
        yield return null;
    }
}
