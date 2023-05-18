using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using CA_TaskMaster.ViewModels;
using CA_TaskMaster.Models;
using CA_TaskMaster.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Maui;

namespace CA_TaskMaster.Tests
{
    public class AddTaskViewModelTests
    {
        private Mock<INavigation> _mockNavigation;
        private TaskDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _mockNavigation = new Mock<INavigation>();
            _dbContext = new TaskDbContext();
            _dbContext.Database.EnsureCreated();
        }

        [Test]
        public void AddTaskCommand_WithValidFields_AddsNewTask()
        {
            // Arrange
            var addTaskViewModel = new AddTaskViewModel(_mockNavigation.Object);
            addTaskViewModel.NewTask = new MyTask
            {
                TaskName = "Test Task",
                TaskDescription = "Test Description",
                TaskPriority = "High",
                TaskDueDate = DateTime.Now.AddDays(1)
            };

            int initialCount = _dbContext.Tasks.Count();

            // Act
            addTaskViewModel.AddTaskCommand.Execute(null);

            // Assert
            NUnit.Framework.Assert.AreEqual(initialCount + 1, _dbContext.Tasks.Count());
        }

        // Additional tests can be written following the above pattern
    }
}
