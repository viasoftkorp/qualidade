using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ServicosNaoConformidades.Controllers;

public class ServicoNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get ServicoNaoConformidade com sucesso")]
    public async Task GetServicoNaoConformidadeControllerWithSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IServicoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IServicoNaoConformidadeservice>();
        var produtoSolucaoInput = new ServicoNaoConformidadeOutput(TestUtils.ObjectMother.GetServicoNaoConformidade(0));
        var expectedResult = new ProdutoNaoConformidadeOutput(TestUtils.ObjectMother.GetProdutoNaoConformidade(0));

        fakeService.Get(produtoSolucaoInput.IdNaoConformidade, produtoSolucaoInput.Id).Returns(produtoSolucaoInput);

        var controller = new ServicoNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(produtoSolucaoInput.IdNaoConformidade,produtoSolucaoInput.Id);
        
        //Assert
        var result = output as OkObjectResult;
        
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Get ServicoNaoConformidade sem sucesso")]
    public async Task GetServicoNaoConformidadeControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IServicoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IServicoNaoConformidadeservice>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new ServicoNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(idNaoConformidade,id);
        
        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "GetViewList Controller")]
    public async Task GetListServicoNaoConformidadeController()
    {
        // Arrange
        var fakeViewService = Substitute.For<IServicoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IServicoNaoConformidadeservice>();
        var viewOutput = new ServicoNaoConformidadeViewOutput(TestUtils.ObjectMother.GetServicoNaoConformidade(0), 
            TestUtils.ObjectMother.GetProduto(0), TestUtils.ObjectMother.GetRecurso(0));
        
        var getOutput = new PagedResultDto<ServicoNaoConformidadeViewOutput>
        {
            Items = new List<ServicoNaoConformidadeViewOutput> {viewOutput},
            TotalCount = 1
        };
    
        var input = new PagedFilteredAndSortedRequestInput { };
    
        fakeViewService.GetListView(viewOutput.IdNaoConformidade, input).Returns(getOutput);
    
        var controller = new ServicoNaoConformidadeController(fakeService,fakeViewService);
    
        // Act
        var output = await controller.GetListView(viewOutput.IdNaoConformidade, input);
    
        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }
     [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IServicoNaoConformidadeservice>();
        var fakeViewService = Substitute.For<IServicoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var servicoSolucaoNaoConformidadeInput = new ServicoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Horas = TestUtils.ObjectMother.Ints[0],
            Minutos = TestUtils.ObjectMother.Ints[0],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[0],
        };

        await fakeService.Insert(idNaoConformidade, servicoSolucaoNaoConformidadeInput);

        var controller = new ServicoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Insert(servicoSolucaoNaoConformidadeInput.IdNaoConformidade, servicoSolucaoNaoConformidadeInput);

        // Assert
        var result = output.Result as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IServicoNaoConformidadeservice>();
        var fakeViewService = Substitute.For<IServicoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var servicoNaoConformidadeInput = new ServicoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Horas = TestUtils.ObjectMother.Ints[0],
            Minutos = TestUtils.ObjectMother.Ints[0],
            IdRecurso = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[0],
        };
        await fakeService.Update(idNaoConformidade, servicoNaoConformidadeInput.Id, servicoNaoConformidadeInput);

        var controller = new ServicoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Update(idNaoConformidade, servicoNaoConformidadeInput.Id, servicoNaoConformidadeInput);

        // Assert
        var result = output.Result as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IServicoNaoConformidadeservice>();
        var fakeViewService = Substitute.For<IServicoNaoConformidadeViewService>();
        var idCausa = TestUtils.ObjectMother.Guids[0];
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        await fakeService.Remove(idNaoConformidade, idCausa);

        var controller = new ServicoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Remove(idNaoConformidade, idCausa);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }
}