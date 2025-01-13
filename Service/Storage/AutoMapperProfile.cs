using AutoMapper;
using Google.Protobuf.Collections;
using Newtonsoft.Json.Linq;
using Plato.MDM.DataAccess.Postgres.Protos;
using Plato.MDM.Storage.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DirectoryReply, MdmDirectoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(dest => dest.DirectoryDomainId, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.DirectoryDomainId) ? (Guid?)null : Guid.Parse(src.DirectoryDomainId)))
            .ForMember(dest => dest.DirectoryLevelId, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.DirectoryLevelId) ? (Guid?)null : Guid.Parse(src.DirectoryLevelId)));

        CreateMap<MdmDirectoryDto, DirectoryReply>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? ""))
            .ForMember(dest => dest.DirectoryDomainId, opt => opt.MapFrom(src =>
                src.DirectoryDomainId.HasValue ? src.DirectoryDomainId.Value.ToString() : string.Empty))
            .ForMember(dest => dest.DirectoryLevelId, opt => opt.MapFrom(src =>
                src.DirectoryLevelId.HasValue ? src.DirectoryLevelId.Value.ToString() : string.Empty));

        CreateMap<VersionByDirectoryReply, MdmDirectoryVersionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(dest => dest.DirectoryId, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.DirectoryId) ? (Guid?)null : Guid.Parse(src.DirectoryId)));

        CreateMap<MdmDirectoryVersionDto, VersionByDirectoryReply>()
            .ForMember(dest => dest.Id, from => from.MapFrom(src => $"{src.Id}"))
            .ForMember(dest => dest.DirectoryId, from => from.MapFrom(src => $"{src.DirectoryId}"))
            .ForMember(dest => dest.DataSourceName, from => from.MapFrom(src => $"{src.DataSourceName}"))
            .ForMember(dest => dest.DataSourceDate, from => from.MapFrom(src => $"{src.DataSourceDate}"))
            .ForMember(dest => dest.DataSourceUrl, from => from.MapFrom(src => $"{src.DataSourceUrl}"))
            .ForMember(dest => dest.Version, from => from.MapFrom(src => $"{src.Version}"))
            .ForMember(dest => dest.VersionDate, from => from.MapFrom(src => $"{src.VersionDate}"))
            .ForMember(dest => dest.VersionDescription, from => from.MapFrom(src => $"{src.VersionDescription}"))
            .ForMember(dest => dest.TableName, from => from.MapFrom(src => $"{src.TableName}"));

        CreateMap<DirectoryDataResponse, MdmTableDataDto>()
            .ForMember(dest => dest.TableName, from => from.MapFrom(src => $"{src.TableName}"))
            .ForMember(dest => dest.MainTable, from => from.MapFrom(src => JArray.Parse(src.MainTable)))
            .ForMember(dest => dest.ForeignTables, from => from.MapFrom(src => MapToForeignTables(src.ForeignTables)));
    }

    private Dictionary<string, JArray> MapToForeignTables(MapField<string, string> foreignTables)
    {
        var mappedTables = new Dictionary<string, JArray>();

        foreach (var foreignTable in foreignTables)
            mappedTables.Add(foreignTable.Key, JArray.Parse(foreignTable.Value));

        return mappedTables;
    }
}