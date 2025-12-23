public enum ControlType
{
    [EnumMark("开始")]
    Begin,
    [EnumMark("结束")]
    End,
    [EnumMark("答题步骤")]
    TheoryStep,
    [EnumMark("操作步骤")]
    SimulationStep,
    [EnumMark("故障现象")]
    FaultFixPhenomenon,
    [EnumMark("故障原因")]
    FaultfixCause,
    [EnumMark("故障确认")]
    FaultfixConfirm
}