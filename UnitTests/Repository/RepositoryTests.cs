using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Repository
{
    public class DatabaseFixture : IDisposable
    {
        public LootDbContext DbContext { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<LootDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            DbContext = new LootDbContext(options);
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var entity = new GuildApplication { Id = 1, CharacterName = "abc", ApplicantId = "applicant1", Profession = "Herbalist", Specialization = "Damage", WarcraftLogs = new Uri("https://abc.com"), Class = CharacterClass.Mage, RaidRoles = "dps tank healer" };

            DbContext.Add(entity);

            var secondEntity = new GuildApplication { Id = 2, CharacterName = "abcdded", ApplicantId = "applicant123", Profession = "Herbalista", Specialization = "Damager", WarcraftLogs = new Uri("https://abcdfd.com"), Class = CharacterClass.Druid, RaidRoles = "dpses tanks healers" };

            DbContext.Add(secondEntity);

            DbContext.SaveChanges();
        }

        public void Dispose()
        {            
            DbContext.Database.EnsureDeleted(); 
            DbContext.Dispose();
        }
    }

    public class RepositoryTests : IClassFixture<DatabaseFixture>
    {
        private TestRepository _repository;

        public RepositoryTests(DatabaseFixture fixture)
        {
            _repository = new TestRepository(fixture.DbContext);
        }

        [Fact]
        public async Task Create_ShouldAddEntityAndReturnIt()
        {
            // Arrange
            var entity = new GuildApplication { Id = 3, CharacterName = "ab3c", ApplicantId = "applicant31", Profession = "H3rbalist", Specialization = "Damage", WarcraftLogs = new Uri("https://abc.com"), Class = CharacterClass.Mage, RaidRoles = "dps3 tank healer" };

            // Act
            var result = await _repository.Create(entity);

            // Assert            
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.CharacterName, result.CharacterName);
            Assert.Equal(entity.ApplicantId, result.ApplicantId);
            Assert.Equal(entity.Profession, result.Profession);
            Assert.Equal(entity.Specialization, result.Specialization);
            Assert.Equal(entity.WarcraftLogs, result.WarcraftLogs);
            Assert.Equal(entity.Class, result.Class);
            Assert.Equal(entity.RaidRoles, result.RaidRoles);
        }

        [Fact]
        public async Task GetEntityById_ShouldReturnEntity()
        {
            int requiredId = 3;

            var result = await _repository.GetById(requiredId);

            Assert.NotNull(result);
            Assert.Equal(requiredId, result.Id);
        }

        [Fact]
        public async Task UpdateEntity_ShouldDeleteEntity()
        {
            var entity = new GuildApplication { Id = 1, CharacterName = "abcddd", ApplicantId = "applicant12", Profession = "Herbalista", Specialization = "Damager", WarcraftLogs = new Uri("https://abcdf.com"), Class = CharacterClass.Warlock, RaidRoles = "dps tanks healers" };

            var result = await _repository.Update(entity);

            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.CharacterName, result.CharacterName);
            Assert.Equal(entity.ApplicantId, result.ApplicantId);
            Assert.Equal(entity.Profession, result.Profession);
            Assert.Equal(entity.Specialization, result.Specialization);
            Assert.Equal(entity.WarcraftLogs, result.WarcraftLogs);
            Assert.Equal(entity.Class, result.Class);
            Assert.Equal(entity.RaidRoles, result.RaidRoles);
        }

        [Fact]
        public async Task Get_AllEntities()
        {
            var result = await _repository.GetAll();

            Assert.NotNull(result);
            Assert.True(result.Count > 1);
        }

        [Fact]
        public async Task Delete_ById()
        {
            int idToDelete = 1;

            await _repository.DeleteById(idToDelete);

            var result = await _repository.GetById(idToDelete);

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_ByEntity()
        {
            var entity = await _repository.GetById(2);

            await _repository.Delete(entity);

            var result = await _repository.GetById(entity.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task DoesExist()
        {
            int idToSearch = 1;

            bool result = await _repository.DoesExist(idToSearch);

            Assert.True(result);
        }
    }
}
