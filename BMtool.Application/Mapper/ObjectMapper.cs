using AutoMapper;
using BMtool.Application.Models;
using BMtool.Core.Entities;

namespace BMtool.Application.Mapper
{
    public class BMtoolDtoMapper : Profile
    {
        public BMtoolDtoMapper()
        {
            CreateMap<Department, DepartmentModel>().ReverseMap();

            CreateMap<UpdateDto, RegisterModel>().ReverseMap();
        }
    }
}
