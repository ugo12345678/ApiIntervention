using AutoMapper;
using AutoMapper.Extensions.EnumMapping;

namespace Business.Extensions
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Skip member mapping and use a EnumMapping converter convention to convert to the destination type
        /// </summary>
        /// <remarks>Not used for LINQ projection (ProjectTo)</remarks>
        /// <param name="mappingExpression">Mapping configuration options</param>
        /// <typeparam name="TSource">Source enum type</typeparam>
        /// <typeparam name="TDestination">Destination enum type</typeparam>
        public static IEnumMappingExpression<TSource, TDestination> ConvertUsingEnumNameMapping<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
            where TSource : struct, Enum
            where TDestination : struct, Enum
            => mappingExpression.ConvertUsingEnumMapping(opt => opt.MapByName());
    }
}
