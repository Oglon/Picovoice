using Pv.Unity;
using UnityEngine;

public class ColleagueResponseOffice2 : ResponseScript
{
    [field: SerializeField] public DialogueResponse Friendly_Escape { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_Escape { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_Escape { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_Keycard { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_Keycard { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_Keycard { get; private set; }

    [field: SerializeField] public DialogueResponse Fixed { get; private set; }

    [field: SerializeField] public DialogueResponse Phone { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_SecretPrize { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_SecretPrize { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_SecretPrize { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_NeedHelp { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_NeedHelp { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_NeedHelp { get; private set; }
    [field: SerializeField] public DialogueResponse ALT_NeedHelp { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_WantedToTalkToMe { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_WantedToTalkToMe { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_WantedToTalkToMe { get; private set; }

    [field: SerializeField] public DialogueResponse Weather { get; private set; }
    [field: SerializeField] public DialogueResponse Game { get; private set; }
    [field: SerializeField] public DialogueResponse HowIsItGoing { get; private set; }
    [field: SerializeField] public DialogueResponse Travel { get; private set; }
    [field: SerializeField] public DialogueResponse Name { get; private set; }
    [field: SerializeField] public DialogueResponse WorkingOn { get; private set; }
    [field: SerializeField] public DialogueResponse Hobbies { get; private set; }
    [field: SerializeField] public DialogueResponse FavoriteMovie { get; private set; }
    [field: SerializeField] public DialogueResponse News { get; private set; }
    [field: SerializeField] public DialogueResponse Weekend { get; private set; }
    [field: SerializeField] public DialogueResponse HowDoYouLikeItHere { get; private set; }

    [field: SerializeField] public DialogueResponse Curse1 { get; private set; }
    [field: SerializeField] public DialogueResponse Curse2 { get; private set; }
    [field: SerializeField] public DialogueResponse Curse3 { get; private set; }
    [field: SerializeField] public DialogueResponse Curse4 { get; private set; }

    [field: SerializeField] public DialogueResponse NotUnderstood1 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood2 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood3 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood4 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood5 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood6 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood7 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood8 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood9 { get; private set; }
    [field: SerializeField] public DialogueResponse NotUnderstood10 { get; private set; }

    [field: SerializeField] public DialogueResponse VolumeResponse1 { get; private set; }
    [field: SerializeField] public DialogueResponse VolumeResponse2 { get; private set; }
    [field: SerializeField] public DialogueResponse VolumeResponse3 { get; private set; }

    [field: SerializeField] public DialogueResponse ThankResponse1 { get; private set; }
    [field: SerializeField] public DialogueResponse ThankResponse2 { get; private set; }
    [field: SerializeField] public DialogueResponse ThankResponse3 { get; private set; }

    [field: SerializeField] public DialogueResponse RudeCooldown { get; private set; }

    [field: SerializeField] public DialogueResponse SorryNecessary { get; private set; }
    [field: SerializeField] public DialogueResponse SorryUnnecessary { get; private set; }
    [field: SerializeField] public DialogueResponse LeaveMeAlone { get; private set; }

    public DialogueResponse PreviousResponse { get; private set; }

    [field: SerializeField] public Objective Q1O2;
    [field: SerializeField] public Objective Q2O1;
    [field: SerializeField] public Objective Q3O2;


    [field: SerializeField] private ObjectiveHandler objectiveHandler;
    private Quest currentQuest;

    public override DialogueResponse GetResponse(Inference inference, bool sensitive, int rudeIncidents,
        float rudeCooldown)
    {
        currentQuest = objectiveHandler.GetCurrentQuest();
        Objective currentObjective = currentQuest.currentObjective;

        string intent = sensitive ? inference.Intent : RemoveSensitive(inference.Intent);

        if (inference.IsUnderstood)
        {
            if (rudeCooldown > 0 && intent != "Sorry")
            {
                if (rudeIncidents >= 3)
                {
                    GetRudeResponse();
                }
                else
                {
                    GetRudeCooldownResponse();
                }
            }

            if (intent == "Sorry")
            {
                if (rudeCooldown <= 0)
                {
                    return PreviousResponse = SorryUnnecessary;
                }

                if (rudeIncidents < 3)
                {
                    return PreviousResponse = SorryNecessary;
                }

                return PreviousResponse = LeaveMeAlone;
            }

            if (intent == "Friendly_SecretPrize")
            {
                return PreviousResponse = Friendly_SecretPrize;
            }

            if (intent == "Normal_SecretPrize")
            {
                return PreviousResponse = Normal_SecretPrize;
            }

            if (intent == "Unfriendly_SecretPrize")
            {
                return PreviousResponse = Unfriendly_SecretPrize;
            }

            if (intent == "Friendly_NeedHelp")
            {
                if (currentObjective.questIndex < Q1O2.questIndex)
                {
                    return PreviousResponse = ALT_NeedHelp;
                }

                if (Q1O2 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = Friendly_NeedHelp;
                }

                return PreviousResponse = ALT_NeedHelp;
            }

            if (intent == "Normal_NeedHelp")
            {
                if (currentObjective.questIndex < Q1O2.questIndex)
                {
                    return PreviousResponse = ALT_NeedHelp;
                }

                if (Q1O2 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = Normal_NeedHelp;
                }

                return PreviousResponse = ALT_NeedHelp;
            }

            if (intent == "Unfriendly_NeedHelp")
            {
                if (currentObjective.questIndex < Q1O2.questIndex)
                {
                    return PreviousResponse = ALT_NeedHelp;
                }

                if (Q1O2 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = Unfriendly_NeedHelp;
                }

                return PreviousResponse = ALT_NeedHelp;
            }

            if (intent == "Friendly_WantedToTalkToMe")
            {
                return PreviousResponse = Friendly_WantedToTalkToMe;
            }

            if (intent == "Normal_WantedToTalkToMe")
            {
                return PreviousResponse = Normal_WantedToTalkToMe;
            }

            if (intent == "Unfriendly_WantedToTalkToMe")
            {
                return PreviousResponse = Unfriendly_WantedToTalkToMe;
            }

            if (intent == "Friendly_Keycard")
            {
                if (Q3O2 == currentObjective)
                {
                }

                return PreviousResponse = Friendly_Keycard;
            }

            if (intent == "Normal_Keycard")
            {
                if (Q3O2 == currentObjective)
                {
                }

                return PreviousResponse = Normal_Keycard;
            }

            if (intent == "Unfriendly_Keycard")
            {
                if (Q3O2 == currentObjective)
                {
                }

                return PreviousResponse = Unfriendly_Keycard;
            }

            if (intent == "Friendly_Escape")
            {
                if (currentObjective.questIndex < Q2O1.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q2O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Normal_Escape;
            }

            if (intent == "Normal_Escape")
            {
                if (currentObjective.questIndex < Q2O1.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q2O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Normal_Escape;
            }

            if (intent == "Unfriendly_Escape")
            {
                if (currentObjective.questIndex < Q2O1.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q2O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Normal_Escape;
            }

            if (intent == "Friendly_Yes")
            {
                return PreviousResponse = ThanksResponse();
            }

            if (intent == "Normal_Yes")
            {
                return PreviousResponse = ThanksResponse();
            }

            if (intent == "Unfriendly_Yes")
            {
                return PreviousResponse = ThanksResponse();
            }

            if (intent == "Curse")
            {
                return PreviousResponse = CurseResponse();
            }

            if (intent == "Repeat")
            {
                return PreviousResponse;
            }

            if (intent == "Thanks")
            {
                return PreviousResponse = ThanksResponse();
            }

            if (intent == "Weather")
            {
                return PreviousResponse = Weather;
            }

            if (intent == "Game")
            {
                return PreviousResponse = Game;
            }

            if (intent == "HowIsItGoing")
            {
                return PreviousResponse = HowIsItGoing;
            }

            if (intent == "Travel")
            {
                return PreviousResponse = Travel;
            }

            if (intent == "Name")
            {
                return PreviousResponse = Name;
            }

            if (intent == "WorkingOn")
            {
                return PreviousResponse = WorkingOn;
            }

            if (intent == "Hobbies")
            {
                return PreviousResponse = Hobbies;
            }

            if (intent == "FavoriteMovie")
            {
                return PreviousResponse = FavoriteMovie;
            }

            if (intent == "News")
            {
                return PreviousResponse = News;
            }

            if (intent == "Weekend")
            {
                return PreviousResponse = Weekend;
            }

            if (intent == "HowDoYouLikeItHere")
            {
                return PreviousResponse = HowDoYouLikeItHere;
            }
            return PreviousResponse = NotUnderstood();
        }

        return PreviousResponse = NotUnderstood();
    }

    private DialogueResponse NotUnderstood()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 11);
        DialogueResponse response = ScriptableObject.CreateInstance<DialogueResponse>();

        switch (responseInt)
        {
            case 1:
                response = NotUnderstood1;
                break;

            case 2:
                response = NotUnderstood2;
                break;

            case 3:
                response = NotUnderstood3;
                break;

            case 4:
                response = NotUnderstood4;
                break;

            case 5:
                response = NotUnderstood5;
                break;

            case 6:
                response = NotUnderstood6;
                break;

            case 7:
                response = NotUnderstood7;
                break;

            case 8:
                response = NotUnderstood8;
                break;

            case 9:
                response = NotUnderstood9;
                break;

            case 10:
                response = NotUnderstood10;
                break;
        }

        return response;
    }

    public string RemoveSensitive(string intent)
    {
        Debug.Log(intent);
        return intent.Replace("Friendly_", "Normal_").Replace("Unfriendly_", "Normal_");
    }

    private DialogueResponse CurseResponse()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 5);
        DialogueResponse response = ScriptableObject.CreateInstance<DialogueResponse>();

        switch (responseInt)
        {
            case 1:
                response = Curse1;
                break;

            case 2:
                response = Curse2;
                break;

            case 3:
                response = Curse3;
                break;

            case 4:
                response = Curse4;
                break;
        }

        return response;
    }

    public override DialogueResponse GetVolumeResponse()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 4);
        DialogueResponse response = ScriptableObject.CreateInstance<DialogueResponse>();

        switch (responseInt)
        {
            case 1:
                response = VolumeResponse1;
                break;

            case 2:
                response = VolumeResponse2;
                break;

            case 3:
                response = VolumeResponse3;
                break;
        }

        return response;
    }

    public override DialogueResponse GetRudeResponse()
    {
        return LeaveMeAlone;
    }

    public override DialogueResponse GetRudeCooldownResponse()
    {
        return RudeCooldown;
    }

    private DialogueResponse ThanksResponse()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 4);
        DialogueResponse response = ScriptableObject.CreateInstance<DialogueResponse>();

        switch (responseInt)
        {
            case 1:
                response = ThankResponse1;
                break;

            case 2:
                response = ThankResponse2;
                break;

            case 3:
                response = ThankResponse3;
                break;
        }

        return response;
    }
}