using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using SimpleFileBrowser;

public class MoviePlayerController : MonoBehaviour
{
    [SerializeField]
    VideoPlayer[] _videoPlayers;

    int _prepareCount;

    // Start is called before the first frame update
    void Start()
    {
        FileBrowser.SetFilters(
            true ,
            ".mp4" 
        );
        FileBrowser.SetDefaultFilter( ".mp4" );
        FileBrowser.AddQuickLink( "Users" , "C:\\Users" , null );

        StartCoroutine( ShowLoadDialogCoroutine() );

    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        _prepareCount = 0;
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files , false , null , null , "Load Files" , "Load");
        Debug.Log( FileBrowser.Success );
        if (FileBrowser.Success)
        {
            var filePass = FileBrowser.Result[0];
            // とりあえず全て同一ムービーを流す
            foreach (var videoPlayer in _videoPlayers)
            {
                videoPlayer.url = filePass;
                videoPlayer.prepareCompleted += OnPrepareCompleted;
                videoPlayer.Prepare();
            }
        }
    }

    void OnPrepareCompleted(VideoPlayer completedVideoPlayer)
    {
        ++_prepareCount;
        if (_prepareCount == _videoPlayers.Length)
        {
            foreach (var videoPlayer in _videoPlayers)
            {
                videoPlayer.Play();
            }
        }
    }
}
