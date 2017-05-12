using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCManager : MonoBehaviour
{
    public GameObject virus_indicator;
    public GameObject scan_indicator;

    public Slider virus_bar;
    public float virus_bar_increment_percent = 10f;
    public float anim_duration = 4f;


    private void Start()
    {
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);
    }


    public void StartVirusIndicator()
    {
        virus_indicator.SetActive(true);
        scan_indicator.SetActive(false);
    }


    public void IncrementVirusBar()
    {
        virus_bar.value += virus_bar_increment_percent;

        if(virus_bar.value >= virus_bar.maxValue)//if maxed
        {
            TriggerCataclysm();//start the cataclysm
        }
    }


    private void TriggerCataclysm()
    {
        virus_bar.value = virus_bar.minValue;
    }


    private IEnumerator EndVirusIndicator()
    {
        yield return new WaitForSeconds(anim_duration);
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);
    }
}
