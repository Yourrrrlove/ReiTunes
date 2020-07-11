﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReiTunes.Core {

    public interface IEventRepository {

        public IEnumerable<IEvent> GetEvents(Guid aggregateId);

        public IEnumerable<IEvent> GetAllEvents();

        public void Save(IEvent @event);

        public void Save(IEnumerable<IEvent> events);

        public bool ContainsEvent(Guid eventId);
    }
}