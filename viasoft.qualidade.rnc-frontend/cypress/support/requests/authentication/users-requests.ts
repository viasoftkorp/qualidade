import { UserOutput } from '@viasoft/administration'
import { IPagedResultOutputDto } from '@viasoft/common'
import { ids, strings, validEmails } from 'cypress/support/test-utils'

export const getAllUsersRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/authentication/users?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            email:validEmails[0],
            firstName: strings[0],
            secondName: strings[1]
          } as UserOutput,
          {
            id:ids[1],
            email:validEmails[1],
            firstName: strings[2],
            secondName: strings[3]
          } as UserOutput
        ]
      } as IPagedResultOutputDto<UserOutput>
    }
  } as CypressRequestV2<UserOutput>
}
export const getUserByIdRequest = () => {
  return {
    url: 'qualidade/rnc/gateway/authentication/users/**',
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        firstName: 'Admin',
        secondName: 'Admin',
        email: 'admin@korp.com.br',
        login: "admin",
        isActive: true,
      } as UserOutput
    },
  } as CypressRequestV2<UserOutput>
}
