using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Camp : MonoBehaviour
{
    public int waves;
    int[] enemyPerWave;
    public int[] melee;
    public int[] range;
    public int[] incendiary;
    public int[] hunter;
    public int[] lumberjack;

    int meleeCount;
    int rangeCount;
    int incendiaryCount;
    int hunterCount;
    int lumberjackCount;

    [HideInInspector]
    public SOCamp soCamp;
    public SOSave soSave;
    public SOFort soFort;
    public SOPlayer soPlayer;
    bool completed;
    public GameObject doors;
    [HideInInspector]
    public List<GameObject> door;
    public GameObject cabins;
    [HideInInspector]
    public List<GameObject> cabin;
    public GameObject firstEnemies;
    [HideInInspector]
    public List<GameObject> firstEnemy;
    public GameObject triggers;
    [HideInInspector]
    public List<GameObject> trigger;
    public FortFire fireBoitata;
    int enemyCount;
    int indexCabin;
    bool firstEnable;
    float countBlueFirePercent;
    public GameObject bonfire;
    public UnityEvent OnEndCamp;

    void Awake()
    {
        SetLists();
        SetEnemies();
        soCamp = (SOCamp)ScriptableObject.CreateInstance(typeof(SOCamp));
        SetConfiguration();
    }

    void SetEnemies()
    {
        int countage = 0;

        enemyPerWave = new int[waves+1];

        if(melee.Length != waves) melee = new int[waves];
        if(range.Length != waves) range = new int[waves];
        if(incendiary.Length != waves) incendiary = new int[waves];
        if(hunter.Length != waves) hunter = new int[waves];
        if(lumberjack.Length != waves) lumberjack = new int[waves];

        for(int i = 0; i < waves; i++) {
            countage += melee[i];
            countage += range[i];
            countage += incendiary[i];
            countage += hunter[i];
            countage += lumberjack[i];
            enemyPerWave[i+1] = countage;
            countage = 0;
        }

        enemyPerWave[0] = firstEnemy.Count;

        float totalEnemies = 0;

        for(int i = 0; i < enemyPerWave.Length; i++) {
            totalEnemies += enemyPerWave[i];
        }

        countBlueFirePercent = 1/totalEnemies;

        if(fireBoitata != null) fireBoitata.quantityToAdd = countBlueFirePercent;

    }

    void SetLists()
    {
        door.Clear();
        cabin.Clear();
        firstEnemy.Clear();
        trigger.Clear();
        if(fireBoitata != null) fireBoitata.ResetToRedFire();

        for(int i = 0; i < doors.transform.childCount; i++) {
            door.Add(doors.transform.GetChild(i).gameObject);
            if(this.gameObject.tag != "Fort")
                door[i].GetComponent<BlueFireWall>().EndFire();
        }

        for(int i = 0; i < cabins.transform.childCount; i++) {
            cabin.Add(cabins.transform.GetChild(i).gameObject);
            cabin[i].SetActive(true);
            if(bonfire != null) cabin[i].GetComponent<Cabin>().bonfire = bonfire;
        }

        for(int i = 0; i < firstEnemies.transform.childCount; i++) {
            firstEnemy.Add(firstEnemies.transform.GetChild(i).gameObject);
            firstEnemy[i].SetActive(true);
        }

        for(int i = 0; i < triggers.transform.childCount; i++) {
            trigger.Add(triggers.transform.GetChild(i).gameObject);
            trigger[i].SetActive(true);
        }

    }

    private void Start() 
    {
        OnEnable();
    }
    

    void SetConfiguration()
    {
        soCamp.waves = waves + 1;
        soCamp.enemyPerWaves = enemyPerWave;
        soCamp.actualWave = 0;
        soCamp.killCount = 0;

    }

    void StartCamp()
    {
        foreach(GameObject d in door)
        {
            d.GetComponent<BlueFireWall>().StartFire();
        }

        if(firstEnemy.Count > 0)
        {
            foreach(GameObject e in firstEnemy)
            {
                e.GetComponent<EnemyManager>().soEnemy.Summon();
            }
        }
        else
        {
            soCamp.DieEnemy();
        }
    }

    void SummonEnemies()
    {
        if(enemyCount < enemyPerWave[soCamp.actualWave])
        {
            if(meleeCount > 0)
            {
                meleeCount--;
                cabin[indexCabin].GetComponent<Cabin>().SummonEnemy("melee");
            }
            else if(rangeCount > 0)
            {
                rangeCount--;
                cabin[indexCabin].GetComponent<Cabin>().SummonEnemy("range");
            }
            else if(incendiaryCount > 0)
            {
                incendiaryCount--;
                cabin[indexCabin].GetComponent<Cabin>().SummonEnemy("incendiary");
            }
            else if(hunterCount > 0)
            {
                hunterCount--;
                cabin[indexCabin].GetComponent<Cabin>().SummonEnemy("hunter");
            }
            else if(lumberjackCount > 0)
            {
                lumberjackCount--;
                cabin[indexCabin].GetComponent<Cabin>().SummonEnemy("lumberjack");
            }
            


            indexCabin++;
            if(indexCabin >= cabin.Count) indexCabin = 0;
            enemyCount++;
            StartCoroutine(SummonDelay());
        }
    }

    void NextWave()
    {
        meleeCount = melee[soCamp.actualWave - 1];
        rangeCount = range[soCamp.actualWave - 1];
        incendiaryCount = incendiary[soCamp.actualWave - 1];
        hunterCount = hunter[soCamp.actualWave - 1];
        lumberjackCount = lumberjack[soCamp.actualWave - 1];
        enemyCount = 0;
        SummonEnemies();
    }

    void ConclusionCamp()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Vozes/Risada", transform.position);

        enemyCount = 0;
        foreach(GameObject d in door)
        {
            d.GetComponent<BlueFireWall>().EndFire();
        }

        foreach(GameObject t in trigger)
        {
            t.SetActive(false);
        }

        if(gameObject.tag == "Fort") soPlayer.LevelUp();
        
        completed = true;
        soFort.CompleteSpace();
        soPlayer.soPlayerHealth.RecoverHealth();
        OnEndCamp?.Invoke();
    }

    IEnumerator SummonDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SummonEnemies();
    }

    void EnemyDied()
    {
        soCamp.DieEnemy();
    }

    void CompleteAnticipated()
    {
        if(!completed && gameObject.tag != "Fort")
        {
            enemyCount = 0;
            foreach(GameObject d in door)
            {
                d.GetComponent<BlueFireWall>().EndFire();
            }

            foreach(GameObject t in trigger)
            {
                t.SetActive(false);
            }

            foreach(GameObject f in firstEnemy)
            {
                f.SetActive(false);
            }

            foreach(GameObject c in cabin)
            {
                c.GetComponent<Cabin>().DisableAll();
            }

            completed = true;
        }
    }

    void CompleteFort()
    {
        if(!completed && gameObject.tag == "Fort")
        {
            enemyCount = 0;
            foreach(GameObject d in door)
            {
                d.GetComponent<BlueFireWall>().EndFire();
            }

            if(fireBoitata != null) fireBoitata.SetToBlueFire();

            foreach(GameObject t in trigger)
            {
                t.SetActive(false);
            }

            foreach(GameObject f in firstEnemy)
            {
                f.SetActive(false);
            }
            
            foreach(GameObject c in cabin)
            {
                c.GetComponent<Cabin>().DisableAll();
            }
            completed = true;
        }
    }

    void Restart()
    {
        if(!completed)
        {
            StopAllCoroutines();
            soCamp.actualWave = 0;
            soCamp.killCount = 0;
            enemyCount = 0;
            indexCabin = 0;

            if(fireBoitata != null) fireBoitata.ResetToRedFire();

            foreach(GameObject t in trigger)
            {
                t.SetActive(true);
            }

            if(gameObject.tag != "Fort")
            {
                foreach(GameObject d in door)
                {
                    d.GetComponent<BlueFireWall>().EndFire();
                }
            }

            foreach(GameObject c in cabin)
            {
                c.GetComponent<Cabin>().Restart();
            }

            foreach(GameObject f in firstEnemy)
            {
                f.SetActive(true);
                f.GetComponent<EnemyMove>().transform.position = f.GetComponent<EnemyMove>().firstLocal;
                f.GetComponent<EnemyMove>().detected = false;
                f.GetComponent<EnemyMove>().soEnemy.state = SOEnemy.State.STOPPED;
                //f.GetComponent<EnemyMove>().Restart();
            }
        }

    }


    public void OnEnable()
    {
        if(firstEnable)
        {
            soCamp.EnterCampEvent.AddListener(StartCamp);
            soCamp.NextWaveEvent.AddListener(NextWave);
            soCamp.ConclusionCampEvent.AddListener(ConclusionCamp);
            soSave.RestartEvent.AddListener(Restart);
            foreach(GameObject e in firstEnemy)
            {
                e.GetComponent<EnemyManager>().soEnemy.DieEvent.AddListener(EnemyDied);
                if (bonfire != null) e.GetComponent<EnemyManager>().bonfire = bonfire;
            }
            soFort.CompleteChallengesEvent.AddListener(CompleteAnticipated);
            soFort.CompleteFortEvent.AddListener(CompleteFort);
        }
        firstEnable = true;
    }
    public void OnDisable()
    {
        soCamp.EnterCampEvent.RemoveListener(StartCamp);
        soCamp.NextWaveEvent.RemoveListener(NextWave);
        soCamp.ConclusionCampEvent.RemoveListener(ConclusionCamp);
        soSave.RestartEvent.RemoveListener(Restart);
        foreach(GameObject e in firstEnemy)
        {
                e.GetComponent<EnemyManager>().soEnemy.DieEvent.RemoveListener(EnemyDied);
        }
        soFort.CompleteChallengesEvent.RemoveListener(CompleteAnticipated);
        soFort.CompleteFortEvent.RemoveListener(CompleteFort);
    }
}
