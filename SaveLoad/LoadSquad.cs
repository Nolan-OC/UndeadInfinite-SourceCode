using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;
using Cinemachine;

public class LoadSquad : MonoBehaviour
{
    public CinemachineVirtualCamera followCam;
    public Transform squadTransform;
    public levelReferences references;
    public List<GameObject> survivorPrefabs;
    List<GameObject> instantiatedCharacters = new List<GameObject>();

    void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
        SaveData squadData = JsonUtility.FromJson<SaveData>(json);

        List<string> names = new List<string>();
        if(squadData.survivor1Name != "")
            names.Add(squadData.survivor1Name);
        if (squadData.survivor2Name != "")
            names.Add(squadData.survivor2Name);
        if (squadData.survivor3Name != "")
            names.Add(squadData.survivor3Name);
        if (squadData.survivor4Name != "")
            names.Add(squadData.survivor4Name);

        SpawnSquad(names);
        LoadHealthBars(names.Count);

    }
    private void SpawnSquad(List<string>names)
    {
        int spawnOffset = 0;
        foreach (string name in names)  //for each name sent by json file
        {
            for (int i = 0; i < survivorPrefabs.Count; i++) //check name against the list of prefab characters
            {
                if (survivorPrefabs[i].name == name)    //when a match is found, instantiate the character
                {
                    Vector3 distMod = new Vector3(-spawnOffset, 0, 0);    //will add one to spacing between each instantiation
                    spawnOffset+=2;
                    instantiatedCharacters.Add(Instantiate(survivorPrefabs[i],squadTransform.position + distMod,Quaternion.identity,squadTransform));
                    if (survivorPrefabs[i].name == names[0])    //if the instantiated character's name is also the 1st name in json squad, make it player
                    {
                        GameObject player = instantiatedCharacters[instantiatedCharacters.Count - 1];
                        player.tag = "Survivors";
                        player.GetComponent<HumanSurvivor>().gameManager = gameObject;
                        MakePlayerCharacter(player);
                    }
                    else     //follow the character in front of you
                    {
                        //enable all scripts to allow this player to be a companion
                        GameObject follower = instantiatedCharacters[instantiatedCharacters.Count - 1];
                        follower.tag = "Survivors";
                        follower.GetComponent<HumanSurvivor>().gameManager = gameObject;
                        follower.GetComponent<FollowPlayer>().enabled = true;
                        follower.GetComponent<FollowPlayer>().followTarget = instantiatedCharacters[instantiatedCharacters.Count - 2];
                        follower.GetComponent<FollowerCombat>().enabled = true;
                        follower.GetComponent<NavMeshAgent>().enabled = true;
                    }
                    references.Squad = instantiatedCharacters;  //create permanent list of squad for reference
                }
            }
        }
    }
    public void MakePlayerCharacter(GameObject player)
    {
        //enable all scripts to allow this character to be controlled by input
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerMovement>().camTransform = references.camTransform;
        player.GetComponent<PlayerMovement>().moveJoystick = references.moveJoystick;
        player.GetComponent<PlayerMovement>().lookJoystick = references.lookJoystick;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<HumanSurvivor>().isPlayer = true;

        followCam.LookAt = player.transform;
        followCam.Follow = player.transform;
    }
    private void LoadHealthBars(int squadCount)
    {
            GetComponent<HealthBarManager>().SpawnHealthBars(squadCount);
    }
    private class SaveData
    {
        public int squadSize;

        public string survivor1Name;
        public string survivor2Name;
        public string survivor3Name;
        public string survivor4Name;

        public bool survivor1Unlocked;  //Butcher
        public bool survivor2Unlocked;  //Farmer
        public bool survivor3Unlocked;  //CoolGuy
        public bool survivor4Unlocked;  //Lumberjack
        public bool survivor5Unlocked;  //Nun
        public bool survivor6Unlocked;  //Cop
        public bool survivor7Unlocked;  //Punk
        public bool survivor8Unlocked;  //Red
        public bool survivor9Unlocked;  //CowGal
        public bool survivor10Unlocked; //Ooma
        public bool survivor11Unlocked; //Agent
    }
}
