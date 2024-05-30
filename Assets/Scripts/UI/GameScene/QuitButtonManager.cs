using UnityEngine;
using UnityEngine.UI;

public class QuitButtonManager : MonoBehaviour
{
    public Button gameTurnButton; // Gameturn Button
    public Button gameRoomQuitButton; // GameroomQuit Button

    void Start()
    {
        // 초기에는 GameroomQuit Button이 보이지 않도록 합니다.
        gameRoomQuitButton.gameObject.SetActive(false);

        // Gameturn Button을 눌렀을 때의 동작을 설정합니다.
        gameTurnButton.onClick.AddListener(() =>
        {
            // GameroomQuit Button을 활성화합니다.
            gameRoomQuitButton.gameObject.SetActive(true);
        });

        // [태명씨에게 전달] GameroomQuit Button을 눌렀을 때의 동작을 설정합니다.
        gameRoomQuitButton.onClick.AddListener(() =>
        {
            // TODO: GameroomQuit Button의 기능을 구현합니다.

            // GameroomQuit Button을 비활성화합니다.
            gameRoomQuitButton.gameObject.SetActive(false);
        });
    }

    void Update()
    {
        // 다른 곳을 터치했을 때 GameroomQuit Button을 비활성화합니다.
        if (Input.GetMouseButton(0) && !gameRoomQuitButton.gameObject.GetComponent<RectTransform>().rect.Contains(Input.mousePosition))
        {
            gameRoomQuitButton.gameObject.SetActive(false);
        }
    }
}