/// <reference types="cypress" />

declare namespace Cypress {
  interface Chainable {
    //#region cypress-image-snapshot
    /**
     * Takes a screenshot of the screen and compare with the saved one
     * @repo https://github.com/palmerhq/cypress-image-snapshot
     */
    matchImageSnapshot(name?: string): Chainable<Element>
    matchImageSnapshot(options: object): Chainable<Element>
    matchImageSnapshot(name: string, options: object): Chainable<Element>

    //#endregion cypress-image-snapshot
    // TODO: Add cypress plugins methods here
    batchRequestStub(requestsToStub: CypressRequest | CypressRequest[]): Chainable<string[]>;
    validateRequestBody(body: RequestBody, response: any): Chainable<Element>;
    validateRequestStatusCode(statusCode?: number, expectedStatusCode? :number ):Chainable<Element>;
    getVsCheckbox(formControlName:string, parentSelector?:string):Chainable<Element>;
    getAutoCompleteItem(options: getAutoCompleteItemOptions):Chainable<Element>;
    getVsTextArea(formControlName:string, parentSelector?:string):Chainable<Element>;
    getVsTabGroupItem(itemLabel:string, parentSelector: string):Chainable<Element>;
    getVsInput(formControlName:string, parentSelector?:string): Chainable<Element>;
    getVsDatePicker(formControlName:string, parentSelector?:string): Chainable<Element>;
  }
}
interface CypressRequest {
  url: string;
  method: 'GET' | 'POST' | 'DELETE' | 'PUT';
  response: any;
  expectedBody?:any

}
interface CypressRequestV2<T> {
  url: string;
  method: 'GET' | 'POST' | 'DELETE' | 'PUT';
  response: CypressBody<T>;
  expectedBody: any;
}
interface CypressBody<T> {
  statusCode: number;
  body: T;
}
interface CypressResponse{
  statusCode:number,
  body:any
}
interface getAutoCompleteItemOptions {
  formControlName:string;
  request:CypressRequestV2<unknown>;
  selector:string;
  eq?:number;
}
