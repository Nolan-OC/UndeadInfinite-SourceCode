using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanSurvivor : MonoBehaviour
{
    public Animator anim;
    public bool alive = true;
    [SerializeField] private float health;
    public bool isPlayer;

    public GameObject zombieSelf;
    public GameObject gameManager;
    public int unitIndex = -1;
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        //squad = gameManager.GetComponent<playerReferences>().
        if (isPlayer)
        {
            if (TryGetComponent(out PlayerMovement p_movement))
            {
                anim = p_movement.anim;
            }
        }
        else
            anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (alive)
        {
            health -= damage;
            anim.Play("Damage");
            if(unitIndex>=0)
                gameManager.GetComponent<HealthBarManager>().DisplayDamage(unitIndex, damage);
            if (health<=0)  //DEAD
            {
                //MUST INCREASE ZOMIBE KILL GOAL AS THERE IS A NEW ZOMBIE TO BE SPAWNED AT DEATH
                gameManager.GetComponent<GameEnd>().SetZombieKillGoal(1);
                alive = false;
                anim.Play("Die");
                if(!isPlayer)
                {
                    if (TryGetComponent(out Civilian civilianScript))
                    {
                        civilianScript.Death();
                    }
                    else if (TryGetComponent(out FollowPlayer follower))
                    {
                        follower.Disable();
                        SpawnZombieForm();
                    }
                    else if(TryGetComponent(out TutorialCivilian tutorialCivilian))
                    {
                        tutorialCivilian.Death();
                    }
                }
                if(isPlayer)
                {
                    NewPlayerCharacter();
                    StartCoroutine("PlayerDeath");
                }
            }
        }
    }
    private void NewPlayerCharacter()
    {
        List<GameObject> squad = new List<GameObject>();
        int index = squad.IndexOf(gameObject);
        Debug.Log(index + 1 + " index of new player character");
        foreach (Transform human in transform.parent)
        {
            //if human is alive, add to list
            if (human.GetComponent<HumanSurvivor>().alive)
            {
                squad.Add(human.gameObject);
            }
        }
        if (squad.Count > 0)
        {
            //SwitchComponents of new player
            GameObject player = squad[index + 1].gameObject;
            gameManager.GetComponent<LoadSquad>().MakePlayerCharacter(player);
            player.GetComponent<FollowPlayer>().enabled = false;
            GetComponent<FollowerCombat>().StopCombat();
            GetComponent<EquippedWeapon>().CancelInvoke();
            GetComponent<FollowerCombat>().CancelInvoke();
            GetComponent<FollowPlayer>().CancelInvoke();
            //follower.GetComponent<FollowPlayer>().followTarget = instantiatedCharacters[instantiatedCharacters.Count - 2];
            player.GetComponent<FollowerCombat>().enabled = false;
            player.GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            //check if timed survival, 
            if(gameManager.GetComponent<GameEnd>().bonusConditions == GameEnd.BonusConditions.Time)
            {
                //if yes then check if got bronze or above, if yes then go to victory screen
            }
            //if didn't get bronze or above then go to defeat screen,
            //if not timed survival then go to defeat screen.
            gameManager.GetComponent<GameEnd>().DefeatScreen();
        }
    }
    private void SpawnZombieForm()
    {
        GameObject zombie = Instantiate(zombieSelf, transform.position, transform.rotation);
        zombie.GetComponent<Zombie>().target = gameManager.GetComponent<levelReferences>().Squad[0];
        zombie.GetComponent<Zombie>().humans = gameManager.GetComponent<levelReferences>().zombieSpawnerWithList.GetComponent<ZombieSpawner>().survivorMasterList;
        zombie.GetComponent<Zombie>().gameEnd = gameManager.GetComponent<GameEnd>();
    }
    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        isPlayer = false;
        //spawn zombie version of human
        SpawnZombieForm();

        //destroy self
        gameObject.SetActive(false);
        yield return null;
    }
}
