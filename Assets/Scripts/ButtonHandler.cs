using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    /// <summary>
    ///   React to a button click event.  Used in the UI Button action definition.
    /// </summary>
    /// <param name="button"></param>
    public void onButtonClicked(Button button) 
    {
        // which GameObject?
        GameObject go = GameObject.Find("GameController");
        if (go != null)
        {
            HomePage gameController = go.GetComponent<HomePage>();
            if (gameController == null)
            {
                Debug.LogError("Missing game controller...");
                return;
            }
            if (button.name == "JoinButton")
            {
                gameController.onJoinButtonClicked();
            }
            else if (button.name == "LeaveButton")
            {
                gameController.onLeaveButtonClicked();
            }
            else if (button.name == "Switch")
            {
                gameController.onSwitchButtonClicked();
            }
            else if (button.name == "External")
            {
                gameController.onExternalCameraClicked();
            }
            else if (button.name == "MuteVideo")
            {
                gameController.OnMuteVideoClicked(!muteVideo);
                muteVideo = !muteVideo;
            }
            else if (button.name == "MuteAudio")
            {
                gameController.OnMuteAudioClicked(!muteAudio);
                muteAudio = !muteAudio;
            }
        }
    }

    bool muteVideo = false;
    bool muteAudio = false;
}
