using Pv.Unity;
using UnityEngine;

public class BossResponseOffice2 : ResponseScript
{
    [field: SerializeField] public DialogueResponse Friendly_Escape { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_Escape { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_Escape { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_5276 { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_5276 { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_5276 { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_Rhino { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_Rhino { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_Rhino { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_SecretPrize { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_SecretPrize { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_SecretPrize { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_NeedHelp { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_NeedHelp { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_NeedHelp { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_WantedToTalkToMe { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_WantedToTalkToMe { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_WantedToTalkToMe { get; private set; }
    [field: SerializeField] public DialogueResponse WantedToTalkToMeALT { get; private set; }
    [field: SerializeField] public DialogueResponse WantedToTalkToMe2 { get; private set; }
    [field: SerializeField] public DialogueResponse WantedToTalkToMe3 { get; private set; }
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

    [field: SerializeField] public Objective Q1O1;
    [field: SerializeField] public Objective Q3O1;
    [field: SerializeField] public Objective Q4O1;
    [field: SerializeField] public Objective Q4O3;

    [field: SerializeField] private ObjectiveHandler objectiveHandler;
    private Quest currentQuest;

    public override DialogueResponse GetResponse(Inference inference, bool sensitive, int rudeIncidents,
        float rudeCooldown)
    {
        currentQuest = objectiveHandler.GetCurrentQuest();
        Objective currentObjective = currentQuest.currentObjective;

        string intent = sensitive ? inference.Intent : RemoveSensitive(inference.Intent);

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

        if (inference.IsUnderstood)
        {
            if (intent == "Friendly_SecretPrice")
            {
                if (Q1O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Friendly_SecretPrize;
            }

            if (intent == "SecretPrice")
            {
                if (Q1O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Normal_SecretPrize;
            }

            if (intent == "Unfriendly_SecretPrice")
            {
                if (Q1O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Unfriendly_SecretPrize;
            }

            if (intent == "Friendly_5276")
            {
                return PreviousResponse = Friendly_5276;
            }

            if (intent == "5276")
            {
                return PreviousResponse = Normal_5276;
            }

            if (intent == "Unfriendly_5276")
            {
                return PreviousResponse = Unfriendly_5276;
            }

            if (intent == "Friendly_NeedHelp")
            {
                return PreviousResponse = Friendly_NeedHelp;
            }

            if (intent == "NeedHelp")
            {
                return PreviousResponse = Normal_NeedHelp;
            }

            if (intent == "Unfriendly_NeedHelp")
            {
                return PreviousResponse = Unfriendly_NeedHelp;
            }

            if (intent == "Friendly_WantedToTalkToMe")
            {
                if (Q3O1 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = Friendly_WantedToTalkToMe;
                }
                
                if (Q4O1 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = WantedToTalkToMe2;
                }
                if (Q4O3 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = WantedToTalkToMe3;
                }

                return PreviousResponse = WantedToTalkToMeALT;
            }

            if (intent == "WantedToTalkToMe")
            {
                if (Q3O1 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = Normal_WantedToTalkToMe;
                }
                if (Q4O1 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = WantedToTalkToMe2;
                }
                if (Q4O3 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = WantedToTalkToMe3;
                }
                return PreviousResponse = WantedToTalkToMeALT;
            }

            if (intent == "Unfriendly_WantedToTalkToMe")
            {
                if (Q3O1 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = Unfriendly_WantedToTalkToMe;
                }
                if (Q4O1 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = WantedToTalkToMe2;
                }
                if (Q4O3 == currentObjective)
                {
                    currentQuest.Progress();
                    return PreviousResponse = WantedToTalkToMe3;
                }
                return PreviousResponse = WantedToTalkToMeALT;
            }

            if (intent == "Friendly_Escape")
            {
                return PreviousResponse = Friendly_Escape;
            }

            if (intent == "Escape")
            {
                return PreviousResponse = Normal_Escape;
            }

            if (intent == "Unfriendly_Escape")
            {
                return PreviousResponse = Unfriendly_Escape;
            }

            if (intent == "Friendly_Rhino")
            {
                return PreviousResponse = Friendly_Rhino;
            }

            if (intent == "Rhino")
            {
                return PreviousResponse = Normal_Rhino;
            }

            if (intent == "Unfriendly_Rhino")
            {
                return PreviousResponse = Unfriendly_Rhino;
            }

            if (intent == "Friendly_Yes")
            {
                return PreviousResponse = ThanksResponse();
            }

            if (intent == "Yes")
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
        }
        else
        {
            return PreviousResponse = NotUnderstood();
        }

        return PreviousResponse;
    }

    private DialogueResponse NotUnderstood()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 11);
        DialogueResponse response = new DialogueResponse();

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
        return intent.Replace("Friendly_", "").Replace("Unfriendly_", "");
    }

    private DialogueResponse CurseResponse()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 5);
        DialogueResponse response = new DialogueResponse();

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
        DialogueResponse response = new DialogueResponse();

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
        DialogueResponse response = new DialogueResponse();

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