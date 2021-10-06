﻿namespace DapperHeroes.Core.Dapper.Repositories
{
    public class DapperHeroesRepository : IHeroesRepository
    {
        private readonly IDapperr _dapperr;

        public DapperHeroesRepository(IDapperr dapperr)
        {
            _dapperr = dapperr;
        }

        public IEnumerable<Hero> All(int page, int maxRecords)
        {
            var sql = @"
                SELECT h.Id, h.Category, h.Created, h.HasCape, h.IsAlive, h.Name, h.Powers
                FROM Heroes AS h
                ORDER BY h.Created DESC
                LIMIT @maxRecords OFFSET @page";
            var dp = new DynamicParameters(new { @maxRecords = maxRecords, @page = page });
            return _dapperr.Query<Hero>(sql, dp);
        }

        public Hero Create(Hero hero)
        {
            var sql = @"
            INSERT INTO Heroes (
                Name,
                Category,
                HasCape,
                IsAlive,
                Powers,
                Created)
            VALUES
            (
                @name, @category, @hasCape, @isAlive, @powers, @created
            );
            SELECT h.* FROM Heroes h WHERE h.Id = last_insert_rowid();
        ";
            var dp = new DynamicParameters(new
            {
                @name = hero.Name,
                @category = hero.Category,
                @hasCape = hero.HasCape,
                @isAlive = hero.IsAlive,
                @powers = hero.Powers,
                @created = DateTime.Now
            });

            return _dapperr.Query<Hero>(sql, dp).FirstOrDefault();
        }

        public void Delete(long id)
        {
            var sql = @"DELETE FROM Heroes WHERE Id = @id";
            _dapperr.Execute(sql, new DynamicParameters(new { id }));
        }

        public IEnumerable<Hero> SearchByName(string heroName)
        {
            var sql = @"SELECT h.* FROM Heroes h WHERE h.NAME LIKE %@heroName%";
            var dp = new DynamicParameters(new { heroName });
            return _dapperr.Query<Hero>(sql, dp);
        }

        public Hero Single(long id)
        {
            var sql = @"SELECT h.* FROM Heroes h WHERE h.Id == @id";
            var dp = new DynamicParameters(new { id });
            return _dapperr.Query<Hero>(sql, dp).FirstOrDefault();
        }

        public Hero Update(long id, Hero hero)
        {
            var sql = @"
                UPDATE Heroes 
                SET Name = @name, 
                Category = @category, 
                HasCape = @hasCape, 
                IsAlive = @isAlive, 
                Powers = @powers;
                SELECT h.* FROM Heroes h WHERE h.Id = @id";

            var dp = new DynamicParameters(new
            {
                @id = id,
                @name = hero.Name,
                @category = hero.Category,
                @hasCape = hero.HasCape,
                @isAlive = hero.IsAlive,
                @powers = hero.Powers
            });

            return _dapperr.Query<Hero>(sql, dp).FirstOrDefault();
        }
    }
}