using AutoMapper;
using todofinal.Application.DTOs;
using todofinal.Domain.Entities;

namespace todofinal.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ToDoTask (Veritabanı Nesnesi) -> ToDoDto (Dışarı Verilecek Nesne)
        // ReverseMap() sayesinde tam tersi dönüşüm de (Dto -> Entity) otomatik tanımlanır.
        CreateMap<ToDoTask, TodoDto>();
        CreateMap<CreateTodoDto, ToDoTask>();
        CreateMap<UpdateTodoDto, ToDoTask>();
    }
}