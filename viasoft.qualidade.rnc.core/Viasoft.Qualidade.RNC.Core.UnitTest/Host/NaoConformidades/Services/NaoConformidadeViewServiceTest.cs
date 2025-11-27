using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services;

public class NaoConformidadeViewServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get List NaoConformidadeView")]
    public async Task GetListNaoConformidadeViewTest()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var produto = TestUtils.ObjectMother.GetProduto(0);
        var pessoa = TestUtils.ObjectMother.GetCliente(0);
        var usuario = TestUtils.ObjectMother.GetUsuario(0);
        var natureza = TestUtils.ObjectMother.GetNatureza(0);
        var naoConformidade = TestUtils.ObjectMother.GetNaoConformidade(0);

        await mocker.Cliente.InsertAsync(pessoa);
        await mocker.Usuario.InsertAsync(usuario);
        await mocker.Produto.InsertAsync(produto);
        await mocker.Natureza.InsertAsync(natureza);
        await mocker.NaoConformidade.InsertAsync(naoConformidade);
        await UnitOfWork.SaveChangesAsync();

        var expectedResult = GetNaoConformidadeViewOutput(0);
        expectedResult.Natureza = natureza.Codigo + " - " + natureza.Descricao;
        expectedResult.Cliente = pessoa.Codigo + " - " + pessoa.RazaoSocial;
        expectedResult.Produto = produto.Codigo + " - " + produto.Descricao;
        
        // Act
        var getListOutput = await service.GetListView(new PagedFilteredAndSortedRequestInput());
        
        // Assert
        getListOutput.Items[0].Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se origem for inspeção de entrada, deve ter fornecedor e não cliente")]
    public async Task GetListNaoConformidadeViewTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var produto = TestUtils.ObjectMother.GetProduto(0);
        var pessoa = TestUtils.ObjectMother.GetCliente(0);
        var usuario = TestUtils.ObjectMother.GetUsuario(0);
        var natureza = TestUtils.ObjectMother.GetNatureza(0);
        var naoConformidade = TestUtils.ObjectMother.GetNaoConformidade(0);
        naoConformidade.Origem = OrigemNaoConformidade.InspecaoEntrada;
        
        await mocker.Cliente.InsertAsync(pessoa);
        await mocker.Usuario.InsertAsync(usuario);
        await mocker.Produto.InsertAsync(produto);
        await mocker.Natureza.InsertAsync(natureza);
        await mocker.NaoConformidade.InsertAsync(naoConformidade);
        await UnitOfWork.SaveChangesAsync();
        
        // Act
        var getListOutput = await service.GetListView(new PagedFilteredAndSortedRequestInput());
        
        // Assert
        getListOutput.Items[0].Cliente.Should().BeNullOrEmpty();
        getListOutput.Items[0].CodigoCliente.Should().BeNullOrEmpty();
        getListOutput.Items[0].IdCliente.Should().BeNull();
        getListOutput.Items[0].NomeCliente.Should().BeNullOrEmpty();
        
        getListOutput.Items[0].Fornecedor.Should().Be($"{pessoa.Codigo} - {pessoa.RazaoSocial}");
        getListOutput.Items[0].CodigoFornecedor.Should().Be(pessoa.Codigo);
        getListOutput.Items[0].IdFornecedor.Should().Be(naoConformidade.IdPessoa);
        getListOutput.Items[0].NomeFornecedor.Should().Be(pessoa.RazaoSocial);

    }
    
    [Fact(DisplayName = "Se origem não inspeção de entrada, deve ter cliente e não fornecedor")]
    public async Task GetListNaoConformidadeViewTest3()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var produto = TestUtils.ObjectMother.GetProduto(0);
        var pessoa = TestUtils.ObjectMother.GetCliente(0);
        var usuario = TestUtils.ObjectMother.GetUsuario(0);
        var natureza = TestUtils.ObjectMother.GetNatureza(0);
        var naoConformidade = TestUtils.ObjectMother.GetNaoConformidade(0);
        naoConformidade.Origem = OrigemNaoConformidade.Cliente;
        
        await mocker.Cliente.InsertAsync(pessoa);
        await mocker.Usuario.InsertAsync(usuario);
        await mocker.Produto.InsertAsync(produto);
        await mocker.Natureza.InsertAsync(natureza);
        await mocker.NaoConformidade.InsertAsync(naoConformidade);
        await UnitOfWork.SaveChangesAsync();
        
        // Act
        var getListOutput = await service.GetListView(new PagedFilteredAndSortedRequestInput());
        
        // Assert
        getListOutput.Items[0].Fornecedor.Should().BeNullOrEmpty();
        getListOutput.Items[0].CodigoFornecedor.Should().BeNullOrEmpty();
        getListOutput.Items[0].IdFornecedor.Should().BeNull();
        getListOutput.Items[0].NomeFornecedor.Should().BeNullOrEmpty();
        
        getListOutput.Items[0].Cliente.Should().Be($"{pessoa.Codigo} - {pessoa.RazaoSocial}");
        getListOutput.Items[0].CodigoCliente.Should().Be(pessoa.Codigo);
        getListOutput.Items[0].IdCliente.Should().Be(naoConformidade.IdPessoa);
        getListOutput.Items[0].NomeCliente.Should().Be(pessoa.RazaoSocial);

    }

    private NaoConformidadeViewOutput GetNaoConformidadeViewOutput(int index)
    {
        var naoConformidadeViewOutput = new NaoConformidadeViewOutput
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index],
            Origem = OrigemNaoConformidade.Cliente,
            Status = StatusNaoConformidade.Aberto,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[index],
            NumeroNotaFiscal = TestUtils.ObjectMother.Ints[index].ToString(),
            IdNatureza = TestUtils.ObjectMother.Guids[index],
            DescricaoNatureza = TestUtils.ObjectMother.Strings[index],
            CodigoNatureza = TestUtils.ObjectMother.Ints[index],
            Natureza = TestUtils.ObjectMother.Strings[index],
            IdCliente = TestUtils.ObjectMother.Guids[index],
            NomeCliente = TestUtils.ObjectMother.Strings[index],
            CodigoCliente = TestUtils.ObjectMother.Ints[index].ToString(),
            Cliente = TestUtils.ObjectMother.Strings[index],
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            IdProduto = TestUtils.ObjectMother.Guids[index],
            DescricaoProduto = TestUtils.ObjectMother.Strings[index],
            CodigoProduto = TestUtils.ObjectMother.Ints[index].ToString(),
            Produto = TestUtils.ObjectMother.Strings[index],
            Revisao = TestUtils.ObjectMother.Ints[index].ToString(),
            Equipe = TestUtils.ObjectMother.Strings[index],
            IdLote = TestUtils.ObjectMother.Guids[index],
            NumeroLote = TestUtils.ObjectMother.Ints[index].ToString(),
            DataFabricacaoLote = TestUtils.ObjectMother.Datas[index],
            CampoNf = TestUtils.ObjectMother.Strings[index],
            IdCriador = TestUtils.ObjectMother.Guids[index],
            LoteTotal = false,
            LoteParcial = false,
            Rejeitado = false,
            AceitoConcessao = false,
            RetrabalhoPeloCliente = false,
            RetrabalhoNoCliente = false,
            NaoConformidadeEmPotencial = false,
            RelatoNaoConformidade = false,
            MelhoriaEmPotencial = false,
            Descricao = TestUtils.ObjectMother.Strings[index],
            Incompleta = false,
            NomeUsuarioCriador = TestUtils.ObjectMother.Strings[index],
            SobrenomeUsuarioCriador = TestUtils.ObjectMother.Strings[index],
            NomeFornecedor = "",
            CodigoFornecedor = "",
            Fornecedor = "",
        };
        return naoConformidadeViewOutput;
    }

    private NaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new NaoConformidadeServiceMocker()
        {
            NaoConformidade = ServiceProvider.GetService<IRepository<NaoConformidade>>(),
            Produto = ServiceProvider.GetService<IRepository<Produto>>(),
            Cliente = ServiceProvider.GetService<IRepository<Cliente>>(),
            Natureza = ServiceProvider.GetService<IRepository<Natureza>>(),
            Usuario = ServiceProvider.GetService<IRepository<Usuario>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private NaoConformidadeViewService GetService(NaoConformidadeServiceMocker mocker)
    {
        var service = new NaoConformidadeViewService(mocker.NaoConformidade, mocker.Produto, mocker.Cliente, 
            mocker.Natureza, mocker.Usuario, mocker.FakeCurrentCompany);

        return service;
    }

    public class NaoConformidadeServiceMocker
    {
        public IRepository<NaoConformidade> NaoConformidade { get; set; }
        public IRepository<Produto> Produto { get; set; }
        public IRepository<Cliente> Cliente { get; set; }
        public IRepository<Natureza> Natureza { get; set; }
        public IRepository<Usuario> Usuario { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}