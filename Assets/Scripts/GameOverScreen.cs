using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public Text pointsText;
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = $"{score:n0} Points";
    }
        public void RoadTrackButton()
    {
        SceneManager.LoadScene(0);
    }
        public void DirtTrackButton()
    {
        SceneManager.LoadScene(1);
    }
}
