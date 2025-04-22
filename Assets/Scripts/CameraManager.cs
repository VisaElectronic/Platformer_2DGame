using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(
            GetComponent<CinemachineVirtualCamera>().Follow == null &&
            PlayerManager.instance.currentPlayer != null
        )
            GetComponent<CinemachineVirtualCamera>().Follow = PlayerManager.instance.currentPlayer.transform;
    }
}
