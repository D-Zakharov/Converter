using Converter.Domain.DB;
using Microsoft.EntityFrameworkCore;

namespace Converter.Domain.Services.Factories
{
    public class RequestDataFactory : IRequestDataFactory
    {
        private readonly IDbContextFactory<ConverterDbContext> dbContextFactory;

        public RequestDataFactory(IDbContextFactory<ConverterDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public IRequestData GetRequestHandler()
        {
            return new RequestData(dbContextFactory.CreateDbContext());
        }
    }
}
