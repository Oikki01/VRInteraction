using UnityEngine.EventSystems;

public interface InterfaceUsed : IEventSystemHandler
{
    void Used(InputEventData game);

    void Using(InputEventData game);

    void UnUsed(InputEventData game);
}