using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities;

public interface IBaseExternalEntityService
{
    public Task BatchInserirNaoCadastrados(List<Guid> idsEntidades);
    public Task InserirSeNaoCadastrado(Guid idEntidade);
}