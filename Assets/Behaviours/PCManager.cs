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
    public float max_meteor = 4;
    public float meteor_max_spawn_delay = 5f;
    public float meteor_min_spawn_delay = 1f;
    public float cataclysm_duration = 10;
   
    private bool cataclysm_active = false;
    private int meteor_count = 0;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // debug
        {
            IncrementVirusBar();
        }

        if (cataclysm_active)
        {
            RunCataclysm(); // simulate cataclysm event
        }
    }


    private void Start()
    {
        virus_indicator.SetActive(false); // setup indictors
        scan_indicator.SetActive(true);
    }


    public void StartVirusIndicator()
    {
        CancelInvoke();

        virus_indicator.SetActive(true); // show skull
        scan_indicator.SetActive(false);

        Invoke("EndVirusIndicator", anim_duration); // stop animation after time
    }


    public void IncrementVirusBar()
    {
        if (!cataclysm_active)
        {
            virus_bar.value += virus_bar_increment_percent; // increment bar
            StartVirusIndicator();

            if (virus_bar.value >= virus_bar.maxValue) // if maxed
            {
                TriggerCataclysm(); // start the cataclysm
            }
        }
    }


    private void TriggerCataclysm()
    {
        if (!cataclysm_active)
        {
            CancelInvoke(); // cancel indicator disable if cataclysm started
        }

        Invoke("EndCataclysm", cataclysm_duration); // end cataclysm after some time
        cataclysm_active = true;
    }


    private void RunCataclysm()
    {
        virus_indicator.SetActive(true);//always show skull during cataclysm (bit hacky)
        scan_indicator.SetActive(false);
        
        if (meteor_count < max_meteor)
        {
            Invoke("SpawnMeteor", Random.Range(meteor_min_spawn_delay, meteor_max_spawn_delay)); // spawn meteor if we haven't reached the limit
            ++meteor_count;
        }
    }


    private void SpawnMeteor()
    {
        GameObject meteor_clone = Instantiate(meteor_prefab);
        Vector2 random_circle_location = Random.insideUnitCircle * meteor_spawn_point.spawn_radius;

        meteor_clone.transform.position = new Vector3(random_circle_location.x,
            meteor_spawn_point.transform.position.y, random_circle_location.y); // spawn meteor at random position

        --meteor_count; //decrement max meteor when finished
    }


    private void EndCataclysm()
    {
        CancelInvoke();
        cataclysm_active = false;
        meteor_count = 0;

        virus_bar.value = virus_bar.minValue;
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);     
    }


    private void EndVirusIndicator()
    {
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);
    }

}
