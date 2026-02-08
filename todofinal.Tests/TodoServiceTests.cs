using Moq;
using AutoMapper;
using FluentValidation;
using todofinal.Application.Services;
using todofinal.Application.Interfaces;
using todofinal.Application.DTOs;
using todofinal.Domain.Entities;
using Xunit;

public class TodoServiceTests
{
    private readonly Mock<ITodoRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateTodoDto>> _validatorMock;
    private readonly TodoService _todoService;

    public TodoServiceTests()
    {
        _repoMock = new Mock<ITodoRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateTodoDto>>();
        
        _todoService = new TodoService(_repoMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetTaskByIdAsync_KayıtVarsa_TodoDtoDonmeli()
    {
        // Arrange (Hazırlık)
        var taskId = 1;
        var task = new ToDoTask { Id = taskId, Title = "Test Görevi" };
        var dto = new TodoDto { Id = taskId, Title = "Test Görevi" };

        _repoMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
        _mapperMock.Setup(m => m.Map<TodoDto>(task)).Returns(dto);

        // Act (Eylem)
        var result = await _todoService.GetTaskByIdAsync(taskId);

        // Assert (Doğrulama)
        Assert.NotNull(result);
        Assert.Equal(taskId, result.Id);
        _repoMock.Verify(r => r.GetByIdAsync(taskId), Times.Once); // Metot tam 1 kere çağrıldı mı?
    }
}