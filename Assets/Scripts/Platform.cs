using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] List<Sprite> platformColorsList = new();
    public Color platformColor;
    SpriteRenderer spriteRenderer;
    public bool isNext;
    public bool canCheck;
    public enum Color
    {
        Yellow,
        Blue,
        Green,
        Red
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangePlatformColor(Color.Yellow);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!canCheck) return;
            CheckOrder();
        }
    }

    public void ChangePlatformColor(Color _color)
    {
        platformColor = _color;
        UpdatePlatformColor();
    }

    void UpdatePlatformColor()
    {
        if(!spriteRenderer)
        {
            Debug.Log("nie dzia³a");
            return;
        }
        switch(platformColor)
        {
            case Color.Yellow:
                spriteRenderer.sprite = platformColorsList[0];
                break;
            case Color.Blue:
                spriteRenderer.sprite = platformColorsList[1];
                break;
            case Color.Green:
                spriteRenderer.sprite = platformColorsList[2];
                break;
            case Color.Red:
                spriteRenderer.sprite = platformColorsList[3];
                break;
        }
    }

    public void ChangePlatformColorTemporarily(Color _color, float time)
    {
        platformColor = _color;
        UpdatePlatformColor();

        platformColor = Color.Yellow;
        Invoke(nameof(UpdatePlatformColor), time);
    }

    void CheckOrder()
    {
        
        if (isNext)
        {
            isNext = false;
            GameManager.Instance.PlatformChecked(true);
            ChangePlatformColorTemporarily(Color.Green, 1f);
            
        }
        else
        {
            GameManager.Instance.PlatformChecked(false);
            canCheck = false;
            ChangePlatformColor(Color.Red);
        }
    }
}
