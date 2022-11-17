using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// using Newtonsoft.Json;

// 11/16/2022 08:57 am - SSN 

using System.Text.Json;
using Newtonsoft.Json;
using static ssn_373_slack_SendMessage.FeedbackUtil;

namespace ssn_373_slack_SendMessage
{
    internal class Slack_Utility
    {





        public static async Task SendMessage(string channel, string message)
        {
            bool testAccess = (message ?? "").ToLower() == "testaccess";



            RunStatus.Rest();

            string? slackToken = Environment.GetEnvironmentVariable("ssn_slack_token_send_messages");

            if (string.IsNullOrEmpty(slackToken))
            {
                RunStatus.Add(PROGRESS_RECORD_TYPE.WARNNING, "ssn-20221116-0904", "Environment variable for Slack token is not set [ssn_slack_token_send_messages]");
                RunStatus.Add(PROGRESS_RECORD_TYPE.WARNNING, "",String.Format("\n\n{0}\n\n", @"  C:\>SETX ssn_slack_token_send_messages ""<YOUR-SLACK-USER-OAUTH-TOKEN>"" /M"));
                return;
            }


            if (string.IsNullOrWhiteSpace(channel))
            {
                RunStatus.Add(PROGRESS_RECORD_TYPE.WARNNING, "ssn-20221116-0913", "Channel is null or empty.");
            }


            if (string.IsNullOrWhiteSpace(message) && !testAccess)
            {
                RunStatus.Add(PROGRESS_RECORD_TYPE.WARNNING, "ssn-20221116-0914", "Message is null or empty.  Type 'TestAccess' for a message, if you are testing only.");

            }


            if (RunStatus.HasErrors)
            {
                return ;
            }


            try
            {
                SlackPayload? payload = null;

                if (!string.IsNullOrWhiteSpace(message) && !testAccess)
                {
                    payload = new SlackPayload
                    {
                        as_user = true,  // User owns the token
                        channel = channel,
                        text = message
                    };
                }





                string url_SlackAPI = $"https://slack.com/api/chat.postMessage";


                if (payload == null)
                {
                    url_SlackAPI = $"https://slack.com/api/auth.test";
                }




                RunStatus.Add(await SendMessage_sub(url_SlackAPI, slackToken, payload));

            }
            catch (Exception ex)
            {

                RunStatus.Add(PROGRESS_RECORD_TYPE.ERROR, "ssn-20221116-0931", ex);

            }


        }



        private static async Task<ProgressRecord> SendMessage_sub(string slackSendBaseUrl_v2, string slackToken, SlackPayload? payload)
        {

            ProgressRecord record = new ProgressRecord();


            using (var client = new HttpClient())
            {

                FormUrlEncodedContent? urlEncodedString = null;

                if (payload != null)
                {
                    urlEncodedString = ObjectConverter.ObjectToUrlEncodedString(payload);
                }


                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", slackToken);


                var response = await client.PostAsync(slackSendBaseUrl_v2, urlEncodedString);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                record.slackReturnRecord = JsonConvert.DeserializeObject<SlackReturnRecord>(content);


            }

            return record;
        }


    }
}
