using ConferenceScheduler.Entities;
using ConferenceScheduler.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferenceScheduler.Exceptions;

namespace ConferenceScheduler.Builders
{
    public class RoomCollectionBuilder
    {
        List<Room> _rooms = new List<Room>();
        List<RoomBuilder> _roomBuilders = new List<RoomBuilder>();

        public IEnumerable<Room> Build()
        {
            return this.Build(1);
        }

        public IEnumerable<Room> Build(int nextId)
        {
            var results = new List<Room>();

            // Add all pre-built rooms to the results
            results.AddRange(_rooms);
            foreach (var room in _rooms)
                if (room.Id.Equals(0))
                {
                    room.Id = nextId;
                    nextId++;
                }

            // Add the output of all RoomBuilders to the results
            foreach (var roomBuilder in _roomBuilders)
            {
                var room = roomBuilder.Build(nextId);
                results.Add(room);
                if (room.Id.Equals(nextId))
                    nextId++;
            }

            return results;
        }

        public RoomCollectionBuilder Add(Room room)
        {
            if (!room.Id.Equals(0))
                if (this.Build().Select(r => r.Id).Contains(room.Id))
                    throw new DuplicateEntityException(typeof(Room), room.Id);

            _rooms.Add(room);
            return this;
        }

        public RoomCollectionBuilder Add(RoomBuilder roomBuilder)
        {
            if (roomBuilder.HasSpecifiedId())
            {
                var roomId = roomBuilder.GetSpecifiedId();
                if (this.Build().Select(r => r.Id).Contains(roomId))
                    throw new DuplicateEntityException(typeof(Room), roomId);
            }

            _roomBuilders.Add(roomBuilder);
            return this;
        }
    }
}
