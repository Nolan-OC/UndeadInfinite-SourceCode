using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public List<GameObject> zombies;
    public List<GameObject> survivorMasterList;
    public int zombiesToSpawn;
    public int zombiesSpawned;

    public GameObject squad;
    public GameObject playerRef;
    public GameObject gameManager;

    private void Awake()
    {
        gameManager.GetComponent<GameEnd>().SetZombieKillGoal(zombiesToSpawn);
    }
    private void Start()
    {
        foreach (Transform child in squad.transform)
        {
            survivorMasterList.Add(child.gameObject);
        }
        playerRef = survivorMasterList[0];

        StartCoroutine("SpawnTimer");
    }

    private void Spawn()
    {
        int randomIndex = Random.Range(0, zombies.Count-1);
        GameObject zombie = Instantiate(zombies[randomIndex], this.transform.position, this.transform.rotation);
        zombie.GetComponent<Zombie>().target = playerRef;
        zombie.GetComponent<Zombie>().humans = survivorMasterList;
        zombie.GetComponent<Zombie>().gameEnd = gameManager.GetComponent<GameEnd>();
    }
    private IEnumerator SpawnTimer()
    {
        while(zombiesSpawned < zombiesToSpawn)
        {
            yield return new WaitForSeconds(2);
            Spawn();
            zombiesSpawned++;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
