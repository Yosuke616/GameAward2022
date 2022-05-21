using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperChange : MonoBehaviour
{
    [SerializeField]
    private Image _paperChange;
    [SerializeField]
    private float FADESPEED = 0.1f;

    private float valueSpeed;

    private bool _isFadeIn, _isFadeOut;

    public Camera openingCamera;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _paperChange.gameObject.SetActive(false);
        _isFadeOut = false;
        _isFadeIn = false;
        valueSpeed = 0.0f;

        openingCamera.enabled = true;
        mainCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFadeOut)
        {
            FadeOut();
        }
        else if (_isFadeIn)
        {
            FadeIn();
        }
    }

    public void FadeStart()
    {
        _paperChange.gameObject.SetActive(true);

        _isFadeOut = true;
    }

    private void FadeOut()
    {
        valueSpeed += FADESPEED * Time.deltaTime;
        _paperChange.material.SetFloat("_Value", valueSpeed);

        if (valueSpeed >= 1.0f)
        {
            valueSpeed = 1.0f;
            _isFadeOut = false;
            _isFadeIn = true;

            openingCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }

    private void FadeIn()
    {
        valueSpeed -= FADESPEED * Time.deltaTime;
        _paperChange.material.SetFloat("_Value", valueSpeed);

        if (valueSpeed <= 0.0f)
        {
            valueSpeed = 0.0f;
            _paperChange.gameObject.SetActive(false);
            _isFadeIn = false;
        }
    }
}
