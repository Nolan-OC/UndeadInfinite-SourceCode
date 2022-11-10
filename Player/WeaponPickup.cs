using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject squad;
    [SerializeField]private List<CompanionLevel> companionLevels;
    public int spawnTimer;
    private List<GameObject> boxComponents;

    private void Awake()
    {

    }
    private void Start()
    {
        /*
        foreach (Transform child in transform)
        {
            //boxComponents.Add(child.gameObject);
            //child.gameObject.SetActive(false);
        }
        */
        foreach (Transform child in squad.transform)
        {
            companionLevels.Add(child.GetComponent<CompanionLevel>());
        }
        //StartCoroutine("SpawnInSeconds");
    }
    private IEnumerator SpawnInSeconds()
    {
        float currentTime = 0;
        while(currentTime<spawnTimer)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        foreach(GameObject component in boxComponents)
        {
            component.SetActive(true);
        }
        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Survivors")
        {
                foreach (CompanionLevel level in companionLevels)
                {
                    level.LevelUp();
                }

                Destroy(this.gameObject);
        }
    }
}
