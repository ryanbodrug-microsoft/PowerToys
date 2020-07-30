﻿using Microsoft.PowerToys.Telemetry;
using Microsoft.PowerToys.Telemetry.Events;
using System.Diagnostics.Tracing;

namespace ColorPicker.Telemetry
{
    [EventData]
    public class ColorPickerZoomOpenedEvent : EventBase, IEvent
    {
        public PartA_PrivTags PartA_PrivTags => PartA_PrivTags.ProductAndServiceUsage;
    }
}
