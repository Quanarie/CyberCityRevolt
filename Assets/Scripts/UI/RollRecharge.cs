using UnityEngine;

public class RollRecharge : MonoBehaviour
{
    private Animator _anim;
    private static readonly int Recharge = Animator.StringToHash("Recharge");

    private void Start()
    {
        if (!TryGetComponent(out _anim))
        {
            Debug.LogError("No animator on rollRecharge object");
        }

        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(() => _anim.SetTrigger(Recharge));
    }
}
