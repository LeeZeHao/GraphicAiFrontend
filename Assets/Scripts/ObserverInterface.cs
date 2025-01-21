using UnityEngine;

public abstract class ObserverInterface : MonoBehaviour
{
    public abstract void UpdateObserver(string response, int mood = 0);

    public abstract void ServerError(string error);

    public abstract void Waiting();
}
