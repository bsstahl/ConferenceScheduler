﻿using ConferenceScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceScheduler.Optimizer.DataSetsTest
{
    public class EventHandler
    {
        public IEnumerable<Assignment> Assignments { get; private set; }

        public void EngineUpdateEventHandler(ProcessUpdateEventArgs args)
        {
            var assignments = args.Assignments;
            this.Assignments = assignments;
            this.Assignments.WriteSchedule();
        }
    }
}
