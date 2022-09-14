using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    public int waves;
    public int[] enemyPerWave;

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
    public GameObject showThings;
    [HideInInspector]
    public List<GameObject> showThing;
    int enemyCount;
    int indexCabin;
    bool firstEnable;
    void Awake()
    {
        SetLists();
        if(waves != enemyPerWave.Length) enemyPerWave = new int[waves];
        if(firstEnemy.Count > 0) enemyPerWave[0] = firstEnemy.Count;
        soCamp = (SOCamp)ScriptableObject.CreateInstance(typeof(SOCamp));
        SetConfiguration();
    }

    void SetLists()
    {
        door.Clear();
        cabin.Clear();
        firstEnemy.Clear();
        trigger.Clear();
        showThing.Clear();

        for(int i = 0; i < doors.transform.childCount; i++) {
            door.Add(doors.transform.GetChild(i).gameObject);
            door[i].SetActive(false);
        }

        for(int i = 0; i < cabins.transform.childCount; i++) {
            cabin.Add(cabins.transform.GetChild(i).gameObject);
            cabin[i].SetActive(true);
        }

        for(int i = 0; i < firstEnemies.transform.childCount; i++) {
            firstEnemy.Add(firstEnemies.transform.GetChild(i).gameObject);
            firstEnemy[i].SetActive(true);
        }

        for(int i = 0; i < triggers.transform.childCount; i++) {
            trigger.Add(triggers.transform.GetChild(i).gameObject);
            trigger[i].SetActive(true);
        }

        
        for(int i = 0; i < showThings.transform.childCount; i++) {
            showThing.Add(showThings.transform.GetChild(i).gameObject);
            showThing[i].SetActive(false);
            
        }
        
    }

    private void Start() 
    {
        OnEnable();
    }
    

    void SetConfiguration()
    {
        soCamp.waves = waves;
        soCamp.enemyPerWaves = enemyPerWave;
        soCamp.actualWave = 0;
        soCamp.killCount = 0;

    }

    void StartCamp()
    {
        foreach(GameObject d in door)
        {
            d.SetActive(true);
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
            SummonEnemies();
        }
    }

    void SummonEnemies()
    {
        if(enemyCount < enemyPerWave[soCamp.actualWave])
        {
            cabin[indexCabin].GetComponent<Cabin>().SummonEnemy();
            indexCabin++;
            if(indexCabin >= cabin.Count) indexCabin = 0;
            enemyCount++;
            StartCoroutine(SummonDelay());
        }
    }

    void NextWave()
    {
        enemyCount = 0;
        SummonEnemies();
    }

    void ConclusionCamp()
    {
        enemyCount = 0;
        foreach(GameObject d in door)
        {
            d.SetActive(false);
        }

        foreach(GameObject s in showThing)
        {
            s.SetActive(true);
        }

        completed = true;
        soFort.CompleteSpace();
        soPlayer.soPlayerHealth.RecoverHealth();
    }

    IEnumerator SummonDelay()
    {
        yield return new WaitForSeconds(1);
        SummonEnemies();
    }

    void EnemyDied()
    {
        soCamp.DieEnemy();
    }

    void Restart()
    {
        if(!completed)
        {
            soCamp.actualWave = 0;
            soCamp.killCount = 0;
            enemyCount = 0;
            indexCabin = 0;

            
            foreach(GameObject t in trigger)
            {
                t.SetActive(true);
            }
            

            foreach(GameObject d in door)
            {
                d.SetActive(false);
            }
            foreach(GameObject c in cabin)
            {
                c.GetComponent<Cabin>().Restart();
            }
            foreach(GameObject f in firstEnemy)
            {
                f.SetActive(true);
                f.GetComponent<EnemyMove>().Restart();
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
            }
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
    }
}
