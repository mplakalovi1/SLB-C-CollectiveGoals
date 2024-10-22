namespace QAction_990
{
    using System;

    using MyExtension;

    using Skyline.DataMiner.Scripting;
    using Skyline.DataMiner.Utils.Table.ContextMenu;

    internal class ContextMenuTableManagercollectiveGoals : ContextMenuTableManagerBase
    {
        private readonly int autoIncPid;

        public ContextMenuTableManagercollectiveGoals(SLProtocol protocol, object contextMenuData, int tablePid, int autoIncPid)
            : base(protocol, contextMenuData, tablePid)
        {
            if (autoIncPid < 0)
            {
                throw new ArgumentException(autoIncPid + " is < 0.", nameof(autoIncPid));
            }

            this.autoIncPid = autoIncPid;
        }

        protected override void AddItem()
        {
            // Prepare
            int autoIncKey = Convert.ToInt32(Protocol.GetParameter(autoIncPid)) + 1;
            object[] rowData = new object[5];
            rowData[0] = Convert.ToString(autoIncKey); // PK;
            rowData[1] = Data[0]; // Name;
            rowData[2] = Data[1]; // Description;
            rowData[3] = Data[2]; // Status;
            rowData[4] = Data[3]; // Due Date;

            // Sanity Checks
            // PK Check:
            if (Protocol.Exists(TablePid, (string)rowData[0]))
            {
                Protocol.Log($"QA{Protocol.QActionID}|AddItem|Table '{TablePid}' : Row with key '{rowData[0]}' already exists.", LogType.Error, LogLevel.NoLogging);
                return;
            }

            // Name [IDX] Check:
            if(MyExtension.ValueExists(Protocol, TablePid, Parameter.Collectivegoals.Idx.collectivegoalsname_1002, rowData[1]))
            {
                Protocol.Log($"QA{Protocol.QActionID}|AddItem|Table '{TablePid}' : Row with name '{rowData[1]}' already exists.", LogType.Error, LogLevel.NoLogging);
                Protocol.ShowInformationMessage($"Name '{rowData[1]}' already exists.");
                return;
            }
            ////Protocol.Log($"QA{Protocol.QActionID}|AddItem|Table '{TablePid}' : {String.Join(";", rowData)}", LogType.DebugInfo, LogLevel.NoLogging);

            // Add
            Protocol.AddRow(TablePid, rowData);
            if (autoIncPid > -1)
            {
                Protocol.SetParameter(autoIncPid, rowData[0]);
            }
        }

        protected override void DeleteItems()
        {
            ////Protocol.Log($"QA{Protocol.QActionID}|DeleteItems|Table '{tablePid}' : {String.Join(";", Data)}", LogType.DebugInfo, LogLevel.NoLogging);

            Protocol.DeleteRow(TablePid, Data);
        }

        protected override void EditItem()
        {
            foreach (var item in Data)
            {
                Protocol.Log($"QA{Protocol.QActionID}|EditItem|{item}", LogType.DebugInfo, LogLevel.NoLogging);

            }
            string rowKey = Data[Data.Length - 1];

            object[] rowData = new object[5];
            rowData[0] = null;
            rowData[1] = null; // Name;
            rowData[2] = Data[0]; // Description;
            rowData[3] = Data[1]; // Status;
            rowData[4] = Data[2]; // Due Date;

            Protocol.SetRow(TablePid, rowKey, rowData);
        }
    }
}