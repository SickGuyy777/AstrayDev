using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public static PlayerCursor Instance;
    
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private CursorAnimation[] animations;
    
    private Dictionary<string, CursorAnimation> animationDictionary = new Dictionary<string, CursorAnimation>();
    private Coroutine currentAnimationCoroutine;
    
    
    private void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        #endregion

        SetCursor(defaultCursor);
        
        foreach (CursorAnimation cursorAnimation in animations)
        {
            animationDictionary.Add(cursorAnimation.animName, cursorAnimation);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            StartPlayAnimation("Click");
        if(Input.GetMouseButtonUp(0))
            StopPlayAnimation();
    }

    public void StartPlayAnimation(string animName)
    {
        if(currentAnimationCoroutine != null)
            StopCoroutine(currentAnimationCoroutine);

        if (!animationDictionary.ContainsKey(animName))
        {
            Debug.LogError("Cursor animation does not exist with name: " + animName);
            return;
        }
        
        CursorAnimation cursorAnimation = animationDictionary[animName];
        currentAnimationCoroutine = StartCoroutine(PlayAnimation(cursorAnimation));
    }
    
    public void StopPlayAnimation()
    {
        if(currentAnimationCoroutine != null)
            StopCoroutine(currentAnimationCoroutine);

        SetCursor(defaultCursor);
        currentAnimationCoroutine = null;
    }

    private void SetCursor(Texture2D texture2D)
    {
        Cursor.SetCursor(texture2D, new Vector2(-1, 1), CursorMode.ForceSoftware);
    }

    private IEnumerator PlayAnimation(CursorAnimation cursorAnim)
    {
        float duration = cursorAnim.duration / cursorAnim.textures.Length;

        do
        {
            foreach (Texture2D texture in cursorAnim.textures)
            {
                SetCursor(texture);
                yield return new WaitForSeconds(duration);
            }
            
        } while (cursorAnim.looping);
        
        SetCursor(defaultCursor);
        currentAnimationCoroutine = null;
    }
}

[Serializable]
public class CursorAnimation
{
    public string animName = "New Animation";
    public float duration = 1;
    public bool looping = false;
    public Texture2D[] textures = null;
}
