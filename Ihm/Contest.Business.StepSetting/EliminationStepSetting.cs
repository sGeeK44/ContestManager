﻿using System;
using System.Runtime.Serialization;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [DataContract(Name = "ELIMINATION_STEP_SETTING")]
    public class EliminationStepSetting : StepSetting, IEliminationStepSetting
    {
        #region Constructors

        private EliminationStepSetting() {}

        /// <summary>
        /// Instance a new EliminationStepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for qualification step</param>
        /// <param name="firstStep">Set the first step for elimination step</param>
        public EliminationStepSetting(IMatchSetting matchSetting, EliminationType firstStep)
            : base(matchSetting)
        {
            FirstStep = firstStep;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get first elimination step
        /// </summary>
        [DataMember(Name = "FIRST_STEP")]
        public EliminationType FirstStep { get; set; }

        #endregion

        #region Methods

        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            base.PrepareCommit(unitOfWorks);
            unitOfWorks.InsertOrUpdate<IEliminationStepSetting>(this);
        }

        public virtual void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotFiniteNumberException();
        }

        #endregion
    }
}