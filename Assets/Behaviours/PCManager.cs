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
    public float max_meteor = 4;
    public float meteor_max_spawn_delay = 5f;
    public float meteor_min_spawn_delay = 1f;
    public float cataclysm_duration = 10;
    public MeteorManager meteor_manager;
    public Light scene_light;
    public Color light_alarm_colour;
    public float alarm_light_intensity = 1;

    private float light_default_intensity = 0;
    private Color light_default_colour;     
    private bool cataclysm_active = false;
    private int meteor_count = 0;


    private void Update()
    {
        UpdateDebugControls();
        
        if (cataclysm_active)
        {
            RunCataclysm(); // simulate cataclysm event
        }
    }


    private void UpdateDebugControls()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // debug
        {
            IncrementVirusBar();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // debug
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//if in editor exit play mode
#else
        Application.Quit();//if a build quit the application
#endif       
        }
    }


    private void Start()
    {
        virus_indicator.SetActive(false); // setup indictors
        scan_indicator.SetActive(true);
        light_default_colour = scene_light.color;
        light_default_intensity = scene_light.intensity;
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

        GameObject.FindObjectOfType<AudioManager>().PlayOneShot("alarm");
        scene_light.color = light_alarm_colour;
        scene_light.intensity = alarm_light_intensity;
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
        meteor_manager.SpawnMeteor();
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
        scene_light.color = light_default_colour;
        scene_light.intensity = light_default_intensity;    
    }


    private void EndVirusIndicator()
    {
        virus_indicator.SetActive(false);
        scan_indicator.SetActive(true);
    }

}
