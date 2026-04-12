using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class FundoService : IFundoService
    {
        private readonly IFundoRepository _fundoRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "fundos";
        public FundoService(IFundoRepository fundoRepository, IMemoryCache cache)
        {
            _fundoRepository = fundoRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<FundoResponse>> BuscarTodosAsync()
        {
            if (_cache.TryGetValue(CacheKey, out IEnumerable<FundoResponse>? cached) && cached is not null)
                return cached;

            var fundos = await _fundoRepository.BuscarTodosAsync();
            _cache.Set(CacheKey, fundos, TimeSpan.FromMinutes(5));
            return fundos;
        }

        public async Task<FundoResponse?> BuscarPorCodigoAsync(string codigo)
        {
            return await _fundoRepository.BuscarPorCodigoAsync(codigo);
        }

        public async Task<FundoResponse> CriarFundoAsync(CriarFundoRequest criarFundoRequest)
        {
            var fundo = new Fundo(criarFundoRequest.Codigo, criarFundoRequest.Nome, criarFundoRequest.Cnpj, criarFundoRequest.CodigoTipo);
            await _fundoRepository.CriarFundoAsync(fundo);
            _cache.Remove(CacheKey);
            return new FundoResponse
            {
                Codigo = fundo.Codigo,
                Nome = fundo.Nome,
                Cnpj = fundo.Cnpj,
                CodigoTipo = fundo.CodigoTipo
            };
        }

        public async Task AtualizarFundoAsync(string codigo, AtualizarFundoRequest atualizarFundoRequest)
        {
            var fundo = new Fundo(codigo, atualizarFundoRequest.Nome, atualizarFundoRequest.Cnpj, atualizarFundoRequest.CodigoTipo);
            await _fundoRepository.AtualizarFundoAsync(fundo);
            _cache.Remove(CacheKey);
        }

        public async Task DeletarFundoAsync(string codigo)
        {
            await _fundoRepository.DeletarFundoAsync(codigo);
            _cache.Remove(CacheKey);
        }

        public async Task MovimentarPatrimonioAsync(string codigo, decimal valor)
        {
            await _fundoRepository.MovimentarPatrimonioAsync(codigo, valor);
            _cache.Remove(CacheKey);
        }
    }
}
