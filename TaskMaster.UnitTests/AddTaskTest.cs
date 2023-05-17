using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskMaster.UnitTests
{
    public class AddTaskTest
    {
        //private Mock<INavigation> _mockNavigation;
        //private Mock<TaskDbContext> _mockDbContext;
        //private AddTaskViewModel _viewModel;

        //public AddTaskTest()
        //{
        //    _mockNavigation = new Mock<INavigation>();
        //    _mockDbContext = new Mock<TaskDbContext>();

        //    _viewModel = new AddTaskViewModel(_mockNavigation.Object, _mockDbContext.Object);
        //}

        //[Fact]
        //public void AddTaskCommand_ShouldAddTask()
        //{
        //    // Arrange
        //    var newTask = new MyTask { TaskName = "Test task" };
        //    _viewModel.NewTask = newTask;

        //    // Act
        //    _viewModel.AddTaskCommand.Execute(null);

        //    // Assert
        //    _mockDbContext.Verify(db => db.Tasks.Add(newTask), Times.Once);
        //    _mockDbContext.Verify(db => db.SaveChanges(), Times.Once);
        //}
    }

}
