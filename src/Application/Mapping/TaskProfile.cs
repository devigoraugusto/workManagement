using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<UpdateTaskDto, TaskItem>();
            CreateMap<TaskItem, TaskItemDto>();
        }
    }
}
