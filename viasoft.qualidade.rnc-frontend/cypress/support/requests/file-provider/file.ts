export const addMultipleFiles = () => {
  return {
  url: 'qualidade/rnc/gateway/file-provider/app/**/domain/**/subdomain/**/file',
  method: 'POST',
  response: {
    statusCode: 200,
    body: {
      totalCount: 2,
      items: [{
        contentType:"image/png",
        creationTime:"2022-12-21T18:02:50.0821567Z",
        creatorId:"4e6296a4-db80-464c-8957-9a380c79369b",
        extension:"png",
        fileSize:220229,
        filename:"Screenshot (1).png",
        id:"b709afaf-5aab-41a7-bf0f-08dae355b987",
        lastModification:"2022-11-25T13:31:13.9620000Z",
        lastVisualizationTime:null,
        name:"Screenshot (1)",
        subdomain:"dd379b56-4ace-65a3-7248-6a4f28c2a40a",
        tags:[],
        userName:"admin admin"
      },
      {
        contentType:"image/png",
        creationTime:"2022-12-21T18:02:50.0821567Z",
        creatorId:"4e6296a4-db80-464c-8957-9a380c79369b",
        extension:"png",
        fileSize:220228,
        filename:"Screenshot (2).png",
        id:"b709afaf-5aab-41a7-bf0f-08dae355b967",
        lastModification:"2022-11-25T13:31:13.9620000Z",
        lastVisualizationTime:null,
        name:"Screenshot (2)",
        subdomain:"dd379b56-4ace-65a3-7248-6a4f28c2a40a",
        tags:[],
        userName:"admin user"
      },
    ]
  }
  }
} as CypressRequest
}

export const getFilesList = () => {
  return {
  url: 'qualidade/rnc/gateway/file-provider/app/**/domains',
  method: 'POST',
  response:{
    statusCode: 200,
    body:{
      totalCount: 2,
      items:
      [
        {
          contentType:"image/png",
          creationTime:"2022-12-21T18:02:50.0821567Z",
          creatorId:"4e6296a4-db80-464c-8957-9a380c79369b",
          extension:"png",
          fileSize:220229,
          filename:"Screenshot (1).png",
          id:"b709afaf-5aab-41a7-bf0f-08dae355b987",
          lastModification:"2022-11-25T13:31:13.9620000Z",
          lastVisualizationTime:null,
          name:"Screenshot (1)",
          subdomain:"dd379b56-4ace-65a3-7248-6a4f28c2a40a",
          tags:[],
          userName:"admin admin"
        },
        {
          contentType:"image/png",
          creationTime:"2022-12-21T18:02:50.0821567Z",
          creatorId:"4e6296a4-db80-464c-8957-9a380c79369b",
          extension:"png",
          fileSize:220228,
          filename:"Screenshot (2).png",
          id:"b709afaf-5aab-41a7-bf0f-08dae355b967",
          lastModification:"2022-11-25T13:31:13.9620000Z",
          lastVisualizationTime:null,
          name:"Screenshot (2)",
          subdomain:"dd379b56-4ace-65a3-7248-6a4f28c2a40a",
          tags:[],
          userName:"admin user"
        },
      ]
    }
  }
} as CypressRequest
}

export const deleteFile = () => {
  return {
  url: 'qualidade/rnc/gateway/file-provider/file/**',
  method: 'DELETE',
  response: {}
} as CypressRequest
}

export const downloadFiles = () => {
  return {
  url: 'qualidade/rnc/gateway/file-provider/file-grid',
  method: 'POST',
  response: {}
} as CypressRequest
}

export const downloadFileRequest = () => {
  return {
  url: 'qualidade/rnc/gateway/file-provider/file/**/download',
  method: 'GET',
  response: {}
} as CypressRequest
}


