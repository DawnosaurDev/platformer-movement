using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class DemoManager : MonoBehaviour
{
    private Camera _cam;
    private PlayerMovement _player;
    [SerializeField] private PlayerData[] playerTypes;
    [SerializeField] private Tilemap[] levels;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private TextMeshProUGUI nameText;

    private int _currentPlayerTypeIndex;
    private int _currentTilemapIndex;
    private Color _currentForegroundColor;

    public SceneData SceneData;

    private void Awake()
    {
        _cam = FindObjectOfType<Camera>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        SetSceneData(SceneData);
        SwitchLevel(0);
        SwitchPlayerType(0);
    }

    public void SetSceneData(SceneData data)
    {
        SceneData = data;

        //Update the camera and tilemap color according to the new data.
        _cam.orthographicSize = data.camSize;
        _cam.backgroundColor = data.backgroundColor;
        levels[_currentTilemapIndex].color = data.foregroundColor;

        _currentForegroundColor = data.foregroundColor;
    }

    public void SwitchPlayerType(int index)
    {
        _player.Data = playerTypes[index];
        _currentPlayerTypeIndex = index;

        switch(index)
        {
            case 0:
                nameText.text = "Celeste";
                break;
            case 1:
                nameText.text = "Hollow Knight";
                break;
            case 2:
                nameText.text = "Super Meat Boy";
                break;
        }
    }

    public void SwitchLevel(int index)
    {
        //Switch tilemap active and apply color.
        levels[_currentTilemapIndex].gameObject.SetActive(false);
        levels[index].gameObject.SetActive(true);
        levels[index].color = _currentForegroundColor;
        levels[_currentTilemapIndex] = levels[index];

        _player.transform.position = spawnPoint.position;

        _currentTilemapIndex = index;
    }
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Switch to next level. Uses "?" to indicate that if the expression in the brackets before is true
            //then 0 will be passed throuh else it will increse by 1.
            SwitchLevel((_currentTilemapIndex == levels.Length - 1) ? 0 : _currentTilemapIndex + 1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Switch to next level. Uses "?" to indicate that if the expression in the brackets before is true
            //then 0 will be passed throuh else it will increse by 1.
            SwitchPlayerType((_currentPlayerTypeIndex == playerTypes.Length - 1) ? 0 : _currentPlayerTypeIndex + 1);
        }
    }
}
