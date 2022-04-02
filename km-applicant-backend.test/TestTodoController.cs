using System;
using System.Threading.Tasks;
using FluentAssertions;
using km_applicant_backend.Controllers;
using km_applicant_backend.Models;
using km_applicant_backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace km_applicant_backend.test
{
    /** Unit Test class Variable Explanation
     * In test classes, there will be some similar variable
     * result       : result we get after execute the todoController, 
                     it usually in the form ActionResult<Todo>
                     in some cases IEnumerable<Todo>
     
     * value        : the return value from result, usually in the form Todo.class
     
     * randomTodo   : Fake Todo or dummy Todo, created for mock purpose
                     Should contain right format, and value Todo
    
     * updateTodo   : Fake Todo, which created for update action mock, 
                     updateTodo should be different from randomTodo

     * In some cases there will be, this code executed, just to make sure todo exist,
       before execute another function using this todo
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
     */

    public class TestTodoController
    {
        private Mock<ITodoRepository> _repository = new Mock<ITodoRepository>();
        private readonly Random randomValue = new();
        private
        string[] randomString =
        {
            "First", "Second", "Third",
            "Fourth","Fifth", "Sixth",
            "Seventh","Eighth","Ninth"
        }; //to fill random string for title, and descriptions

        [SetUp]
        public void Setup()
        {
            var controller = new TodoController(_repository.Object);
        }

        #region Testing : GetAllTodosAsync
        [Test]
        public async Task GetAllTodosAsync_WithExistingTodo_ReturnAllTodos()
        {
            //arrange
            var randomTodo = new[] { CreateRandomTodo(), CreateRandomTodo(), CreateRandomTodo() };
            _repository.Setup(_repo => _repo.GetAllTodosAsync())
                .ReturnsAsync(randomTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.GetAllTodosAsync();

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(randomTodo, result);
        }
        #endregion

        #region Testing : GetTodoByIdAsync
        [Test]
        public async Task GetTodoByIdAsync_WithExistingTodo_ReturnExpectedTodo()
        {
            //arrange
            var randomId = randomValue.Next(10);
            Todo randomTodo = CreateRandomTodo();
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomId))
                .ReturnsAsync((Todo)randomTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.GetTodoByIdAsync(randomId);
            var value = (result as ActionResult<Todo>).Value;

            //asert
            Assert.NotNull(value);
            Assert.AreEqual(randomTodo, value);
        }

        [Test]
        public async Task GetTodoByIdAsync_WithNonExistingId_ReturnNotFound()
        {
            //arrange
            int randomId = randomValue.Next(10);
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomId))
                .ReturnsAsync((Todo)null);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.GetTodoByIdAsync(randomId);
            var value = (result as ActionResult<Todo>).Value;

            //assert
            Assert.IsNull(value);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Testing : CreateTodoAsync
        [Test]
        public async Task CreateTodoAsync_WithTodoToCreate_ReturnCreatedTodo()
        {
            //arrange
            Todo randomTodo = CreateRandomTodo();
            _repository.Setup(_repo => _repo.CreateTodoAsync(randomTodo))
                .ReturnsAsync((Todo)randomTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = (await controller.CreateTodoAsync(randomTodo));
            var value = (Todo)(result.Result as CreatedAtActionResult).Value;

            //assert
            Assert.NotNull(value.id);
            Assert.NotNull(value.title);
            Assert.NotNull(value.expirationDate);
            Assert.LessOrEqual(value.percentageOfCompletion, 100);
            Assert.AreEqual(randomTodo, value);

        }
        [Test]
        public async Task CreateTodoAsync_WithBadValueTodoToCreate_ReturnBadRequest()
        {
            //arrange
            Todo randomTodo = CreateBadValueTodo();
            _repository.Setup(_repo => _repo.CreateTodoAsync(randomTodo))
                .ReturnsAsync((Todo)null);

            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.CreateTodoAsync(randomTodo);

            //assert
            Assert.Null(result.Value);
            result.Result.Should().BeOfType<BadRequestResult>();

        }
        #endregion

        #region Testing : UpdateTodoAsync
        [Test]
        public async Task UpdateTodoAsync_WithExistingTodoToUpdate_ReturnUpdatedTodo()
        {
            //arrange
            Todo randomTodo = CreateRandomTodo();
            Todo updateTodo = UpdateRandomTodo();
            updateTodo.id = randomTodo.id;
            var controller = new TodoController(_repository.Object);

            //act
            var value = (await controller.UpdateTodoAsync(randomTodo.id, updateTodo) as ActionResult<Todo>).Value;

            //assert
            Assert.NotNull(randomTodo.id);
            Assert.AreEqual(value, updateTodo);
            Assert.AreEqual(value.title, updateTodo.title);
            Assert.GreaterOrEqual(value.expirationDate, DateTime.Now);
            Assert.LessOrEqual(value.percentageOfCompletion, 100);

        }

        [Test]
        public async Task UpdateTodoAsync_WithBadValueTodoToUpdate_ReturnBadRequest()
        {
            //arrange
            Todo randomTodo = CreateRandomTodo();
            Todo updateTodo = CreateBadValueTodo();
            var controller = new TodoController(_repository.Object);

            //act
            var result = (await controller.UpdateTodoAsync(randomTodo.id, updateTodo) as ActionResult<Todo>);

            //assert
            Assert.IsNull(result.Value);
            result.Result.Should().BeOfType<BadRequestResult>();

        }

        [Test]
        public async Task UpdateTodoAsync_MissmatchTodoIdToUpdate_ReturnBadRequest()
        {
            //arrange
            int randomId = randomValue.Next(10);
            Todo randomTodo = UpdateRandomTodo();
            var controller = new TodoController(_repository.Object);

            //act
            var result = (await controller.UpdateTodoAsync(randomId, randomTodo) as ActionResult<Todo>);

            //assert
            Assert.IsNull(result.Value);
            Assert.AreNotEqual(randomId, randomTodo.id);
            result.Result.Should().BeOfType<BadRequestResult>();

        }
        #endregion

        #region Testing : DeleteTodoAsyncAsync
        [Test]
        public async Task DeleteTodoAsync_WithExistingTodoToDelete_ReturnOk()
        {
            //arrange
            Todo randomTodo = CreateRandomTodo();
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.DeleteTodoAsync(randomTodo.id);

            //assert
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteTodoAsync_WithNonExistingTodoToDelete_ReturnNotFound()
        {
            //arrange
            Todo randomTodo = CreateRandomTodo();
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)null);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.DeleteTodoAsync(randomTodo.id);

            //assert
            result.Should().BeOfType<NotFoundResult>();
        }
        #endregion

        #region Testing : MarkTodoAsDoneAsync
        [Test]
        public async Task MarkTodoAsDoneAsync_WithExistingTodo_ReturnExpectedTodo()
        {
            //arrange
            Todo randomTodo = CreateRandomTodo();
            var updateTodo = randomTodo;
            updateTodo.percentageOfCompletion = 100;
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
            _repository.Setup(_repo => _repo.MarkTodoAsDoneAsync(randomTodo))
                .ReturnsAsync((Todo)updateTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.MarkTodoAsDoneAsync(randomTodo.id);
            var value = (result as ActionResult<Todo>).Value;

            //asert
            Assert.NotNull(value);
            Assert.AreEqual(100, value.percentageOfCompletion);
            Assert.AreEqual(randomTodo, value);
        }

        [Test]
        public async Task MarkTodoAsDoneAsync_WithNonExistingTodo_ReturnNotFound()
        {
            //arrange
            int randomId = randomValue.Next(10);
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomId))
                .ReturnsAsync((Todo)null);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.MarkTodoAsDoneAsync(randomId);
            var value = (result as ActionResult<Todo>).Value;

            //assert
            Assert.IsNull(value);
            result.Result.Should().BeOfType<NotFoundResult>();
        }
        #endregion

        #region Testing : ChangePercentage
        [Test]
        public async Task ChangePercentage_WithExistingTodoRightValue_ReturnExpectedTodo()
        {
            //arrange
            var updatedPercentage = 20;
            Todo randomTodo = CreateRandomTodo();
            var updateTodo = randomTodo;
            updateTodo.percentageOfCompletion = updatedPercentage;
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
            _repository.Setup(_repo => _repo.ChangeTodoPercentageAsync(randomTodo, randomTodo.id))
                .ReturnsAsync((Todo)updateTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.ChangeTodoPercentageAsync(randomTodo.id, updatedPercentage);
            var value = (result as ActionResult<Todo>).Value;

            //asert
            Console.WriteLine(randomTodo.title);
            Assert.NotNull(value);
            Assert.AreEqual(randomTodo, value);
            Assert.AreEqual(randomTodo.id, value.id);
            Assert.AreEqual(randomTodo.title, value.title);
            Assert.AreEqual(randomTodo.descriptions, value.descriptions);
            Assert.AreEqual(randomTodo.expirationDate, value.expirationDate);
            Assert.AreEqual(updatedPercentage, result.Value.percentageOfCompletion);
        }

        [Test]
        public async Task ChangePercentage_WithBadValue_ReturnBadRequest()
        {
            //arrange
            var updatedPercentage = 120;
            Todo randomTodo = CreateRandomTodo();
            var updateTodo = randomTodo;
            updateTodo.percentageOfCompletion = updatedPercentage;
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
            _repository.Setup(_repo => _repo.ChangeTodoPercentageAsync(randomTodo, randomTodo.id))
                .ReturnsAsync((Todo)updateTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.ChangeTodoPercentageAsync(randomTodo.id, updatedPercentage);
            var value = (result as ActionResult<Todo>).Value;

            //assert
            Assert.IsNull(value);
            result.Result.Should().BeOfType<BadRequestResult>();
        }
        #endregion

        #region Testing : GetIncomingTodo
        [Test]
        public async Task GetIncomingTodo_WithExistingTodo_ReturnAllTodos()
        {
            //arange
            int randomDays = randomValue.Next(10);
            var randomTodo = new[] { CreateRandomTodo(), CreateRandomTodo(), CreateRandomTodo() };
            _repository.Setup(_repo => _repo.GetIncomingTodoAsync(randomDays))
                .ReturnsAsync(randomTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.GetIncomingTodoAsync(randomDays);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(randomTodo, result);
        }
        #endregion

        #region generate dummy todo
        private Todo CreateRandomTodo()
        {
            var randTitle = randomValue.Next(randomString.Length);
            var randDesc = randomValue.Next(randomString.Length);
            return new()
            {
                id = randomValue.Next(10),
                title = randomString[randTitle],
                descriptions = randomString[randDesc],
                percentageOfCompletion = 0,
                expirationDate = DateTime.Now
            };
        }

        private Todo UpdateRandomTodo()
        {
            var randTitle = randomValue.Next(randomString.Length);
            var randDesc = randomValue.Next(randomString.Length);
            double randNumber = randomValue.Next(1, 21);
            return new()
            {
                id = randomValue.Next(10),
                title = randomString[randTitle],
                descriptions = randomString[randDesc],
                percentageOfCompletion = randomValue.Next(100),
                expirationDate = DateTime.Now.AddDays(randNumber)
            };
        }

        private Todo CreateBadValueTodo()
        {
            return new()
            {
                title = string.Empty,
                percentageOfCompletion = 101,
                expirationDate = DateTime.MinValue
            };
        }
        #endregion

    }
}
