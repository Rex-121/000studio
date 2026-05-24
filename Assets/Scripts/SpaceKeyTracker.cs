using UnityEngine;
using TMPro;
using System.Collections;
using 判定系统;

public class SpaceKeyTracker : MonoBehaviour
{
    public TMP_Text promptLabel;
    public TMP_Text resultLabel;

    int targetDuration;
    float roundStartTime;
    float pressStartTime;
    bool isPressing;
    bool waiting;

    void Start()
    {
        NextRound();
    }

    void NextRound()
    {
        waiting = true;
        isPressing = false;
        roundStartTime = Time.time;
        resultLabel.text = "";

        if (Random.value < 0.5f)
        {
            targetDuration = 0;
            promptLabel.text = "短按";
        }
        else
        {
            targetDuration = Random.Range(1, 4);
            promptLabel.text = $"长按 {targetDuration} 秒";
        }
    }

    void Update()
    {
        if (waiting)
        {
            resultLabel.text = $"反应时间：{Time.time - roundStartTime:F1} 秒";
        }

        if (Input.GetKeyDown(KeyCode.Space) && waiting)
        {
            pressStartTime = Time.time;
            isPressing = true;
            waiting = false;
        }

        if (isPressing)
        {
            float held = Time.time - pressStartTime;
            if (targetDuration > 0)
                resultLabel.text = $"按住中：{held:F1} / {targetDuration} 秒";
            else
                resultLabel.text = "松开！";
        }

        if (Input.GetKeyUp(KeyCode.Space) && isPressing)
        {
            isPressing = false;
            float duration = Time.time - pressStartTime;
            float reactionTime = pressStartTime - roundStartTime;
            string grade = "Miss";
            float 延迟 = reactionTime * 1000f;

            grade = 判定.抉择(延迟).名称;

            resultLabel.text = $"<color={ColorForGrade(grade)}>{grade}！反应 {reactionTime:F2}s + 按住 {duration:F2}s</color>";

            StartCoroutine(WaitAndNext(1.5f));
        }
    }

    string ColorForGrade(string grade) => grade switch
    {
        "Perfect" => "green",
        "Great" => "cyan",
        "Okay" => "yellow",
        _ => "red"
    };

    IEnumerator WaitAndNext(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        promptLabel.text = "--";
        resultLabel.text = "";
        yield return new WaitForSeconds(0.5f);
        NextRound();
    }
}
