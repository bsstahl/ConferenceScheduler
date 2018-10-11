using Xunit;
using ConferenceScheduler.Entities;
using ConferenceScheduler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceScheduler.Builders;
using Xunit.Abstractions;

namespace ConferenceScheduler.Optimizer.DataSetsTest
{
    public enum Topic
    {
        SoftSkills = 1,
        IOT = 2,
        Python = 3,
        Agile = 4,
        AI = 5,
        Mobile = 6,
        Cloud = 7,
        Blockchain = 8,
        DevOps = 9,
        Angular = 10,
        Javascript = 11,
        DotNetCore = 12,
        BI = 13,
        ReactRedux = 14,
        IOTLab = 15
    }

    public class SoCalCodeCampSanDiego2017
    {
        ITestOutputHelper _output;

        public SoCalCodeCampSanDiego2017(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ScheduleWithoutPreferences()
        {
            var engine = (null as IConferenceOptimizer).Create();
            var sessions = new SessionsCollection();
            var timeslots = new List<Timeslot>();


            // Presenters
            var presenterRichClingman = Presenter.Create(1, "Rich Clingman");
            var presenterTreyHunner = Presenter.Create(2, "Trey Hunner");
            var presenterMichaelLebo = Presenter.Create(3, "Michael Lebo");
            var presenterAaronRuckman = Presenter.Create(4, "Aaron Ruckman");
            var presenterDanielEgan = Presenter.Create(5, "Daniel Egan");
            var presenterLorraineKan = Presenter.Create(6, "Lorraine Kan");
            var presenterChrisLucian = Presenter.Create(7, "Chris Lucian");
            var presenterBretStateham = Presenter.Create(8, "Bret Stateham");
            var presenterRobinShahan = Presenter.Create(9, "Robin Shahan");
            var presenterJustinJames = Presenter.Create(10, "Justin James");
            var presenterTobiasHughes = Presenter.Create(11, "Tobias Hughes");
            var presenterWendySteinman = Presenter.Create(12, "Wendy Steinman");
            var presenterOgunTigli = Presenter.Create(13, "Ogun Tigli");
            var presenterHibaMughal = Presenter.Create(14, "Hiba Mughal");
            var presenterEricFloe = Presenter.Create(15, "Eric Floe");
            var presenterHattanShobokshi = Presenter.Create(16, "Hattan Shobokshi");
            var presenterJonBachelor = Presenter.Create(17, "Jon Bachelor");
            var presenterPaulWhitmer = Presenter.Create(18, "Paul Whitmer");
            var presenterDavidFord = Presenter.Create(19, "David Ford");
            var presenterVaziOkhandiar = Presenter.Create(20, "Vazi Okhandiar");
            var presenterChrisStead = Presenter.Create(21, "Chris Stead");
            var presenterRyanMilbourne = Presenter.Create(22, "Ryan Milbourne");
            var presenterMaxNodland = Presenter.Create(23, "Max Nodland");
            var presenterBarryStahl = Presenter.Create(24, "Barry Stahl");
            var presenterJustineCocci = Presenter.Create(25, "Justine Cocci");
            var presenterChrisGriffith = Presenter.Create(26, "Chris Griffith");
            var presenterIotLaboratory = Presenter.Create(27, "IOT Laboratory");




            // Sessions
            var session01 = sessions.Add(1, (int)Topic.SoftSkills, presenterRichClingman);
            var session02 = sessions.Add(2, (int)Topic.IOT, presenterRichClingman);

            var session03 = sessions.Add(3, (int)Topic.Python, presenterTreyHunner);
            var session04 = sessions.Add(4, (int)Topic.Python, presenterTreyHunner);

            var session05 = sessions.Add(5, null, presenterMichaelLebo);

            var session06 = sessions.Add(6, (int)Topic.Agile, presenterAaronRuckman);

            var session07 = sessions.Add(7, (int)Topic.AI, presenterDanielEgan);

            var session08 = sessions.Add(8, (int)Topic.Mobile, presenterLorraineKan);

            var session09 = sessions.Add(9, (int)Topic.Agile, presenterChrisLucian);

            var session10 = sessions.Add(10, (int)Topic.IOT, presenterBretStateham);

            var session11 = sessions.Add(11, (int)Topic.Cloud, presenterRobinShahan);

            var session12 = sessions.Add(12, (int)Topic.SoftSkills, presenterJustinJames);

            var session13 = sessions.Add(13, (int)Topic.Blockchain, presenterTobiasHughes);

            var session14 = sessions.Add(14, null, presenterWendySteinman);

            var session15 = sessions.Add(15, (int)Topic.Angular, presenterOgunTigli);

            var session16 = sessions.Add(16, (int)Topic.Mobile, presenterHibaMughal);

            var session17 = sessions.Add(17, (int)Topic.Mobile, presenterEricFloe);

            var session18 = sessions.Add(18, (int)Topic.Javascript, presenterHattanShobokshi);

            var session19 = sessions.Add(19, (int)Topic.Mobile, presenterJonBachelor);

            var session20 = sessions.Add(20, (int)Topic.DotNetCore, presenterPaulWhitmer);

            var session21 = sessions.Add(21, (int)Topic.Javascript, presenterDavidFord);

            var session22 = sessions.Add(22, (int)Topic.BI, presenterVaziOkhandiar);

            var session23 = sessions.Add(23, (int)Topic.Javascript, presenterChrisStead);

            var session24 = sessions.Add(24, (int)Topic.Blockchain, presenterRyanMilbourne);
            var session25 = sessions.Add(25, (int)Topic.Blockchain, presenterRyanMilbourne);

            var session26 = sessions.Add(26, (int)Topic.Javascript, presenterJustinJames);
            var session27 = sessions.Add(27, (int)Topic.Mobile, presenterJustinJames);

            var session28 = sessions.Add(28, (int)Topic.ReactRedux, presenterMaxNodland);
            var session29 = sessions.Add(29, (int)Topic.ReactRedux, presenterMaxNodland);

            var session30 = sessions.Add(30, (int)Topic.AI, presenterBarryStahl);

            var session31 = sessions.Add(31, (int)Topic.AI, presenterJustineCocci);
            var session32 = sessions.Add(32, (int)Topic.AI, presenterJustineCocci);

            var session33 = sessions.Add(33, (int)Topic.DevOps, presenterHattanShobokshi);

            var session34 = sessions.Add(34, (int)Topic.Mobile, presenterChrisGriffith);

            var session35 = sessions.Add(35, (int)Topic.IOTLab, presenterIotLaboratory);
            var session36 = sessions.Add(36, (int)Topic.IOTLab, presenterIotLaboratory);
            var session37 = sessions.Add(37, (int)Topic.IOTLab, presenterIotLaboratory);
            var session38 = sessions.Add(38, (int)Topic.IOTLab, presenterIotLaboratory);
            var session39 = sessions.Add(39, (int)Topic.IOTLab, presenterIotLaboratory);
            var session40 = sessions.Add(40, (int)Topic.IOTLab, presenterIotLaboratory);


            // Session dependencies
            session25.AddDependency(session24);

            session36.AddDependency(session35);
            session37.AddDependency(session36);
            session38.AddDependency(session37);
            session39.AddDependency(session38);
            session40.AddDependency(session39);


            // Timeslots
            timeslots.Add(Timeslot.Create(1, 8.75));
            timeslots.Add(Timeslot.Create(2, 10));
            timeslots.Add(Timeslot.Create(3, 11.25));
            timeslots.Add(Timeslot.Create(4, 13.5));
            timeslots.Add(Timeslot.Create(5, 14.75));
            timeslots.Add(Timeslot.Create(6, 16));

            var rooms = new RoomCollectionBuilder()
                .Add(new RoomBuilder().Id(1).Capacity(10))
                .Add(new RoomBuilder().Id(2).Capacity(10))
                .Add(new RoomBuilder().Id(3).Capacity(10))
                .Add(new RoomBuilder().Id(4).Capacity(10))
                .Add(new RoomBuilder().Id(5).Capacity(10))
                .Add(new RoomBuilder().Id(6).Capacity(10))
                .Add(new RoomBuilder().Id(7).Capacity(10))
                .Add(new RoomBuilder().Id(8).Capacity(10)
                    .AddTimeslotUnavailable(1)
                    .AddTimeslotUnavailable(2)
                    .AddTimeslotUnavailable(3))
                .Build();

            // Create the schedule
            var assignments = engine.Process(sessions, rooms, timeslots);

            // Name the sessions for output
            var sn = new Dictionary<int, string>();
            sn.Add(session01.Id, "Pay it Forward Through Foster Care");
            sn.Add(session02.Id, "Intro to AWS IOT Device Shadow");
            sn.Add(session03.Id, "Zen of Python: Readability");
            sn.Add(session04.Id, "Comprehensible Comprehensions");
            sn.Add(session05.Id, "Digital Speech Within 125Hz Bandwidth");
            sn.Add(session06.Id, "PM Menu:Digging in");
            sn.Add(session07.Id, "Bots:New Enterprise Apps");
            sn.Add(session08.Id, "Flutter:Getting Started");
            sn.Add(session09.Id, "Mob Prog & Lofty Goals");
            sn.Add(session10.Id, "Azure IOT Edge 101");
            sn.Add(session11.Id, "Azure Funcs w/ Azure Storage");
            sn.Add(session12.Id, "Everyone is Public Speaker");
            sn.Add(session13.Id, "Hashcash:Alg of Bitcoin");
            sn.Add(session14.Id, "Timey-Wimey Stuff");
            sn.Add(session15.Id, "Angular NGX for Beginners");
            sn.Add(session16.Id, "Building Android Apps in AWS");
            sn.Add(session17.Id, "Native Ionic Dev");
            sn.Add(session18.Id, "DI in JavaScript");
            sn.Add(session19.Id, "Mobile Dev for .Net Devs");
            sn.Add(session20.Id, "Simple Back-end APIs");
            sn.Add(session21.Id, "Kotlin:Introduction");
            sn.Add(session22.Id, "Power BI Dashboardsx");
            sn.Add(session23.Id, "JS Like a Kung-Fu Student");
            sn.Add(session24.Id, "Bitcoin 101");
            sn.Add(session25.Id, "Blockchain 101");
            sn.Add(session26.Id, "Rapid REST Dev w/Node & Sails");
            sn.Add(session27.Id, "Native Mobile Dev With TACO");
            sn.Add(session28.Id, "Redux:Introduction");
            sn.Add(session29.Id, "React:Getting Started");
            sn.Add(session30.Id, "Devs Survey of AI");
            sn.Add(session31.Id, "ML:Intro to Image & Text Analysis");
            sn.Add(session32.Id, "ChatBots:Intro using Node");
            sn.Add(session33.Id, "Accidental DevOps:CI for .NET");
            sn.Add(session34.Id, "What is Ionic");
            sn.Add(session35.Id, "IOT Lab 1");
            sn.Add(session36.Id, "IOT Lab 2");
            sn.Add(session37.Id, "IOT Lab 3");
            sn.Add(session38.Id, "IOT Lab 4");
            sn.Add(session39.Id, "IOT Lab 5");
            sn.Add(session40.Id, "IOT Lab 6");

            // Display the results
            _output.WriteRoomConfiguration(rooms);
            _output.WriteLine("*********************************************");
            _output.WriteSchedule(assignments, sessions, sn);

        }
    }
}
