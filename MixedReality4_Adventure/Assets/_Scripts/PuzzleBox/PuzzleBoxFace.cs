﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// @author: David Liebemann
/// </summary>
[RequireComponent(typeof(Renderer))]
public class PuzzleBoxFace : MonoBehaviour {

    public UnityAction<PuzzleBoxFace> OnCorrectTouchDetected;

    [SerializeField]
    private int id;

    [SerializeField]
    private int touchCount;

    //public int TouchCount { get { return touchCount; } }

    public int ID { get { return id; } }

    [SerializeField]
    private Texture2D puzzleMasterIcon;

    [SerializeField]
    private Texture2D image;

    private Material faceMaterial;
    public bool WasCorrectlyTouched { get; private set; }
    public bool WasFinalized { get; private set; }

    private Color CorrectColor = Color.green;
    private Color WrongColor = Color.red;
    private Color WaitingColor = Color.yellow;
    private Color NeutralColor = Color.white;


	// Use this for initialization
	void Start () {
        faceMaterial = GetComponent<Renderer>().material;
        WasCorrectlyTouched = false;
        WasFinalized = false;
    }

    private void OnMouseDown()
    {
        if (Application.isEditor && Input.anyKey)
        {
            OnCorrectTouchCount();
        }else if (!WasCorrectlyTouched)
        {
            StartCoroutine(TollerantTouchCheck());
        } 
    }

    private IEnumerator TollerantTouchCheck()
    {
        float elapsedTime = 0.0f;
        float tollerance = 0.25f;
        while(elapsedTime < tollerance)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (Input.touchCount == touchCount && !WasCorrectlyTouched)
        {
            OnCorrectTouchCount();

        }
        else if (Input.touchCount != touchCount && !WasCorrectlyTouched)
            StartCoroutine(OnWrongTouchCount());
    }

    private void OnCorrectTouchCount()
    {
        Debug.Log("Correct: " + touchCount);
        if (null != OnCorrectTouchDetected)
        {
            OnCorrectTouchDetected.Invoke(this);
        }  
    }

    public IEnumerator OnWrongTouchCount()
    {
        Debug.Log("Wrong: " + touchCount);
        WasCorrectlyTouched = false;
        WasFinalized = false;
        float elapsedTime = 0.0f;
        float allertedTime = 0.5f;
        faceMaterial.color = WrongColor;
        while (elapsedTime < allertedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!WasCorrectlyTouched)
            ResetTouchable();

        yield return null;
    }

    public void ResetTouchable()
    {
        WasCorrectlyTouched = false;
        WasFinalized = false;
        faceMaterial.color = NeutralColor;
    }

    public void OnRegisterNetworkCorrectTouch()
    {
        Debug.Log("Network: " + touchCount);
        if (!WasCorrectlyTouched)
        {
            WasCorrectlyTouched = true;
            faceMaterial.color = WaitingColor;
        }
        
    }

    public void OnFinalizeCorrectTouch()
    {
        Debug.Log("Finalize: " + touchCount);
        WasCorrectlyTouched = true;
        WasFinalized = true;
        faceMaterial.color = CorrectColor;
    }

    public void SetNumberVisibility(bool visible)
    {
        if (null == faceMaterial)
            faceMaterial = GetComponent<Renderer>().material;

        if (visible)
        {
            faceMaterial.mainTexture = image;
        }
        else
        {
            faceMaterial.mainTexture = puzzleMasterIcon;
        }
    }
}
