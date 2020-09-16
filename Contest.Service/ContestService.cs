using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using Contest.Domain.Players;
using Contest.Domain.Settings;

namespace Contest.Service
{
    public class ContestService
    {
        [Import] private IUnitOfWorks UnitOfWorks { get; set; }
        [Import] private IAddressFactory AddressFactory { get; set; }
        [Import] private IGameSettingFactory GameSettingFactory { get; set; }
        [Import] private ITeamFactory TeamFactory { get; set; }

        public ContestService()
        {
            FlippingContainer.Instance.ComposeParts(this);
        }

        public IContest Create(CreateContestCmd cmd)
        {
            return WithTransaction(() =>
            {
                var address = AddressFactory.Create(cmd.StreetNumber, cmd.Street, cmd.ZipCode, cmd.City);
                UnitOfWorks.Save(address);

                var areaType = cmd.Indoor ? AreaType.Indoor : AreaType.Outdoor;
                var physicalSettings = PhysicalSetting.Create(address, areaType, cmd.CountField);
                UnitOfWorks.Save(physicalSettings);

                var gameSetting = GameSettingFactory.Create(cmd.CountMinPlayerByTeam, cmd.CountMaxPlayerByTeam);
                UnitOfWorks.Save(gameSetting);

                var newContest = Domain.Games.Contest.Create(cmd.Date, physicalSettings, gameSetting);
                UnitOfWorks.Save(newContest.EliminationSetting.MatchSetting);
                UnitOfWorks.Save(newContest.EliminationSetting);
                UnitOfWorks.Save(newContest);

                return newContest;
            });
        }

        public void CreatePerson(string lastName, string firstName, string alias, string mail, bool canMailing, bool isMemberOfAssociation)
        {
            WithTransaction(() =>
            {
                var person = Person.Create(lastName, firstName, alias);
                person.Mail = mail;
                person.CanMailing = canMailing;
                person.IsMember = isMemberOfAssociation;
                UnitOfWorks.Save(person);
            });
        }

        public void UpdatePerson(IPerson person, string lastName, string firstName, string alias, string mail, bool canMailing, bool isMemberOfAssociation)
        {
            WithTransaction(() =>
            {
                person.SetIndentity(lastName, firstName, alias);
                person.Mail = mail;
                person.CanMailing = canMailing;
                person.IsMember = isMemberOfAssociation;
                UnitOfWorks.Save(person);
            });
        }

        public void RemovePerson(IContest contest, IList<IPerson> persons)
        {
            WithTransaction(() =>
            {
                foreach (var person in persons)
                {
                    var team = person.GetTeam(contest);
                    if (team != null)
                    {
                        var association = team.RemovePlayer(person);
                        UnitOfWorks.Delete(association);

                        if (team.Members.Count != 0)
                            continue;

                        contest.UnRegister(team);
                        UnitOfWorks.Delete(team);
                    }
                    UnitOfWorks.Delete(person);
                }
            });
        }

        public void CreateTeam(IContest contest, string name, IList<IPerson> persons)
        {
            WithTransaction(() =>
            {
                var team = TeamFactory.Create(contest, name);
                UnitOfWorks.Save(team);

                foreach (var person in persons)
                {
                    var association = team.AddPlayer(person);
                    UnitOfWorks.Save(association);
                }
            });
        }

        public void AddPlayer(ITeam team, IList<IPerson> persons)
        {
            WithTransaction(() =>
            {
                foreach (var person in persons)
                {
                    var assocation = team.AddPlayer(person);
                    UnitOfWorks.Save(assocation);
                }
            });
        }

        public void RemovePlayer(IContest contest, IList<IPerson> persons)
        {
            WithTransaction(() =>
            {
                foreach (var person in persons)
                {
                    var team = person.GetTeam(contest);
                    if (team == null)
                        continue;

                    var association = team.RemovePlayer(person);
                    UnitOfWorks.Delete(association);
                    
                    if (team.Members.Count != 0)
                        continue;

                    contest.UnRegister(team);
                    UnitOfWorks.Delete(team);
                }
            });
        }

        public void UpdateContest(IContest contest)
        {
            WithTransaction(() =>
            {
                UnitOfWorks.Save(contest.EliminationSetting.MatchSetting);
                UnitOfWorks.Save(contest.EliminationSetting);

                if (contest.QualificationSetting != null)
                {
                    UnitOfWorks.Save(contest.QualificationSetting.MatchSetting);
                    UnitOfWorks.Save(contest.QualificationSetting);
                }

                if (contest.ConsolingEliminationSetting != null)
                {
                    UnitOfWorks.Save(contest.ConsolingEliminationSetting.MatchSetting);
                    UnitOfWorks.Save(contest.ConsolingEliminationSetting);
                }
            });
        }

        public void StartContest(IContest currentContest)
        {
            WithTransaction(() =>
            {
                currentContest.StartContest();
                UnitOfWorks.Save(currentContest);
                foreach (var field in currentContest.FieldList)
                {
                    UnitOfWorks.Save(field);
                }
                foreach (var phase in currentContest.PhaseList)
                {
                    UnitOfWorks.Save(phase);
                    foreach (var gameStep in phase.GameStepList)
                    {
                        UnitOfWorks.Save(gameStep.CurrentMatchSetting);
                        UnitOfWorks.Save(gameStep);
                        foreach (var team in gameStep.TeamGameStepList)
                        {
                            UnitOfWorks.Save(team);
                        }
                        foreach (var match in gameStep.MatchList)
                        {
                            UnitOfWorks.Save(match);
                        }
                    }
                    foreach (var teamPhase in phase.TeamPhaseList)
                    {
                        UnitOfWorks.Save(teamPhase);
                    }
                }
            });
        }

        public void SaveContest(IContest currentContest)
        {
            WithTransaction(() =>
            {
                UnitOfWorks.Save(currentContest);
                foreach (var field in currentContest.FieldList)
                {
                    UnitOfWorks.Save(field);
                }
                foreach (var phase in currentContest.PhaseList)
                {
                    UnitOfWorks.Save(phase);
                    foreach (var gameStep in phase.GameStepList)
                    {
                        UnitOfWorks.Save(gameStep.CurrentMatchSetting);
                        UnitOfWorks.Save(gameStep);
                        foreach (var team in gameStep.TeamGameStepList)
                        {
                            UnitOfWorks.Save(team);
                        }
                        foreach (var match in gameStep.MatchList)
                        {
                            UnitOfWorks.Save(match);
                        }
                    }
                    foreach (var teamPhase in phase.TeamPhaseList)
                    {
                        UnitOfWorks.Save(teamPhase);
                    }
                }
            });
        }

        private void WithTransaction(Action action)
        {
            try
            {
                UnitOfWorks.Begin();
                action();
                UnitOfWorks.Commit();
            }
            catch (Exception)
            {
                UnitOfWorks.Rollback();
                throw;
            }
        }

        private T WithTransaction<T>(Func<T> action)
        {
            try
            {
                UnitOfWorks.Begin();
                var result = action();
                UnitOfWorks.Commit();
                return result;
            }
            catch (Exception)
            {
                UnitOfWorks.Rollback();
                throw;
            }
        }
    }
}
