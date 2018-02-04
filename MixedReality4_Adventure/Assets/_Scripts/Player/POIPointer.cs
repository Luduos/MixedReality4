﻿using UnityEngine;

/// <summary>
/// @author: David Liebemann
/// </summary>
public class POIPointer : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer NormalSprite;

    [SerializeField]
    private SpriteRenderer BigSprite;

    [SerializeField]
    private TextMesh NameMesh;

    [SerializeField]
    private float PointerDistanceToPlayer = 2.0f;

    [SerializeField]
    private float DisappearDistance = 2.0f;

    [SerializeField]
    float DirectionMatchPercentage = 0.96f;

    private string poiName;
    public string POIName { get { return poiName; } set { poiName = value; NameMesh.text = value; } }
    public Vector2 UnityTarget { get; set; }
    public bool IsActive { get; set; }

    public int ID { get; set; }

    public POI poiObject { get; set; }

    private Color normalColor = new Color(0f, 0.462745f, 1f, 0.917648f);

    private void Start()
    {
    }

    public void OnPlayerPositionChanged(Vector2 PlayerPosition)
    {
        UnityTarget = poiObject.transform.localPosition;
        Vector3 dir = new Vector3(UnityTarget.x - PlayerPosition.x, UnityTarget.y - PlayerPosition.y, 0.0f);
        if (dir.magnitude > DisappearDistance * PointerDistanceToPlayer)
        {
            this.gameObject.SetActive(true);
            dir = PointerDistanceToPlayer * dir.normalized;
            this.transform.localPosition = dir;
            this.transform.localRotation = Quaternion.LookRotation(Vector3.forward, dir);
        }
        else
        {
            this.gameObject.SetActive(false);
        }     
    }

    public void OnPlayerRotationChanged(Vector2 forward)
    {
        float dotForwardTarget = Vector2.Dot(this.transform.up, forward);
        if(dotForwardTarget > DirectionMatchPercentage)
        {
            BigSprite.gameObject.SetActive(true);
            NormalSprite.gameObject.SetActive(false);
            IsActive = true;
        }
        else
        {
            BigSprite.gameObject.SetActive(false);
            NormalSprite.gameObject.SetActive(true);
            IsActive = false;
        }
    }

    public void OnMarkSelected()
    {
        NormalSprite.color = Color.red;
        BigSprite.color = Color.red;
    }

    public void OnMakeTarget()
    {
        NormalSprite.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        NormalSprite.color = Color.green;
        BigSprite.color = Color.green;
        BigSprite.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        poiObject.GetComponent<Renderer>().material.color = Color.green;
    }

    public void OnMakeNormal()
    {
        NormalSprite.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        NormalSprite.color = normalColor;
        BigSprite.color = normalColor;
        BigSprite.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        poiObject.GetComponent<Renderer>().material.color = Color.red;
    }
      
}
