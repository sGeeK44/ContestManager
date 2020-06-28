﻿using System.ComponentModel.Composition;
using System.Configuration;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using Contest.Domain.Players;
using Contest.Domain.Settings;
using SmartWay.Orm.Interfaces;
using SmartWay.Orm.Sql;
using SmartWay.Orm.Sqlite;

namespace Contest.Ihm
{
    [Export(typeof(IDataStore))]
    public class  ContestDatastore : SqlDataStore
    {
        public ContestDatastore()
            : base(new SqliteDbEngine(ConfigurationManager.AppSettings["DatabasePath"], ""), new SqliteFactory())
        {
            AddType< Domain.Games.Contest>();
            AddType<GameSetting>();
            AddType<Address>();
            AddType<PhysicalSetting>();
            AddType<EliminationStepSetting>();
            AddType<QualificationStepSetting>();
            AddType<MatchSetting>();
            AddType<Team>();
            AddType<Match>();
            AddType<Person>();
            AddType<Phase>();
            AddType<EliminationStep>();
            AddType<QualificationStep>();
            AddType<Field>();
            AddType<Relationship<ITeam, IGameStep>>();
            AddType<Relationship<ITeam, IPhase>>();
        }
    }
}