using UnityEngine;

public class RollRecharge : MonoBehaviour
{
    private Animator animator;
    private static readonly int Recharge = Animator.StringToHash("Recharge");

    private void Start()
    {
        if (!TryGetComponent(out animator))
        {
            Debug.LogError("No Animator on rollRecharge object");
        }

        Singleton.Instance.PlayerData.Movement.StartedRolling.AddListener(() => animator.SetTrigger(Recharge));
    }
}
