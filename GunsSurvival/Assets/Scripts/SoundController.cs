using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance
    {
        get
        {
            if (!instance) return null;
            return instance;
        }
    }
    private static SoundController instance = null;

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
