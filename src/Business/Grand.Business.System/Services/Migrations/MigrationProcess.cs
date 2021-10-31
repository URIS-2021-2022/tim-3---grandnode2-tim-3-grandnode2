﻿using Grand.Business.Common.Interfaces.Logging;
using Grand.Domain.Data;
using Grand.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Business.System.Services.Migrations
{
    public class MigrationProcess : IMigrationProcess
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        private readonly IRepository<MigrationDb> _repositoryMigration;

        public MigrationProcess(
            IDatabaseContext databaseContext,
            IServiceProvider serviceProvider,
            ILogger logger,
            IRepository<MigrationDb> repositoryMigration)
        {
            _databaseContext = databaseContext;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _repositoryMigration = repositoryMigration;
        }

        public virtual MigrationResult RunProcess(IMigration migration)
        {
            var result = RunProcessInternal(migration);
            try
            {
                if (result.Success)
                    SaveMigration(result);
                else
                    _logger.InsertLog(Domain.Logging.LogLevel.Error, $"Something went wrong during migration process {migration.Name}");
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Exception run migration {migration.Name}", ex);
            }
        }

        private MigrationResult RunProcessInternal(IMigration migration)
        {
            var model = new MigrationResult {
                Success = migration.UpgradeProcess(_databaseContext, _serviceProvider),
                Migration = migration,
            };
            return model;
        }

        private void SaveMigration(MigrationResult migrationResult)
        {
            _repositoryMigration.Insert(new MigrationDb() {
                Identity = migrationResult.Migration.Identity,
                Name = migrationResult.Migration.Name,
                Version = migrationResult.Migration.Version.ToString(),
                CreatedOnUtc = DateTime.UtcNow,
            });
        }

        private IList<MigrationDb> GetMigrationDb()
        {
            return _repositoryMigration.Table.ToList();
        }

        public virtual void RunMigrationProcess()
        {
            var migrationsDb = GetMigrationDb();
            var migrationManager = new MigrationManager();
            foreach (var item in migrationManager.GetCurrentMigrations())
            {
                if (migrationsDb.FirstOrDefault(x => x.Identity == item.Identity) == null)
                {
                    RunProcess(item);
                }
            }
        }

    }
}
