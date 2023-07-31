using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class CarLapCounter : MonoBehaviour
{
    int passedCheckpointNumber = 0;
    float timeAtLastPassedCheckpoint = 0;

    int numberOfPassedCheckpoints = 0;

    int lapsCompleted = 0;
    const int lapsToComplete = 2;

    bool isRaceCompleted = false;

    public Checkpoint checkpoint;

    // bool isHideRoutineRunning = false;

    private PointsController pointsController;

    public GameOverScreen gameOverScreen;

    //Events
    public event Action<CarLapCounter> OnPassCheckpoint;
    private void Awake()
    {
        pointsController = GetComponent<PointsController>();
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckpoints;
    }
    public float GetTimeAtLastCheckpoint()
    {
        return timeAtLastPassedCheckpoint;
    }



    void OnTriggerEnter2D(Collider2D collider2D)
    {
        
        if (collider2D.CompareTag("Checkpoint"))
        {
            //Once a car has completed the race we don't need to check any checkpoints or laps. 
            if (isRaceCompleted)
                return;

            checkpoint = collider2D.GetComponent<Checkpoint>();

            //Make sure that the car is passing the checkpoints in the correct order. The correct checkpoint must have exactly 1 higher value than the passed checkpoint
            if (passedCheckpointNumber + 1 == checkpoint.checkpointNumber)
            {
                passedCheckpointNumber = checkpoint.checkpointNumber;

                numberOfPassedCheckpoints++;

                //Store the time at the checkpoint
                timeAtLastPassedCheckpoint = Time.time;

                if (checkpoint.isFinishLine)
                {
                    passedCheckpointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete)
                        isRaceCompleted = true;
                }


                //Invoke the passed checkpoint event
                OnPassCheckpoint?.Invoke(this);

                //Now show the cars position as it has been calculated
                if (isRaceCompleted)
                {
                    int score = pointsController.GetCurrentScore();
                    gameOverScreen.Setup(score);
                }
            }
        }
    }
}