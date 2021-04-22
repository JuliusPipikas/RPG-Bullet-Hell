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

    // Good! You defended the chest! Lucky you, there was a potion there. Not that you really needed it, but here you go anyway!
    // Similar with village ^^
    // Well, you protected your chest, but don't really need the potions inside. Hmm... How about a different skin for your character? Yeah, that sounds good.
    // Oh, you did it again. Well... Now you get the SUPER rare skin! Yeah... That's right. *cough*

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
