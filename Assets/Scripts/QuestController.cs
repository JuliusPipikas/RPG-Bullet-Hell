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
    private GameObject counter;
    [SerializeField]
    private GameObject villagersLeft;

    [SerializeField]
    private EncounterGenerator EG;

    [SerializeField]
    private GameObject spawnPoof;
    [SerializeField]
    private GameObject healUp;

    [SerializeField]
    private List<AudioClip> adventureBeginnings; //(in case you don't know how to control your character, guess you'll have to lose and check out the Main Menu again!)
    [SerializeField]
    private List<string> adventureBeginningsSub;

    [SerializeField]
    private List<AudioClip> goblinEncounterBeginnings; 
    [SerializeField]
    private List<string> goblinEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> goblinEncounterEndings; 
    [SerializeField]
    private List<string> goblinEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> skeletonEncounterBeginnings;  
    [SerializeField]
    private List<string>skeletonEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> skeletonEncounterEndings; 
    [SerializeField]
    private List<string> skeletonEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> wizardEncounterBeginnings; 
    [SerializeField]
    private List<string> wizardEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> wizardEncounterEndings; 
    [SerializeField]
    private List<string> wizardEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> raiderEncounterBeginnings; 
    [SerializeField]
    private List<string> raiderEncounterBeginningsSub;
    [SerializeField]
    private List<AudioClip> raiderEncounterEndings;
    [SerializeField]
    private List<string> raiderEncounterEndingsSub;

    [SerializeField]
    private List<AudioClip> miscEncounterBeginnings;
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

    private int timesFacedGoblins = 0;
    private int timesFacedBandits = 0;
    private int timesFacedSkeletons = 0;
    private int timesFacedSlime = 0;
    private int timesFacedMagicCaster = 0;
    private int timesFacedProtection = 0;
    private int timesFacedVillage = 0;
    private int timesSaved = 0;
    private int timesSavedPerfectly = 0;
    private int timesFailedToSave = 0;
    private int timesFailedToSocialize = 0;
    private bool lost = false;
    private int rand;
    private bool canContinue = false;
    private bool canContinueMain = false;
    private float clipLength;
    private List<int> visited;
    private List<int> visitedPermanent;
    private int playerHPBefore;
    private bool inTime = true;
    private int timesFailedPrior;
    private int villCnt = 0;
    private bool currentlyDeleting = false;

    [SerializeField]
    private List<GameObject> PlayerLevels = new List<GameObject>();

    void Start()
    {
        visited = new List<int>();
        visitedPermanent = new List<int>();
        StartCoroutine(Beginning());
    }

    public IEnumerator Beginning()
    {
        rand = Random.Range(0, adventureBeginnings.Count);
        clipLength = adventureBeginnings[rand].length;

        StartCoroutine(playSubtitlesAndAudio(adventureBeginningsSub[rand], adventureBeginnings[rand]));
        yield return new WaitForSeconds(clipLength);

        for (int i = 0; i < 3; i++)
        {
            playerHPBefore = GameObject.Find("Player").GetComponent<PlayerController>().getHealth();
            
            while (true)
            {
                rand = Random.Range(0, 7);

                if (!visited.Contains(rand) && !visitedPermanent.Contains(rand))
                {
                    if (rand >= 4)
                    {
                        if(rand == 6)
                        {
                            timesFailedPrior = timesFailedToSocialize;
                        }
                        else
                        {
                            timesFailedPrior = timesFailedToSave;
                        }
                        visitedPermanent.Add(rand);
                        visited.Add(4);
                        visited.Add(5);
                        visited.Add(6);
                    }
                    else
                    {
                        visited.Add(rand);
                    }
                    break;
                }
            }

            canContinue = false;
            StartCoroutine(findStartAudio(rand, 1));
            yield return new WaitForSeconds(clipLength);

            Encounter(1, rand);

            yield return new WaitForSeconds(2.5f);
            StartCoroutine(check(rand));
            canContinue = false;
            while (!canContinue) yield return null;

            canContinue = false;
            StartCoroutine(findEndAudio(rand, 1)); 
            yield return new WaitForSeconds(clipLength);

            yield return new WaitForSeconds(0.5f);
        }

        if (!lost) StartCoroutine(Mid());
    }

    public IEnumerator Mid()
    {
        yield return null;
        StartCoroutine(findStartAudio(8, 1));
        yield return new WaitForSeconds(clipLength);
        PlayerLevels[0].GetComponent<SpriteRenderer>().sprite = PlayerLevels[1].GetComponent<SpriteRenderer>().sprite;
        PlayerLevels[0].GetComponents<CapsuleCollider2D>()[0] = PlayerLevels[1].GetComponents<CapsuleCollider2D>()[0];
        PlayerLevels[0].GetComponents<CapsuleCollider2D>()[1] = PlayerLevels[1].GetComponents<CapsuleCollider2D>()[1];
        PlayerLevels[0].transform.GetChild(0).GetComponents<CapsuleCollider2D>()[0] = PlayerLevels[1].transform.GetChild(0).GetComponent<CapsuleCollider2D>();
        PlayerLevels[0].transform.GetChild(1).transform.localScale = PlayerLevels[1].transform.GetChild(1).transform.localScale;
        GameObject spawn = Instantiate(spawnPoof, PlayerLevels[0].transform.position, Quaternion.identity);
        PlayerLevels[0].GetComponent<PlayerController>().movementVelocity += 0.3f;
        visited.Clear();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 3; i++)
        {
            playerHPBefore = GameObject.Find("Player").GetComponent<PlayerController>().getHealth();

            while (true)
            {
                rand = Random.Range(0, 7);

                if (!visited.Contains(rand) && !visitedPermanent.Contains(rand))
                {
                    if (rand >= 4)
                    {
                        if (rand == 6)
                        {
                            timesFailedPrior = timesFailedToSocialize;
                        }
                        else
                        {
                            timesFailedPrior = timesFailedToSave;
                        }
                        visitedPermanent.Add(rand);
                        visited.Add(4);
                        visited.Add(5);
                        visited.Add(6);
                    }
                    else
                    {
                        visited.Add(rand);
                    }
                    break;
                }
            }

            canContinue = false;
            StartCoroutine(findStartAudio(rand, 2));
            yield return new WaitForSeconds(clipLength);

            Encounter(2, rand);

            yield return new WaitForSeconds(2.5f);
            StartCoroutine(check(rand));
            canContinue = false;
            while (!canContinue) yield return null;

            canContinue = false;
            StartCoroutine(findEndAudio(rand, 2));
            yield return new WaitForSeconds(clipLength);

            yield return new WaitForSeconds(0.5f);
        }

        while (!canContinue) yield return null;

        if (!lost) StartCoroutine(Finale());
    }

    public IEnumerator Finale()
    {
        yield return null;
        StartCoroutine(findStartAudio(8, 2));
        yield return new WaitForSeconds(clipLength);
        PlayerLevels[0].GetComponent<SpriteRenderer>().sprite = PlayerLevels[2].GetComponent<SpriteRenderer>().sprite;
        PlayerLevels[0].GetComponents<CapsuleCollider2D>()[0] = PlayerLevels[2].GetComponents<CapsuleCollider2D>()[0];
        PlayerLevels[0].GetComponents<CapsuleCollider2D>()[1] = PlayerLevels[2].GetComponents<CapsuleCollider2D>()[1];
        PlayerLevels[0].transform.GetChild(0).GetComponents<CapsuleCollider2D>()[0] = PlayerLevels[2].transform.GetChild(0).GetComponent<CapsuleCollider2D>();
        PlayerLevels[0].transform.GetChild(1).transform.localScale = PlayerLevels[2].transform.GetChild(1).transform.localScale;
        GameObject spawn = Instantiate(spawnPoof, PlayerLevels[0].transform.position, Quaternion.identity);
        PlayerLevels[0].GetComponent<PlayerController>().movementVelocity += 0.3f;
        visited.Clear();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 3; i++)
        {
            playerHPBefore = GameObject.Find("Player").GetComponent<PlayerController>().getHealth();

            while (true)
            {
                rand = Random.Range(0, 7);

                if (!visited.Contains(rand) && !visitedPermanent.Contains(rand))
                {
                    if (rand >= 4)
                    {
                        if (rand == 6)
                        {
                            timesFailedPrior = timesFailedToSocialize;
                        }
                        else
                        {
                            timesFailedPrior = timesFailedToSave;
                        }
                        visitedPermanent.Add(rand);
                        visited.Add(4);
                        visited.Add(5);
                        visited.Add(6);
                    }
                    else
                    {
                        visited.Add(rand);
                    }
                    break;
                }
            }

            canContinue = false;
            StartCoroutine(findStartAudio(rand, 3));
            yield return new WaitForSeconds(clipLength);

            Encounter(3, rand);

            yield return new WaitForSeconds(2.5f);
            StartCoroutine(check(rand));
            canContinue = false;
            while (!canContinue) yield return null;

            canContinue = false;
            StartCoroutine(findEndAudio(rand, 3));
            yield return new WaitForSeconds(clipLength);

            yield return new WaitForSeconds(0.5f);
        }

        while (!canContinue) yield return null;

        if (!lost)
        {
            if (timesFacedSkeletons > timesFacedGoblins)
            {
                yield return null;
                StartCoroutine(findStartAudio(7, 3));
                yield return new WaitForSeconds(clipLength);
                yield return Encounter(4, 1);
            }
            else if (timesFacedSkeletons < timesFacedGoblins)
            {
                yield return null;
                StartCoroutine(findStartAudio(7, 3));
                yield return new WaitForSeconds(clipLength);
                yield return Encounter(4, 0);
            }
            else
            {
                int rand1 = Random.Range(0, 2);
                yield return null;
                StartCoroutine(findStartAudio(7, 3));
                yield return new WaitForSeconds(clipLength);
                yield return Encounter(4, rand1);
            }
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(check(rand));
            canContinue = false;
            while (!canContinue) yield return null;
        }

        if (!lost)
        {
            canContinue = false;
            StartCoroutine(findEndAudio(7, 3));
            yield return new WaitForSeconds(clipLength);

            yield return new WaitForSeconds(1f);

            canContinue = false;
            StartCoroutine(findStartAudio(9, 3));
            yield return new WaitForSeconds(clipLength);
        }
    }

    public IEnumerator Lost()
    {
        yield return new WaitForSeconds(0f);
        lost = true;
    }

    public object Encounter(int intensity, int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(EG.spawnGoblinAmbush(intensity));
                timesFacedGoblins++;
                break;
            case 1:
                StartCoroutine(EG.spawnSkeletalRuins(intensity));
                timesFacedSkeletons++;
                break;
            case 2:
                StartCoroutine(EG.spawnWizardLair(intensity));

                if (intensity == 3)
                {
                    timesFacedMagicCaster++;
                }

                timesFacedSlime++;
                break;
            case 3:
                StartCoroutine(EG.spawnRaiderAttack(intensity));
                timesFacedBandits++;
                break;
            case 4:
                StartCoroutine(EG.spawnChestProtect(intensity));
                break;
            case 5:
                StartCoroutine(EG.spawnPrincessProtect(intensity));
                break;
            case 6:
                StartCoroutine(EG.spawnVillage(intensity));
                counter.SetActive(true);
                villagersLeft.SetActive(true);
                counter.GetComponent<Text>().text = "15";
                villagersLeft.GetComponent<Text>().text = "5/5";
                StartCoroutine(villageEncounterTracker());
                break;
        }
        return null;
    }

    IEnumerator villageEncounterTracker()
    {
        yield return null;
        int cnt = 15;
        yield return new WaitForSeconds(2.5f);

        canContinue = false;
        while (!canContinue)
        {
            yield return null;
            yield return new WaitForSeconds(1f);
            cnt--;
            counter.GetComponent<Text>().text = cnt.ToString();
            if(cnt == 0)
            {
                timesFailedToSocialize++;
                GameObject.Find("Player").GetComponent<PlayerController>().SwapWeapon(1);
                GameObject.Find("Player").GetComponent<PlayerController>().canSwap = true;
                counter.SetActive(false);
                villagersLeft.SetActive(false);
                StartCoroutine(EG.Despawn());
                canContinue = true;
            }
        }
    }

    IEnumerator findStartAudio(int index, int stage)
    {
        yield return null;
        canContinue = false;
        if (index == 0) // Goblin
        {
            if (stage == 1)
            {
                int rand1 = Random.Range(0, 2);
                StartCoroutine(playSubtitlesAndAudio(goblinEncounterBeginningsSub[rand1], goblinEncounterBeginnings[rand1]));
                clipLength = goblinEncounterBeginnings[rand1].length;
                canContinue = true;
            }
            else if (stage == 2)
            {
                if (timesFacedGoblins == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(goblinEncounterBeginningsSub[2], goblinEncounterBeginnings[2]));
                    clipLength = goblinEncounterBeginnings[2].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(goblinEncounterBeginningsSub[3], goblinEncounterBeginnings[3]));
                    clipLength = goblinEncounterBeginnings[3].length;
                    canContinue = true;
                }
            }
            else
            {
                if (timesFacedGoblins == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(goblinEncounterBeginningsSub[4], goblinEncounterBeginnings[4]));
                    clipLength = goblinEncounterBeginnings[4].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(goblinEncounterBeginningsSub[5], goblinEncounterBeginnings[5]));
                    clipLength = goblinEncounterBeginnings[5].length;
                    canContinue = true;
                }
            }
        }
        if (index == 1) // Skeleton
        {
            if (stage == 1)
            {
                int rand1 = Random.Range(0, 2);
                StartCoroutine(playSubtitlesAndAudio(skeletonEncounterBeginningsSub[rand1], skeletonEncounterBeginnings[rand1]));
                clipLength = skeletonEncounterBeginnings[rand1].length;
                canContinue = true;
            }
            else if (stage == 2)
            {
                if (timesFacedSkeletons == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(skeletonEncounterBeginningsSub[2], skeletonEncounterBeginnings[2]));
                    clipLength = skeletonEncounterBeginnings[2].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(skeletonEncounterBeginningsSub[3], skeletonEncounterBeginnings[3]));
                    clipLength = skeletonEncounterBeginnings[3].length;
                    canContinue = true;
                }
            }
            else
            {
                if (timesFacedSkeletons == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(skeletonEncounterBeginningsSub[4], skeletonEncounterBeginnings[4]));
                    clipLength = skeletonEncounterBeginnings[4].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(skeletonEncounterBeginningsSub[5], skeletonEncounterBeginnings[5]));
                    clipLength = skeletonEncounterBeginnings[5].length;
                    canContinue = true;
                }
            }
        }
        if (index == 2)
        {
            if (stage == 1)
            {
                int rand1 = Random.Range(0, 2);
                StartCoroutine(playSubtitlesAndAudio(wizardEncounterBeginningsSub[rand1], wizardEncounterBeginnings[rand1]));
                clipLength = wizardEncounterBeginnings[rand1].length;
                canContinue = true;
            }
            else if (stage == 2)
            {
                if (timesFacedSlime == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(wizardEncounterBeginningsSub[2], wizardEncounterBeginnings[2]));
                    clipLength = wizardEncounterBeginnings[2].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(wizardEncounterBeginningsSub[3], wizardEncounterBeginnings[3]));
                    clipLength = wizardEncounterBeginnings[3].length;
                    canContinue = true;
                }
            }
            else
            {
                if (timesFacedSlime == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(wizardEncounterBeginningsSub[4], wizardEncounterBeginnings[4]));
                    clipLength = wizardEncounterBeginnings[4].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(wizardEncounterBeginningsSub[5], wizardEncounterBeginnings[5]));
                    clipLength = wizardEncounterBeginnings[5].length;
                    canContinue = true;
                }
            }
        }
        if (index == 3)
        {
            if (stage == 1)
            {
                int rand1 = Random.Range(0, 2);
                StartCoroutine(playSubtitlesAndAudio(raiderEncounterBeginningsSub[rand1], raiderEncounterBeginnings[rand1]));
                clipLength = raiderEncounterBeginnings[rand1].length;
                canContinue = true;

            }
            else if (stage == 2)
            {
                if (timesFacedBandits == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(raiderEncounterBeginningsSub[2], raiderEncounterBeginnings[2]));
                    clipLength = raiderEncounterBeginnings[2].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(raiderEncounterBeginningsSub[3], raiderEncounterBeginnings[3]));
                    clipLength = raiderEncounterBeginnings[3].length;
                    canContinue = true;
                }
            }
            else
            {
                if (timesFacedBandits == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(raiderEncounterBeginningsSub[4], raiderEncounterBeginnings[4]));
                    clipLength = raiderEncounterBeginnings[4].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(raiderEncounterBeginningsSub[5], raiderEncounterBeginnings[5]));
                    clipLength = raiderEncounterBeginnings[5].length;
                    canContinue = true;
                }
            }
        }
        if (index == 4)
        {
            if (stage == 1)
            {
                StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[0], miscEncounterBeginnings[0]));
                clipLength = miscEncounterBeginnings[0].length;
                canContinue = true;
            }
            else if (stage == 2)
            {
                if (timesFacedProtection == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[3], miscEncounterBeginnings[3]));
                    clipLength = miscEncounterBeginnings[3].length;
                    canContinue = true;
                }
                else
                {
                    if(timesFailedToSave == 0)
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[6], miscEncounterBeginnings[6]));
                        clipLength = miscEncounterBeginnings[6].length;
                        canContinue = true;
                    }
                    else
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[7], miscEncounterBeginnings[7]));
                        clipLength = miscEncounterBeginnings[7].length;
                        canContinue = true;
                    }
                }
            }
            else
            {
                if (timesFacedProtection == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[3], miscEncounterBeginnings[3]));
                    clipLength = miscEncounterBeginnings[3].length;
                    canContinue = true;
                }
                else
                {
                    if (timesFailedToSave == 0)
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[9], miscEncounterBeginnings[9]));
                        clipLength = miscEncounterBeginnings[9].length;
                        canContinue = true;
                    }
                    else
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[10], miscEncounterBeginnings[10]));
                        clipLength = miscEncounterBeginnings[10].length;
                        canContinue = true;
                    }
                }
            }
        }
        if (index == 5)
        {
            if (stage == 1)
            {
                StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[1], miscEncounterBeginnings[1]));
                clipLength = miscEncounterBeginnings[1].length;
                canContinue = true;
            }
            else if (stage == 2)
            {
                if (timesFacedProtection == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[4], miscEncounterBeginnings[4]));
                    clipLength = miscEncounterBeginnings[4].length;
                    canContinue = true;
                }
                else
                {
                    if (timesFailedToSave == 0)
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[6], miscEncounterBeginnings[6]));
                        clipLength = miscEncounterBeginnings[6].length;
                        canContinue = true;
                    }
                    else
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[7], miscEncounterBeginnings[7]));
                        clipLength = miscEncounterBeginnings[7].length;
                        canContinue = true;
                    }
                }
            }
            else
            {
                if (timesFacedProtection == 0)
                {
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[4], miscEncounterBeginnings[4]));
                    clipLength = miscEncounterBeginnings[4].length;
                    canContinue = true;
                }
                else
                {
                    if (timesFailedToSave == 0)
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[9], miscEncounterBeginnings[9]));
                        clipLength = miscEncounterBeginnings[9].length;
                        canContinue = true;
                    }
                    else
                    {
                        StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[10], miscEncounterBeginnings[10]));
                        clipLength = miscEncounterBeginnings[10].length;
                        canContinue = true;
                    }
                }
            }
        }
        if (index == 6)
        {
            if (stage == 1)
            {
                StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[2], miscEncounterBeginnings[2]));
                clipLength = miscEncounterBeginnings[2].length;
                canContinue = true;
            }
            else if (stage == 2)
            {
                StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[5], miscEncounterBeginnings[5]));
                clipLength = miscEncounterBeginnings[5].length;
                canContinue = true;
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(miscEncounterBeginningsSub[8], miscEncounterBeginnings[8]));
                clipLength = miscEncounterBeginnings[8].length;
                canContinue = true;
            }
        }
        if (index == 7)
        {
            if (timesFacedSkeletons > timesFacedGoblins)
            {
                if (timesFacedMagicCaster == 1)
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterBeginningsSub[1], bossEncounterBeginning[1]));
                    clipLength = bossEncounterBeginning[1].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterBeginningsSub[0], bossEncounterBeginning[0]));
                    clipLength = bossEncounterBeginning[0].length;
                    canContinue = true;
                }
            }
            else if (timesFacedSkeletons < timesFacedGoblins)
            {
                if (timesFacedGoblins + timesFacedBandits >= 4)
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterBeginningsSub[3], bossEncounterBeginning[3]));
                    clipLength = bossEncounterBeginning[3].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterBeginningsSub[2], bossEncounterBeginning[2]));
                    clipLength = bossEncounterBeginning[2].length;
                    canContinue = true;
                }
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(bossEncounterBeginningsSub[4], bossEncounterBeginning[4]));
                clipLength = bossEncounterBeginning[4].length;
                canContinue = true;
            }
        }
        if (index == 8)
        {
            if (stage == 1)
            {
                int rand1 = Random.Range(0, 2);
                StartCoroutine(playSubtitlesAndAudio(levelUpSub[rand1], levelUp[rand1]));
                clipLength = levelUp[rand1].length;
                canContinue = true;
            }
            else
            {
                int rand1 = Random.Range(2, 4);
                StartCoroutine(playSubtitlesAndAudio(levelUpSub[rand1], levelUp[rand1]));
                clipLength = levelUp[rand1].length;
                canContinue = true;
            }
        }
        if (index == 9)
        {
            if (timesFailedToSave == 0 && timesFailedToSocialize == 0)
            {
                StartCoroutine(playSubtitlesAndAudio(adventureEndingsSub[0], adventureEndings[0]));
                clipLength = adventureEndings[0].length;
                canContinue = true;
            }
            else if (timesFailedToSave == 2 && timesFailedToSocialize == 1)
            {
                StartCoroutine(playSubtitlesAndAudio(adventureEndingsSub[2], adventureEndings[2]));
                clipLength = adventureEndings[2].length;
                canContinue = true;
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(adventureEndingsSub[1], adventureEndings[1]));
                clipLength = adventureEndings[1].length;
                canContinue = true;
            }
        }
    }

    IEnumerator findEndAudio(int index, int stage)
    {
        yield return null;
        canContinue = false;
        if (index == 0)
        {
            if(GameObject.Find("Player").GetComponent<PlayerController>().getHealth() - playerHPBefore == 0)
            {
                StartCoroutine(playSubtitlesAndAudio(goblinEncounterEndingsSub[0+(stage-1)*2], goblinEncounterEndings[0+(stage-1)*2]));
                clipLength = goblinEncounterEndings[0 + (stage - 1) * 2].length;
                canContinue = true;
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(goblinEncounterEndingsSub[1 + (stage - 1) * 2], goblinEncounterEndings[1 + (stage - 1) * 2]));
                clipLength = goblinEncounterEndings[1 + (stage - 1) * 2].length;
                canContinue = true;
            }
        }
        if (index == 1)
        {
            if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() - playerHPBefore == 0)
            {
                StartCoroutine(playSubtitlesAndAudio(skeletonEncounterEndingsSub[0 + (stage - 1) * 2], skeletonEncounterEndings[0 + (stage - 1) * 2]));
                clipLength = skeletonEncounterEndings[0 + (stage - 1) * 2].length;
                canContinue = true;
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(skeletonEncounterEndingsSub[1 + (stage - 1) * 2], skeletonEncounterEndings[1 + (stage - 1) * 2]));
                clipLength = skeletonEncounterEndings[1 + (stage - 1) * 2].length;
                canContinue = true;
            }
        }
        if (index == 2)
        {
            if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() - playerHPBefore == 0)
            {
                StartCoroutine(playSubtitlesAndAudio(wizardEncounterEndingsSub[0 + (stage - 1) * 2], wizardEncounterEndings[0 + (stage - 1) * 2]));
                clipLength = wizardEncounterEndings[0 + (stage - 1) * 2].length;
                canContinue = true;
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(wizardEncounterEndingsSub[1 + (stage - 1) * 2], wizardEncounterEndings[1 + (stage - 1) * 2]));
                clipLength = wizardEncounterEndings[1 + (stage - 1) * 2].length;
                canContinue = true;
            }
        }
        if (index == 3)
        {
            if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() - playerHPBefore == 0)
            {
                StartCoroutine(playSubtitlesAndAudio(raiderEncounterEndingsSub[0 + (stage - 1) * 2], raiderEncounterEndings[0 + (stage - 1) * 2]));
                clipLength = raiderEncounterEndings[0 + (stage - 1) * 2].length;
                canContinue = true;
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(raiderEncounterEndingsSub[1 + (stage - 1) * 2], raiderEncounterEndings[1 + (stage - 1) * 2]));
                clipLength = raiderEncounterEndings[1 + (stage - 1) * 2].length;
                canContinue = true;
            }
        }
        if (index == 4)
        {
            if (timesFailedToSave - timesFailedPrior == 0)
            {
                if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() == 10)
                {
                    int ind = 0 + (stage - 1) * 9;
                    if (ind > 17) ind = 9;
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                    clipLength = miscEncounterEndings[ind].length;
                    canContinue = true;
                }
                else
                {
                    int ind = 1 + (stage - 1) * 9;
                    if (ind > 17) ind = 10;
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                    clipLength = miscEncounterEndings[ind].length;
                    canContinue = true;

                }
            }
            else
            {
                int ind = 2 + (stage - 1) * 9;
                if (ind > 17) ind = 11;
                StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                clipLength = miscEncounterEndings[ind].length;
                canContinue = true;
            }
        }
        if (index == 5)
        {
            if (timesFailedToSave - timesFailedPrior == 0)
            {
                if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() == 10)
                {
                    int ind = 3 + (stage - 1) * 9;
                    if (ind > 17) ind = 12;
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                    clipLength = miscEncounterEndings[ind].length;
                    canContinue = true;
                }
                else
                {
                    int ind = 4 + (stage - 1) * 9;
                    if (ind > 17) ind = 13;
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                    clipLength = miscEncounterEndings[ind].length;
                    canContinue = true;

                }
            }
            else
            {
                int ind = 5 + (stage - 1) * 9;
                if (ind > 17) ind = 14;
                StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                clipLength = miscEncounterEndings[ind].length;
                canContinue = true;
            }
        }
        if (index == 6)
        {
            if (timesFailedToSocialize - timesFailedPrior == 0)
            {
                if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() == 10)
                {
                    int ind = 6 + (stage - 1) * 9;
                    if (ind > 17) ind = 15;
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                    clipLength = miscEncounterEndings[ind].length;
                    canContinue = true;
                }
                else
                {
                    int ind = 7 + (stage - 1) * 9;
                    if (ind > 17) ind = 16;
                    StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                    clipLength = miscEncounterEndings[ind].length;
                    canContinue = true;

                }
            }
            else
            {
                int ind = 8 + (stage - 1) * 9;
                if (ind > 17) ind = 17;
                StartCoroutine(playSubtitlesAndAudio(miscEncounterEndingsSub[ind], miscEncounterEndings[ind]));
                clipLength = miscEncounterEndings[ind].length;
                canContinue = true;
            }
        }
        if (index == 7)
        {
            if (timesFacedSkeletons > timesFacedGoblins)
            {
                if (timesFacedMagicCaster == 1)
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterEndingsSub[1], bossEncounterEndings[1]));
                    clipLength = bossEncounterEndings[1].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterEndingsSub[0], bossEncounterEndings[0]));
                    clipLength = bossEncounterEndings[0].length;
                    canContinue = true;
                }
            }
            else if (timesFacedSkeletons < timesFacedGoblins)
            {
                if (timesFacedGoblins + timesFacedBandits >=4)
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterEndingsSub[3], bossEncounterEndings[3]));
                    clipLength = bossEncounterEndings[3].length;
                    canContinue = true;
                }
                else
                {
                    StartCoroutine(playSubtitlesAndAudio(bossEncounterEndingsSub[2], bossEncounterEndings[2]));
                    clipLength = bossEncounterEndings[2].length;
                    canContinue = true;
                }
            }
            else
            {
                StartCoroutine(playSubtitlesAndAudio(bossEncounterEndingsSub[4], bossEncounterEndings[4]));
                clipLength = bossEncounterEndings[4].length;
                canContinue = true;
            }
        }
    }

    IEnumerator check(int i)
    {
        if (i >= 0 && i < 6)
        {
            yield return null;
            StartCoroutine(checkForEnemies(i));
        }
        if (i == 4 || i == 5)
        {
            yield return null;
            StartCoroutine(checkForShovables());
        }
        if (i == 6)
        {
            yield return null;
            StartCoroutine(checkForVillagers());
        }
        yield return null;
    }

    IEnumerator checkForEnemies(int index)
    {
        List<GameObject> objects = EG.GetObjects();
        bool test = false;
        bool canContinue1 = false;

        while (!canContinue1 && !canContinue)
        {
            test = true;
            foreach (GameObject obj in objects)
            {
                if (obj && (obj.layer == LayerMask.NameToLayer("Enemy") || obj.layer == LayerMask.NameToLayer("EnemyStack")))
                {
                    test = false;
                }
            }

            if(test == true)
            {
                canContinue1 = true;
                canContinue = true;
                StartCoroutine(EG.Despawn());
                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(clipLength);
                if (index > 3)
                {
                    if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() == 10)
                    {
                        timesSavedPerfectly++;
                    }
                    else
                    {
                        timesSaved++;
                        int hp = GameObject.Find("Player").GetComponent<PlayerController>().getHealth();
                        if(hp + 3 > 10)
                        {
                            GameObject.Find("Player").GetComponent<PlayerController>().changeHealth(10 - hp);
                        }
                        else
                        {
                            GameObject.Find("Player").GetComponent<PlayerController>().changeHealth(3);
                        }
                        GameObject spawn = Instantiate(healUp, PlayerLevels[0].transform.position, Quaternion.identity);
                    }
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(0f);
    }

    IEnumerator checkForShovables()
    {
        List<GameObject> objects = EG.GetObjects();
        bool test = false;
        bool canContinue1 = false;

        while (!canContinue1 && !canContinue)
        {
            yield return null;
            test = true;
            foreach (GameObject obj in objects)
            {
                if (obj && obj.layer == LayerMask.NameToLayer("ShovableObject"))
                {
                    test = false;
                }
            }

            if (test == true)
            {
                canContinue1 = true;
                timesFailedToSave++;
                StartCoroutine(EG.Despawn());
            }
            yield return null;
        }
        canContinue = true;
        yield return new WaitForSeconds(0f);
    }

    IEnumerator checkForVillagers()
    {
        List<GameObject> objects = EG.GetObjects();
        bool test = false;
        bool canContinue1 = false;

        while (!canContinue1 && !canContinue)
        {
            test = true;
            villCnt = 0;
            foreach (GameObject obj in objects)
            {
                if (obj && obj.layer == LayerMask.NameToLayer("Villager"))
                {
                    test = false;
                    villCnt++;
                }
            }

            villagersLeft.GetComponent<Text>().text = (villCnt + "/5");

            if (test == true)
            {
                canContinue = true;
                canContinue1 = true;
                StartCoroutine(EG.Despawn());
                GameObject.Find("Player").GetComponent<PlayerController>().SwapWeapon(1);
                GameObject.Find("Player").GetComponent<PlayerController>().canSwap = true;
                counter.SetActive(false);
                villagersLeft.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(clipLength);
                if (GameObject.Find("Player").GetComponent<PlayerController>().getHealth() != 10)
                {
                    int hp = GameObject.Find("Player").GetComponent<PlayerController>().getHealth();
                    if (hp + 3 > 10)
                    {
                        GameObject.Find("Player").GetComponent<PlayerController>().changeHealth(10 - hp);
                    }
                    else
                    {
                        GameObject.Find("Player").GetComponent<PlayerController>().changeHealth(3);
                    }
                    GameObject spawn = Instantiate(healUp, PlayerLevels[0].transform.position, Quaternion.identity);
                }
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
