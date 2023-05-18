using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using NUnit.Framework;
//using CA_TaskMaster.ViewModels;

namespace TaskMaster.UnitTests
{
    [TestFixture]
    public class AddTaskTest
    {
        //private AddTaskViewModel _viewModel;
        //private Mock<TaskDbContext> _dbContext;

        //[SetUp]
        //public void SetUp()
        //{
        //    // This is run before each test. It's like a mini constructor for tests.
        //    _dbContext = new Mock<TaskDbContext>();  // We're using Moq here to mock our DbContext.
        //    _viewModel = new AddTaskViewModel(_dbContext.Object); // Insert your actual ViewModel constructor parameters here.
        //}

        //[Test]
        //public async Task AddTaskCommand_ShouldAddTask()
        //{
        //    // Arrange
        //    MyTask taskToAdd = new MyTask
        //    {
        //        TaskId = 1,
        //        TaskName = "Test Task",
        //        TaskDescription = "Test Task Description",
        //        TaskPriority = "High",
        //        TaskDueDate = DateTime.Now.AddDays(1),
        //        TaskCompletionStatus = false
        //    };

        //    _viewModel.NewTask = taskToAdd;

        //    // Act
        //    await _viewModel.AddTaskCommand.ExecuteAsync();

        //    // Assert
        //    _dbContext.Verify(db => db.Add(It.IsAny<MyTask>()), Times.Once);
        //    _dbContext.Verify(db => db.SaveChanges(), Times.Once);
        //}
    }
}
