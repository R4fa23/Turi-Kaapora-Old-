using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class FireTrap : MonoBehaviour
{
    public SOPlayer soPlayer;
    bool fired;
    [SerializeField] VisualEffect vfxFire;
    //[SerializeField] bool updateSize;
    [SerializeField] BoxCollider box;

    [Range(0, 2)]
    public float quantity = 1;

    public float fireRateMax = 32;
    public float smokeRateMax = 100;
    public float decalRateMax = 32;
    public float cindersRateMax = 16;

    public float fireRateAtual;
    public float smokeRateAtual;
    public float decalRateAtual;
    public float cindersRateAtual;

    public Vector2 size;

    private void Start()
    {
        FireBox();
    }

    private void OnValidate()
    {        
        FireBox();  
        RatesPercent();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!fired)
            {
                soPlayer.soPlayerHealth.Burned(3);
                fired = true;
                StartCoroutine(CooldownFire());
            }            
        }
    }

    IEnumerator CooldownFire()
    {
        yield return new WaitForSeconds(0.5f);
        fired = false;
    }

    public void FireBox()
    {
        box.size = new Vector3(size.x, 1, size.y);
        vfxFire.SetVector3("Box Area", new Vector3(box.size.x, 0.1f, box.size.z));
        vfxFire.SetVector3("Box Center", new Vector3(box.center.x, 0, box.center.z));
        
    }

    public void RatesPercent()
    {     
        fireRateAtual = quantity * fireRateMax;
        smokeRateAtual = quantity * smokeRateMax;
        decalRateAtual = quantity * decalRateMax;
        cindersRateAtual = quantity * cindersRateMax;

        vfxFire.SetFloat("Flame Rate", fireRateAtual);
        vfxFire.SetFloat("Smoke Rate", smokeRateAtual);
        vfxFire.SetFloat("Decal Rate", decalRateAtual);
        vfxFire.SetFloat("Cinders Rate", cindersRateAtual);
    }


}
