
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void Footstep()
    {
        Debug.Log("Footstep Animation Event!");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayFootstep();
        }
    }
}
