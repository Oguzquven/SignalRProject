using AutoMapper;
using SignalR.DtoLayer.MenuTableDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
	public class MenuTableMapping : Profile
	{
		public MenuTableMapping()
		{
			// MenuTable'dan DTO'lara eşlemeler
			CreateMap<MenuTable, ResultMenuTableDto>();
			CreateMap<MenuTable, CreateMenuTableDto>();
			CreateMap<MenuTable, UpdateMenuTableDto>();
			CreateMap<MenuTable, GetMenuTableDto>();

			// DTO'lardan MenuTable'a eşlemeler
			CreateMap<CreateMenuTableDto, MenuTable>();
			CreateMap<UpdateMenuTableDto, MenuTable>();
		}
	}
}
