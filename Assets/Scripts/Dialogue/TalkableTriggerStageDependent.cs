using UnityEngine;

public class TalkableTriggerStageDependent : TalkableTrigger
{
    [SerializeField] private int stageShouldBeDone;
    
    protected override void Update()
    {
        if (!Singleton.Instance.EnemySpawner.IsStageDone(stageShouldBeDone)) return;
        
        base.Update();
    }

    private void OnTriggerStay2D(Collider2D col) => OnTriggerEnter2D(col);

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (!Singleton.Instance.EnemySpawner.IsStageDone(stageShouldBeDone)) return;
        
        base.OnTriggerEnter2D(col);
    }
}