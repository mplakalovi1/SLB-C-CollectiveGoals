namespace MyExtension
{
    using System;
    using System.Text.RegularExpressions;

    using Skyline.DataMiner.Scripting;

    public static class DevOpsTrackingHelper
    {
        public static bool IsDisplayKeyValid(SLProtocol protocol, string displayKey)
        {
            string message;

            // Checking if it's null, empty or white-spaces only:
            if (String.IsNullOrWhiteSpace(displayKey))
            {
                message = "The 'Name' must be non-empty. Please enter a different value.";

                protocol.Log($"QA{protocol.QActionID}{message}", LogType.Allways, LogLevel.NoLogging);
                protocol.ShowInformationMessage(message);
                return false;
            }

            // Checking for specific characters:
            // Semicolons (";"), question marks ("?") and asterisks ("*") must be avoided:
            string pattern = "[;?*]";

            if (Regex.IsMatch(displayKey, pattern))
            {
                message = "The 'Name' must not contain Semicolons (';'), question marks ('?') and asterisks ('*'). Please enter a different value.";

                protocol.Log($"QA{protocol.QActionID}{message}", LogType.Allways, LogLevel.NoLogging);
                protocol.ShowInformationMessage(message);
                return false;
            }

            // Checking if primaryKey already exists:
            if (protocol.Exists(Parameter.Devopstrackingtable.tablePid, displayKey))
            {
                message = "The 'Name' must be unique; the entered value already exists. Please choose a different name.";

                protocol.Log($"QA{protocol.QActionID}{message}", LogType.Allways, LogLevel.NoLogging);
                protocol.ShowInformationMessage(message);
                return false;
            }

            return true;
        }
    }
}