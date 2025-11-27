using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Defeitos.Services.DefeitoServiceTests;

public class GetTests : DefeitoServiceTest
{
    [Fact(DisplayName = "Get Defeito with Success")]
    public async Task GetDefeitoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var input = TestUtils.ObjectMother.GetDefeito(0);
        
        await mocker.Defeitos.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var defeito = await mocker.Defeitos.FindAsync(TestUtils.ObjectMother.Guids[0]);
        defeito.Should().BeEquivalentTo(output);
    }
    
    
    [Fact(DisplayName = "Get Defeito Returns Null")]
    public async Task GetDefeitoWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetDefeito(0);
        
        await mocker.Defeitos.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().BeNull();
    }


    [Fact(DisplayName = "GetList DefeitoView with Success")]
    public async Task GetListDefeitoViewWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var defeitos = new List<Defeito>
        {
            TestUtils.ObjectMother.GetDefeito(0),
            TestUtils.ObjectMother.GetDefeito(1)
        };
        await mocker.Defeitos.InsertRangeAsync(defeitos);
        var solucoes = new List<Solucao>
        {
            TestUtils.ObjectMother.GetSolucao(0),
            TestUtils.ObjectMother.GetSolucao(1)
        };
        await mocker.Solucoes.InsertRangeAsync(solucoes);
        var causas = new List<Causa>
        {
            TestUtils.ObjectMother.GetCausa(0),
            TestUtils.ObjectMother.GetCausa(1)
        };
        await mocker.Causas.InsertRangeAsync(causas);
        
        await UnitOfWork.SaveChangesAsync();

        var input = new PagedFilteredAndSortedRequestInput();

        var expectedResult = new PagedResultDto<DefeitoViewOutput>
        {
            TotalCount = 2,
            Items = new List<DefeitoViewOutput>
            {
                new DefeitoViewOutput
                {
                    Id = defeitos[0].Id,
                    Codigo = defeitos[0].Codigo,
                    IdSolucao = defeitos[0].IdSolucao,
                    IdCausa = defeitos[0].IdCausa,
                    Descricao = defeitos[0].Descricao,
                    Detalhamento = defeitos[0].Detalhamento,
                    IsAtivo = defeitos[0].IsAtivo,
                    CodigoCausa = causas[0].Codigo,
                    CodigoSolucao = solucoes[0].Codigo,
                    DescricaoCausa = causas[0].Descricao,
                    DescricaoSolucao = solucoes[0].Descricao
                },
                new DefeitoViewOutput
                {
                    Id = defeitos[1].Id,
                    Codigo = defeitos[1].Codigo,
                    IdSolucao = defeitos[1].IdSolucao,
                    IdCausa = defeitos[1].IdCausa,
                    Descricao = defeitos[1].Descricao,
                    Detalhamento = defeitos[1].Detalhamento,
                    IsAtivo = defeitos[1].IsAtivo,
                    CodigoCausa = causas[1].Codigo,
                    CodigoSolucao = solucoes[1].Codigo,
                    DescricaoCausa = causas[1].Descricao,
                    DescricaoSolucao = solucoes[1].Descricao
                }
            }
        };

        //Act
        var output = await service.GetViewList(input);

        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
}