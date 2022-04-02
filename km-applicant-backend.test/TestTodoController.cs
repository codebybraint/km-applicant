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
    public class TestTodoController
    {
        private Mock<ITodoRepository> _repository = new Mock<ITodoRepository>();
        private readonly Random rand = new(); //to generate random value
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

            Console.WriteLine(result);

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
            var j = rand.Next(10);
            Todo randomTodo = CreateRandomTodo();
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(j))
                .ReturnsAsync((Todo) randomTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.GetTodoByIdAsync(j);
            var value = (result as ActionResult<Todo>).Value;

            //asert
            Assert.NotNull(value);
            Assert.AreEqual(randomTodo, value);
        }

        [Test]
        public async Task GetTodoByIdAsync_WithNonExistingTodo_ReturnNotFound()
        {
            //arrange
            int j = rand.Next(10);
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(j))
                .ReturnsAsync((Todo)null);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.GetTodoByIdAsync(j);
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
                .ReturnsAsync((Todo) randomTodo);
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
            Todo _updated = UpdateRandomTodo();
            _updated.id = randomTodo.id;

            var controller = new TodoController(_repository.Object);

            //act
            var value = (await controller.UpdateTodoAsync(randomTodo.id, _updated) as ActionResult<Todo>).Value;

            //assert
            Assert.NotNull(randomTodo.id);
            Assert.AreEqual(value, _updated);
            Assert.AreEqual(value.title, _updated.title);
            Assert.GreaterOrEqual(value.expirationDate, DateTime.Now);
            Assert.LessOrEqual(value.percentageOfCompletion, 100);

        }

        [Test]
        public async Task UpdateTodoAsync_WithBadValueTodoToUpdate_ReturnBadRequest()
        {
            //arrange
            Todo randomTodo = CreateRandomTodo();
            Todo _updated = CreateBadValueTodo();

            var controller = new TodoController(_repository.Object);

            //act
            var result = (await controller.UpdateTodoAsync(randomTodo.id, _updated) as ActionResult<Todo>);

            //assert
            Assert.IsNull(result.Value);
            result.Result.Should().BeOfType<BadRequestResult>();

        }

        [Test]
        public async Task UpdateTodoAsync_MissmatchTodoIdToUpdate_ReturnBadRequest()
        {
            //arrange
            int j = rand.Next(10);
            Todo randomTodo = UpdateRandomTodo();

            var controller = new TodoController(_repository.Object);

            //act
            var result = (await controller.UpdateTodoAsync(j, randomTodo) as ActionResult<Todo>);

            //assert
            Assert.IsNull(result.Value);
            Assert.AreNotEqual(j, randomTodo.id);
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
            var _updated = randomTodo;
            _updated.percentageOfCompletion = 100;
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
            _repository.Setup(_repo => _repo.MarkTodoAsDoneAsync(randomTodo))
                .ReturnsAsync((Todo)_updated);
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
            int j = rand.Next(10);
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(j))
                .ReturnsAsync((Todo)null);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.MarkTodoAsDoneAsync(j);
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
            var i = 20;
            Todo randomTodo = CreateRandomTodo();
            var _updated = randomTodo;
            _updated.percentageOfCompletion = i;
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
            _repository.Setup(_repo => _repo.ChangeTodoPercentageAsync(randomTodo, randomTodo.id))
                .ReturnsAsync((Todo)_updated);

            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.ChangeTodoPercentageAsync(randomTodo.id, i);
            var value = (result as ActionResult<Todo>).Value;

            //asert
            Console.WriteLine(randomTodo.title);
            Assert.NotNull(value);
            Assert.AreEqual(randomTodo, value);
            Assert.AreEqual(randomTodo.id, value.id);
            Assert.AreEqual(randomTodo.title, value.title);
            Assert.AreEqual(randomTodo.descriptions, value.descriptions);
            Assert.AreEqual(randomTodo.expirationDate, value.expirationDate);
            Assert.AreEqual(20, result.Value.percentageOfCompletion);
        }

        [Test]
        public async Task ChangePercentage_WithBadValue_ReturnBadRequest()
        {
            //arrange
            var i = 120;
            Todo randomTodo = CreateRandomTodo();
            var _updated = randomTodo;
            _updated.percentageOfCompletion = i;
            _repository.Setup(_repo => _repo.GetTodoByIdAsync(randomTodo.id))
                .ReturnsAsync((Todo)randomTodo);
            _repository.Setup(_repo => _repo.ChangeTodoPercentageAsync(randomTodo, randomTodo.id))
                .ReturnsAsync((Todo)_updated);

            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.ChangeTodoPercentageAsync(randomTodo.id, i);
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
            int j = rand.Next(10);
            var randomTodo = new[] { CreateRandomTodo(), CreateRandomTodo(), CreateRandomTodo() };

            _repository.Setup(_repo => _repo.GetIncomingTodoAsync(j))
                .ReturnsAsync(randomTodo);
            var controller = new TodoController(_repository.Object);

            //act
            var result = await controller.GetIncomingTodoAsync(j);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(randomTodo, result);
        }
        #endregion

        #region generate dummy todo
        private Todo CreateRandomTodo()
        {
            var randTitle = rand.Next(randomString.Length);
            var randDesc = rand.Next(randomString.Length);
            return new()
            {
                id = rand.Next(10),
                title = randomString[randTitle],
                descriptions = randomString[randDesc],
                percentageOfCompletion = 0,
                expirationDate = DateTime.Now
            };
        }

        private Todo UpdateRandomTodo()
        {
            var randTitle = rand.Next(randomString.Length);
            var randDesc = rand.Next(randomString.Length);
            double rand_number = rand.Next(1,21);
            return new()
            {
                id = rand.Next(10),
                title = randomString[randTitle],
                descriptions = randomString[randDesc],
                percentageOfCompletion = rand.Next(100),
                expirationDate = DateTime.Now.AddDays(rand_number)
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
