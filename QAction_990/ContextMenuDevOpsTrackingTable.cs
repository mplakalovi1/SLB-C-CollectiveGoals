namespace QAction_990
{
    using System;

    using MyExtension;

    using Skyline.DataMiner.Scripting;
    using Skyline.DataMiner.Utils.Table.ContextMenu;

    internal enum Action
    {
        AddEmployee = 1,
        RemoveEmployee = 0,
    }

    internal class ContextMenuDevOpsTrackingTable : ContextMenu<Action>
    {
        public ContextMenuDevOpsTrackingTable(SLProtocol protocol, object contextMenuData, int tablePid)
            : base(protocol, contextMenuData, tablePid)
        {
        }

        public override void ProcessContextMenuAction()
        {
            switch (Action)
            {
                case Action.AddEmployee:
                    AddEmployee(Data);
                    Protocol.Log("QA" + Protocol.QActionID + "|ContextMenuDevOpsTrackingTable|ProcessContextMenuAction|AddEmployee", LogType.DebugInfo, LogLevel.NoLogging);
                    break;
                case Action.RemoveEmployee:
                    RemoveEmployee(Data);
                    Protocol.Log("QA" + Protocol.QActionID + "|ContextMenuDevOpsTrackingTable|ProcessContextMenuAction|RemoveEmployee", LogType.DebugInfo, LogLevel.NoLogging);
                    break;
                default:
                    Protocol.Log("QA" + Protocol.QActionID + "|ContextMenuDevOpsTrackingTable|Process|Unexpected ContextMenu value '" + ActionRaw + "'", LogType.Error, LogLevel.NoLogging);
                    break;
            }
        }

        private void AddEmployee(string[] data)
        {
            var employeeName = data[0].Trim(); // Removing leading or trailing white-spaces, if there's any;

            // Validating the Display Key (Employee Name):
            if (!DevOpsTrackingHelper.IsDisplayKeyValid(Protocol, employeeName))
            {
                return;
            }

            var row = new DevopstrackingtableQActionRow
            {
                Devopstrackingindex_1001 = Guid.NewGuid().ToString(),
                Devopstrackingname_1002 = employeeName,
            };

            Protocol.AddRow(TablePid, row.ToObjectArray());
        }

        private void RemoveEmployee(string[] data)
        {
            Protocol.DeleteRow(TablePid, data[0]);
        }
    }
}