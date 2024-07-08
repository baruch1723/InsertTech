using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _progressText;

    public void SetProgress(float value)
    {
        _slider.value = value;
        SetText("Loading progress: " + (value * 100) + "%");
    }
    
    public void SetText(string value)
    {
        _progressText.text = value;
    }
    
    public void Init()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
