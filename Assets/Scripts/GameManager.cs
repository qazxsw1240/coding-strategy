using UnityEngine;

namespace CodingStrategy
{
    public class GameManager : MonoBehaviour
    {
        public void Awake()
        {
            Debug.Log("GameManager.Awake() invoked.");
        }

        public void Start()
        {
            Debug.Log("GameManager.Start() invoked.");
        }

        public void Update()
        {
            Debug.Log("GameManager.Update() invoked.");
        }
    }
}
