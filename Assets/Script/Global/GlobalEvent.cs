using AirplaneControllerScript;
using System;

public static class GlobalEvent {


    // airplane creation event
    internal static event Action<ushort, AirplaneController> onAirplaneCreatedEvent = delegate { };
    internal static event Action<AirplaneController, ushort> onSurfaceMapNodeCreatedEvent = delegate { };
    internal static event Action<AirplaneController, ushort> onRadarMapNodeCreatedEvent = delegate { };
    // airplane creattion method
    internal static void OnAirplaneCreated(ushort rGlobapApID, AirplaneController rAirplaneController) {
        GlobalData.allActiveAirplane.Add(rGlobapApID, rAirplaneController);
        onAirplaneCreatedEvent.Invoke(rGlobapApID, rAirplaneController);
    }
    internal static void OnRadarMapNodeCreated(AirplaneController rAirplaneController, ushort rStartLineIndex) {
        onRadarMapNodeCreatedEvent.Invoke(rAirplaneController, rStartLineIndex);
    }
    internal static void OnSurfaceMapNodeCreated(AirplaneController rAirplaneController, ushort rStartLineIndex) {
        onSurfaceMapNodeCreatedEvent.Invoke(rAirplaneController, rStartLineIndex);
    }







    // infro view event
    internal static event Action onScoreCountChangedEvent = delegate { };
    // info view method
    internal static void OnScoreCountChange() {
        onScoreCountChangedEvent.Invoke();
    }


}

