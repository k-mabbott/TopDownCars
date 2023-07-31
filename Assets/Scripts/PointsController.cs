using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PointsController : MonoBehaviour
{

    private bool wall = false;

    public Text scoreText;
    public Text driftText;
    private int score;
    private int drift;

    //Components
    CarController carController;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        drift = 0;
        driftText.text = "Drift: " + drift.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentScore();
    }

    public int GetCurrentScore()
    {
        if (carController.IsTireSchreeching(out float lateralVelocity, out bool isBraking))
        {
            //If car is braking
            if (!isBraking && lateralVelocity > .1f || lateralVelocity < -.1f)
            {
                drift = 1;
                if (drift > 0)
                {
                    if(wall)
                    {
                        drift = 0;
                    } else {
                        score += drift;
                    }
                    drift += 1;
                    driftText.text = "drift: " + drift.ToString();
                }
                //If we are not braking we are drifting
            }
            score += drift;
            scoreText.text = $"SCORE: {score:n0}";
            
            drift = 0;
        }
        return score;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // wall = true;
        score -= 300;
        scoreText.text = "Score: " + score.ToString();
    }
}
