import { ConfiguracaoGeralOutput } from '../../../../apps/rnc/src/api-clients/Configuracoes/Gerais/configuracao-geral-output';
import { ConfiguracaoGeralInput } from '../../../../apps/rnc/src/api-clients/Configuracoes/Gerais/configuracao-geral-input';

export const getConfiguracoesGeraisRequest = () => {
  return {
    url: `qualidade/rnc/gateway/configuracoes-gerais`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        considerarApenasSaldoApontado: false,
      } as ConfiguracaoGeralOutput,
    },
  } as CypressRequestV2<ConfiguracaoGeralOutput>;
};
export const updateConfiguracoesGeraisRequest = () => {
  return {
    url: `qualidade/rnc/gateway/configuracoes-gerais`,
    method: 'PUT',
    response: {
      statusCode: 200,
      body: {}
    },
    expectedBody: {
    } as ConfiguracaoGeralInput,
  } as CypressRequestV2<ConfiguracaoGeralOutput>;
};
