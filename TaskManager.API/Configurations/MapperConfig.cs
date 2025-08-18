using AutoMapper;
using TaskManager.API.Data;
using TaskManager.API.Models.TaskItems;
using TaskManager.API.Models.Users;

namespace TaskManager.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ApiUser, UserDto>().ReverseMap();

            CreateMap<TaskItem, TaskItemDto>().ReverseMap();

        }
    }
}
