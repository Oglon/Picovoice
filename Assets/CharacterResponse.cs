using Pv.Unity;
using UnityEngine;

public abstract class ResponseScript : MonoBehaviour
{
    public abstract DialogueResponse GetResponse(Inference intent, bool sensitive, int rudeIncidents,
        float rudeCooldown);

    public abstract DialogueResponse GetVolumeResponse();

    public abstract DialogueResponse GetRudeResponse();
    public abstract DialogueResponse GetRudeCooldownResponse();
}