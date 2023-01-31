using Pv.Unity;
using UnityEngine;

public class BossResponseOffice1 : ResponseScript
{
    [field: SerializeField] public DialogueResponse Friendly_1111 { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_1111 { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_1111 { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_SomeoneInMyOffice { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_SomeoneInMyOffice { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_SomeoneInMyOffice { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_North { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_North { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_North { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_NeedHelp { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_NeedHelp { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_NeedHelp { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_InternKnowsAboutFile { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_InternKnowsAboutFile { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_InternKnowsAboutFile { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_CameraCode { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_CameraCode { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_CameraCode { get; private set; }

    [field: SerializeField] public DialogueResponse Friendly_Donut { get; private set; }
    [field: SerializeField] public DialogueResponse Normal_Donut { get; private set; }
    [field: SerializeField] public DialogueResponse Unfriendly_Donut { get; private set; }

    [field: SerializeField] public DialogueResponse AlreadyToldCode { get; private set; }

    [field: SerializeField] public DialogueResponse Documents { get; private set; }

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

    [field: SerializeField] public Objective Q4O1;
    [field: SerializeField] public Objective Q4O3;

    [field: SerializeField] private ObjectiveHandler objectiveHandler;
    private Quest currentQuest;

    private Analytics _analytics;

    public override DialogueResponse GetResponse(Inference inference, bool sensitive, int rudeIncidents,
        float rudeCooldown)
    {
        currentQuest = objectiveHandler.GetCurrentQuest();
        Objective currentObjective = currentQuest.currentObjective;

        if (inference.IsUnderstood)
        {
            _analytics = GameObject.Find("Analytics").GetComponent<Analytics>();
            _analytics.AddGeneral(inference.Intent, Time.timeSinceLevelLoad, getColleagueType(),
                _analytics.getLastDistance());

            string intent = sensitive ? inference.Intent : RemoveSensitive(inference.Intent);

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

            if (intent == "Friendly_1111")
            {
                return PreviousResponse = Friendly_1111;
            }

            if (intent == "Normal_1111")
            {
                return PreviousResponse = Normal_1111;
            }

            if (intent == "Unfriendly_1111")
            {
                return PreviousResponse = Unfriendly_1111;
            }

            if (intent == "Friendly_SomeoneInMyOffice")
            {
                return PreviousResponse = Friendly_SomeoneInMyOffice;
            }

            if (intent == "Normal_SomeoneInMyOffice")
            {
                return PreviousResponse = Normal_SomeoneInMyOffice;
            }

            if (intent == "Unfriendly_SomeoneInMyOffice")
            {
                return PreviousResponse = Unfriendly_SomeoneInMyOffice;
            }

            if (intent == "Friendly_North")
            {
                return PreviousResponse = Friendly_North;
            }

            if (intent == "Normal_North")
            {
                return PreviousResponse = Normal_North;
            }

            if (intent == "Unfriendly_North")
            {
                return PreviousResponse = Unfriendly_North;
            }

            if (intent == "Friendly_NeedHelp")
            {
                return PreviousResponse = Friendly_NeedHelp;
            }

            if (intent == "Normal_NeedHelp")
            {
                return PreviousResponse = Normal_NeedHelp;
            }

            if (intent == "Unfriendly_NeedHelp")
            {
                return PreviousResponse = Unfriendly_NeedHelp;
            }

            if (intent == "Friendly_CameraCode")
            {
                if (currentObjective.questIndex < Q4O1.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q4O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Friendly_CameraCode;
            }

            if (intent == "Normal_CameraCode")
            {
                if (currentObjective.questIndex < Q4O1.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q4O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Normal_CameraCode;
            }

            if (intent == "Unfriendly_CameraCode")
            {
                if (currentObjective.questIndex < Q4O1.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q4O1 == currentObjective)
                {
                    currentQuest.Progress();
                }

                return PreviousResponse = Unfriendly_CameraCode;
            }

            if (intent == "Friendly_Donut")
            {
                if (currentObjective.questIndex < Q4O3.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q4O3 == currentObjective)
                {
                    currentQuest.Progress();
                }

                if (currentObjective.questIndex > Q4O3.questIndex)
                {
                    return PreviousResponse = AlreadyToldCode;
                }

                return PreviousResponse = Friendly_Donut;
            }

            if (intent == "Normal_Donut")
            {
                if (currentObjective.questIndex < Q4O3.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q4O3 == currentObjective)
                {
                    currentQuest.Progress();
                }

                if (currentObjective.questIndex > Q4O3.questIndex)
                {
                    return PreviousResponse = AlreadyToldCode;
                }

                return PreviousResponse = Normal_Donut;
            }

            if (intent == "Unfriendly_Donut")
            {
                if (currentObjective.questIndex < Q4O3.questIndex)
                {
                    return NotUnderstood();
                }

                if (Q4O3 == currentObjective)
                {
                    currentQuest.Progress();
                }

                if (currentObjective.questIndex > Q4O3.questIndex)
                {
                    return PreviousResponse = AlreadyToldCode;
                }

                return PreviousResponse = Unfriendly_Donut;
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
    private string getColleagueType()
    {
        if (name.Contains("Intern"))
        {
            return "Intern";
        }
        else if (name.Contains("Boss"))
        {
            return "Boss";
        }
        else
        {
            return "Colleague";
        }
    }
}