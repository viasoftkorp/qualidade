import { IPagedResultOutputDto } from "@viasoft/common"
import { IPagelessResultDto } from "@viasoft/rnc/api-clients/Utils/iPagelessResultDto"
import { ProdutoOutput } from "@viasoft/rnc/app/pages/shared/produto-autocomplete-select/produto-autocomplete-select.component"
import { ids, strings, codes } from "cypress/support/test-utils"

export const getAllProductsRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/produtos?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            codigo:`${codes[0]}`,
            descricao: strings[0],
            idCategoria:ids[0]
          } as ProdutoOutput,
          {
            id:ids[1],
            codigo:`${codes[1]}`,
            descricao: strings[1],
            idCategoria:ids[1]
          } as ProdutoOutput,
          {
            id:ids[2],
            codigo:`${codes[2]}`,
            descricao: strings[2],
            idCategoria:ids[2]
          } as ProdutoOutput
        ]
      } as IPagedResultOutputDto<ProdutoOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<ProdutoOutput>>
}
export const getAllPagelessProductsRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/produtos/pageless?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        items:[
          {
            id:ids[0],
            codigo:`${codes[0]}`,
            descricao: strings[0],
            idCategoria:ids[0]
          } as ProdutoOutput,
          {
            id:ids[1],
            codigo:`${codes[1]}`,
            descricao: strings[1],
            idCategoria:ids[1]
          } as ProdutoOutput,
          {
            id:ids[2],
            codigo:`${codes[2]}`,
            descricao: strings[2],
            idCategoria:ids[2]
          } as ProdutoOutput,
          {
            id:ids[3],
            codigo:`${codes[3]}`,
            descricao: strings[3],
            idCategoria:ids[3]
          } as ProdutoOutput,
          {
            id:ids[4],
            codigo:`${codes[4]}`,
            descricao: strings[4],
            idCategoria:ids[4]
          } as ProdutoOutput,
          {
            id:ids[5],
            codigo:`${codes[5]}`,
            descricao: strings[5],
            idCategoria:ids[5]
          } as ProdutoOutput,
          {
            id:ids[6],
            codigo:`${codes[6]}`,
            descricao: strings[6],
            idCategoria:ids[6]
          } as ProdutoOutput,
          {
            id:ids[7],
            codigo:`${codes[7]}`,
            descricao: strings[7],
            idCategoria:ids[7]
          } as ProdutoOutput,
          {
            id:ids[8],
            codigo:`${codes[8]}`,
            descricao: strings[8],
            idCategoria:ids[8]
          } as ProdutoOutput,
          {
            id:ids[9],
            codigo:`${codes[9]}`,
            descricao: strings[9],
            idCategoria:ids[9]
          } as ProdutoOutput,
        ]
      } as IPagelessResultDto<ProdutoOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<ProdutoOutput>>
}
export const getProductByIdRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/produtos/**`,
    method:'GET',
    response: {
      statusCode: 200,
      body: {
        id:ids[0],
        codigo:`${codes[0]}`,
        descricao: strings[0],
        idCategoria:ids[0]
      } as ProdutoOutput
    }
  } as CypressRequestV2<ProdutoOutput>
}
