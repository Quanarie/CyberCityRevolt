using UnityEngine;

// I want to draw an animation of flames under the feet of Gray indicating the recharge time of Roll
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
