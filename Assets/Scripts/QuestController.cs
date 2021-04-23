using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    [SerializeField]
    private AudioSource SFX;
    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private GameObject subtitles;

    [SerializeField]
    private EncounterGenerator EG;

    [SerializeField]
    private List<AudioClip> adventureBeginnings; // 1, 2, 3 (in case you don't know how to control your character, guess you'll have to lose and check out the Main Menu again!)
    [SerializeField]
    private List<string> adventureBeginningsSub;

    [SerializeField]
    private List<AudioClip> goblinEncounterBeginnings; // Early 1, Early 2, Mid 1, Mid 2, (timesFacedGoblins a few / none) Late 1, Late 2 (timesFacedGoblins a few / none)
    [SerializeField]
    private List<string> goblinEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> goblinEncounterEndings; // Early 1, Early 2, Mid 1, Mid 2, Late 1, Late 2 (damaged/undamaged)
    [SerializeField]
    private List<string> goblinEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> skeletonEncounterBeginnings;  // Early 1, Early 2, Mid 1, Mid 2 (timesFacedSkeletons a few / none), Late 1, Late 2 (timesFacedSkeletons a few / none)
    [SerializeField]
    private List<string>skeletonEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> skeletonEncounterEndings; // Early 1, Early 2, Mid 1, Mid 2, Late 1, Late 2 (damaged/undamaged)
    [SerializeField]
    private List<string> skeletonEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> wizardEncounterBeginnings; // Early 1, Early 2, Mid 1, Mid 2, Late 1, Late 2 (timesFacedSlimes a few / none)
    [SerializeField]
    private List<string> wizardEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> wizardEncounterEndings; // Early 1, Early 2, Mid 1, Mid 2, Late 1, Late 2 (damaged/undamaged)
    [SerializeField]
    private List<string> wizardEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> raiderEncounterBeginnings; // Early 1, Early 2, Mid 1, Mid 2, Late 1, Late 2 (timesFacedRaider a few / none)
    [SerializeField]
    private List<string> raiderEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> raiderEncounterEndings; // Early 1, Early 2, Mid 1, Mid 2, Late 1, Late 2 (damaged/undamaged)
    [SerializeField]
    private List<string> raiderEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> miscEncounterBeginnings; // Early 1, Early 2, Mid 1, Mid 2, Late 1, Late 2 (timesFacedGoblins a few / none) // Remarks regarding whether you failed once or twice before. Or if you succeeded last time or not.
    [SerializeField]
    private List<string> miscEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> miscEncounterEndings;
    // Good! You defended the chest! Lucky you, there was a potion there. Not that you really needed it, but here you go anyway!
    // Similar with village ^^
    // Well, you protected your chest, but don't really need the potions inside. Hmm... How about a different skin for your character? Yeah, that sounds good.
    // Oh, you did it again. Well... Now you get the SUPER rare skin! Yeah... That's right. *cough*

    [SerializeField]
    private List<string> miscEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> bossEncounterBeginning; // depends on encounter types. Whoever you've encountered most (out of Goblin or Skeleton, maybe Wizard + Necro special mention, RaiderBoss + GobliNStack mention (their general was 4 goblins in a trenchoat!). If you've encountered all of them equally, then the narrator remarks on that says that he'll choose at random
    [SerializeField]
    private List<string> bossEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> bossEncounterEndings; // depends on encounter types.Whoever you've encountered most (out of Goblin or Skeleton, maybe Wizard + Necro special mention, RaiderBoss + GobliNStack mention (their general was 4 goblins in a trenchoat!). If you've encountered all of them equally, then the narrator remarks on that says that he'll choose at random
    [SerializeField]
    private List<string> bossEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> levelUp; // First 1, First 2, Second 1, Second 2
    [SerializeField]
    private List<string> levelUpSub;

    [SerializeField]
    private List<AudioClip> adventureEndings;
    [SerializeField]
    private List<string> adventureEndingsSub;

    private int timesFacedCombat = 0;
    private int timesFacedGoblins = 0;
    private int timesFacedBandits = 0;
    private int timesFacedSkeletons = 0;
    private int timesFacedMagicCaster = 0;
    private int timesSaved = 0;
    private int timesSavedPerfectly = 0;
    private int timesFailedToSave = 0;
    private int timesSocialized = 0;
    private int timesFailedToSocialize = 0;
    private bool lost = false;
    private int rand;
    private bool canContinue = false;
    private bool canContinueMain = false;
    private float clipLength;

    void Start()
    {
        StartCoroutine(Beginning());
    }

    public IEnumerator Beginning()
    {
        rand = Random.Range(0, adventureBeginnings.Count);
        clipLength = adventureBeginnings[rand].length;

        StartCoroutine(playSubtitlesAndAudio(adventureBeginningsSub[rand], adventureBeginnings[rand]));
        
        yield return new WaitForSeconds(clipLength);

        rand = Random.Range(0, 4);

        switch (rand)
        {
            case 0:
                StartCoroutine(EG.spawnGoblinAmbush(1));
                break;
            case 1:
                StartCoroutine(EG.spawnSkeletalRuins(1));
                break;
            case 2:
                StartCoroutine(EG.spawnRaiderAttack(1));
                break;
            case 3:
                StartCoroutine(EG.spawnWizardLair(1));
                break;
            case 4:
                StartCoroutine(EG.spawnChestProtect(1));
                break;
            case 5:
                StartCoroutine(EG.spawnPrincessProtect(1));
                break;
            case 6:
                StartCoroutine(EG.spawnVillage(1));
                break;
        }

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(check(rand));

        canContinue = false;

        while (!canContinue) yield return null;

        Debug.Log("AAA");

        //yield return new WaitForSeconds(0f);
        //if (!lost) StartCoroutine(Mid());
    }

    public IEnumerator Mid()
    {
        yield return new WaitForSeconds(0f);
    }

    public IEnumerator Finale()
    {
        yield return new WaitForSeconds(0f);
    }

    public IEnumerator Lost()
    {
        yield return new WaitForSeconds(0f);
    }

    IEnumerator check(int i)
    {
        if(i >= 0 && i < 6)
        {
            yield return checkForEnemies();
        }
        //if(i )
    }

    IEnumerator checkForEnemies()
    {
        List<GameObject> objects = EG.GetObjects();
        bool test = false;
        canContinue = false;

        while (!canContinue)
        {
            test = true;
            foreach (GameObject obj in objects)
            {
                if (obj && obj.layer == LayerMask.NameToLayer("Enemy"))
                {
                    test = false;
                }
            }

            if(test == true)
            {
                canContinue = true;
                StartCoroutine(EG.Despawn());
            }
            yield return null;
        }
        yield return new WaitForSeconds(0f);
    }

    IEnumerator playSubtitlesAndAudio(string text, AudioClip AC)
    {
        Text t = subtitles.GetComponent<Text>();
        t.text = text;
        subtitles.SetActive(true);

        SFX.PlayOneShot(AC);
        yield return new WaitForSeconds(clipLength);

        subtitles.SetActive(false);
        canContinue = true;
    }
}
