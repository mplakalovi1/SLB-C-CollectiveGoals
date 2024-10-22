namespace MyExtension
{
    using System;

    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Scripting;

    /// <summary>
    /// MyExtension Class. Contains logic to be reused across solution.
    /// </summary>
    public static class MyExtension
    {
        /// <summary>
        /// Method for checking if value already exists in specified table column.
        /// </summary>
        /// <param name="protocol">Link with SLProtocol process.</param>
        /// <param name="tablePid">Table Pid.</param>
        /// <param name="columnIdx">Column Index.</param>
        /// <param name="value">Value to be checked.</param>
        /// <returns>Flag indicating if value exists or not.</returns>
        public static bool ValueExists(SLProtocol protocol, int tablePid, uint columnIdx, object value)
        {
            object[] tableColumn = (object[])((object[])protocol.NotifyProtocol((int)NotifyType.NT_GET_TABLE_COLUMNS, tablePid, new[] { columnIdx }))[0];
            return Array.Exists(tableColumn, element => element.Equals(value));
        }
    }
}