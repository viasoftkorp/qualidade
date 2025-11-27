using System.Collections.Generic;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeValidatorServices;

public class ValidateServicosTests : OperacaoRetrabalhoNaoConformidadeValidatorServiceTest
{
    [Fact(DisplayName = "Se nenhuma máquina foi adicionada, deve retornar NenhumaMaquinaCadastrada")]
    public void ValidateServicosTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var operacaoRetrabalhoNaoConformidadeInput = GetOperacaoRetrabalhoNaoConformidadeInput(0);
        
        // Act
        var result =  service.ValidateMaquina(operacaoRetrabalhoNaoConformidadeInput);

        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.NenhumMaquinaCadastrada);
    }
    
    [Fact(DisplayName = "Se máquina foi adicionada, deve retornar Ok")]
    public void ValidateServicosTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var operacaoRetrabalhoNaoConformidadeInput = GetOperacaoRetrabalhoNaoConformidadeInput(0);

        operacaoRetrabalhoNaoConformidadeInput.Maquinas = GetMaquinasInput(1, 1);

        // Act
        var result =  service.ValidateMaquina(operacaoRetrabalhoNaoConformidadeInput);
        
        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.Ok);
    }

    private OperacaoRetrabalhoNaoConformidadeInput GetOperacaoRetrabalhoNaoConformidadeInput(int index)
    {
        var operacaoRetrabalhoNaoConformidadeInput = new OperacaoRetrabalhoNaoConformidadeInput
        {
            NumeroOperacaoARetrabalhar = TestUtils.ObjectMother.Ints[index].ToString(),
            Quantidade = TestUtils.ObjectMother.Ints[0],
            Maquinas = new List<MaquinaInput>()
        };
        return operacaoRetrabalhoNaoConformidadeInput;
    }
    private List<MaquinaInput> GetMaquinasInput(int numeroMaquinas, int numeroMateriaisPorMaquina)
    {
        var output = new List<MaquinaInput>();
        
        for (int i = 0; i < numeroMaquinas; i++)
        {
            var maquina = new MaquinaInput()
            {
                Horas = TestUtils.ObjectMother.Ints[i],
                Detalhamento = TestUtils.ObjectMother.Strings[i],
                IdRecurso = TestUtils.ObjectMother.Guids[i],
                Minutos = TestUtils.ObjectMother.Ints[i],
                Id = TestUtils.ObjectMother.Guids[i],
                Materiais = GetMateriaisInput(numeroMateriaisPorMaquina)
            };    
            output.Add(maquina);
        }
        
        return output;
    }
    private List<MaterialInput> GetMateriaisInput(int numeroMateriais)
    {
        var output = new List<MaterialInput>();
        
        for (int i = 0; i < numeroMateriais; i++)
        {
            var material = new MaterialInput()
            {
                Quantidade = TestUtils.ObjectMother.Ints[i],
                Detalhamento = TestUtils.ObjectMother.Strings[i],
                IdProduto = TestUtils.ObjectMother.Guids[i],
                Id = TestUtils.ObjectMother.Guids[i],
                IdMaquina = TestUtils.ObjectMother.Guids[i]
            };    
            output.Add(material);
        }
        
        return output;
    }
}