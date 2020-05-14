// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Tracing;

namespace Microsoft.PowerToys.Telemetry
{
    /// <summary>
    /// Privacy Tag values
    /// </summary>
    public enum PartA_PrivTags
           : ulong
    {
        /// <nodoc/>
        None = 0,

        /// <nodoc/>
        ProductAndServicePerformance = 0x0000000001000000u,

        /// <nodoc/>
        ProductAndServiceUsage = 0x0000000002000000u,
    }

    /// <summary>
    /// Base class for telemetry events.
    /// </summary>
    public class TelemetryBase : EventSource
    {
        /// <summary>
        /// The event tag for this event source.
        /// </summary>
        public const EventTags ProjectTelemetryTagProductAndServicePerformance = (EventTags)0x0000000001000000u;

        /// <summary>
        /// The event keyword for this event source.
        /// </summary>
        public const EventKeywords ProjectKeywordMeasure = (EventKeywords)0x0000400000000000;

        /// <summary>
        /// Group ID for Powertoys project.
        /// </summary>
        private static readonly string[] PowerToysTelemetryTraits = { "ETW_GROUP", "{4f50731a-89cf-4782-b3e0-dce8c90476ba}" };

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryBase"/> class.
        /// </summary>
        /// <param name="eventSourceName">.</param>
        public TelemetryBase(
            string eventSourceName)
            : base(
            eventSourceName,
            EventSourceSettings.EtwSelfDescribingEventFormat,
            PowerToysTelemetryTraits)
        {
            return;
        }
    }
}
