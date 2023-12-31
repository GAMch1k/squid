using System;
using System.Collections.Generic;
using UnityEngine;


public struct ShadowFrame
{
    public ShadowFrame(Vector3 pos, float speed, bool isj, bool rd)
    {
        Position = pos;
        AnimCondSpeed = speed;
        AnimCondIsJumping = isj;
        RightDirection = rd;
    }
    
    public Vector3 Position;
    public float AnimCondSpeed;
    public bool AnimCondIsJumping;
    public bool RightDirection;
}

public class ShadowTheater : MonoBehaviour
{
    private TimeManager _timeManager;
    private Transform _playerTransform;
    private Animator _playerAnimator;
    private GameObject _shadowPrefab;

    private List<GameObject> _shadows;
    private List<List<ShadowFrame>> _shadowTraces;
    private bool _recordNewTraces;
    public Animator animGameOver;
    public GameObject objectPanelGameOver;
    
    private void Start()
    {
        _recordNewTraces = true;
        _timeManager = GameObject.FindWithTag("timemanager").GetComponent<TimeManager>();
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        
        _shadowPrefab = Resources.Load("Prefabs/ShadowActor") as GameObject;
        
        _shadows = new List<GameObject>();
        _shadowTraces = new List<List<ShadowFrame>>();

        TimeManager.NewTimeCycleEvent += _newTimeCycle;
        TimeManager.GameOverEvent += _stopNewRecordings;
        TimeManager.GameOverEvent += _uiGameOver;
    }

    private void OnDisable()
    {
        TimeManager.NewTimeCycleEvent -= _newTimeCycle;
        TimeManager.GameOverEvent -= _stopNewRecordings;
    }

    private void FixedUpdate()
    {
        var currentTick = _timeManager.GetCurrentTick();
        var currentRun = _timeManager.GetCurrentRun();
        ShadowFrame shadowFrame;

        if (currentRun < 0)
        {
            return;
        }

        for (int i = 0; i < _shadows.Count; i++)
        {
            if (_shadowTraces[i].Count <= currentTick)
            {
                continue;
            }

            shadowFrame = _shadowTraces[i][currentTick];

            _shadows[i].GetComponent<Transform>().position = shadowFrame.Position;

            var shadowScale = _shadows[i].GetComponent<Transform>().localScale;
            var xScale = shadowFrame.RightDirection ? 2 : -2;
            _shadows[i].GetComponent<Transform>().localScale = new Vector3(xScale, shadowScale.y, shadowScale.z);
            
            var shadowAnimator = _shadows[i].GetComponent<Animator>();
            shadowAnimator.SetFloat("speed", shadowFrame.AnimCondSpeed);
            shadowAnimator.SetBool("isJumping", shadowFrame.AnimCondIsJumping);
        }

        if (!_recordNewTraces) return;
        
        var playerPos = _playerTransform.position;
        var animCondSpeed = _playerAnimator.GetFloat("speed");
        var animIsJumping = _playerAnimator.GetBool("isJumping");
        var rd = _playerTransform.localScale.x > 0 ? true : false;

        shadowFrame = new ShadowFrame(playerPos, animCondSpeed, animIsJumping, rd);
        try
        {
            _shadowTraces[currentRun].Add(shadowFrame);
        }
        catch (Exception e)
        {
            _newTimeCycle();
        }
        
    }

    private void _newTimeCycle()
    {
        var currentRun = _timeManager.GetCurrentRun();
        if (currentRun > 0)
        {
            // fix if on time cycle end shadow was moving
            var lastFrameIndex = _shadowTraces[currentRun - 1].Count - 1;
            var modifiableShadowFrame = _shadowTraces[currentRun - 1][lastFrameIndex];
            var newShadowFrame = new ShadowFrame(modifiableShadowFrame.Position, 0, false, modifiableShadowFrame.RightDirection);
            _shadowTraces[currentRun - 1][lastFrameIndex] = newShadowFrame;
            
            GameObject newShadow = Instantiate(_shadowPrefab, gameObject.transform);
            _shadows.Add(newShadow);
        }

        List<ShadowFrame> whiteTemplate = new List<ShadowFrame>();
        _shadowTraces.Add(whiteTemplate);
    }
    private void _uiGameOver() {
        //objectPanelGameOver.SetActive(true);
        //animGameOver.SetTrigger("up");
    }
    private void _stopNewRecordings()
    {
        _recordNewTraces = false;
    }
}