﻿using AutoMapper;
using Bytes2you.Validation;
using SotnWiki.Data.Common.Contracts;
using SotnWiki.DTOs.RunViewsDTOs;
using SotnWiki.Models;
using System.Collections.Generic;
using System.Linq;

namespace SotnWiki.Data.Common.Repositories
{
    public class RunRepository : EfGenericRepository<Run>, IRunRepository
    {
        public RunRepository(ISotnWikiDbContext context)
            : base(context)
        {
        }

        public IEnumerable<LeaderboardRunDTO> GetRunsInCategory(string categoryName)
        {
            Guard.WhenArgument(categoryName, "categoryName").IsNullOrEmpty().Throw();

            return this.DbSet.Where(x => x.Category.ToString() == categoryName).ProjectToList<LeaderboardRunDTO>();
        }

        public LeaderboardRunDTO GetWorldRecordInCategory(string categoryName)
        {
            Guard.WhenArgument(categoryName, "categoryName").IsNullOrEmpty().Throw();

            return this.DbSet.Where(x => x.Category.ToString() == categoryName).OrderByDescending(r => r.Time).ProjectToFirstOrDefault<LeaderboardRunDTO>();
        }
    }
}
