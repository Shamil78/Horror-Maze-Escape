using UnityEngine;
using UnityEngine.AI;

public class NavMeshObstacleController : MonoBehaviour
{
    private NavMeshObstacle navObstacle;

    private void Start()
    {
        navObstacle = GetComponent<NavMeshObstacle>();
    }

    // Call this function to disable the obstacle (Allow enemy to pass)
    public void DisableObstacle()
    {
        if (navObstacle != null)
        {
            navObstacle.enabled = false;
        }
    }

    // Call this function to enable the obstacle (Block enemy again)
    public void EnableObstacle()
    {
        if (navObstacle != null)
        {
            navObstacle.enabled = true;
        }
    }
}
