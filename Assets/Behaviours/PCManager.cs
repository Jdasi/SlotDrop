using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCManager : MonoBehaviour
{
    
    [Header("Indicators")]
    public GameObject virus_indicator;
    public float anim_duration = 4f;
    public GameObject scan_indicator;
    public Slider virus_bar;
    public float virus_bar_increment_percent = 10f;

    [Header("Cataclysm")]
    public GameObject meteor_prefab;
    public SpawnAreaCircle meteor_spawn_point;
    [Range(0,1)]
    public float meteor_spawn_chance_percentage = 0.25f;
    public float cataclysm_duration = 10;
   
    private bool cataclysm_active = false;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            IncrementVirusBar();
        }
        if(cataclysm_active)
        {
            RunCataclysm();
        }
    }


    private void Start()
    {
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);
    }


    public void StartVirusIndicator()
    {
        virus_indicator.SetActive(true);
        scan_indicator.SetActive(false);
        StartCoroutine(EndVirusIndicator());
    }


    public void IncrementVirusBar()
    {
        virus_bar.value += virus_bar_increment_percent;
        StartVirusIndicator();
        if (virus_bar.value >= virus_bar.maxValue)//if maxed
        {
            TriggerCataclysm();//start the cataclysm
        }
    }


    private void TriggerCataclysm()
    {
        StartCoroutine(EndCataclysm());
        cataclysm_active = true;
    }


    private void RunCataclysm()
    {
        virus_indicator.SetActive(true);
        scan_indicator.SetActive(false);
        if (Random.Range(0, 1) < meteor_spawn_chance_percentage)
        {
            GameObject meteor_clone = Instantiate(meteor_prefab);
            Vector2 random_circle_location = Random.insideUnitCircle * meteor_spawn_point.spawn_radius;

            meteor_clone.transform.position = new Vector3(random_circle_location.x,
                meteor_spawn_point.transform.position.y, random_circle_location.y);
        }
    }


    private IEnumerator EndCataclysm()
    {
        yield return new WaitForSeconds(cataclysm_duration);
        cataclysm_active = false;
        virus_bar.value = virus_bar.minValue;
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);
    }


    private IEnumerator EndVirusIndicator()
    {
        yield return new WaitForSeconds(anim_duration);
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);
    }
}
