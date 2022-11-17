using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ssn_373_slack_SendMessage.Slack_Utility;

// 11/17/2022 02:29 am - SSN 

namespace ssn_373_slack_SendMessage
{
    internal class FeedbackUtil
    {

        public static List<ProgressRecord> progressRecords = new List<ProgressRecord>();


        public enum PROGRESS_RECORD_TYPE
        {
            PROGRESS = 2,
            ERROR = 3,
            WARNNING = 4
        }


        public class ProgressRecord
        {
            public string? ProgressCode { get; set; }
            public string? ProgessMessage { get; set; }
            public Exception? exception { get; set; }
            public PROGRESS_RECORD_TYPE RecordType { get; set; }
            public SlackReturnRecord? slackReturnRecord { get; set; }

        }


        internal class RunStatus
        {


            internal static void Rest()
            {
                progressRecords = new List<ProgressRecord>();
            }


            internal static void Add(ProgressRecord? progressRecord)
            {
                if (progressRecord != null) progressRecords.Add(progressRecord);
            }


            internal static void Add(PROGRESS_RECORD_TYPE messageType, string recordCode, string progressMessage)
            {
                progressRecords.Add(new ProgressRecord { RecordType = messageType, ProgressCode = recordCode, ProgessMessage = progressMessage });
            }


            internal static void Add(PROGRESS_RECORD_TYPE messageType, string recordCode, Exception exception)
            {
                progressRecords.Add(new ProgressRecord { RecordType = messageType, ProgressCode = recordCode, exception = exception });
            }


            public static bool HasErrors { get { return progressRecords.Any(r => r.RecordType == PROGRESS_RECORD_TYPE.ERROR); } }


        }



        #region Slack response message

        public class SlackReturnRecord
        {
            public bool ok { get; set; }
            public string? error { get; set; }
            public string? url { get; set; }
            public string? team { get; set; }
            public string? user { get; set; }
            public string? team_id { get; set; }
            public string? user_id { get; set; }
            public string? bot_id { get; set; }
            public string? channel { get; set; }
            public string? ts { get; set; }
            public bool is_interprise_install { get; set; }

            public SlackReturnRecord_Message? message { get; set; }
        }


        public class SlackReturnRecord_Message
        {
            public string? bot_id { get; set; }
            public string? type { get; set; }
            public string? text { get; set; }
            public string? user { get; set; }
            public string? ts { get; set; }
            public string? app_id { get; set; }
            public string? team { get; set; }
        }


        public class SlackPayload
        {
            public string? channel { get; set; }
            public string? text { get; set; }
            public bool as_user { get; set; }


        }

        #endregion Slack response message

    }
}
