using System;
using QAction_990;
using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: DevOps Points - ContextMenu.
/// </summary>
public static class QAction
{
    /// <summary>
    /// The QAction entry point.
    /// </summary>
    /// <param name="protocol">Link with SLProtocol process.</param>
    /// <param name="contextMenuData"><see cref="Object"/> containing the table ContextMenu data.</param>
    public static void Run(SLProtocol protocol, object contextMenuData)
    {
        try
        {
            ContextMenuDevOpsTrackingTable contextMenu = new ContextMenuDevOpsTrackingTable(
                protocol,
                contextMenuData,
                Parameter.Devopstrackingtable.tablePid);
            contextMenu.ProcessContextMenuAction();
        }
        catch (Exception ex)
        {
            protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
        }
    }
}