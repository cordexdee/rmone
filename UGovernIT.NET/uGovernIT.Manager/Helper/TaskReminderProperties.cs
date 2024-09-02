using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace uGovernIT.Web.Helpers
{
    [DataContractAttribute]
    public class TaskReminderProperties
    {
        /// <summary>
        /// This is to save whether the reminder is enabled or not
        /// </summary>
        [DataMember]
        public bool Reminder1 { get; set; }

        /// <summary>
        /// This is to save the reminder duration in Minutes
        /// </summary>
        [DataMember]
        public double Reminder1Duration { get; set; }

        /// <summary>
        /// Reminder1Frequency is to save the reminder frequency i.e. After/Before
        /// </summary>
        [DataMember]
        public string Reminder1Frequency { get; set; }

        /// <summary>
        /// This is to save whether the reminder is enabled or not
        /// </summary>
        [DataMember]
        public bool Reminder2 { get; set; }

        /// <summary>
        /// This is to save the reminder duration in Minutes
        /// </summary>
        [DataMember]
        public double Reminder2Duration { get; set; }

        /// <summary>
        /// Reminder2Frequency is to save the reminder frequency i.e. After/Before
        /// </summary>
        [DataMember]
        public string Reminder2Frequency { get; set; }

    }
}