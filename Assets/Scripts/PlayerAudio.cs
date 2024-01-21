using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource walkSource;
    [SerializeField]
    private AudioSource pickUpSource;
    [SerializeField]
    private AudioSource putDownSource;
    [SerializeField]
    private AudioSource rotateSource;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!walkSource.isPlaying)
        {
            walkSource.Play();
        }
    }
    public void OnPickUp(InputAction.CallbackContext context)
    {
        pickUpSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
