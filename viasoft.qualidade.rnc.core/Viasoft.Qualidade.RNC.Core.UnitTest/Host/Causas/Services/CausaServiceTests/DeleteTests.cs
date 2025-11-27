using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;

public class DeleteTests : CausaServiceTest
{
    [Fact(DisplayName = "Delete Causa with Success")]
    public async Task DeleteCausaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causaInserida = TestUtils.ObjectMother.GetCausa(0);
        
        await mocker.Causas.InsertAsync(causaInserida);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Delete(causaInserida.Id);

        //Assert
        var causaEncontrada = await mocker.Causas.AnyAsync(causa => causa.Id == causaInserida.Id);
        
        causaEncontrada.Should().BeFalse();
        output.Should().Be(ValidationResult.Ok);
    }

    
    [Fact(DisplayName = "Delete Causa with NotFound Id")]
    public async Task DeleteCausaWithNotFoundId()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
    [Fact(DisplayName = "Se causa utilizada por um defeito, deve retornar EntidadeEmUso")]
    public async Task DeleteTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causa = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
        };
        await mocker.Causas.InsertAsync(causa, true);

        await mocker.Defeitos.InsertAsync(new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0]
        }, true);
        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);

        //Assert
        output.Should().Be(ValidationResult.EntidadeEmUso);
        mocker.Causas.Should().NotBeEmpty();
    }
    
    [Fact(DisplayName = "Se causa utilizada por uma nao conformidade, deve retornar EntidadeEmUso")]
    public async Task DeleteTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causa = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
        };
        await mocker.Causas.InsertAsync(causa, true);

        await mocker.CausaNaoConformidades.InsertAsync(new CausaNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0]
        }, true);
        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);

        //Assert
        output.Should().Be(ValidationResult.EntidadeEmUso);
        mocker.Causas.Should().NotBeEmpty();
    }
}