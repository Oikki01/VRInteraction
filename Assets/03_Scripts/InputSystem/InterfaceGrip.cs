using UnityEngine.EventSystems;

public interface InterfaceGrip : IEventSystemHandler
{
    void GripOn(InputEventData game);

    void Griping(InputEventData game);

    void GripDown(InputEventData game);
}