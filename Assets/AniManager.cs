using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class AniManager : MonoBehaviour
{
    public bool is_start;
    public AudioSource audiosource;
    public AudioClip[] clip;
    public int currentIndex;
    public Transform ani_Start;
    public Transform ani_pos1;
    public Transform ani_pos2;
    public GameObject Worker;
    public GameObject XROrigin;
    public float Move_Speed = 0.3f;
    
    GameObject worker;
    public SceneTest scenechange;
    public bool is_start_coroutine = false;
    public GameObject Start_Button;
    // Start is called before the first frame update
    void Start()
    {
        is_start = false;
        currentIndex = 0;
        worker = Instantiate(Worker, ani_Start.position, ani_Start.transform.rotation);
    }
    public void is_Start()
    {
        is_start=true;
        StartCoroutine(Start_animation());
        Destroy(Start_Button);
    }
    IEnumerator Start_animation()
    {
        yield return null;
        worker.transform.position = Vector3.MoveTowards(worker.transform.position, ani_pos1.position, Move_Speed * Time.deltaTime);
        worker.GetComponent<Animator>().SetBool("isWalk", true);
        audiosource = worker.GetComponent<AudioSource>();
        if(worker.transform.position != ani_pos1.position)
        {
            StartCoroutine(Start_animation());
        }
        else
        {
            worker.transform.LookAt(XROrigin.transform.position);
            worker.GetComponent<Animator>().SetBool("isWalk", false);
        }
        if (!is_start_coroutine)
        {
            StartCoroutine(Animation_1());
        }
    }
    private IEnumerator Animation_1()
    {
        is_start_coroutine = true;
        yield return null;
        if(currentIndex == 0)
        {
            audiosource.clip = clip[currentIndex];
        }
        audiosource.Play();
        StartCoroutine(WaitforEndClip(1.0f));
        
    }
    private IEnumerator Animation_2() 
    { 
        yield return null;
        worker.transform.LookAt(ani_pos2);
        audiosource.Play();
        StartCoroutine(WaitforEndClip(1.0f));
    }

    private IEnumerator WaitforEndClip(float add_time = 0)
    {
        yield return new WaitForSeconds(audiosource.clip.length+add_time);
        currentIndex++;
        if(currentIndex==clip.Length)
        {
            Destroy(audiosource);
            scenechange.is_next = true;
        }
        else
        {
            audiosource.clip = clip[currentIndex];
            if (currentIndex <= 2)
            {

                StartCoroutine(Animation_1());
            }
            else if (currentIndex > 2)
            {
                StartCoroutine(Animation_2());
            }
        }
        

    }
}
