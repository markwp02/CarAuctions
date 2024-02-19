using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine("--> Consuming auction created: " + context.Message.Id);

        var itemUpdated = _mapper.Map<Item>(context.Message);

        await DB.Update<Item>()
            .MatchID(itemUpdated.ID)
            .ModifyOnly(a => new
                {
                    a.Make,
                    a.Model,
                    a.Year,
                    a.Color,
                    a.Mileage
                }, itemUpdated)
            .ExecuteAsync();    
    }
}
