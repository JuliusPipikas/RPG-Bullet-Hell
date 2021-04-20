using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> adventureBeginnings;

    [SerializeField]
    private List<AudioClip> goblinEncounterBeginnings;
    [SerializeField]
    private List<AudioClip> goblinEncounterEndings;

    [SerializeField]
    private List<AudioClip> skeletonEncounterBeginnings;
    [SerializeField]
    private List<AudioClip> skeletonEncounterEndings;

    [SerializeField]
    private List<AudioClip> miscEncounterBeginnings;
    [SerializeField]
    private List<AudioClip> miscEncounterEndings;

    [SerializeField]
    private List<AudioClip> bossEncounterBeginning;
    [SerializeField]
    private List<AudioClip> bossEncounterEndings;

    [SerializeField]
    private List<AudioClip> levelUp;

    [SerializeField]
    private List<AudioClip> adventureEndings;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
