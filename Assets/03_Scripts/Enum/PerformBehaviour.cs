public enum PerformBehaviour
{
    [EnumMark("无")]
    None,
    [EnumMark("限制输入整型最大值")]
    RangeIntMaxInput,
    [EnumMark("限制输入整型最小值")]
    RangeIntMinInput,
    [EnumMark("输入浮点数")]
    FloatInput,
    [EnumMark("选择材质添加类别")]
    MaterialAddType,
    [EnumMark("选择动态图播放模式类别")]
    FlashPlayMode,
    [EnumMark("模型对照表Cid")]
    CompareCid
}