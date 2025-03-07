using UnityEngine;
using UnityEngine.UI;

namespace CodingStrategy.UI.GameScene
{
    public class OnClickCommandDetail : MonoBehaviour
    {
        public GameObject detailPrefab;
        public string Id;

        private void Start()
        {
            GetComponent<Button>()
               .onClick.AddListener(
                    () =>
                    {
                        GameObject alwaysOnTop = GameObject.Find("AlwaysOnTop");
                        foreach (Transform child in alwaysOnTop.transform)
                        {
                            if (child.name.Contains("CommandDetail"))
                            {
                                Destroy(child.gameObject);
                            }
                        }
                        GameObject detail = Instantiate(detailPrefab, alwaysOnTop.transform);
                        detail.transform.position = Input.mousePosition;
                        detail.GetComponent<SetCommandDetail>().Id = Id;
                        alwaysOnTop.GetComponent<CommandDetailEvent>().setCommandDetail =
                            detail.GetComponent<SetCommandDetail>();
                        alwaysOnTop.GetComponent<CommandDetailEvent>().OnCommandClickEvent.Invoke(Id);
                    });
        }
    }
}
