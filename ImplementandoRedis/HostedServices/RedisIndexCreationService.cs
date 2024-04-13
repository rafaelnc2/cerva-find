﻿using ImplementandoRedis.Core.Entities;
using Redis.OM;
using StackExchange.Redis;

namespace ImplementandoRedis.Api.HostedServices;

public class RedisIndexCreationService : BackgroundService
{
    private readonly RedisConnectionProvider _provider;
    private readonly ILogger<RedisIndexCreationService> _logger;

    public RedisIndexCreationService(IConnectionMultiplexer redisMultiplexerConnect, ILogger<RedisIndexCreationService> logger)
    {
        _provider = new RedisConnectionProvider(redisMultiplexerConnect);
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Criando índices no Redis");

        foreach (var item in ListaIndicesParaCriar())
        {
            var created = await _provider.Connection.CreateIndexAsync(item);

            if (created)
                _logger.LogInformation($"Índice {item.Name} criado com sucesso");
            else
                _logger.LogInformation($"Índice {item.Name} ja existe");
        }
    }

    private IEnumerable<Type> ListaIndicesParaCriar() =>
        new List<Type>()
        {
            typeof(Cerveja),
            typeof(TipoCerveja)
        };
}
