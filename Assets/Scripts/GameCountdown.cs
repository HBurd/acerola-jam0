using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCountdown : MonoBehaviour
{
    [SerializeField]
    double game_time = 120.0f;

    double game_end_time;

    bool game_over = false;

    [SerializeField]
    Net net;

    [SerializeField]
    GameObject restart;

    void Start()
    {
        game_end_time = Time.timeAsDouble + game_time;
    }

    void Update()
    {
        if (game_over)
        {
            return;
        }

        double time_left = game_end_time - Time.timeAsDouble;

        if (time_left < 0.0)
        {
            game_over = true;
            int hiscore = PlayerPrefs.GetInt("hiscore", 0);

            if (net.GetAliens() > hiscore)
            {
                PlayerPrefs.SetInt("hiscore", net.GetAliens());
                GetComponent<TMP_Text>().text = "New highscore!";
            }
            else
            {
                GetComponent<TMP_Text>().text = "Your highscore is " + hiscore.ToString();
            }

            restart.SetActive(true);
        }
        else
        {
            GetComponent<TMP_Text>().text = ((int)time_left).ToString() + " seconds remaining";
        }
    }
}
