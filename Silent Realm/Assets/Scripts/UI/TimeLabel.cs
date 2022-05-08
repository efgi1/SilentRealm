using UnityEngine;
using UnityEngine.UI;

public class TimeLabel : MonoBehaviour
{
    private Text timeText;

    void Start()
    {
        timeText = GetComponent<Text>();

        TimeRecorder recorder = TimeRecorder.Instance;
        if (recorder == null)
        {
            timeText.text = "00:00.00";
        }
        else
        {
            int totalMS = Mathf.RoundToInt(recorder.totalPlayTime * 1000);

            int hours = totalMS / (60 * 60 * 1000);
            totalMS = totalMS % (60 * 60 * 1000);

            int minutes = totalMS / (60 * 1000);
            totalMS = totalMS % (60 * 1000);
    
            int seconds = totalMS / 1000;
            totalMS = totalMS % 1000;

            int milliseconds = Mathf.RoundToInt(totalMS / 10f);

            string sHours = hours.ToString("00");
            string sMinutes = minutes.ToString("00");
            string sSeconds = seconds.ToString("00");
            string sMS = milliseconds.ToString("00");

            if (hours > 0)
            {
                timeText.text = sHours + ":" + sMinutes + ":" + sSeconds + "." + sMS;
            }
            else
            {
                timeText.text = sMinutes + ":" + sSeconds + "." + sMS;
            }
        }
    }
}
