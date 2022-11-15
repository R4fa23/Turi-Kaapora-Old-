using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteFire : MonoBehaviour
{
    public GameObject target;

    public float vel;

    bool toDisapear;
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Slerp(transform.position, target.transform.position, Time.deltaTime * vel);

            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                target.GetComponentInParent<FortFire>().AddQuantity();
                gameObject.SetActive(false);
            }
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * vel);
            if (!toDisapear)
            {
                toDisapear = true;
                StartCoroutine(Disapear());
            }
        } 
    }

    IEnumerator Disapear()
    {
        yield return new WaitForSeconds(15);
        gameObject.SetActive(false);
    }

	private void OnDisable()
	{
        target = null;
        toDisapear = false;
	}
}
