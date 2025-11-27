using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeServices;

public class GetOperacoesViewTests : OperacaoRetrabalhoNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Se endpoint chamado, para cada operação encontrada deve fazer join com recurso e retornar a view de operações")]
    public async Task GetOperacoesViewTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var operacoes = new List<Operacao>
        {
            new Operacao
            {
                IdRecurso = TestUtils.ObjectMother.Guids[0],
                Id = TestUtils.ObjectMother.Guids[0],
                NumeroOperacao = "011",
                IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[0]
            },
            new Operacao
            {
                IdRecurso = TestUtils.ObjectMother.Guids[1],
                Id = TestUtils.ObjectMother.Guids[1],
                NumeroOperacao = "012",
                IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[0]
            },
            new Operacao
            {
                IdRecurso = TestUtils.ObjectMother.Guids[2],
                Id = TestUtils.ObjectMother.Guids[2],
                NumeroOperacao = "013",
                IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[0]
            },
            new Operacao
            {
                IdRecurso = TestUtils.ObjectMother.Guids[3],
                Id = TestUtils.ObjectMother.Guids[3],
                NumeroOperacao = "014",
                IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[3]
            },
        };
        var recursos = new List<Recurso>
        {
            new Recurso
            {
                Id = TestUtils.ObjectMother.Guids[0],
                Descricao = TestUtils.ObjectMother.Strings[0],
                Codigo = TestUtils.ObjectMother.Ints[0].ToString(),
            },
            new Recurso
            {
                Id = TestUtils.ObjectMother.Guids[1],
                Descricao = TestUtils.ObjectMother.Strings[1],
                Codigo = TestUtils.ObjectMother.Ints[1].ToString(),
            },
            new Recurso
            {
                Id = TestUtils.ObjectMother.Guids[2],
                Descricao = TestUtils.ObjectMother.Strings[2],
                Codigo = TestUtils.ObjectMother.Ints[2].ToString(),
            }
        };
        var naoConformidadeRepository = ServiceProvider.GetService<IRepository<NaoConformidade>>();
        
        var operacoesRetrabalho = new List<OperacaoRetrabalhoNaoConformidade>
        {
            new OperacaoRetrabalhoNaoConformidade
            {
                Id = TestUtils.ObjectMother.Guids[0],
                IdNaoConformidade = TestUtils.ObjectMother.Guids[0]
            },
            new OperacaoRetrabalhoNaoConformidade
            {
                Id = TestUtils.ObjectMother.Guids[3],
                IdNaoConformidade = TestUtils.ObjectMother.Guids[3]
            },
        };
        var naoConformidades = new List<NaoConformidade>
        {
            TestUtils.ObjectMother.GetNaoConformidade(0),
            TestUtils.ObjectMother.GetNaoConformidade(1),
            TestUtils.ObjectMother.GetNaoConformidade(2),
            TestUtils.ObjectMother.GetNaoConformidade(3),
        };
        await naoConformidadeRepository.InsertRangeAsync(naoConformidades);
        await mocker.OperacaoRetrabalhoNaoConformidades.InsertRangeAsync(operacoesRetrabalho);
        await mocker.Recursos.InsertRangeAsync(recursos);
        await mocker.Operacoes.InsertRangeAsync(operacoes);
    
        await UnitOfWork.SaveChangesAsync();

        var expectedResult = new PagedResultDto<OperacaoViewOutput>
        {
            Items = new List<OperacaoViewOutput>
            {
                new OperacaoViewOutput
                {
                    Id = TestUtils.ObjectMother.Guids[0],
                    NumeroOperacao = "011",
                    IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[0],
                    IdRecurso = TestUtils.ObjectMother.Guids[0],
                    DescricaoRecurso = TestUtils.ObjectMother.Strings[0],
                    CodigoRecurso = TestUtils.ObjectMother.Ints[0].ToString()
                },
                new OperacaoViewOutput
                {
                    Id = TestUtils.ObjectMother.Guids[1],
                    NumeroOperacao = "012",
                    IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[0],
                    IdRecurso = TestUtils.ObjectMother.Guids[1],
                    DescricaoRecurso = TestUtils.ObjectMother.Strings[1],
                    CodigoRecurso = TestUtils.ObjectMother.Ints[1].ToString()
                },
                new OperacaoViewOutput
                {
                    Id = TestUtils.ObjectMother.Guids[2],
                    NumeroOperacao = "013",
                    IdOperacaoRetrabalhoNaoConformdiade = TestUtils.ObjectMother.Guids[0],
                    IdRecurso = TestUtils.ObjectMother.Guids[2],
                    DescricaoRecurso = TestUtils.ObjectMother.Strings[2],
                    CodigoRecurso = TestUtils.ObjectMother.Ints[2].ToString()
                }  
            },
            TotalCount = 3
        };
        // Act
        var output = await service.GetOperacoesView(TestUtils.ObjectMother.Guids[0], new PagedFilteredAndSortedRequestInput());
        
        // Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
}