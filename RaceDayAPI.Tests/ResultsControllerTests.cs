using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.Controllers;
using RaceDayAPI.Models;
using RaceDayAPI.Tests.Helpers;

namespace RaceDayAPI.Tests
{
    public class ResultsControllerTests
    {
        private void SeedEnrolment(RaceDayAPI.Data.RaceDayContext context)
        {
            context.Enrolments.Add(new Enrolment
            {
                EnrolmentID = 1,
                ParticipantID = 2,
                EventID = 1,
                CategoryID = 1,
                EnrolmentDate = DateTime.UtcNow,
                Status = "Approved"
            });
            context.SaveChanges();
        }

        [Fact]
        public async Task CreateResult_AsOrganiserOwner_Succeeds()
        {
            var context = TestHelpers.CreateInMemoryContext();
            TestHelpers.SeedBasicData(context);
            SeedEnrolment(context);

            var controller = new ResultsController(context);
            TestHelpers.SetSession(controller, 1, "Organiser");

            var result = await controller.CreateResult(new Result
            {
                EnrolmentID = 1,
                FinishTime = TimeSpan.FromHours(3),
                FinishPosition = 15
            });

            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Single(context.Results);
        }

        [Fact]
        public async Task CreateResult_NotOwner_Forbidden()
        {
            var context = TestHelpers.CreateInMemoryContext();
            TestHelpers.SeedBasicData(context);
            SeedEnrolment(context);

            context.Users.Add(new User
            {
                UserID = 3,
                FirstName = "Other",
                LastName = "Org",
                Email = "other@org.com",
                PasswordHash = "x",
                Role = "Organiser"
            });
            context.Organisers.Add(new Organiser
            {
                OrganiserID = 3,
                OrganisationName = "Other Org"
            });
            context.SaveChanges();

            var controller = new ResultsController(context);
            TestHelpers.SetSession(controller, 3, "Organiser");

            var result = await controller.CreateResult(new Result
            {
                EnrolmentID = 1,
                FinishTime = TimeSpan.FromHours(3),
                FinishPosition = 15
            });

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, obj.StatusCode);
        }

        [Fact]
        public async Task CreateResult_InvalidPosition_ReturnsBadRequest()
        {
            var context = TestHelpers.CreateInMemoryContext();
            TestHelpers.SeedBasicData(context);
            SeedEnrolment(context);

            var controller = new ResultsController(context);
            TestHelpers.SetSession(controller, 1, "Organiser");

            var result = await controller.CreateResult(new Result
            {
                EnrolmentID = 1,
                FinishTime = TimeSpan.FromHours(3),
                FinishPosition = 0
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateResult_DuplicateResult_ReturnsConflict()
        {
            var context = TestHelpers.CreateInMemoryContext();
            TestHelpers.SeedBasicData(context);
            SeedEnrolment(context);

            context.Results.Add(new Result
            {
                ResultID = 1,
                EnrolmentID = 1,
                FinishTime = TimeSpan.FromHours(3),
                FinishPosition = 15,
                RecordedAt = DateTime.UtcNow
            });
            context.SaveChanges();

            var controller = new ResultsController(context);
            TestHelpers.SetSession(controller, 1, "Organiser");

            var result = await controller.CreateResult(new Result
            {
                EnrolmentID = 1,
                FinishTime = TimeSpan.FromHours(4),
                FinishPosition = 20
            });

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task GetResultByEnrolment_Owner_ReturnsOk()
        {
            var context = TestHelpers.CreateInMemoryContext();
            TestHelpers.SeedBasicData(context);
            SeedEnrolment(context);

            context.Results.Add(new Result
            {
                ResultID = 1,
                EnrolmentID = 1,
                FinishTime = TimeSpan.FromHours(3),
                FinishPosition = 15,
                RecordedAt = DateTime.UtcNow
            });
            context.SaveChanges();

            var controller = new ResultsController(context);
            TestHelpers.SetSession(controller, 2, "Participant");

            var result = await controller.GetResultByEnrolment(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetResult_NotFound_Returns404()
        {
            var context = TestHelpers.CreateInMemoryContext();
            TestHelpers.SeedBasicData(context);

            var controller = new ResultsController(context);
            TestHelpers.SetSession(controller, 1, "Organiser");

            var result = await controller.GetResult(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}