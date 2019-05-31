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
    public enum SoCalSanDiegoTopic
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

            var sessions = new SessionCollectionBuilder()
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.SoftSkills)
                    .Name("Pay it Forward Through Foster Care")
                    .AddPresenter(presenterRichClingman))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.IOT)
                    .Name("Intro to AWS IOT Device Shadow")
                    .AddPresenter(presenterRichClingman))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Python)
                    .Name("Zen of Python: Readability")
                    .AddPresenter(presenterTreyHunner))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Python)
                    .Name("Comprehensible Comprehensions")
                    .AddPresenter(presenterTreyHunner))
                .AddSession(new SessionBuilder()
                    .Name("Digital Speech Within 125Hz Bandwidth")
                    .AddPresenter(presenterMichaelLebo))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Agile)
                    .Name("PM Menu:Digging in")
                    .AddPresenter(presenterAaronRuckman))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.AI)
                    .Name("Bots:New Enterprise Apps")
                    .AddPresenter(presenterDanielEgan))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Mobile)
                    .Name("Flutter:Getting Started")
                    .AddPresenter(presenterLorraineKan))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Agile)
                    .Name("Mob Prog & Lofty Goals")
                    .AddPresenter(presenterChrisLucian))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.IOT)
                    .Name("Azure IOT Edge 101")
                    .AddPresenter(presenterBretStateham))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Cloud)
                    .Name("Azure Funcs w/ Azure Storage")
                    .AddPresenter(presenterRobinShahan))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.SoftSkills)
                    .Name("Everyone is Public Speaker")
                    .AddPresenter(presenterJustinJames))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Blockchain)
                    .Name("Hashcash:Alg of Bitcoin")
                    .AddPresenter(presenterTobiasHughes))
                .AddSession(new SessionBuilder()
                    .Name("Timey-Wimey Stuff")
                    .AddPresenter(presenterWendySteinman))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Angular)
                    .Name("Angular NGX for Beginners")
                    .AddPresenter(presenterOgunTigli))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Mobile)
                    .Name("Building Android Apps in AWS")
                    .AddPresenter(presenterHibaMughal))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Mobile)
                    .Name("Native Ionic Dev")
                    .AddPresenter(presenterEricFloe))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Javascript)
                    .Name("DI in JavaScript")
                    .AddPresenter(presenterHattanShobokshi))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Mobile)
                    .Name("Mobile Dev for .Net Devs")
                    .AddPresenter(presenterJonBachelor))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.DotNetCore)
                    .Name("Simple Back-end APIs")
                    .AddPresenter(presenterPaulWhitmer))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Javascript)
                    .Name("Kotlin:Introduction")
                    .AddPresenter(presenterDavidFord))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.BI)
                    .Name("Power BI Dashboards")
                    .AddPresenter(presenterVaziOkhandiar))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Javascript)
                    .Name("JS Like a Kung-Fu Student")
                    .AddPresenter(presenterChrisStead))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Blockchain)
                    .Name("Bitcoin 101")
                    .AddPresenter(presenterRyanMilbourne)
                    .AddDependentSession(new SessionBuilder()
                        .Topic(SoCalSanDiegoTopic.Blockchain)
                        .Name("Blockchain 101")
                        .AddPresenter(presenterRyanMilbourne)))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Javascript)
                    .Name("Rapid REST Dev w/Node & Sails")
                    .AddPresenter(presenterJustinJames))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Mobile)
                    .Name("Native Mobile Dev With TACO")
                    .AddPresenter(presenterJustinJames))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.ReactRedux)
                    .Name("Redux:Introduction")
                    .AddPresenter(presenterMaxNodland))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.ReactRedux)
                    .Name("React:Getting Started")
                    .AddPresenter(presenterMaxNodland))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.AI)
                    .Name("Devs Survey of AI")
                    .AddPresenter(presenterBarryStahl))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.AI)
                    .Name("ML:Intro to Image & Text Analysis")
                    .AddPresenter(presenterJustineCocci))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.AI)
                    .Name("ChatBots:Intro using Node")
                    .AddPresenter(presenterJustineCocci))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.DevOps)
                    .Name("Accidental DevOps:CI for .NET")
                    .AddPresenter(presenterHattanShobokshi))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.Mobile)
                    .Name("What is Ionic")
                    .AddPresenter(presenterChrisGriffith))
                .AddSession(new SessionBuilder()
                    .Topic(SoCalSanDiegoTopic.IOTLab)
                    .Name("IOT Lab 1")
                    .AddPresenter(presenterIotLaboratory)
                    .AddDependentSession(new SessionBuilder()
                        .Topic(SoCalSanDiegoTopic.IOTLab)
                        .Name("IOT Lab 2")
                        .AddPresenter(presenterIotLaboratory)
                        .AddDependentSession(new SessionBuilder()
                            .Topic(SoCalSanDiegoTopic.IOTLab)
                            .Name("IOT Lab 3")
                            .AddPresenter(presenterIotLaboratory))
                            .AddDependentSession(new SessionBuilder()
                                .Topic(SoCalSanDiegoTopic.IOTLab)
                                .Name("IOT Lab 4")
                                .AddPresenter(presenterIotLaboratory))
                                .AddDependentSession(new SessionBuilder()
                                    .Topic(SoCalSanDiegoTopic.IOTLab)
                                    .Name("IOT Lab 5")
                                    .AddPresenter(presenterIotLaboratory))
                                    .AddDependentSession(new SessionBuilder()
                                        .Topic(SoCalSanDiegoTopic.IOTLab)
                                        .Name("IOT Lab 6")
                                        .AddPresenter(presenterIotLaboratory))))
                .Build();

            // Timeslots
            var timeslots = new TimeslotCollectionBuilder()
                .Add(new TimeslotBuilder(1).StartingAt(08.75))
                .Add(new TimeslotBuilder(2).StartingAt(10.00))
                .Add(new TimeslotBuilder(3).StartingAt(11.25))
                .Add(new TimeslotBuilder(4).StartingAt(13.50))
                .Add(new TimeslotBuilder(5).StartingAt(14.75))
                .Add(new TimeslotBuilder(6).StartingAt(16.00))
                .Build();

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

            // Display the results
            _output.WriteRoomConfiguration(rooms);
            _output.WriteTimeslotConfiguration(timeslots);
            _output.WriteLine("*********************************************");
            _output.WriteSchedule(assignments, sessions);

        }
    }
}
