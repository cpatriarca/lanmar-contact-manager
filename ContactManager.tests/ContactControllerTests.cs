using System.Collections.Generic;
using System.Linq;
using ContactManager.Application;
using ContactManager.Application.Models;
using ContactManager.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ContactManager.Tests.Controllers
{
    public class ContactControllerTests
    {
        private readonly Mock<IContactService> _mockService;
        private readonly Mock<ILogger<ContactController>> _mockLogger;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _mockService = new Mock<IContactService>();
            _mockLogger = new Mock<ILogger<ContactController>>();
            _controller = new ContactController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var result = _controller.Index(null);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Search_WithSearchTerm_ReturnsFilteredResults()
        {
            var term = "test";
            var contacts = new List<ContactDto>
                {
                    new()
                    {
                        FirstName = "Test",
                        LastName = "User",
                        CompanyName = "TestCompany",
                        Mobile = "1234567890",
                        Email = "test@example.com"
                    }
                };
            _mockService.Setup(s => s.SearchContacts(term)).Returns(contacts);

            var result = _controller.Search(term) as PartialViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ContactDto>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Search_WithEmptyTerm_ReturnsAllResults()
        {
            var contacts = new List<ContactDto>
                {
                    new()
                    {
                        FirstName = "All",
                        LastName = "User",
                        CompanyName = "AllCompany",
                        Mobile = "0987654321",
                        Email = "all@example.com"
                    }
                };
            _mockService.Setup(s => s.GetAllContacts()).Returns(contacts);

            var result = _controller.Search(null) as PartialViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ContactDto>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Create_Get_ReturnsView()
        {
            var result = _controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_PostInvalidModel_ReturnsViewWithModel()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var contact = new ContactDto
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                CompanyName = string.Empty,
                Mobile = string.Empty,
                Email = string.Empty
            };
            var result = _controller.Create(contact) as ViewResult;

            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void Create_PostValidModel_RedirectsToIndex()
        {
            var contact = new ContactDto
            {
                FirstName = "Valid",
                LastName = "User",
                CompanyName = "ValidCompany",
                Mobile = "1234567890",
                Email = "valid@example.com"
            };

            var result = _controller.Create(contact) as RedirectToActionResult;

            _mockService.Verify(s => s.AddContact(contact), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Edit_GetContactFound_ReturnsView()
        {
            var contact = new ContactDto
            {
                Id = 1,
                FirstName = "Edit",
                LastName = "User",
                CompanyName = "EditCompany",
                Mobile = "1234567890",
                Email = "edit@example.com"
            };
            _mockService.Setup(s => s.GetContactById(1)).Returns(contact);

            var result = _controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<ContactDto>(result.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Edit_GetContactNotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetContactById(999)).Returns((ContactDto)null);

            var result = _controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_PostInvalidModel_ReturnsViewWithModel()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var contact = new ContactDto
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                CompanyName = string.Empty,
                Mobile = string.Empty,
                Email = string.Empty
            };
            var result = _controller.Edit(contact) as ViewResult;

            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void Edit_PostValidModel_RedirectsToIndex()
        {
            var contact = new ContactDto
            {
                Id = 1,
                FirstName = "Edited",
                LastName = "User",
                CompanyName = "EditedCompany",
                Mobile = "1234567890",
                Email = "edited@example.com"
            };

            var result = _controller.Edit(contact) as RedirectToActionResult;

            _mockService.Verify(s => s.UpdateContact(contact), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Delete_ReturnsRedirectToIndex_WhenContactExists()
        {
            // Arrange
            var contact = new ContactDto
            {
                Id = 1,
                FirstName = "Edited",
                LastName = "User",
                CompanyName = "EditedCompany",
                Mobile = "1234567890",
                Email = "edited@example.com"
            };

            _mockService.Setup(s => s.GetContactById(1)).Returns(contact);

            // Act
            var result = _controller.Delete(1);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _mockService.Verify(s => s.DeleteContact(1), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetContactById(999)).Returns((ContactDto)null);

            // Act
            var result = _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockService.Verify(s => s.DeleteContact(It.IsAny<int>()), Times.Never);
        }
    }
}
