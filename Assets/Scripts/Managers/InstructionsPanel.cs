using UnityEngine;

public class InstructionsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelToControl;
    [SerializeField] private float timeScaleWhenPanelClosed = 1.0f;
    [SerializeField] private float timeScaleWhenPanelOpen = 0.0f;

    private void Awake()
    {
        Time.timeScale = timeScaleWhenPanelOpen;
    }

    private void Update()
    {
        OnPressKey(); //Нажал, закрыл, начал играть      
    }

    private void OnPressKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (panelToControl == null)
            {
                return;
            }

            if (panelToControl.activeSelf)
            {
                SoundManager.Instance.PlayCloseWindowSound();
                panelToControl.SetActive(false);
                Time.timeScale = timeScaleWhenPanelClosed;
            }
        }
    }
}
