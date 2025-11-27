using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Controller;

public class NaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get NaoConformidade com sucesso")]
    public async Task GetControllerWithSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<INaoConformidadeViewService>();
        var fakeService = Substitute.For<INaoConformidadeService>();
        var viewOutput = new NaoConformidadeOutput(TestUtils.ObjectMother.GetNaoConformidade(0));

        fakeService.Get(viewOutput.Id).Returns(viewOutput);

        var controller = new NaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(viewOutput.Id);
        
        //Assert
        var result = output as OkObjectResult;

        result.Value.Should().BeEquivalentTo(viewOutput);
        result.StatusCode.Should().Be(200);
    }
    
    [Fact(DisplayName = "GetView NaoConformidade sem sucesso")]
    public async Task GetControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<INaoConformidadeViewService>();
        var fakeService = Substitute.For<INaoConformidadeService>();
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new NaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(id);
        
        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "GetViewList Controller")]
    public async Task GetListController()
    {
        // Arrange
        var fakeViewService = Substitute.For<INaoConformidadeViewService>();
        var fakeService = Substitute.For<INaoConformidadeService>();
        var viewOutput = new NaoConformidadeViewOutput();
        
        var getOutput = new PagedResultDto<NaoConformidadeViewOutput>
        {
            Items = new List<NaoConformidadeViewOutput> {viewOutput},
            TotalCount = 1
        };

        var input = new PagedFilteredAndSortedRequestInput { };

        fakeViewService.GetListView(input).Returns(getOutput);

        var controller = new NaoConformidadeController(fakeService,fakeViewService);

        // Act
        var output = await controller.GetListView(input);

        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }
     [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<INaoConformidadeService>();
        var fakeViewService = Substitute.For<INaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = OrigemNaoConformidade.Cliente,
            Status = StatusNaoConformidade.Aberto,
            IdNotaFiscal = null,
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdPessoa = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdLote = default,
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[0],
            CampoNf = null,
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = null,
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = null,
        };

        await fakeService.Create(input);

        var controller = new NaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Create(input);

        // Assert
        var result = output as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }
    
    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<INaoConformidadeService>();
        var fakeViewService = Substitute.For<INaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            Origem = OrigemNaoConformidade.Cliente,
            Status = StatusNaoConformidade.Aberto,
            IdNotaFiscal = null,
            IdNatureza = TestUtils.ObjectMother.Guids[2],
            IdPessoa = TestUtils.ObjectMother.Guids[2],
            IdProduto = TestUtils.ObjectMother.Guids[2],
            IdLote = default,
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[2],
            CampoNf = null,
            IdCriador = TestUtils.ObjectMother.Guids[0],
            Revisao = "1",
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            Equipe = null,
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = null,
        };

        var controller = new NaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Update(idNaoConformidade, input);

        // Assert
        var result = output as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }
    
    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<INaoConformidadeService>();
        var fakeViewService = Substitute.For<INaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        var controller = new NaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Delete(idNaoConformidade);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }
    
}
