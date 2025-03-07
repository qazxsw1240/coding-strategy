using Photon.Realtime;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    public TextMeshProUGUI roomNameText; // 방 이름을 표시할 TextMeshProUGUI 컴포넌트
    public TextMeshProUGUI roomDescriptionText; // 방 설명을 표시할 TextMeshProUGUI 컴포넌트
    public RawImage standardImage; // "Standard" 이미지
    public RoomInfo roomInfo;

    // 방 정보 설정
    public void SetRoomInfo(RoomInfo room)
    {
        roomInfo = room;
        roomNameText.text = room.Name;
        standardImage.gameObject.SetActive(false); // 초기에는 "Standard" 이미지를 비활성화

        Button button = GetComponent<Button>();
        button.onClick.AddListener(
            () =>
            {
                standardImage.gameObject.SetActive(true);
                roomDescriptionText.text = GetRoomDescription(room);
            });
    }

    // 방 설명을 가져오는 함수
    private string GetRoomDescription(RoomInfo room)
    {
        // 여기에서 방 설명을 생성하고 반환합니다.
        return $"Room Name: {room.Name}\nMax Players: {room.MaxPlayers}\nCurrent Players: {room.PlayerCount}";
    }

    public void CheckDestroyRoom()
    {
        if (roomInfo.PlayerCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
