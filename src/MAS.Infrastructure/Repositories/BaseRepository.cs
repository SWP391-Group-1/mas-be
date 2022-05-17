using AutoMapper;
using MAS.Infrastructure.Data;
using System;

namespace MAS.Infrastructure.Repositories
{
    public class BaseRepository
    {
        protected readonly IMapper mapper;
        protected readonly AppDbContext context;

        public BaseRepository(IMapper mapper, AppDbContext context)
        {
            this.mapper = mapper;
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
