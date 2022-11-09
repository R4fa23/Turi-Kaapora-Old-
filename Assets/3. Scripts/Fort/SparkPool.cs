using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkPool : MonoBehaviour
{
	public SOPlayer soPlayer;
    public GameObject[] sparks;

	private void Start()
	{
		foreach (GameObject s in sparks)
		{
			s.SetActive(false);
		}
	}

	void SummonSpark(GameObject enemy)
	{
		int index = 0;
		bool stop = false;

		do
		{
			if (sparks[index].activeInHierarchy)
			{
				index++;
			}
			else
			{
				sparks[index].transform.position = enemy.transform.position;
				sparks[index].SetActive(true);
				if(enemy.GetComponent<EnemyManager>().bonfire != null) sparks[index].GetComponent<RouteFire>().target = enemy.GetComponent<EnemyManager>().bonfire;
				stop = true;
			}

			if (index >= sparks.Length) stop = true;
		} while (!stop);
	}

	private void OnEnable()
	{
		soPlayer.soPlayerAttack.EnemyDieEvent.AddListener(SummonSpark);
	}

	private void OnDisable()
	{
		soPlayer.soPlayerAttack.EnemyDieEvent.RemoveListener(SummonSpark);
	}
}
