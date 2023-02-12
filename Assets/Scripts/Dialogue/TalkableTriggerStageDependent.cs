using UnityEngine;

public class TalkableTriggerStageDependent : TalkableTrigger
{
    [SerializeField] private int stageShouldBeDone;
    
    protected override void Update()
    {
        if (!Singleton.Instance.EnemySpawner.IsStageDone(stageShouldBeDone)) return;
        
        base.Update();
    }

    protected override void OnTriggerStay2D(Collider2D col)
    {
        if (!Singleton.Instance.EnemySpawner.IsStageDone(stageShouldBeDone)) return;
        
        base.OnTriggerStay2D(col);
    }
}