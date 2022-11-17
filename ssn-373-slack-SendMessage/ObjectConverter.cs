// 11/16/2022 05:37 pm - SSN 

namespace ssn_373_slack_SendMessage
{
    internal class ObjectConverter
    {
        public static Dictionary<string, string> ObjectToDictionary(object obj)
        {
            // https://stackoverflow.com/questions/9115413/is-there-an-easy-way-to-convert-object-properties-to-a-dictionarystring-string
            return obj.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(obj)?.ToString() ?? "");
        }


        public static FormUrlEncodedContent ObjectToUrlEncodedString(object obj)
        {
            return new FormUrlEncodedContent(ObjectToDictionary(obj));
        }

    }
}
