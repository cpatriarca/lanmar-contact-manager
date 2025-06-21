using System;
using System.Collections.Generic;
using System.Linq;
using ContactManager.Application;
using ContactManager.Application.Models;
using ContactManager.Infrastructure;
using ContactManager.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ContactManager.Tests
{
    public class ContactServiceTests
    {
        private ContactDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ContactDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ContactDbContext(options);
        }

        private ContactService GetService(ContactDbContext context)
        {
            return new ContactService(context);
        }

        [Fact]
        public void AddContact_ShouldAddContactToDatabase()
        {
            using var context = GetDbContext();
            var service = GetService(context);
            var dto = new ContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                CompanyName = "OpenAI",
                Mobile = "123456789",
                Email = "john.doe@example.com"
            };

            service.AddContact(dto);

            Assert.Single(context.Contacts);
            Assert.Equal("John", context.Contacts.First().FirstName);
        }

        [Fact]
        public void UpdateContact_ShouldUpdateExistingContact()
        {
            using var context = GetDbContext();
            var existing = new Contact
            {
                FirstName = "Old",
                LastName = "Name",
                CompanyName = "OldCo",
                Mobile = "123456789",
                Email = "old@example.com"
            };
            context.Contacts.Add(existing);
            context.SaveChanges();

            var service = GetService(context);
            var dto = new ContactDto
            {
                Id = existing.Id,
                FirstName = "New",
                LastName = "Name",
                CompanyName = "NewCo",
                Mobile = "987654321",
                Email = "new@example.com"
            };

            service.UpdateContact(dto);

            var updated = context.Contacts.Find(existing.Id);
            Assert.Equal("New", updated.FirstName);
            Assert.Equal("NewCo", updated.CompanyName);
        }

        [Fact]
        public void GetContactById_ShouldReturnCorrectContact()
        {
            using var context = GetDbContext();
            var contact = new Contact
            {
                FirstName = "Alice",
                LastName = "Smith",
                CompanyName = "ExampleCorp",
                Mobile = "5551234567",
                Email = "alice.smith@example.com"
            };
            context.Contacts.Add(contact);
            context.SaveChanges();

            var service = GetService(context);
            var result = service.GetContactById(contact.Id);

            Assert.NotNull(result);
            Assert.Equal("Alice", result.FirstName);
        }

        [Fact]
        public void GetAllContacts_ShouldReturnAllContacts()
        {
            using var context = GetDbContext();
            context.Contacts.AddRange(
                new Contact
                {
                    FirstName = "A",
                    LastName = "LastNameA",
                    CompanyName = "CompanyA",
                    Mobile = "1234567890",
                    Email = "a@example.com"
                },
                new Contact
                {
                    FirstName = "B",
                    LastName = "LastNameB",
                    CompanyName = "CompanyB",
                    Mobile = "0987654321",
                    Email = "b@example.com"
                }
            );
            context.SaveChanges();

            var service = GetService(context);
            var results = service.GetAllContacts();

            Assert.Equal(2, results.Count());
        }

        [Fact]
        public void SearchContacts_ShouldReturnMatches_CaseInsensitive()
        {
            using var context = GetDbContext();
            context.Contacts.AddRange(
                new Contact { FirstName = "John", LastName = "Smith", CompanyName = "Acme", Mobile = "0987654321", Email = "js@example.com" },
                new Contact { FirstName = "jane", LastName = "doe", CompanyName = "Globex", Mobile = "1234567890", Email = "jd@example.com" }
            );
            context.SaveChanges();

            var service = GetService(context);
            var results = service.SearchContacts("JOHN");

            Assert.Single(results);
            Assert.Equal("John", results.First().FirstName);
        }

        [Fact]
        public void DeleteContact_RemovesContact_WhenContactExists()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new ContactService(context);

            var contact = new Contact
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                CompanyName = "Acme Corp",
                Mobile = "123456",
                Email = "john@example.com"
            };

            context.Contacts.Add(contact);
            context.SaveChanges();

            // Act
            service.DeleteContact(1);

            // Assert
            var deleted = context.Contacts.Find(1);
            Assert.Null(deleted);
        }

        [Fact]
        public void DeleteContact_DoesNothing_WhenContactDoesNotExist()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new ContactService(context);

            // Act (delete non-existent contact)
            service.DeleteContact(999);

            // Assert (should not throw and contact count remains zero)
            Assert.Empty(context.Contacts);
        }
    }
}
