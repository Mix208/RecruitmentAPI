using Xunit;                                        // Framework de test
using Moq;                                          // Pour simuler IConfiguration
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAPI.API.Data;
using RecruitmentAPI.API.Controllers;
using RecruitmentAPI.API.Models;
using RecruitmentAPI.API.DTOs.Auth;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Threading.Tasks;

namespace RecruitmentAPI.Tests
{
    public class AuthControllerTests
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public AuthControllerTests()
        {
            // On crée une base "In-Memory" simulée
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Fact]
        public async Task Register_Returns_BadRequest_If_Email_Exists()
        {
            // Arrange
    using var context = new AppDbContext(_dbOptions);
    var mockConfig = new Mock<IConfiguration>();
    var controller = new AuthController(context, mockConfig.Object);

    var dto = new UserRegisterDto
    {
        Email = "new@email.com",
        Password = "123",  // Trop court !
        Username = "ShortPwdUser",
        Role = "Candidat"
    };

    // Act
    var result = await controller.Register(dto);

    // Assert
    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal("Le mot de passe doit contenir au moins 8 caractères.", badRequest.Value);
        }
    }
}
