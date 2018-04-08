using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIServerClient : GUIMenuChoose {

    private GameObject server_button;
    private GameObject client_button;
    private GameObject master;

    // Use this for initialization
    public override void Start () {
        base.Start();

        server_button = GameObject.Find("ServerButton");
        client_button = GameObject.Find("ClientButton");
        
        master = GameObject.FindGameObjectWithTag(Tags.Master);

    }

    protected override void OnSelect (GameObject target)
    {
        if (target == server_button)
        {
            
            StartHost();
            
        }
        else if (target == client_button)
        {
            
            StartClient();
            
        }
        else if (target.tag == Tags.BackButton) {
            SceneManager.LoadScene("MenuMain");
        }
    }
    
    
    private void StartHost()
    {
        
        this.enabled = false;
        
        var builder = master.GetComponent<GameBuilder>();

        builder.Action = GameBuilder.BuildMode.Host;

        SceneManager.LoadScene("MenuChooseGameMode");

    }

    private void StartClient()
    {

        this.enabled = false;

        var builder = master.GetComponent<GameBuilder>();

        builder.Action = GameBuilder.BuildMode.Client;

        SceneManager.LoadScene("MenuChooseGameMode");

    }

}
