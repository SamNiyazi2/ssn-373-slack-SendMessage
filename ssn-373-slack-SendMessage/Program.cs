// See https://aka.ms/new-console-template for more information

// 11/16/2022 08:55 am - SSN - Creating project for Upwork job https://www.upwork.com/jobs/Console-App-Send-Slack-Messages-NET6_~01eae90e98b5f02d4e/

using ssn_373_slack_SendMessage;
using static ssn_373_slack_SendMessage.FeedbackUtil;




if (args.Length < 2)
{
    Console.WriteLine("\n\n");
    RunStatus.Add(PROGRESS_RECORD_TYPE.PROGRESS, "", "Syntax: ssn-slack-SendMessage <channel> \"<your message>\"\n\n ");
    RunStatus.Add(PROGRESS_RECORD_TYPE.PROGRESS, "", "You can replace message with 'TestAccess' to test your access to Slack.\n\n");
}
else
{
    string channel = args[0];
    string message = args[1];

    if (string.IsNullOrWhiteSpace(channel))
    {
        RunStatus.Add(PROGRESS_RECORD_TYPE.ERROR, "", "You can't pass an empty value for channel.");
    }

    if (string.IsNullOrWhiteSpace(message))
    {
        RunStatus.Add(PROGRESS_RECORD_TYPE.ERROR, "", "You can't pass an empty message.");
    }

    if (!RunStatus.HasErrors)
    {
        Console.WriteLine("\n\n");
        Console.WriteLine("    Sending message:\n");
        Console.WriteLine($"       Channel: {channel}");
        Console.WriteLine($"       Message: {message}");
        Console.WriteLine( "\n\n");

        await Slack_Utility.SendMessage(channel, message);
    }

}


ReportRunStatus();


return;





void ReportRunStatus()
{


    foreach (ProgressRecord r in progressRecords)
    {
        if (r.RecordType == PROGRESS_RECORD_TYPE.ERROR) { Console.ForegroundColor = ConsoleColor.Red; }

        if (r.RecordType == PROGRESS_RECORD_TYPE.WARNNING) { Console.ForegroundColor = ConsoleColor.Yellow; }


        Console.WriteLine($"{r.ProgressCode,-20} {r.ProgessMessage}");


        if (r.exception != null)
        {

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(r.exception.Message);
            Console.WriteLine(r.exception.StackTrace);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

        }



        if (r.slackReturnRecord != null)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("     Slack response:");
            Console.WriteLine(Environment.NewLine);

            int padding = 20;

            wl(padding, "OK", r.slackReturnRecord.ok.ToString());
            wl(padding, "Url", r.slackReturnRecord.url);
            wl(padding, "User_Id", r.slackReturnRecord.user_id);
            wl(padding, "Team_Id", r.slackReturnRecord.team_id);
            wl(padding, "Error", r.slackReturnRecord.error);
            wl(padding, "Channel", r.slackReturnRecord.channel);
            wl(padding, "Ts", r.slackReturnRecord.ts);


            if (r.slackReturnRecord.message != null)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("          Message:");
                Console.WriteLine(Environment.NewLine);

                padding = 30;
                wl(padding, "bot_id", r.slackReturnRecord.message.bot_id);
                wl(padding, "type", r.slackReturnRecord.message.type);
                wl(padding, "text", r.slackReturnRecord.message.text);
                wl(padding, "user", r.slackReturnRecord.message.user);
                wl(padding, "ts", r.slackReturnRecord.message.ts);
                wl(padding, "app_id", r.slackReturnRecord.message.app_id);
                wl(padding, "team", r.slackReturnRecord.message.team);

            }

            Console.WriteLine(Environment.NewLine);

        }

        Console.ResetColor();

    }


    Console.WriteLine(Environment.NewLine);
    Console.WriteLine("End of run.");
    Console.WriteLine(Environment.NewLine);

}



static void wl(int padding, string label, string? value)
{
    label = label.PadLeft(padding);
    if (!string.IsNullOrWhiteSpace(value)) Console.WriteLine($"{label}: {value}");
}

