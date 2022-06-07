using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCoop : MonoBehaviour
{

    public static ToggleCoop instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject textSolo, textCoop, StoreCoop;
    [HideInInspector]
    public bool coop = false;
    bool destroy = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!destroy) { 
            DontDestroyOnLoad(StoreCoop);
            destroy = true;
        }
        if (coop)
        {
            textCoop.SetActive(true);
            textSolo.SetActive(false);
        } else
        {
            textCoop.SetActive(false);
            textSolo.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoToggleCoop()
    {
        textSolo.SetActive(!textSolo.activeSelf);
        textCoop.SetActive(!textCoop.activeSelf);
        coop = !coop;
    }
}
