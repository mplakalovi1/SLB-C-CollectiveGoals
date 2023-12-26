using System;
using System.Linq;

using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: QActionName.
/// </summary>
public static class QAction
{
    private const int Advocate_Threshold = 750;
    private const int Enabler_Threshold = 5000;
    private const int Catalyst_Threshold = 25000;

    public enum Level
    {
        Member,
        Advocate,
        Enabler,
        Catalyst,
    }

    /// <summary>
    /// The QAction entry point.
    /// </summary>
    /// <param name="protocol">Link with SLProtocol process.</param>
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            var primaryKey = protocol.RowKey();
            var devOpsPoints = Convert.ToInt32(protocol.GetParameter(Parameter.Write.devopstrackingpoints_1053));
            protocol.Log($"QA{protocol.QActionID}|Write Logic Triggered!|PK: {primaryKey}| DevOps Points Entered: {devOpsPoints}", LogType.DebugInfo, LogLevel.NoLogging);

            int level;
            if (devOpsPoints >= Catalyst_Threshold)
            {
                level = Convert.ToInt32(Level.Catalyst);
            }
            else if (devOpsPoints >= Enabler_Threshold)
            {
                level = Convert.ToInt32(Level.Enabler);
            }
            else if (devOpsPoints >= Advocate_Threshold)
            {
                level = Convert.ToInt32(Level.Advocate);
            }
            else
            {
                level = Convert.ToInt32(Level.Member);
            }

            protocol.devopstrackingtable[primaryKey, Parameter.Devopstrackingtable.Idx.devopstrackinglevel_1004] = level;

            // Updating total points and Completion rate:
            // Obtaining Points Column:
            object[] pointsColumn = (object[])(
                (object[])protocol.NotifyProtocol(
                        321, // Notify call number.
                        Parameter.Devopstrackingtable.tablePid,
                        new uint[] { Parameter.Devopstrackingtable.Idx.devopstrackingpoints_1003 }))[0];
            foreach (object point in pointsColumn)
            {
                protocol.Log($"QA{protocol.QActionID}|DevOps Column|{point}", LogType.Allways, LogLevel.NoLogging);
            }

            // Obtaining Goal:
            var pointsGoal = Convert.ToInt32(protocol.GetParameter(Parameter.targetgoal_51));
            protocol.Log($"QA{protocol.QActionID}|Points Goal:|{pointsGoal}", LogType.Allways, LogLevel.NoLogging);

            // Calculating and updating total points and completion rate:
            var totalPoints = pointsColumn.Select(obj => Convert.ToInt32(obj)).Sum();
            protocol.Log($"QA{protocol.QActionID}|total points sum|{totalPoints}", LogType.Allways, LogLevel.NoLogging);

            var completionRate = ((double)totalPoints / pointsGoal) * 100;
            protocol.Log($"QA{protocol.QActionID}|Rate:|{completionRate}", LogType.Allways, LogLevel.NoLogging);

            protocol.SetParameters(
                new[] { Parameter.totalpoints_50, Parameter.completionrate_52 },
                new object[] { totalPoints, completionRate });
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }
}