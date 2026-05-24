using UnityEngine;
using TMPro;
using System.Collections;
using 判定系统;

public class SpaceKeyTracker : MonoBehaviour
{
    public TMP_Text promptLabel;
    public TMP_Text resultLabel;

    int targetDuration;
    float roundStartTimeMs;
    float pressStartTimeMs;
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
        roundStartTimeMs = Time.time * 1000f;
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
            resultLabel.text = $"反应时间：{Time.time * 1000f - roundStartTimeMs:F0} ms";
        }

        if (Input.GetKeyDown(KeyCode.Space) && waiting)
        {
            pressStartTimeMs = Time.time * 1000f;
            isPressing = true;
            waiting = false;
        }

        if (isPressing)
        {
            float heldMs = Time.time * 1000f - pressStartTimeMs;
            if (targetDuration > 0)
                resultLabel.text = $"按住中：{heldMs:F0} / {targetDuration * 1000f} ms";
            else
                resultLabel.text = "松开！";
        }

        if (Input.GetKeyUp(KeyCode.Space) && isPressing)
        {
            isPressing = false;
            float durationMs = Time.time * 1000f - pressStartTimeMs;
            float reactionTimeMs = pressStartTimeMs - roundStartTimeMs;
            string grade = "Miss";

            grade = 判定.抉择(reactionTimeMs).名称;

            resultLabel.text = $"<color={ColorForGrade(grade)}>{grade}！反应 {reactionTimeMs:F0}ms + 按住 {durationMs:F0}ms</color>";

            StartCoroutine(WaitAndNext(1.5f));
        }
    }

    string ColorForGrade(string grade) => grade switch
    {
        "Perfect" => "green",
        "Great" => "blue",
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
