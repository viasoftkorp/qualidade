using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.Testing;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Controllers;

public class ImplementacaoEvitarReincidenciaNaoConformidadeTests : UnitTestBase
{
    [Fact(DisplayName="Se chamar busca de todas as implementaçãoes, deve buscar a view de implementações")]
    public async Task GetListViewTest()
    {
        //Arrange
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var input = new GetListViewInput
        {

        };
        var expectedResult = new PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>();
        mocker.ImplementacaoEvitarReincidenciaNaoConformidadeViewService.GetListView(TestUtils.ObjectMother.Guids[0], input).Returns(expectedResult);
        //Act
        var result = await controller.GetListView(TestUtils.ObjectMother.Guids[0], input);
        //Assert
        var actionResult = result as OkObjectResult;
        actionResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    [Fact(DisplayName="Se chamar busca pela implementação, deve retornar a implementação")]
    public async Task GetByIdTest()
    {
        //Arrange
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var input = new PagedFilteredAndSortedRequestInput
        {

        };
        var expectedResult = new ImplementacaoEvitarReincidenciaNaoConformidadeOutput
        {
            Id = TestUtils.ObjectMother.Guids[0]
        };
        mocker.ImplementacaoEvitarReincidenciaNaoConformidadeService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(expectedResult);
        //Act
        var result = await controller.GetById(TestUtils.ObjectMother.Guids[0]);
        //Assert
        var actionResult = result as OkObjectResult;
        actionResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    [Fact(DisplayName="Se chamar inserção deimplementação, deve inserir implementação")]
    public async Task InsertTest()
    {
        //Arrange
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var input = new ImplementacaoEvitarReincidenciaNaoConformidadeInput()
        {

        };
        //Act
        var result = await controller.Insert(TestUtils.ObjectMother.Guids[0], input);
        //Assert
        await mocker.ImplementacaoEvitarReincidenciaNaoConformidadeService.Received(1)
            .Insert(TestUtils.ObjectMother.Guids[0], input);

        var actionResult = result as OkResult;
        actionResult.Should().NotBeNull();
    }
    [Fact(DisplayName="Se chamar update de implementação, deve atualizar implementação")]
    public async Task UpdateTest()
    {
        //Arrange
        var mocker = GetMocker();
        var controller = GetController(mocker);
        var input = new ImplementacaoEvitarReincidenciaNaoConformidadeInput()
        {

        };
        //Act
        var result = await controller.Update(TestUtils.ObjectMother.Guids[0], input);
        //Assert
        await mocker.ImplementacaoEvitarReincidenciaNaoConformidadeService.Received(1)
            .Update(TestUtils.ObjectMother.Guids[0], input);

        var actionResult = result as OkResult;
        actionResult.Should().NotBeNull();
    }
    [Fact(DisplayName="Se chamar remoção de implementação, deve remover a implementação")]
    public async Task RemoveTest()
    {
        //Arrange
        var mocker = GetMocker();
        var controller = GetController(mocker);
        //Act
        var result = await controller.Remove(TestUtils.ObjectMother.Guids[0], TestUtils.ObjectMother.Guids[0]);
        //Assert
        await mocker.ImplementacaoEvitarReincidenciaNaoConformidadeService.Received(1)
            .Remove(TestUtils.ObjectMother.Guids[0], TestUtils.ObjectMother.Guids[0]);

        var actionResult = result as OkResult;
        actionResult.Should().NotBeNull();
    }
    private Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            ImplementacaoEvitarReincidenciaNaoConformidadeService = Substitute.For<IImplementacaoEvitarReincidenciaNaoConformidadeService>(),
            ImplementacaoEvitarReincidenciaNaoConformidadeViewService = Substitute.For<IImplementacaoEvitarReincidenciaNaoConformidadeViewService>()
        };
        return mocker;
    }

    private ImplementacaoEvitarReincidenciaNaoConformidadeController GetController(Mocker mocker)
    {
        var controller = new ImplementacaoEvitarReincidenciaNaoConformidadeController(mocker.ImplementacaoEvitarReincidenciaNaoConformidadeService,
            mocker.ImplementacaoEvitarReincidenciaNaoConformidadeViewService);

        return controller;
    }

    private class Mocker
    {
        public IImplementacaoEvitarReincidenciaNaoConformidadeService
            ImplementacaoEvitarReincidenciaNaoConformidadeService { get; set; }

        public IImplementacaoEvitarReincidenciaNaoConformidadeViewService
            ImplementacaoEvitarReincidenciaNaoConformidadeViewService { get; set; }
    }
}