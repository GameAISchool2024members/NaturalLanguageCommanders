using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GPTController))]
public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private float roundTime = 2;

    private float _timer = 0;
    private GPTController _gptController;
    void Start()
    {
        _gptController = GetComponent<GPTController>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if ( _timer > roundTime )
        {
            _timer -= roundTime;
            _gptController.RoundTick();
        }
    }
}
