using UnityEngine;

public class AutoSave1 : MonoBehaviour
{
    
    void Start()
    {
        PlayerPrefs.SetInt("AutoSave", 2);    
    }

}
