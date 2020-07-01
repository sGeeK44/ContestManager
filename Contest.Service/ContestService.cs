using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain.Games;
using Contest.Domain.Settings;

namespace Contest.Service
{
    public class ContestService
    {
        [Import] private IUnitOfWorks UnitOfWorks { get; set; }
        [Import] private IAddressFactory AddressFactory { get; set; }
        [Import] private IGameSettingFactory GameSettingFactory { get; set; }

        public ContestService()
        {
            FlippingContainer.Instance.ComposeParts(this);
        }

        public IContest Create(CreateContestCmd cmd)
        {
            try
            {
                UnitOfWorks.Begin();

                var address = AddressFactory.Create(cmd.StreetNumber, cmd.Street, cmd.ZipCode, cmd.City);
                UnitOfWorks.Save(address);

                var areaType = cmd.Indoor ? AreaType.Indoor : AreaType.Outdoor;
                var physicalSettings = PhysicalSetting.Create(address, areaType, cmd.CountField);
                UnitOfWorks.Save(physicalSettings);

                var gameSetting = GameSettingFactory.Create(cmd.CountMinPlayerByTeam, cmd.CountMaxPlayerByTeam);
                UnitOfWorks.Save(gameSetting);

                var newContest = Domain.Games.Contest.Create(cmd.Date, physicalSettings, gameSetting);
                UnitOfWorks.Save(newContest);

                UnitOfWorks.Commit();
                return newContest;
            }
            catch (Exception)
            {
                UnitOfWorks.Rollback();
                throw;
            }
        }
    }
}
